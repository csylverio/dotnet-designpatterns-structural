using System;
using WebDesignPattern.Domain.PurchaseTransaction;
using WebDesignPattern.Domain.PurchaseTransaction.Observers;

namespace WebDesignPattern.Domain.InventoryManagement;

public class InventoryManagementService : IOrderObserver
{
    private readonly IProductRepository _productRepository;

    public InventoryManagementService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public void OnOrderUpdated(Order order, OrderEventType eventType)
    {
        if (eventType == OrderEventType.PaymentApproved)
        {
            foreach (var item in order.Items)
            {
                _productRepository.DecreaseStock(item.Product.Id, item.Quantity);
            }
        }
        else if (eventType == OrderEventType.OrderCancelled)
        {
            foreach (var item in order.Items)
            {
                _productRepository.IncreaseStock(item.Product.Id, item.Quantity);
            }
        }
    }
}