using System;

namespace WebDesignPattern.Domain.PurchaseTransaction.Financial;

public interface IPaymentGateway
{
    PaymentGatewayResponse ProcessPayment(decimal amount);
}
