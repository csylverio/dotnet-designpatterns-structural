using System;

namespace WebDesignPattern.Domain.InventoryManagement;

public interface IProductRepository
{
    Product GetById(int productId);
}
