using System;

namespace WebDesignPattern.Domain.PurchaseTransaction.Validators;

public class ValidationResult
{
    public bool IsValid { get; set; }
    public string ErrorMessage { get; set; }
}
