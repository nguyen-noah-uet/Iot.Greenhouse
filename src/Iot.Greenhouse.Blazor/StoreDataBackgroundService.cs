using System;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Iot.Greenhouse.Devices;
using Iot.Greenhouse.Mqtt;
using Iot.Greenhouse.Nodes;
using Iot.Greenhouse.Sensors;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client;

namespace Iot.Greenhouse.Blazor
{
    public class StoreDataBackgroundService : BackgroundService
    {
        private readonly ILogger<StoreDataBackgroundService> _logger;
        private readonly IMqttService _mqttService;
        private readonly ISensorDataService _sensorDataService;
        private readonly IDeviceStatusService deviceStatusService;
        private readonly INodeStatusService nodeStatusService;

        public StoreDataBackgroundService(
            ILogger<StoreDataBackgroundService> logger,
            IMqttService mqttService,
            ISensorDataService sensorDataService,
            IDeviceStatusService deviceStatusService,
            INodeStatusService nodeStatusService)
        {
            _logger = logger;
            _mqttService = mqttService;
            _sensorDataService = sensorDataService;
            this.deviceStatusService = deviceStatusService;
            this.nodeStatusService = nodeStatusService;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("ExecuteAsync StoreDataBackgroundService");
            _mqttService.SubscribeMessageHandler(OnMessageReceived);
            return Task.CompletedTask;
        }

        private async Task OnMessageReceived(MqttApplicationMessageReceivedEventArgs e)
        {
            try
            {
                var payloadString = e.ApplicationMessage.ConvertPayloadToString();
                var topic = e.ApplicationMessage.Topic; 
                Regex nodeStatusRegex = new(@"node-status\/([a-f,0-9,-]+)\/");
                var nodeStatusMatch = nodeStatusRegex.Match(topic);
                if (nodeStatusMatch.Success)
                {
                    Guid nodeId = Guid.Parse(nodeStatusMatch.Groups[1].Value);
                    var nodeStatus = payloadString == "1";
                    await InsertNodeStatus(nodeId, nodeStatus);
                    return;
                }
                Regex sensorRegex = new(@"sensors\/([a-f,0-9,-]+)\/");
                var sensorMatch = sensorRegex.Match(topic);
                if (sensorMatch.Success)
                {
                    Guid sensorId = Guid.Parse(sensorMatch.Groups[1].Value);
                    double sensorValue = double.Parse(payloadString);
                    await InsertSensorData(sensorId, sensorValue);
                    return;
                }
                Regex deviceStatusRegex = new(@"device-status\/([a-f,0-9,-]+)\/");
                var deviceStatusMatch = deviceStatusRegex.Match(topic);
                if (deviceStatusMatch.Success)
                {
                    Guid deviceId = Guid.Parse(deviceStatusMatch.Groups[1].Value);
                    var deviceStatus = payloadString == "1";
                    await InsertDeviceStatus(deviceId, deviceStatus);
                    return;
                }
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when insert data");
            }
            
        }

        private async Task InsertNodeStatus(Guid nodeId, bool nodeStatus)
        {
            NodeStatusCreateDto nodeStatusDto = new()
            {
                NodeId = nodeId,
                IsOnline = nodeStatus,
            };
            await nodeStatusService.CreateAsync(nodeStatusDto);
            
        }

        private async Task InsertDeviceStatus(Guid deviceId, bool deviceStatus)
        {
            DeviceStatusCreateDto deviceStatusDto = new()
            {
                DeviceId = deviceId,
                IsOn= deviceStatus,
            };
            await deviceStatusService.CreateAsync(deviceStatusDto);
        }

        private async Task InsertSensorData(Guid sensorId, double sensorValue)
        {
            try
            {
                SensorDataCreateDto sensorDto = new()
                {
                    SensorId = sensorId,
                    Value = sensorValue
                };
                await _sensorDataService.CreateAsync(sensorDto);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error when insert sensor data");
            }
        }
    }
}
