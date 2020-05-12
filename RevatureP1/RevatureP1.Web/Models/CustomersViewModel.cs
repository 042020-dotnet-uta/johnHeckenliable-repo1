using Microsoft.AspNetCore.Mvc.Rendering;
using RevatureP1.Domain;
using RevatureP1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RevatureP1.Web.Models
{
    public class CustomersViewModel
    {
        public List<Customer> Customers { get; set; }

        public SelectList SearchTypeList { get; set; }

        public int SearchType { get; set; }
        public string SearchString { get; set; }
    }
}
