using Microsoft.EntityFrameworkCore;
using Revaturep1.DataAccess;
using Revaturep1.Domain.Interfaces;
using RevatureP1.Models;
using System;
using Xunit;
using System.Linq;
using System.Collections.Generic;
using System.Reflection.Metadata;
using Revaturep1.DataAccess.Repositories;
using RevatureP1.DataAccess.Repositories;

namespace RevatureP1.Tests
{
    /*
    public class RepositoryTests
    {
        //private readonly IRepository<Order> orderRepo;
        //private readonly IRepository<Product> productRepo;
        //private readonly IRepository<Customer> customerRepo;
        //private readonly IRepository<Store> storeRepo;
        //private readonly IRepository<Inventory> inventoryRepo;

        //public StoreBackend_Tests(IRepository<Order> orderRepo,
        //    IRepository<Product> productRepo,
        //    IRepository<Customer> customerRepo,
        //    IRepository<Store> storeRepo,
        //    IRepository<Inventory> inventoryRepo)
        //{
        //    this.orderRepo = orderRepo;
        //    this.productRepo = productRepo;
        //    this.customerRepo = customerRepo;
        //    this.storeRepo = storeRepo;
        //    this.inventoryRepo = inventoryRepo;
        //}

        [Fact]
        public async void AddsCustomerToDb()
        {
            //Arrange
            var options = BuildInMemoryDb("AddsCustomer");

            string fName = "Bob", lName = "Dole", email = "bdole@email.com";
            var customerInfo = new Customer
            {
                Email = email,
                FirstName = fName,
                LastName = lName
            };

            //Act
            using (var context = new ShoppingDbContext(options))
            {
                var customerRepo = new CustomerRepository(context);
                customerInfo = await customerRepo.Add(customerInfo);
            }
            //Assert
            using (var context = new ShoppingDbContext(options))
            {
                var customer = (from c in context.Customers
                                where c.FirstName == fName && c.LastName == lName
                                select c).Take(1).FirstOrDefault();

                Assert.Equal(customer.CustomerId, customerInfo.CustomerId);
                Assert.Equal(fName, customer.FirstName);
                Assert.Equal(lName, customer.LastName);
                Assert.Equal(email, customer.Email);
            }
        }

        [Fact]
        public async void GetsCustomerById()
        {
            //Arrange
            var options = BuildInMemoryDb("GetsCustomer");
            string fName = "Bob", lName = "Dole", email = "bdole@email.com";
            int id = 1;

            //Act
            using (var context = new ShoppingDbContext(options))
            {
                var customer = new Customer
                {
                    CustomerId = id,
                    FirstName = fName,
                    LastName = lName,
                    Email = email
                };
                context.Add(customer);
                context.SaveChanges();
            }
            //Assert
            using (var context = new ShoppingDbContext(options))
            {
                var customerRepo = new CustomerRepository(context);
                var customerInfo = await customerRepo.Get(id);

                Assert.Equal(fName, customerInfo.FirstName);
                Assert.Equal(lName, customerInfo.LastName);
                Assert.Equal(email, customerInfo.Email);
            }
        }

        [Fact]
        public async void GetCustomerByEmail()
        {
            //Arrange
            var options = BuildInMemoryDb("GetsCustomerByEmail");
            string fName = "Bob", lName = "Dole", email = "bdole@email.com";
            int id = 1;

            //Act
            using (var context = new ShoppingDbContext(options))
            {
                var customer = new Customer
                {
                    CustomerId = id,
                    FirstName = fName,
                    LastName = lName,
                    Email = email
                };
                context.Add(customer);
                context.SaveChanges();
            }
            //Assert
            using (var context = new ShoppingDbContext(options))
            {
                var customerRepo = new CustomerRepository(context);
                var list = await customerRepo.Find(c => c.Email == email);
                var customerInfo = list.FirstOrDefault();

                Assert.Equal(id, customerInfo.CustomerId);
                Assert.Equal(fName, customerInfo.FirstName);
                Assert.Equal(lName, customerInfo.LastName);
                Assert.Equal(email, customerInfo.Email);
            }
        }

        [Fact]
        public async void GetsAllLocations()
        {
            //Arrange
            var options = BuildInMemoryDb("GetsAllLocations");

            //Act
            using (var context = new ShoppingDbContext(options))
            {
                var store = new Store
                {
                    Location = "Location1"
                };
                context.Add(store);
                store = new Store
                {
                    Location = "Location2"
                };
                context.Add(store);
                store = new Store
                {
                    Location = "Location3"
                };
                context.Add(store);
                context.SaveChanges();
            }
            //Assert
            using (var context = new ShoppingDbContext(options))
            {
                var storeRepo = new StoreRepository(context);
                var stores = await storeRepo.All();

                Assert.Equal(3, stores.Count());
            }
        }

        [Fact]
        public async void GetsAllProducts()
        {
            //Arrange
            var options = BuildInMemoryDb("GetsAllProducts");

            //Act
            using (var context = new ShoppingDbContext(options))
            {
                CreateTwoproducts(context);
            }
            //Assert
            using (var context = new ShoppingDbContext(options))
            {
                var productRepo = new ProductRepository(context);
                var products = await productRepo.All();

                Assert.Equal(2, products.Count());
            }
        }
        [Fact]
        public async void GetsAllCustomers()
        {
            //Arrange
            var options = BuildInMemoryDb("GetsAllCustomers");

            //Act
            using (var context = new ShoppingDbContext(options))
            {
                var customer = new Customer
                {
                    CustomerId = 1,
                    FirstName = "Jim",
                    LastName = "Bob",
                    Email = "jimmy@email.com"
                };
                context.Add(customer);
                customer = new Customer
                {
                    CustomerId = 2,
                    FirstName = "Jane",
                    LastName = "Doe",
                    Email = "jdoe@email.com"
                };
                context.Add(customer);

                context.SaveChanges();
            }
            //Assert
            using (var context = new ShoppingDbContext(options))
            {
                var customerRepo = new CustomerRepository(context);
                var customers = await customerRepo.All();

                Assert.Equal(2, customers.Count());
            }
        }

        [Fact]
        public async void GetsCorrectInventoryCount()
        {
            //Arrange
            var options = BuildInMemoryDb("GetsInventoryCount");

            //Act
            using (var context = new ShoppingDbContext(options))
            {
                CreateTwoproducts(context);

                var store = new Store
                {
                    StoreId = 1,
                    Location = "Location1",
                    AvailableProducts = new List<Inventory>
                    {
                        new Inventory
                        {
                            ProductId = 1,
                            StoreId = 1,
                            Quantity = 10
                        },
                        new Inventory
                        {
                            ProductId = 2,
                            StoreId = 1,
                            Quantity = 50
                        }
                    }
                };
                context.Add(store);
                context.SaveChanges();
            }
            //Assert
            using (var context = new ShoppingDbContext(options))
            {
                var inventoryRepo = new InventoryRepository(context);
                var items = await inventoryRepo.Find(i => i.StoreId == 1);

                Assert.Equal(2, items.Count());
            }
        }

        [Fact]
        public async void GetsAllOrdersForLocation()
        {
            //Arrange
            var options = BuildInMemoryDb("GetsLocationOrders");

            //Act
            using (var context = new ShoppingDbContext(options))
            {
                CreateOneCustomer(context);
                CreateTwoproducts(context);

                var store = new Store
                {
                    StoreId = 1,
                    Location = "Location1"
                };
                context.Add(store);
                context.SaveChanges();

                var order = new Order
                {
                    CusomerId = 1,
                    OrderDateTime = DateTime.Now,
                    OrderId = 1,
                    StoreId = 1,
                };
                context.Add(order);
                order = new Order
                {
                    CusomerId = 1,
                    OrderDateTime = DateTime.Now,
                    OrderId = 2,
                    StoreId = 1,
                };
                context.Add(order);
                context.SaveChanges();
            }
            //Assert
            using (var context = new ShoppingDbContext(options))
            {
                var orderRepo = new OrderRepository(context);
                var orders = await orderRepo.Find(o=>o.StoreId == 1);

                Assert.Equal(2, orders.Count());
            }
        }

        [Fact]
        public async void GetsAllOrdersForCustomer()
        {
            //Arrange
            var options = BuildInMemoryDb("GetsCustomersOrders");

            //Act
            using (var context = new ShoppingDbContext(options))
            {
                CreateOneCustomer(context);
                CreateTwoproducts(context);

                var store = new Store
                {
                    StoreId = 1,
                    Location = "Location1"
                };
                context.Add(store);
                context.SaveChanges();

                var order = new Order
                {
                    CusomerId = 1,
                    OrderDateTime = DateTime.Now,
                    OrderId = 1,
                    StoreId = 1,
                };
                context.Add(order);
                order = new Order
                {
                    CusomerId = 1,
                    OrderDateTime = DateTime.Now,
                    OrderId = 2,
                    StoreId = 1,
                };
                context.Add(order);
                context.SaveChanges();
            }
            //Assert
            using (var context = new ShoppingDbContext(options))
            {
                var orderRepo = new OrderRepository(context);
                var orders = await orderRepo.Find(o => o.CusomerId == 1);

                Assert.Equal(2, orders.Count());
            }
        }

        [Fact]
        public async void UpdatesCustomerInfo()
        {
            //Arrange
            var options = BuildInMemoryDb("UpdatesCustomeInfo");

            string fName = "Bob", lName = "Dole", email = "bdole@email.com";
            int id = 1;
            var customerInfo = new Customer
            {
                CustomerId = id,
                Email = email,
                FirstName = fName,
                LastName = lName
            };

            //Act
            using (var context = new ShoppingDbContext(options))
            {
                var customerRepo = new CustomerRepository(context);
                customerInfo = await customerRepo.Add(customerInfo);
            }
            //Assert
            using (var context = new ShoppingDbContext(options))
            {
                var customerRepo = new CustomerRepository(context);
                var customer = new Customer
                {
                    CustomerId = 1,
                    FirstName = "Jane",
                    LastName = "Doe",
                    Email = email
                };
                customerInfo = await customerRepo.Update(customer);

                Assert.Equal(id, customerInfo.CustomerId);
                Assert.Equal("Jane", customerInfo.FirstName);
                Assert.Equal("Doe", customerInfo.LastName);
                Assert.Equal(email, customerInfo.Email);
            }
        }


        [Fact]
        public void AddsOrderToDb()
        {
            //Arrange
            var options = BuildInMemoryDb("AddsOrderToDb");
            int orderId;

            //Act
            using (var context = new ShoppingDbContext(options))
            {
                CreateOneCustomer(context);
                CreateTwoproducts(context);

                var store = new Store
                {
                    StoreId = 1,
                    Location = "Location1",
                    AvailableProducts = new List<Inventory>
                    {
                        new Inventory
                        {
                            ProductId = 1,
                            StoreId = 1,
                            Quantity = 10
                        },
                        new Inventory
                        {
                            ProductId = 2,
                            StoreId = 1,
                            Quantity = 50
                        }
                    }
                };
                context.Add(store);
                context.SaveChanges();

                var unitOfWork = new UnitOfWork(context);

                var prods = new List<OrderDetails>()
                {
                    new OrderDetails()
                    {
                        ProductId = 1,
                        Quantity = 2,
                        PricePaid = 5.99
                    },
                    new OrderDetails()
                    {
                        ProductId=  2,
                        Quantity=  5,
                        PricePaid = 6.99
                    }
                };
                var order = new Order
                {
                    ProductsOrdered = prods,
                    CusomerId = 1,
                    StoreId = 1
                    
                };

                orderId = unitOfWork.OrderRepository.Add(order).Result.OrderId;
            }
            //Assert
            using (var context = new ShoppingDbContext(options))
            {
                var orders = from ord in context.Orders
                             where ord.OrderId == orderId
                             select ord;

                Assert.Single(orders);
            }
        }

        [Fact]
        public void DecrementsInventoryOnOrder()
        {
            //Arrange
            var options = BuildInMemoryDb("DecrementsInventory");
            int orderId;

            //Act
            using (var context = new ShoppingDbContext(options))
            {
                CreateOneCustomer(context);
                CreateTwoproducts(context);

                var store = new Store
                {
                    StoreId = 1,
                    Location = "Location1",
                    AvailableProducts = new List<Inventory>
                    {
                        new Inventory
                        {
                            ProductId = 1,
                            StoreId = 1,
                            Quantity = 10
                        },
                        new Inventory
                        {
                            ProductId = 2,
                            StoreId = 1,
                            Quantity = 50
                        }
                    }
                };
                context.Add(store);
                context.SaveChanges();

                var unitOfWork = new UnitOfWork(context);

                var prods = new List<OrderDetails>()
                {
                    new OrderDetails()
                    {
                        ProductId = 1,
                        Quantity = 2,
                        PricePaid = 5.99
                    }
                };
                var order = new Order
                {
                    ProductsOrdered = prods,
                    CusomerId = 1,
                    StoreId = 1

                };

                orderId = unitOfWork.OrderRepository.Add(order).Result.OrderId;
            }
            //Assert
            using (var context = new ShoppingDbContext(options))
            {
                var item = (from inv in context.StoreInventories
                            where inv.StoreId == 1 && inv.ProductId == 1
                            select inv).Take(1).FirstOrDefault();

                Assert.Equal(8, item.Quantity);
            }
        }

        [Fact]
        public void ThrowsOnNegativeInventory()
        {
            //Arrange
            var options = BuildInMemoryDb("ThrowsException");

            //Act
            using (var context = new ShoppingDbContext(options))
            {
                CreateOneCustomer(context);
                CreateTwoproducts(context);

                var store = new Store
                {
                    StoreId = 1,
                    Location = "Location1",
                    AvailableProducts = new List<Inventory>
                    {
                        new Inventory
                        {
                            ProductId = 1,
                            StoreId = 1,
                            Quantity = 10
                        }
                    }
                };
                context.Add(store);
                context.SaveChanges();
            }
            //Assert
            using (var context = new ShoppingDbContext(options))
            {
                var unitOfWork = new UnitOfWork(context);

                var prods = new List<OrderDetails>()
                {
                    new OrderDetails()
                    {
                        ProductId = 1,
                        Quantity = 12,
                        PricePaid = 5.99
                    }
                };
                var order = new Order
                {
                    ProductsOrdered = prods,
                    CusomerId = 1,
                    StoreId = 1

                };

                //orderId = unitOfWork.OrderRepository.Add(order).Result.OrderId;

                Assert.ThrowsAny<Exception>(() => unitOfWork.OrderRepository.Add(order).Result);
            }
        }

        [Fact]
        public void CancelsOrderOnNegativeInventory()
        {
            //Arrange
            var options = BuildInMemoryDb("CancelsOrder");

            //Act
            using (var context = new ShoppingDbContext(options))
            {
                CreateOneCustomer(context);
                CreateTwoproducts(context);

                var store = new Store
                {
                    StoreId = 1,
                    Location = "Location1",
                    AvailableProducts = new List<Inventory>
                    {
                        new Inventory
                        {
                            ProductId = 1,
                            StoreId = 1,
                            Quantity = 10
                        },
                        new Inventory
                        {
                            ProductId = 2,
                            StoreId = 1,
                            Quantity = 50
                        }
                    }
                };
                context.Add(store);
                context.SaveChanges();
                try
                {
                    var unitOfWork = new UnitOfWork(context);
                    var prods = new List<OrderDetails>()
                    {
                        new OrderDetails()
                        {
                            ProductId = 1,
                            Quantity = 12,
                            PricePaid = 5.99
                        }
                    };
                    var order = new Order
                    {
                        ProductsOrdered = prods,
                        CusomerId = 1,
                        StoreId = 1

                    };

                    unitOfWork.OrderRepository.Add(order);
                }
                catch { }
            }
            //Assert
            using (var context = new ShoppingDbContext(options))
            {
                var orders = (from o in context.Orders
                              select o).ToList();

                Assert.Empty(orders);
            }
        }

        private void CreateOneCustomer(ShoppingDbContext context)
        {
            var customer = new Customer
            {
                CustomerId = 1,
                FirstName = "Jim",
                LastName = "Bob",
                Email = "jimmy@email.com"
            };
            context.Add(customer);
            context.SaveChanges();
        }
        private void CreateTwoproducts(ShoppingDbContext context)
        {
            var product = new Product
            {
                PoductId = 1,
                Price = 2.99,
                ProductDescription = "Prod1"
            };
            context.Add(product);
            product = new Product
            {
                PoductId = 2,
                Price = 5.99,
                ProductDescription = "Prod2"
            };
            context.Add(product);
            context.SaveChanges();
        }

        private DbContextOptions<ShoppingDbContext> BuildInMemoryDb(string name)
        {
            return new DbContextOptionsBuilder<ShoppingDbContext>()
                .UseInMemoryDatabase(databaseName: name)
                .Options;
        }
    }
    */
}
