using System;

namespace WebDesignPattern.Domain.PurchaseTransaction.Validators;

public interface IOrderValidator
{
    IOrderValidator SetNext(IOrderValidator next);
    ValidationResult Validate(Order order);
}
