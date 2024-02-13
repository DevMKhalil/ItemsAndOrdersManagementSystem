using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ItemsAndOrdersManagementSystem.Data;
using ItemsAndOrdersManagementSystem.Models;

namespace ItemsAndOrdersManagementSystem.Pages.Items
{
    public class IndexModel : PageModelBase
    {
        private readonly ItemsAndOrdersManagementSystem.Data.AppDbContext _context;

        public IndexModel(ItemsAndOrdersManagementSystem.Data.AppDbContext context)
        {
            _context = context;
        }

        public IList<Item> Item { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Item = await _context.Items.ToListAsync();
        }
    }
}
