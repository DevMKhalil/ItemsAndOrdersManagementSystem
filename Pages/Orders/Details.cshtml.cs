using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ItemsAndOrdersManagementSystem.Data;
using ItemsAndOrdersManagementSystem.Models;

namespace ItemsAndOrdersManagementSystem.Pages.Orders
{
    public class DetailsModel : PageModelBase
    {
        private readonly ItemsAndOrdersManagementSystem.Data.AppDbContext _context;

        public DetailsModel(ItemsAndOrdersManagementSystem.Data.AppDbContext context)
        {
            _context = context;
        }

        public Order Order { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.AsTracking().AsSplitQuery().Include(x => x.Items).ThenInclude(x => x.Item).FirstOrDefaultAsync(m => m.id == id);
            if (order == null)
            {
                return NotFound();
            }
            else
            {
                Order = order;
            }
            return Page();
        }
    }
}
