using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RevatureP1.Models;
using Revaturep1.DataAccess;
using Revaturep1.Domain.Interfaces;
using RevatureP1.Web.Models;
using RevatureP1.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace RevatureP1.Web.Controllers
{
    public class StoresController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<StoresController> _logger;

        public StoresController(IUnitOfWork unitOfWork,
            ILogger<StoresController> logger)
        {
            this._unitOfWork = unitOfWork;
            this._logger = logger;
        }
        // GET: Stores
        public async Task<IActionResult> Index()
        {
            return View(await _unitOfWork.StoreRepository.All());
        }

        // GET: Stores/Details/5
        public async Task<IActionResult> AvailableProducts(int? id)
        {
            if (id == null)
            {
                _logger.LogDebug("ID is null in Stores//AvailableProducts");
                return NotFound();
            }

            var store = await _unitOfWork.StoreRepository.Get(id);
            if (store == null)
            {
                _logger.LogDebug("Unable to find a store matching id# {0} in Stores//AvailableProducts", id);
                return NotFound();
            }

            return View(store);
        }

        public async Task<IActionResult> OrderHistory(int? id)
        {
            if (id == null)
            {
                _logger.LogDebug("ID is null in Stores//OrderHistory");
                return NotFound();
            }
            var orders = await _unitOfWork.OrderRepository.Find(o => o.StoreId == id);
            if (orders == null)
            {
                _logger.LogDebug("Unable to find an order matching id# {0} in Stores//OrderHistory", id);
                return NotFound();
            }
            var orderViews = new List<OrderViewModel>();
            foreach (var order in orders)
            {
                orderViews.Add(new OrderViewModel
                {
                    Order = order
                });
            }

            return View(orderViews);
        }
        public async Task<IActionResult> OrderDetails(int? id)
        {
            if (id == null)
            {
                _logger.LogDebug("ID is null in Stores//OrderDetails");
                return NotFound();
            }
            var order = await _unitOfWork.OrderRepository.Get(id);

            if (order == null)
            {
                _logger.LogDebug("Unable to find an order matching id# {0} in Stores//OrderDetails", id);
                return NotFound();
            }
            var orderDetails = new OrderDetailsViewModel
            {
                OrderId = order.OrderId,
                Customer = order.Customer,
                Store = order.Store,
                OrderDateTime = order.OrderDateTime
            };
            foreach (var item in order.ProductsOrdered)
            {
                orderDetails.LineItems.Add(
                    new LineItemViewModel
                    {
                        OrderDetails = item
                    });
            }

            return View(orderDetails);
        }
    }
}
