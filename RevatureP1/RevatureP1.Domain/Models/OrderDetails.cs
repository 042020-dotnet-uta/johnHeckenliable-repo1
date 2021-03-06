﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RevatureP1.Models
{
    public class OrderDetails
    {
        [Key]
        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }

        [Key]
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Display(Name = "Price Paid")]
        [DataType(DataType.Currency)]
        public double PricePaid { get; set; }

        public OrderDetails() { }
    }
}