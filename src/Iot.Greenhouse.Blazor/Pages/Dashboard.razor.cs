using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Iot.Greenhouse.Devices;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client;
using Volo.Abp.Application.Dtos;

namespace Iot.Greenhouse.Blazor.Pages;

public partial class Dashboard
{
    private List<NodeViewModel> Nodes = new();
    private IQueryable<NodeViewModel> NodesQuery => Nodes.AsQueryable();
    public List<string> LogMessages { get; set; } = new();

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
            var nodes = await NodeService.GetListAsync(new PagedAndSortedResultRequestDto() { Sorting = "id" });
            for (int i = 0; i < nodes.TotalCount; i++)
            {
                var m = new NodeViewModel()
                {
                    Node = nodes.Items[i],
                    IsOnline = false,
                    Sensors = (await SensorService.GetByNodeId(nodes.Items[i].Id)).Select(x => new SensorViewModel() { Sensor = x }).ToList(),
                    Devices = (await DeviceService.GetByNodeId(nodes.Items[i].Id)).Select(x => new DeviceViewModel() { Device = x }).ToList()
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
            //await OnNodeStatusMessageReceived(topic, payloadString);
        }
        else if (topic.StartsWith(GreenhouseStrings.Topics.Sensors))
        {
            Logger.LogInformation("{topic}: {payload}",topic, payloadString);
            //await OnSensorMessageReceived(topic, payloadString);
        }
        else if (topic.StartsWith(GreenhouseStrings.Topics.DeviceStatus))
        {
            Logger.LogInformation("{topic}: {payload}", topic, payloadString);
            //await OnDeviceStatusMessageReceived(topic, payloadString);
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


    protected override void Dispose(bool disposing)
    {
        MqttService.UnsubscribeMessageHandler(OnMessageReceived);
    }

    private async void TurnOnDevice(DeviceViewModel device)
    {
        try
        {
            await MqttService.PublishAsync(GreenhouseStrings.Topics.Command + device.Device.Id + "/", "1");
        }
        catch (Exception ex)
        {
            ToastService.ShowError(ex.Message);
        }
    }
    private async void TurnOffDevice(DeviceViewModel device)
    {
        try
        {
            await MqttService.PublishAsync(GreenhouseStrings.Topics.Command + device.Device.Id + "/", "0");
        }
        catch (Exception ex)
        {
            ToastService.ShowError(ex.Message);
        }
    }
}