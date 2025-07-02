using System;
using WebDesignPattern.Domain.PurchaseTransaction;
using WebDesignPattern.Domain.PurchaseTransaction.Observers;

namespace WebDesignPattern.Domain.Notification;

public class MobilePushNotificationService : IOrderObserver
{
    private readonly INotificationManager _notificationManager;

    public MobilePushNotificationService(INotificationManager notificationManager)
    {
        _notificationManager = notificationManager;
    }

    public void OnOrderUpdated(Order order, OrderEventType eventType)
    {
        string? message = eventType switch
        {
            OrderEventType.OrderShipped => $"Seu pedido #{order.Id} foi enviado. Rastreio: {order.ShippingResult.TrackingNumber}",
            OrderEventType.OrderDelivered => $"Pedido #{order.Id} entregue! Avalie sua experiência.",
            OrderEventType.PaymentApproved => $"Pagamento do pedido #{order.Id} aprovado!",
            OrderEventType.OrderCreated => $"Pedido #{order.Id} criado com sucesso! Confira os detalhes no app.",
            OrderEventType.PaymentFailed => $"Falha no pagamento do pedido #{order.Id}. Por favor, verifique suas informações de pagamento.",
            OrderEventType.OrderCancelled => $"Pedido #{order.Id} cancelado. Se precisar de ajuda, entre em contato com o suporte.",
            _ => throw new NotImplementedException()
        };

        if (message != null)
        {
            _notificationManager.SendPushNotification(order.Customer.Id, message);
        }
    }
}
