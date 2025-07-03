using System;

namespace WebDesignPattern.Domain.PurchaseTransaction.Validators;

public class ShippingMethodValidator : IOrderValidator
{
    private IOrderValidator _next;

    public IOrderValidator SetNext(IOrderValidator next) => _next = next;

    public ValidationResult Validate(Order order)
    {
        if (order.ShippingMethodId <= 0)
            return new ValidationResult { IsValid = false, ErrorMessage = "Método de envio não selecionado." };

        if ( order.ShippingMethodId > 3 )
            return new ValidationResult { IsValid = false, ErrorMessage = "Método de envio inválido." };

        return _next?.Validate(order) ?? new ValidationResult { IsValid = true };
    }
}
