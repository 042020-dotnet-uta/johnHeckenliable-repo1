using Microsoft.AspNetCore.Mvc.Rendering;
using RevatureP1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace RevatureP1.Web.Models
{
    public class CreateOrderViewModel
    {
        public Customer Customer { get; set; }
        public int StoreId { get; set; }
        public SelectList StoreLocations { get; set; }

        public Store SelectedStore { get; set; }

        public int SelectedProduct { get; set; }
        public int SelectedQuantity { get; set; }
        public List<LineItemViewModel> SelectedProducts { get; set; }

        public double Total 
        {
            get
            {
                var total = 0.0;
                foreach (var item in SelectedProducts)
                {
                    total += item.Total;
                }
                return total;
            }
            set { }
        }

        public CreateOrderViewModel() { SelectedProducts = new List<LineItemViewModel>(); }
    }
}
