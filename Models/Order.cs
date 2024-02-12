using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;

namespace ItemsAndOrdersManagementSystem.Models
{
    public class Order
    {
        [Key]
        [Required]
        public int id { get; set; }

        [ForeignKey(nameof(ApplicationUser))]
        public string UserId { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }

        public ApplicationUser User { get; set; }
        public IEnumerable<Item> Items { get; set; }

    }
}
