using RevatureP1.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RevatureP1.Web.Models
{
    public class CartViewModel
    {
        public Customer Customer { get; set; }

        [Display(Name = "Selected Store")]
        public Store SelectedStore { get; set; }

        [Display(Name = "Included Products")]
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
    }
}
