using System;

namespace WebDesignPattern.Domain.PurchaseTransaction.Shippings;

public class StandardShippingStrategy : IShippingStrategy
{
    public ShippingResult Ship(Order order)
    {
        Console.WriteLine("Iniciando envio padrão (PAC) para o pedido...");
        // Simula o processo de envio padrão
        // Aqui você pode adicionar lógica para calcular o custo, estimar a entrega, etc.
        return new ShippingResult
        {
            TrackingNumber = $"STD-{Guid.NewGuid().ToString().Substring(0, 8)}",
            Carrier = "Correios (PAC)",
            EstimatedDelivery = DateTime.Now.AddDays(5),
            ShippingCost = 15.90m,
            Notes = "Envio padrão: 3-5 dias úteis"
        };
    }

    public bool CanHandle(Order order)
        => order.ShippingMethodId == 1; // ID do método padrão
}
