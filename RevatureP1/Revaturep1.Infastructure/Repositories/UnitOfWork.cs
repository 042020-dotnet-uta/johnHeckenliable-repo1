using Revaturep1.DataAccess;
using Revaturep1.DataAccess.Repositories;
using Revaturep1.Domain.Interfaces;
using RevatureP1.Domain.Interfaces;
using RevatureP1.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RevatureP1.DataAccess.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private ShoppingDbContext _context;

        public UnitOfWork(ShoppingDbContext context)
        {
            this._context = context;
        }

        private IRepository<Order> orderRepository;
        public IRepository<Order> OrderRepository
        {
            get
            {
                if (orderRepository == null)
                    orderRepository = new OrderRepository(_context);

                return orderRepository;
            }
        }

        private IRepository<Product> productRepository;
        public IRepository<Product> ProductRepository
        {
            get
            {
                if (productRepository == null)
                    productRepository = new ProductRepository(_context);

                return productRepository;
            }
        }

        private IRepository<Customer> customerRepository;
        public IRepository<Customer> CustomerRepository
        {
            get
            {
                if (customerRepository == null)
                    customerRepository = new CustomerRepository(_context);

                return customerRepository;
            }
        }

        private IRepository<Store> storeRepository;
        public IRepository<Store> StoreRepository
        {
            get
            {
                if (storeRepository == null)
                    storeRepository = new StoreRepository(_context);

                return storeRepository;
            }
        }

        private IRepository<Inventory> inventoryRepository;
        public IRepository<Inventory> InventoryRepository
        {
            get
            {
                if (inventoryRepository == null)
                    inventoryRepository = new InventoryRepository(_context);

                return inventoryRepository;
            }
        }
    }
}
