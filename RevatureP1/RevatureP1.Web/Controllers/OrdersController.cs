
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RevatureP1.Models;
using Revaturep1.Domain.Interfaces;
using RevatureP1.Web.Models;
using RevatureP1.Web.Helpers;

namespace RevatureP1.Web.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IRepository<Order> orderRepo;
        private readonly IRepository<Product> productRepo;
        private readonly IRepository<Customer> customerRepo;
        private readonly IRepository<Store> storeRepo;
        private readonly IRepository<Inventory> inventoryRepo;

        public OrdersController(IRepository<Order> orderRepo,
            IRepository<Product> productRepo,
            IRepository<Customer> customerRepo,
            IRepository<Store> storeRepo,
            IRepository<Inventory> inventoryRepo)
        {
            this.orderRepo = orderRepo;
            this.productRepo = productRepo;
            this.customerRepo = customerRepo;
            this.storeRepo = storeRepo;
            this.inventoryRepo = inventoryRepo;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var orderViews = new List<OrderViewModel>();
            var orders = await orderRepo.All();
            foreach (var order in orders)
            {
                orderViews.Add(new OrderViewModel
                {
                    Order = order
                });
            }

            return View(orderViews);
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var order = await _context.Orders
            //    .Include(o => o.Customer)
            //    .Include(o => o.Store)
            //    .FirstOrDefaultAsync(m => m.OrderId == id);

            var order = await orderRepo.Get(id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public async Task<IActionResult> Create(int? storeId, int? SelectedProduct, int? SelectedQuantity)
        {
            var customer = SessionHelper.GetObjectFromJson<Customer>(HttpContext.Session, "Customer");
            
            if(customer == null)
            {
                return NotFound();
            }
            var createOrder = new CreateOrderViewModel
            {
                Customer = customer
            };
            
            if (storeId == null)
                storeId = SessionHelper.GetObjectFromJson<int?>(HttpContext.Session, "SelectedStore");
            else
                SessionHelper.SetObjectAsJson(HttpContext.Session, "SelectedStore", storeId);

            var stores = await storeRepo.All();
            createOrder.StoreLocations = new SelectList(stores, "StoreId", "Location");

            if (SelectedProduct != null)
                AddProductToOrder(SelectedProduct.Value, SelectedQuantity.Value);

            if (storeId != null)
            {
                var store = await storeRepo.Get(storeId);
                if (null == store)
                    return NotFound();
                createOrder.SelectedStore = store;

                var cart = SessionHelper.GetObjectFromJson<List<LineItemViewModel>>(HttpContext.Session, "cart");
                createOrder.SelectedProducts = cart == null ? new List<LineItemViewModel>() : cart;
            }

            return View(createOrder);
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind] CreateOrderViewModel order)
        {
            if (ModelState.IsValid)
            {
                //_context.Add(order);
                //await _context.SaveChangesAsync();
                //order = await orderRepo.Add(order);

                return RedirectToAction(nameof(Index));
            }
            //ViewData["CusomerId"] = new SelectList(customerRepo.All().Result, "CustomerId", "CustomerId", order.CusomerId);
            //ViewData["StoreId"] = new SelectList(storeRepo.All().Result, "StoreId", "StoreId", order.StoreId);
            return View(order);
        }

        private void AddProductToOrder(int SelectedProduct, int SelectedQuantity)
        {
            var product = productRepo.Get(SelectedProduct).Result;

            var cart = SessionHelper.GetObjectFromJson<List<LineItemViewModel>>(HttpContext.Session, "cart");

            if (cart == null)
            {
                cart = new List<LineItemViewModel>();

                cart.Add(new LineItemViewModel 
                {  
                    OrderDetails = new OrderDetails
                    {
                        ProductId = product.PoductId,
                        Product = product,
                        PricePaid = product.Price,
                        Quantity = SelectedQuantity
                    }
                   
                });
            }
            else
            {
                int index = isExist(SelectedProduct);
                if (index != -1)
                {
                    cart[index].OrderDetails.Quantity += SelectedQuantity;
                }
                else
                {
                    cart.Add(new LineItemViewModel
                    {
                        OrderDetails = new OrderDetails
                        {
                            ProductId = product.PoductId,
                            Product = product,
                            PricePaid = product.Price,
                            Quantity = SelectedQuantity
                        }

                    });
                }
            }
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);

            //return RedirectToAction("Create");
        }
        private int isExist(int id)
        {
            var cart = SessionHelper.GetObjectFromJson<List<LineItemViewModel>>(HttpContext.Session, "cart");

            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].OrderDetails.ProductId == id)
                {
                    return i;
                }
            }
            return -1;
        }
        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await orderRepo.Get(id);//_context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }
            ViewData["CusomerId"] = new SelectList(customerRepo.All().Result, "CustomerId", "CustomerId", order.CusomerId);
            ViewData["StoreId"] = new SelectList(storeRepo.All().Result, "StoreId", "StoreId", order.StoreId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,CusomerId,StoreId,OrderDateTime")] Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                    //_context.Update(order);
                    //await _context.SaveChangesAsync();
                    order = await orderRepo.Update(order);
                /*
                try
                {

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                */
                return RedirectToAction(nameof(Index));
            }
            ViewData["CusomerId"] = new SelectList(customerRepo.All().Result, "CustomerId", "CustomerId", order.CusomerId);
            ViewData["StoreId"] = new SelectList(storeRepo.All().Result, "StoreId", "StoreId", order.StoreId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var order = await _context.Orders
            //    .Include(o => o.Customer)
            //    .Include(o => o.Store)
            //    .FirstOrDefaultAsync(m => m.OrderId == id);
            var order = await orderRepo.Get(id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //var order = await _context.Orders.FindAsync(id);
            //_context.Orders.Remove(order);
            //await _context.SaveChangesAsync();
            await orderRepo.Delete(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
