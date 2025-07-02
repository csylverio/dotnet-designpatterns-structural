using System;

namespace WebDesignPattern.Domain.Notification;

public interface INotificationManager
{
    void SendPushNotification(int userId, string message);
}
