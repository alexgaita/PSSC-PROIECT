
using Domain.Services;


using Azure.Messaging.ServiceBus;
using CloudNative.CloudEvents;
using CloudNative.CloudEvents.SystemTextJson;
using System.Collections.Concurrent;
using System.Net.Mime;

namespace TakeCommand.Events

{
    public class ServiceBusTopicEventSender : IEventSender, IAsyncDisposable
    {
        private readonly ServiceBusClient client;
        private readonly ConcurrentDictionary<string, ServiceBusSender> senders;
        private readonly JsonEventFormatter jsonEventFormatter = new JsonEventFormatter();

        public ServiceBusTopicEventSender(ServiceBusClient client)
        {
            this.client = client;
            senders = new ConcurrentDictionary<string,ServiceBusSender>();
        }

        public async Task SendAsync<T>(string topicName, T @event)
        {
            var sender = GetOrCreateSender(topicName);
            CloudEvent cloudEvent = CreateCloudEvent<T>(topicName, @event);
            var encodedCloudEvent = jsonEventFormatter.EncodeStructuredModeMessage(cloudEvent, out var contentType);
            ServiceBusMessage message = new(encodedCloudEvent);

            await sender.SendMessageAsync(message);
        }

        private ServiceBusSender GetOrCreateSender(string topicName) =>
            senders.GetOrAdd(topicName, topic => client.CreateSender(topic));

        private static CloudEvent CreateCloudEvent<T>(string topicName, T eventPayload)
        {
            CloudEvent cloudEvent = new();
            cloudEvent.Id = Guid.NewGuid().ToString();
            cloudEvent.DataContentType = MediaTypeNames.Application.Json;
            cloudEvent.Data = eventPayload;
            cloudEvent.Time = DateTimeOffset.Now;
            cloudEvent.Type = typeof(T).Name;
            cloudEvent.Subject = topicName;
            cloudEvent.Source = new("https://www.upt.ro/");
            return cloudEvent;
        }

        public async ValueTask DisposeAsync()
        {
            foreach(var sender in senders.Values)
            {
                await sender.DisposeAsync();
            }
            senders.Clear();
        }
    }
}
