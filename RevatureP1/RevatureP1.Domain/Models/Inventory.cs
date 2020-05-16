using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RevatureP1.Models
{
    public class Inventory
    {
        [Key]
        [ForeignKey("Store")]
        public int StoreId { get; set; }
        public Store Store { get; set; }

        [Key]
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        [Required]
        public int Quantity { get; set; }

        public Inventory() { }
    }
}