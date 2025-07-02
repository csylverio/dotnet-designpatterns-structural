using System;
using WebDesignPattern.Domain.PurchaseTransaction;
using WebDesignPattern.Domain.PurchaseTransaction.Observers;

namespace WebDesignPattern.Domain.Notification;

public class EmailNotificationService : IOrderObserver
{
    private readonly IEmailService _emailService;

    public EmailNotificationService(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public void OnOrderUpdated(Order order, OrderEventType eventType)
    {
        string? subject = eventType switch
        {
            OrderEventType.PaymentApproved => $"Pagamento aprovado - Pedido #{order.Id}",
            OrderEventType.OrderShipped => $"Seu pedido #{order.Id} foi enviado",
            OrderEventType.OrderDelivered => $"Pedido #{order.Id} entregue com sucesso!",
            OrderEventType.OrderCreated => $"Pedido #{order.Id} criado com sucesso",
            OrderEventType.PaymentFailed => $"Falha no pagamento do pedido #{order.Id}",
            OrderEventType.OrderCancelled => $"Pedido #{order.Id} cancelado",
            _ => throw new NotImplementedException($"Evento de notificação não implementado: {eventType}")
        };

        if (subject != null)
        {
            string body = $"Olá {order.Customer.Name},\n\nStatus do pedido: {subject}";
            _emailService.SendEmail(order.Customer.Email, subject, body);
        }
    }
}
