using System;

namespace WebDesignPattern.Domain.InventoryManagement;

public interface IProductRepository
{
    Product GetById(int productId);
    void DecreaseStock(int productId, int quantity);
    void IncreaseStock(int productId, int quantity);
}
