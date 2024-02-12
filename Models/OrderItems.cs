using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ItemsAndOrdersManagementSystem.Models
{
    public class OrderItems
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [ForeignKey(nameof(Order))]
        public int OrderId { get; set; }
        [ForeignKey(nameof(Item))]
        public int ItemId { get; set; }

        public Order Order { get; set; }
        public Item Item { get; set; }
    }
}
