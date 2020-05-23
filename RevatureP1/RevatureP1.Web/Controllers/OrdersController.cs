
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RevatureP1.Models;
using Revaturep1.Domain.Interfaces;
using RevatureP1.Web.Models;
using RevatureP1.Web.Helpers;
using RevatureP1.Domain.Interfaces;
using System;
using Microsoft.Extensions.Logging;

namespace RevatureP1.Web.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<OrdersController> _logger;
        public OrdersController(IUnitOfWork unitOfWork,
            ILogger<OrdersController> logger)
        {
            this._unitOfWork = unitOfWork;
            this._logger = logger;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var orderViews = new List<OrderViewModel>();
            var orders = await _unitOfWork.OrderRepository.All();
            foreach (var order in orders)
            {
                orderViews.Add(new OrderViewModel
                {
                    Order = order
                });
            }

            return View(orderViews);
        }
                
        public async Task<IActionResult> OrderDetails(OrderDetailsViewModel model)
        {
            if (model == null)
            {
                _logger.LogDebug("The model was sent in null in Orders//OrderDetails");
                return NotFound();
            }
            var order = await _unitOfWork.OrderRepository.Get(model.OrderId);

            if (order == null)
            {
                _logger.LogDebug("Unable to find the order for id # {0} in Orders//OrderDetails", model.OrderId);
                return NotFound();
            }
            model = new OrderDetailsViewModel
            {
                OrderId = order.OrderId,
                Customer = order.Customer,
                Store = order.Store,
                OrderDateTime = order.OrderDateTime
            };
            foreach (var item in order.ProductsOrdered)
            {
                model.LineItems.Add(
                    new LineItemViewModel
                    {
                        OrderDetails = item
                    });
            }

            return View(model);
        }

        // GET: Orders/Create
        public async Task<IActionResult> Create(int? storeId, int? SelectedProduct, int? SelectedQuantity)
        {
            var customer = SessionHelper.GetObjectFromJson<Customer>(HttpContext.Session, "Customer");
            
            if(customer == null)
            {
                _logger.LogDebug("Unable to get a customer from the session in Orders//Create");
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

            var stores = await _unitOfWork.StoreRepository.All();
            createOrder.StoreLocations = new SelectList(stores, "StoreId", "Location");

            if (SelectedProduct != null)
                AddProductToOrder(SelectedProduct.Value, SelectedQuantity.Value);

            if (storeId != null)
            {
                var store = await _unitOfWork.StoreRepository.Get(storeId);
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
        public IActionResult Create()
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var newOrder = CreateOrder();

                    var model = new OrderDetailsViewModel
                    {
                        OrderId = newOrder.OrderId
                    };
                    //Clear the session information
                    ClearSessionInfo();

                    return RedirectToAction(nameof(OrderDetails), model);
                }
                catch (ArgumentOutOfRangeException auex)
                {
                    _logger.LogError(auex, "Unable to complete the order.");
                    return RedirectToAction(nameof(Create));
                }
            }
            _logger.LogDebug("Model was invalid in Orders//Create(Post)");
            return RedirectToAction(nameof(Create));
        }

        private Order CreateOrder()
        {
            var customer = SessionHelper.GetObjectFromJson<Customer>(HttpContext.Session, "Customer");
            var storeId = SessionHelper.GetObjectFromJson<int?>(HttpContext.Session, "SelectedStore");
            var cart = SessionHelper.GetObjectFromJson<List<LineItemViewModel>>(HttpContext.Session, "cart");
            List<OrderDetails> details = new List<OrderDetails>();
            foreach (var item in cart)
            {
                item.OrderDetails.Product = null;
                details.Add(item.OrderDetails);
            }

            var order = new Order
            {
                CusomerId = customer.CustomerId,
                Customer = null,
                StoreId = storeId.Value,
                Store = null,//_unitOfWork.StoreRepository.Get(storeId).Result,
                ProductsOrdered = details,
                OrderDateTime = DateTime.Now
            };

            order = _unitOfWork.OrderRepository.Add(order).Result;

            return order;
        }

        private void AddProductToOrder(int SelectedProduct, int SelectedQuantity)
        {
            var product = _unitOfWork.ProductRepository.Get(SelectedProduct).Result;

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

        private void ClearSessionInfo()
        {
            SessionHelper.SetObjectAsJson(HttpContext.Session, "SelectedStore", null);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", null);
        }

        public IActionResult Cancel()
        {
            //Clear the session information
            SessionHelper.SetObjectAsJson(HttpContext.Session, "SelectedStore", null);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", null);

            _logger.LogInformation("Order Canceled");

            return RedirectToAction(nameof(Create));
        }

        // GET: Orders/Edit/5
    //    public async Task<IActionResult> Edit(int? id)
    //    {
    //        if (id == null)
    //        {
    //            return NotFound();
    //        }

    //        var order = await _unitOfWork.OrderRepository.Get(id);//_context.Orders.FindAsync(id);

    //        if (order == null)
    //        {
    //            return NotFound();
    //        }
    //        //ViewData["CusomerId"] = new SelectList(customerRepo.All().Result, "CustomerId", "CustomerId", order.CusomerId);
    //        //ViewData["StoreId"] = new SelectList(storeRepo.All().Result, "StoreId", "StoreId", order.StoreId);
    //        return View(order);
    //    }

    //    // POST: Orders/Edit/5
    //    // To protect from overposting attacks, enable the specific properties you want to bind to, for 
    //    // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> Edit(int id, [Bind("OrderId,CusomerId,StoreId,OrderDateTime")] Order order)
    //    {
    //        if (id != order.OrderId)
    //        {
    //            return NotFound();
    //        }

    //        if (ModelState.IsValid)
    //        {
    //                //_context.Update(order);
    //                //await _context.SaveChangesAsync();
    //                order = await _unitOfWork.OrderRepository.Update(order);
    //            /*
    //            try
    //            {

    //            }
    //            catch (DbUpdateConcurrencyException)
    //            {
    //                if (!OrderExists(order.OrderId))
    //                {
    //                    return NotFound();
    //                }
    //                else
    //                {
    //                    throw;
    //                }
    //            }
    //            */
    //            return RedirectToAction(nameof(Index));
    //        }
    //        //ViewData["CusomerId"] = new SelectList(customerRepo.All().Result, "CustomerId", "CustomerId", order.CusomerId);
    //        //ViewData["StoreId"] = new SelectList(storeRepo.All().Result, "StoreId", "StoreId", order.StoreId);
    //        return View(order);
    //    }

    //    // GET: Orders/Delete/5
    //    public async Task<IActionResult> Delete(int? id)
    //    {
    //        if (id == null)
    //        {
    //            return NotFound();
    //        }

    //        //var order = await _context.Orders
    //        //    .Include(o => o.Customer)
    //        //    .Include(o => o.Store)
    //        //    .FirstOrDefaultAsync(m => m.OrderId == id);
    //        var order = await _unitOfWork.OrderRepository.Get(id);

    //        if (order == null)
    //        {
    //            return NotFound();
    //        }

    //        return View(order);
    //    }

    //    // POST: Orders/Delete/5
    //    [HttpPost, ActionName("Delete")]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> DeleteConfirmed(int id)
    //    {
    //        //var order = await _context.Orders.FindAsync(id);
    //        //_context.Orders.Remove(order);
    //        //await _context.SaveChangesAsync();
    //        await _unitOfWork.OrderRepository.Delete(id);

    //        return RedirectToAction(nameof(Index));
    //    }
    }
}
