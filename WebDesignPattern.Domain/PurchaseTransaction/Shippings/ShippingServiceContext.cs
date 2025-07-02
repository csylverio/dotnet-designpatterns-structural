using System;

namespace WebDesignPattern.Domain.PurchaseTransaction.Shippings;

public class ShippingServiceContext
{
    private readonly List<IShippingStrategy> _strategies;

    public ShippingServiceContext(IEnumerable<IShippingStrategy> strategies)
    {
       _strategies = strategies.ToList();
    }

    public ShippingResult ProcessShipping(Order order)
    {
        var strategy = _strategies.FirstOrDefault(s => s.CanHandle(order))
                      ?? throw new InvalidOperationException("Método de envio não suportado");

        return strategy.Ship(order);
    }
}
