using Microsoft.AspNetCore.Mvc.Rendering;
using RevatureP1.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace RevatureP1.Web.Models
{
    public class CreateOrderViewModel
    {
        public Customer Customer { get; set; }
        [Display(Name ="Store Number")]
        public int StoreId { get; set; }
        [Display(Name ="Store Locations")]
        public SelectList StoreLocations { get; set; }

        [Display(Name = "Selected Store")]
        public Store SelectedStore { get; set; }

        [Display(Name = "Selected Product")]
        public int SelectedProduct { get; set; }

        [Display(Name = "Selected Quantity")]
        public int SelectedQuantity { get; set; }

        [Display(Name = "Selected Products")]
        public List<LineItemViewModel> SelectedProducts { get; set; }

        public double Total 
        {
            get
            {
                var total = 0.0;
                if (SelectedProducts != null)
                {
                    foreach (var item in SelectedProducts)
                    {
                        total += item.Total;
                    }
                }
                return total;
            }
            set { }
        }

        public CreateOrderViewModel() { SelectedProducts = new List<LineItemViewModel>(); }
    }
}
