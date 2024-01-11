namespace Domain.Services;

public interface IEventSender
{
    Task SendAsync<T>(string topicName, T @event);
}