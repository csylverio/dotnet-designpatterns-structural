using System;

namespace WebDesignPattern.Domain.PurchaseTransaction.Validators;

public class CustomerValidator : IOrderValidator
{
    private IOrderValidator _next;

    public IOrderValidator SetNext(IOrderValidator next) => _next = next;

    public ValidationResult Validate(Order order)
    {
        if (string.IsNullOrWhiteSpace(order.Customer?.Cpf))
            return new ValidationResult { IsValid = false, ErrorMessage = "CPF está vazio." };

        // Remove pontos e traço
        var digitsOnly = new string(order.Customer?.Cpf.Where(char.IsDigit).ToArray());

        if (digitsOnly.Length != 11)
            return new ValidationResult { IsValid = false, ErrorMessage = "CPF inválido (deve conter 11 dígitos)." };

        if (digitsOnly == "00000000000")
            return new ValidationResult { IsValid = false, ErrorMessage = "Cliente bloqueado por fraude." };


        return _next?.Validate(order) ?? new ValidationResult { IsValid = true };
    }
}