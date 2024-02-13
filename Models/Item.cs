﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ItemsAndOrdersManagementSystem.Models
{
    public class Item
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        [Column(TypeName = "decimal(7,4)")]
        public decimal Price { get; set; }
    }
}
