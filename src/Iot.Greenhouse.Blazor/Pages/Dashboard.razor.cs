using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Iot.Greenhouse.Nodes;
using Iot.Greenhouse.Sensors;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.FluentUI.AspNetCore.Components;
using MQTTnet;
using MQTTnet.Client;
using Volo.Abp.Application.Dtos;

namespace Iot.Greenhouse.Blazor.Pages;

public partial class Dashboard
{
    private PagedResultDto<NodeDto> _nodes;
    private List<NodeViewModel> Nodes = new();
    private IQueryable<NodeViewModel> NodesQuery => Nodes.AsQueryable();
    public List<string> LogMessages { get; set; } = new();

    private PumpRequest pumpRequest { get; set; } = new();
    private DeviceStatus deviceStatus { get; set; } = new();
    public bool IsFanTurning { get; set; }
    public bool IsLightTurning { get; set; }
    public bool IsPumping { get; set; }
    public bool Loaded { get; set; }
    protected override async Task OnInitializedAsync()
    {
        if (!CurrentUser.IsAuthenticated)
        {
            NavigationManager.NavigateTo("/Account/Login", true);
            return;
        }

        try
        {
            _nodes = await NodeService.GetListAsync(new PagedAndSortedResultRequestDto() { Sorting = "id" });
            for (int i = 0; i < _nodes.TotalCount; i++)
            {
                var m = new NodeViewModel()
                {
                    Node = _nodes.Items[i],
                    Humidity =
                        (await SensorService.GetLatestValue(_nodes.Items[i].Id, SensorType.Humidity)).SensorValue,
                    Temperature = (await SensorService.GetLatestValue(_nodes.Items[i].Id, SensorType.Temperature))
                        .SensorValue,
                    Ec = (await SensorService.GetLatestValue(_nodes.Items[i].Id, SensorType.Ec)).SensorValue,
                    Ph = (await SensorService.GetLatestValue(_nodes.Items[i].Id, SensorType.Ph)).SensorValue
                };
                Nodes.Add(m);
            }

        }
        catch (Exception ex)
        {
            ToastService.ShowError(ex.Message);
        }

        try
        {
            MqttService.UnsubscribeMessageHandler(OnMessageReceived);
            MqttService.SubscribeMessageHandler(OnMessageReceived);
        }
        catch (Exception ex)
        {
            ToastService.ShowError(ex.Message);
        }

        Loaded = true;

    }


    private async Task OnMessageReceived(MqttApplicationMessageReceivedEventArgs e)
    {
        var payloadString = e.ApplicationMessage.ConvertPayloadToString();
        var topic = e.ApplicationMessage.Topic!;
        if (topic.StartsWith(GreenhouseStrings.Topics.NodeStatus))
        {
            Logger.LogInformation("{topic}: {payload}", topic, payloadString);
            await OnNodeStatusMessageReceived(topic, payloadString);
        }
        else if (topic.StartsWith(GreenhouseStrings.Topics.Sensors))
        {
            Logger.LogInformation("{topic}: {payload}",topic, payloadString);
            await OnSensorMessageReceived(topic, payloadString);
        }
        else if (topic.StartsWith(GreenhouseStrings.Topics.DeviceStatus))
        {
            Logger.LogInformation("{topic}: {payload}", topic, payloadString);
            await OnDeviceStatusMessageReceived(topic, payloadString);
        }
        else if (topic.StartsWith(GreenhouseStrings.Topics.Command))
        {
            Logger.LogInformation("{topic}: {payload}", topic, payloadString);
        }
        else if (topic.StartsWith(GreenhouseStrings.Topics.Notifications))
        {
            Logger.LogInformation("{topic}: {payload}", topic, payloadString);
        }
    }

    private async Task OnNodeStatusMessageReceived(string topic, string payloadString)
    {
        try
        {
            // node-status/{nodeId}
            Regex regex = new(@"node-status\/(?<nodeId>\d+)");
            var match = regex.Match(topic);
            if (match.Success)
            {
                var nodeId = int.Parse(match.Groups["nodeId"].Value);
                int status = int.Parse(payloadString);
                Nodes.First(x=>x.Node.Id == nodeId).IsActive = status == 1;
                await InvokeAsync(StateHasChanged);
            }
        } catch (Exception e)
        {
            Logger.LogError(e.Message);
        }
    }

    private async Task OnSensorMessageReceived(string topic, string payloadString)
    {
        try
        {
            // senssos/{nodeId}/{sensorType}
            Regex regex = new(@"sensors\/(?<nodeId>\d+)\/(?<sensorType>\d+)\/");
            var match = regex.Match(topic);
            if (match.Success)
            {
                var nodeId = int.Parse(match.Groups["nodeId"].Value);
                var sensorTypeInt = int.Parse(match.Groups["sensorType"].Value);
                var m = Nodes.Find(x => x.Node.Id == nodeId);
                if (m is null)
                    return;
                double value = double.Parse(payloadString);
                SensorType sensorType = (SensorType)sensorTypeInt;
                switch (sensorType)
                {
                    case SensorType.Humidity:
                        m.Humidity = value;
                        break;
                    case SensorType.Temperature:
                        m.Temperature = value;
                        break;
                    case SensorType.Ph:
                        m.Ph = value;
                        break;
                    case SensorType.Ec:
                        m.Ec = value;
                        break;
                }
                await InvokeAsync(StateHasChanged);
            }
            
        }
        catch (Exception e)
        {
            Logger.LogError(e.Message);
        }

    }

