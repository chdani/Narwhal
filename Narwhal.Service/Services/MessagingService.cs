using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mqtt;
using System.Reactive.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

namespace Narwhal.Service.Services
{
    public class MessagingService
    {
        private readonly ILogger<MessagingService> _logger;

        private Task _initializationTask;
        private IMqttClient _mqttClient;

        public MessagingService(ILogger<MessagingService> logger)
        {
            _logger = logger;

            string messageQueueHost = Environment.GetEnvironmentVariable("MESSAGEQUEUE_HOST");
            if (string.IsNullOrEmpty(messageQueueHost))
                messageQueueHost = "127.0.0.1";

            string messageQueuePortText = Environment.GetEnvironmentVariable("MESSAGEQUEUE_PORT");
            if (string.IsNullOrEmpty(messageQueuePortText) || !int.TryParse(messageQueuePortText, out int messageQueuePort))
                messageQueuePort = 1883;

            _logger.LogInformation($"Connecting to MQTT on {messageQueueHost}:{messageQueuePort}");

            // Initiate connection to MQTT
            try
            {
                _initializationTask = Task.Run(async () =>
                {
                    _mqttClient = await MqttClient.CreateAsync(messageQueueHost, messageQueuePort);
                    await _mqttClient.ConnectAsync();
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Could not create MQTT client");
                throw;
            }

            _logger.LogInformation("Connection successful");
        }

        public async Task<IAsyncEnumerable<byte[]>> Subscribe(string topic)
        {
            await _initializationTask;

            await _mqttClient.SubscribeAsync(topic, MqttQualityOfService.ExactlyOnce);

            return _mqttClient.MessageStream
                .Where(m => m.Topic == topic)
                .Select(m => m.Payload)
                .ToAsyncEnumerable();
        }
    }
}
