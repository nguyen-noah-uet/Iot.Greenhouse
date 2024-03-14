using MQTTnet.Client;
using MQTTnet;
using System.Threading.Tasks;
using System;


namespace Iot.Greenhouse.Mqtt
{
    public class MqttService : IMqttService
    {
        private readonly IMqttClient _mqttClient;
        private readonly MqttClientOptions _mqttOptions;


        public MqttService(MqttClientOptions mqttOptions)
        {
            _mqttOptions = mqttOptions ?? throw new ArgumentNullException(nameof(mqttOptions));
            _mqttClient = new MqttFactory().CreateMqttClient();
        }

        public async Task ConnectAsync()
        {
            if (!_mqttClient.IsConnected)
            {
                await _mqttClient.ConnectAsync(_mqttOptions);
            }
        }

        public async Task DisconnectAsync()
        {
            if (_mqttClient.IsConnected)
            {
                await _mqttClient.DisconnectAsync();
            }
        }

        public async Task PublishAsync(string topic, string payload)
        {
            if (_mqttClient.IsConnected)
            {
                var message = new MqttApplicationMessageBuilder()
                    .WithTopic(topic)
                    .WithPayload(payload)
                    .Build();

                await _mqttClient.PublishAsync(message);
            }
        }

        public async Task SubscribeAsync(string topic)
        {
            if (_mqttClient.IsConnected)
            {
                await _mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic(topic).Build());
            }
        }

        public async Task SubscribeAsync(MqttClientSubscribeOptions options)
        {
            if (_mqttClient.IsConnected)
            {
                await _mqttClient.SubscribeAsync(options);
            }
        }

        public async Task UnsubscribeAsync(string topic)
        {
            if (_mqttClient.IsConnected)
            {
                await _mqttClient.UnsubscribeAsync(topic);
            }
        }

        public void SubscribeMessageHandler(Func<MqttApplicationMessageReceivedEventArgs, Task> handler)
        {
            if (_mqttClient.IsConnected)
            {
                _mqttClient.ApplicationMessageReceivedAsync += handler;
            }
        }
        public void UnsubscribeMessageHandler(Func<MqttApplicationMessageReceivedEventArgs, Task> handler)
        {
            if (_mqttClient.IsConnected)
            {
                _mqttClient.ApplicationMessageReceivedAsync -= handler;
            }
        }
    }
}
