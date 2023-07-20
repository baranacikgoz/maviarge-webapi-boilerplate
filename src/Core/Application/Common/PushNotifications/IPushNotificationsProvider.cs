namespace FSH.WebApi.Application.Common.PushNotifications;

public interface IPushNotificationsProvider
{
    Task SendTo(string userId, ICollection<Message> message);

    Task SendTo(string userId, string templateName);

    Task SendToAll(ICollection<Message> message);

    Task SendToAll(string templateName);

    Task SendToActiveUsers(ICollection<Message> message);

    Task SendToActiveUsers(string templateName);

    Task SendToInactiveUsers(ICollection<Message> message);

    Task SendToInactiveUsers(string templateName);
}