using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Iot.Greenhouse.Mqtt;
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
        private readonly ISensorService _sensorService;

        public StoreDataBackgroundService(
            ILogger<StoreDataBackgroundService> logger,
            IMqttService mqttService,
            ISensorService sensorService)
        {
            _logger = logger;
            _mqttService = mqttService;
            _sensorService = sensorService;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("ExecuteAsync StoreDataBackgroundService");
            _mqttService.SubscribeMessageHandler(OnMessageReceived);
            return Task.CompletedTask;
        }

        private async Task OnMessageReceived(MqttApplicationMessageReceivedEventArgs e)
        {
            var payloadString = e.ApplicationMessage.ConvertPayloadToString();
            var topic = e.ApplicationMessage.Topic;
            switch (topic)
            {
                case GreenhouseStrings.Topics.NodeStatus:
                    break;
                case GreenhouseStrings.Topics.Sensors:
                    await InsertSensorData(payloadString);
                    break;
                case GreenhouseStrings.Topics.DeviceStatus:
                    break;
                case GreenhouseStrings.Topics.Command:
                    break;
                case GreenhouseStrings.Topics.Notifications:
                    break;
            }
        }

        private async Task InsertSensorData(string payloadString)
        {
            try
            {
                var sensorDto = JsonSerializer.Deserialize<SensorCreateDto>(payloadString);
                if (sensorDto is not null)
                {
                    await _sensorService.CreateAsync(sensorDto);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error when insert sensor data");
            }
        }
    }
}
