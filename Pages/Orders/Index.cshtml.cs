using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ItemsAndOrdersManagementSystem.Data;
using ItemsAndOrdersManagementSystem.Models;
using System.Security.Claims;

namespace ItemsAndOrdersManagementSystem.Pages.Orders
{
    public class IndexModel : PageModelBase
    {
        private readonly ItemsAndOrdersManagementSystem.Data.AppDbContext _context;

        public IndexModel(ItemsAndOrdersManagementSystem.Data.AppDbContext context)
        {
            _context = context;
        }

        public IList<Order> Order { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Order = await _context.Orders
                .Where(x => x.UserId == this.User.FindFirstValue(ClaimTypes.NameIdentifier))
                .AsTracking()
                .Include(o => o.User).ToListAsync();
        }
    }
}
