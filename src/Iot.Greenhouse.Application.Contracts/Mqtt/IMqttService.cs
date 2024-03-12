using System;
using System.Threading.Tasks;
using MQTTnet.Client;

namespace Iot.Greenhouse.Mqtt
{
    public interface IMqttService
    {
        Task ConnectAsync();
        Task DisconnectAsync();
        Task PublishAsync(string topic, string payload);
        Task SubscribeAsync(string topic);
        Task SubscribeAsync(MqttClientSubscribeOptions options);
        Task UnsubscribeAsync(string topic);
        void SubscribeMessageHandler(Func<MqttApplicationMessageReceivedEventArgs, Task> handler);
        void UnsubscribeMessageHandler(Func<MqttApplicationMessageReceivedEventArgs, Task> handler);
    }
}
