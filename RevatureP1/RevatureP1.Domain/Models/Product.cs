using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RevatureP1.Models
{
    public class Product
    {
        #region Properties
        [Key]
        public int PoductId { get; set; }

        public string ProductDescription { get; set; }

        public double Price { get; set; }

        #endregion

        #region Constructors
        public Product()
        { }
        #endregion

        #region Methods

        #endregion
    }
}
