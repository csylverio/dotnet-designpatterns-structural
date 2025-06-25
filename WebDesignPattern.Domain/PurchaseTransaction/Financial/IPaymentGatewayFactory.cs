using System;

namespace WebDesignPattern.Domain.PurchaseTransaction.Financial;

public interface IPaymentGatewayFactory
{
    IPaymentGateway Create(int paymentMethodId);
}
