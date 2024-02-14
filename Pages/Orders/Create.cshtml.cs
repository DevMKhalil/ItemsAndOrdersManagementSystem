using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ItemsAndOrdersManagementSystem.Data;
using ItemsAndOrdersManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ItemsAndOrdersManagementSystem.Common;

namespace ItemsAndOrdersManagementSystem.Pages.Orders
{
    public class CreateModel : PageModelBase
    {
        private readonly ItemsAndOrdersManagementSystem.Data.AppDbContext _context;
        public List<SelectListItem> ItemList { get; set; } = new();
        [BindProperty]
        public List<int> NewItemDetailList { get; set; } = new();
        public CreateModel(ItemsAndOrdersManagementSystem.Data.AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGet()
        {
            ItemList = await _context.Items.Select(x => new SelectListItem(x.Name, x.Id.ToString())).ToListAsync();
            
            return Page();
        }

        public Order Order { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (User == null)
            {
                ModelState.AddModelError(string.Empty, "User is not authenticated.");
            }

            if (NewItemDetailList == null || !NewItemDetailList.Any())
            {
                ModelState.AddModelError(string.Empty, "Items list is empty.");
            }
            if (!ModelState.IsValid)
            {
                ItemList = await _context.Items.Select(x => new SelectListItem(x.Name, x.Id.ToString())).ToListAsync();
                return Page();
            }
            Order = new Order();
            this.Order.UserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            this.Order.Items.AddRange(NewItemDetailList.Select(x => new OrderItems { ItemId = x}));

            _context.Orders.Add(Order);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
