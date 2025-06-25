using System;

namespace WebDesignPattern.Domain.CustomerRelationshipManagement;

public interface ICustomerRepository
{
    Customer GetById(int customerId);
}
