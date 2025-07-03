using System;

namespace WebDesignPattern.Domain.PurchaseTransaction.Validators;

public class ItemsValidator : IOrderValidator
{
    private IOrderValidator _next;

    public IOrderValidator SetNext(IOrderValidator next) => _next = next;

    public ValidationResult Validate(Order order)
    {
        if (order.Items == null || order.Items.Count == 0)
            return new ValidationResult { IsValid = false, ErrorMessage = "Pedido sem itens." };

        return _next?.Validate(order) ?? new ValidationResult { IsValid = true };
    }
}