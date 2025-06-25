using System;
using WebDesignPattern.Domain.CustomerRelationshipManagement;

namespace WebDesignPattern.Infra.Data;

public class CustomerRepository : ICustomerRepository
{
    public Customer GetById(int customerId)
    {
        Console.WriteLine("CustomerRepository.GetById");
        return new Customer()
        {
            Id = customerId
        };
    }
}
