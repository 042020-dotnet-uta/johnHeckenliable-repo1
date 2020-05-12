using RevatureP1.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RevatureP1.Web.Models
{
    public class OrderViewModel
    {
        public Order Order { get; set; }

        [Display(Name = "Total Items")]
        public int TotalItems 
        { 
            get
            {
                var total = 0;
                foreach (var lineItem in Order.ProductsOrdered)
                {
                    total += (lineItem.Quantity);
                }

                return total;
            }
            set { }
        }

        [DataType(DataType.Currency)]
        [Display(Name = "Total Sale")]
        public double TotalSale 
        {
            get 
            {
                var total = 0.0;
                foreach (var lineItem in Order.ProductsOrdered)
                {
                    total += (lineItem.PricePaid * lineItem.Quantity);
                }

                return total;
            }

            set { } 
        }
    }
}
