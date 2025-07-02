using System;

namespace WebDesignPattern.Domain.PurchaseTransaction.Shippings;

public interface IShippingStrategy
{
    ShippingResult Ship(Order order);
    bool CanHandle(Order order); // Opcional: para seleção dinâmica
}
