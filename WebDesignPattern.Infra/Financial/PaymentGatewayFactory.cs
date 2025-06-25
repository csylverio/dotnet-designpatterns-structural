using Microsoft.Extensions.DependencyInjection;
using WebDesignPattern.Domain.PurchaseTransaction.Financial;
using WebDesignPattern.Infra.Financial.PagSeguro;
using WebDesignPattern.Infra.Financial.PayPal;

namespace WebDesignPattern.Infra.Financial;

public class PaymentGatewayFactory : IPaymentGatewayFactory
{
    private readonly IServiceProvider _provider;

    public PaymentGatewayFactory(IServiceProvider provider)
    {
        _provider = provider;
    }

    public IPaymentGateway Create(int paymentMethodId)
    {
        IPaymentGateway paymentGateway = paymentMethodId switch
        {
            1 => new PagSeguroAdapter(_provider.GetRequiredService<PagSeguroService>()),
            2 => new PayPalAdapter(_provider.GetRequiredService<PayPalApi>()),
            _ => throw new InvalidOperationException("Provedor de pagamento inválido"),
        };
        return paymentGateway;
    }
}