    private bool ParseDeviceStatus(string status)
    {
        if (status is "ON" or "on" or "On" or "1")
        {
            return true;
        }
        else if (status is "OFF" or "off" or "Off" or "0")
        {
            return false;
        }
        else
        {
            throw new InvalidDataException("Unable to parse device status");
        }
    }

    private async Task OnDeviceStatusMessageReceived(string topic, string payloadString)
    {
        try
        {
            Regex regex = new(@"device-status\/(?<deviceName>\w+)");
            var match = regex.Match(topic);
            if (match.Success)
            {
                var deviceName = match.Groups["deviceName"].Value;
                switch (deviceName)
                {
                    case GreenhouseStrings.Devices.Pump:
                        deviceStatus.PumpStatus = ParseDeviceStatus(payloadString);
                        IsPumping = false;
                        Nodes.FirstOrDefault(x => x.Node.Id == 1)!.IsPumpOn = deviceStatus.PumpStatus;
                        break;
                    case GreenhouseStrings.Devices.Light:
                        deviceStatus.LightStatus = ParseDeviceStatus(payloadString);
                        IsLightTurning = false;
                        Nodes.FirstOrDefault(x => x.Node.Id == 1)!.IsLightOn = deviceStatus.LightStatus;
                        break;
                    case GreenhouseStrings.Devices.Fan:
                        deviceStatus.FanStatus = ParseDeviceStatus(payloadString);
                        IsFanTurning = false;
                        Nodes.FirstOrDefault(x => x.Node.Id == 1)!.IsFanOn = deviceStatus.FanStatus;
                        break;
                    default:
                        break;
                }
                await InvokeAsync(StateHasChanged);
            }
        } 
        catch (Exception e)
        {
            Logger.LogError(e.Message);
        }
        
    }

    protected override void Dispose(bool disposing)
    {
        MqttService.UnsubscribeMessageHandler(OnMessageReceived);
    }

    public class NodeViewModel
    {
        public NodeDto Node { get; set; } = default!;
        public double Humidity { get; set; }
        public double Temperature { get; set; }
        public double Ph { get; set; }
        public double Ec { get; set; }
        public bool IsActive { get; set; } = false;
        public bool? IsPumpOn { get; set; }
        public bool? IsFanOn { get; set; }
        public bool? IsLightOn { get; set; }
    }
    public class PumpRequest
    {
        public double Ph
        {
            get;
            set;
        } = 6.8;

        public double Ec { get; set; } = 700; // unit: µS / cm
    }
    public class DeviceStatus
    {
        public bool PumpStatus { get; set; }
        public bool FanStatus { get; set; }
        public bool LightStatus { get; set; }
    }

    private async Task BtnPumpClick()
    {
        IsPumping = true;
        await InvokeAsync(StateHasChanged);
        if (deviceStatus.PumpStatus)
        {
            await SendStopPumpRequest();
        }
        else
        {
            await SendStartPumpRequest();
        }
        await InvokeAsync(StateHasChanged);
    }
    private async Task BtnLightClick()
    {

        IsLightTurning = true;
        await InvokeAsync(StateHasChanged);
        //await Task.Delay(2000);
        if (deviceStatus.LightStatus)
        {
            await SendTurnOffLightRequest();
        }
        else
        {
            await SendTurnOnLightRequest();
        }
        //IsLightTurning = false;
        await InvokeAsync(StateHasChanged);
    }
    private async Task BtnFanClick()
    {
        
        IsFanTurning = true;
        await InvokeAsync(StateHasChanged);
        //await Task.Delay(2000);
        if (deviceStatus.FanStatus)
        {
            await SendTurnOffFanRequest();
        }
        else
        {
            await SendTurnOnFanRequest();
        }
        //IsFanTurning = false;
        await InvokeAsync(StateHasChanged);
    }

    private async Task SendStartPumpRequest()
    {
        await MqttService.PublishAsync(GreenhouseStrings.Topics.Command + GreenhouseStrings.Devices.Pump +"/", "1");
    }
    private async Task SendStopPumpRequest()
    {
        await MqttService.PublishAsync(GreenhouseStrings.Topics.Command + GreenhouseStrings.Devices.Pump + "/", "0");
    }
    private async Task SendTurnOnLightRequest()
    {
        await MqttService.PublishAsync(GreenhouseStrings.Topics.Command + GreenhouseStrings.Devices.Light +"/", "1");
    }
    private async Task SendTurnOffLightRequest()
    {
        await MqttService.PublishAsync(GreenhouseStrings.Topics.Command + GreenhouseStrings.Devices.Light +"/", "0");
    }

    private async Task SendTurnOffFanRequest()
    {
        await MqttService.PublishAsync(GreenhouseStrings.Topics.Command + GreenhouseStrings.Devices.Fan +"/", "1");
    }
    private async Task SendTurnOnFanRequest()
    {
        await MqttService.PublishAsync(GreenhouseStrings.Topics.Command + GreenhouseStrings.Devices.Light +"/", "0");
    }
}