using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RevatureP1.Models
{
    public class Store
    {
        #region Properties
        [Key]
        public int StoreId { get; set; }

        [Required, Display(Name = "Location Name")]
        public string Location { get; set; }

        public virtual List<Inventory> AvailableProducts { get; set; }
        #endregion

        #region Constructors
        public Store()
        { AvailableProducts = new List<Inventory>(); }
        #endregion

        #region Methods
        #endregion
    }
}
