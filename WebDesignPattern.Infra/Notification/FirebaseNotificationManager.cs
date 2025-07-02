using System;
using WebDesignPattern.Domain.Notification;

namespace WebDesignPattern.Infra.Notification;

public class FirebaseNotificationManager : INotificationManager
{
    public void SendPushNotification(int deviceToken, string message)
    {
        var notification = new
        {
            Title = "Atualização do Pedido",
            Body = message
        };
        // Aqui você integraria com o Firebase Cloud Messaging (FCM) para enviar a notificação
        // Exemplo fictício de envio de notificação
        Console.WriteLine($"Notificação enviada para o dispositivo {deviceToken}: {notification.Title} - {notification.Body}");
        // Em um cenário real, você usaria a biblioteca Firebase Admin SDK para enviar a notificação
    }
}
