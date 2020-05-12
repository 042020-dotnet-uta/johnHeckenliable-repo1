using RevatureP1.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RevatureP1.Web.Models
{
    public class LineItemViewModel
    {
        public OrderDetails OrderDetails { get; set; }

        [DataType(DataType.Currency)]
        public double Total
        { 
            get
            { return OrderDetails.PricePaid * OrderDetails.Quantity; }
            set { }
        }
    }
}
