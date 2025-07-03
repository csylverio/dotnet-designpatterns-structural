using System;

namespace WebDesignPattern.Domain.PurchaseTransaction.Validators;

public class OrderValidationChain
{
    public static IOrderValidator BuildChain()
    {
        var itemsValidator = new ItemsValidator();
        var shippingValidator = new ShippingMethodValidator();
        var customerValidator = new CustomerValidator();

        // Montagem da cadeia
        itemsValidator
            .SetNext(shippingValidator)
            .SetNext(customerValidator);

        return itemsValidator; // Retorna o primeiro link da cadeia
    }
}
