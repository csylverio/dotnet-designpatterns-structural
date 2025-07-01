using System;
using WebDesignPattern.Domain.CustomerRelationshipManagement;
using WebDesignPattern.Domain.PurchaseTransaction.Discount;
using WebDesignPattern.Domain.PurchaseTransaction.Financial;
using WebDesignPattern.Domain.PurchaseTransaction.States;

namespace WebDesignPattern.Domain.PurchaseTransaction;

public class Order
{
    private IOrderState _state;

    public Order()
    {
        _state = new DraftState(); // Estado inicial
    }

    public Order(Customer customer, decimal discount, List<Item> items)
    {
        _state = new DraftState(); // Estado inicial

        Customer = customer;
        Discount = discount;
        Items = items;
    }

    public int Id { get; set; }
    public int CustomerId { get; set; }
    public Customer Customer { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.Now;
    // Relacionamento com itens (1 pedido → N itens)
    public List<Item> Items { get; set; } = new List<Item>();
    public decimal Discount { get; set; } = 0;
    // Relacionamento com pagamentos (1 Order → N Payments para casos de retentativa)
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    public DiscountDetail DiscountDetail { get; internal set; }
    public AccountingStatus AccountingStatus { get; set; }
    public string AccountingDocument { get; set; }
    public DateTime? AccountingDate { get; set; }
    public string AccountingMessage { get; set; }

    public decimal TotalAmount
    {
        //TODO: regras de calculo do desconto
        get { return Items.Sum(item => item.TotalPrice) * (Discount / 100); }
    }

    // -----------------------------------------------------
    // Exemplo de uso do padrão State
    // -----------------------------------------------------

    // Propriedade somente leitura (encapsulamento)
    public OrderStatus Status => _state.Status;

    public void SetState(IOrderState state) => _state = state;
    public void FinalizeOrder() => _state.Finalize(this); // AwaitingPayment
    public PaymentResult MakePayment(int paymentMethodId, IPaymentGatewayFactory gatewayFactory, IPaymentRepository paymentRepository) =>
        _state.Pay(this, paymentMethodId, gatewayFactory, paymentRepository); // PaymentApproved

    public void Ship() => _state.Ship(this); // Shipped
    public void Deliver() => _state.Deliver(this); // Delivered
    public void Cancel() => _state.Cancel(this); // Cancelled
}
