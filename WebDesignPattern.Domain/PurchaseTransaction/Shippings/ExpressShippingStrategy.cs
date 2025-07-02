using System;

namespace WebDesignPattern.Domain.PurchaseTransaction.Shippings;

public class ExpressShippingStrategy : IShippingStrategy
{
    public ShippingResult Ship(Order order)
    {
        Console.WriteLine("Iniciando envio expresso (FedEx) para o pedido...");
        // Simula o processo de envio expresso  
        // Aqui você pode adicionar lógica para calcular o custo, estimar a entrega, etc.
        // Por exemplo, você pode usar uma API de envio para obter informações reais de rastreamento e custo.
        return new ShippingResult
        {
            TrackingNumber = $"EXP-{DateTime.Now:yyyyMMddHHmmss}",
            Carrier = "FedEx",
            EstimatedDelivery = DateTime.Now.AddDays(1),
            ShippingCost = 49.90m,
            Notes = "Entrega expressa em 24h"
        };
    }

    public bool CanHandle(Order order) => order.ShippingMethodId == 2;
}
