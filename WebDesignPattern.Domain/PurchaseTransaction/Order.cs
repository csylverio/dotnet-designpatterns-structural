using System;
using WebDesignPattern.Domain.CustomerRelationshipManagement;
using WebDesignPattern.Domain.PurchaseTransaction.Discount;
using WebDesignPattern.Domain.PurchaseTransaction.Financial;

namespace WebDesignPattern.Domain.PurchaseTransaction;

public class Order
{
    public Order()
    {
    }

    public Order(Customer customer, decimal discount, List<Item> items)
    {
        Customer = customer;
        Discount = discount;
        Items = items;
    }

    public int Id { get; set; }

    public int CustomerId { get; set; }
    public Customer Customer { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.Now;
    public OrderStatus Status { get; set; } = OrderStatus.Draft;

    // Relacionamento com itens (1 pedido → N itens)
    public List<Item> Items { get; set; } = new List<Item>();

    public decimal Discount { get; set; } = 0;

    public decimal TotalAmount
    {
        //TODO: regras de calculo do desconto
        get { return Items.Sum(item => item.TotalPrice) * (Discount / 100); }
    }

    // Relacionamento com pagamentos (1 Order → N Payments para casos de retentativa)
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    public DiscountDetail DiscountDetail { get; internal set; }
}
