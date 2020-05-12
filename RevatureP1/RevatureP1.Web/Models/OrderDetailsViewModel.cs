using RevatureP1.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RevatureP1.Web.Models
{
    public class OrderDetailsViewModel
    {
        [Display(Name = "Order Number")]
        public int OrderId { get; set; }

        [Display(Name = "Order Date")]
        [DataType(DataType.Date)]
        public DateTime OrderDateTime { get; set; }

        public Customer Customer { get; set; }
        public Store Store { get; set; }

        [Display(Name = "Total Items")]
        public int TotalItems
        {
            get
            {
                var total = 0;
                foreach (var lineItem in LineItems)
                {
                    total += (lineItem.OrderDetails.Quantity);
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
                foreach (var lineItem in LineItems)
                {
                    total += (lineItem.Total);
                }

                return total;
            }

            set { }
        }

        public List<LineItemViewModel> LineItems { get; set; }

        public OrderDetailsViewModel() { LineItems = new List<LineItemViewModel>(); }
    }
}
