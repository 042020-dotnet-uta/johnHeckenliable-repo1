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

        [Required, Display(Name = "Product Name")]
        public string ProductDescription { get; set; }

        [Required]
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
