using RevatureP1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RevatureP1.Web.ViewModels
{
    public class CreateOrderViewModel
    {
        public Customer Customer { get; set; }

        public List<Store> Stores { get; set; }
    }
}
