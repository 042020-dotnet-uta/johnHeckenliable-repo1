using Revaturep1.Domain.Interfaces;
using RevatureP1.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RevatureP1.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<Order> OrderRepository { get; }
        IRepository<Product> ProductRepository { get; }
        IRepository<Customer> CustomerRepository { get; }
        IRepository<Store> StoreRepository { get; }
        IRepository<Inventory> InventoryRepository { get; }
    }
}
