using System;

namespace WebDesignPattern.Domain.PurchaseTransaction.Shippings;

public class StorePickupStrategy : IShippingStrategy
{
    public ShippingResult Ship(Order order)
    {
        Console.WriteLine("Iniciando retirada na loja para o pedido...");
        // Simula o processo de retirada na loja
        // Aqui você pode adicionar lógica para verificar a disponibilidade do produto na loja, etc.
        return new ShippingResult
        {
            TrackingNumber = $"PICKUP-{order.Id}",
            Carrier = "Loja Física",
            EstimatedDelivery = DateTime.Now.AddHours(2),
            ShippingCost = 0m,
            Notes = "Pedido disponível para retirada em 2h"
        };
    }

    public bool CanHandle(Order order) => order.ShippingMethodId == 3;
}
