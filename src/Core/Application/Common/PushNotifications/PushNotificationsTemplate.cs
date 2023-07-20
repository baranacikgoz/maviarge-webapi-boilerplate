namespace FSH.WebApi.Application.Common.PushNotifications;

public sealed record PushNotificationsTemplate(string Name, ICollection<Message> Messages);
public sealed record Message(string Language, string Heading, string Content);