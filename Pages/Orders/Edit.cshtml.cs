using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ItemsAndOrdersManagementSystem.Data;
using ItemsAndOrdersManagementSystem.Models;
using NuGet.Packaging;

namespace ItemsAndOrdersManagementSystem.Pages.Orders
{
    public class EditModel : PageModelBase
    {
        private readonly ItemsAndOrdersManagementSystem.Data.AppDbContext _context;
        public EditModel(ItemsAndOrdersManagementSystem.Data.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Order Order { get; set; }
        public List<SelectListItem> ItemList { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Order =  await _context.Orders
                .AsSplitQuery()
                .Include(x => x.Items)
                .FirstOrDefaultAsync(m => m.id == id);

            if (Order is null)
            {
                return NotFound();
            }

            ItemList = await _context.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToListAsync();

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (User == null)
            {
                ModelState.AddModelError(string.Empty, "User is not authenticated.");
            }

            if (!Order.Items.Any())
            {
                ModelState.AddModelError(string.Empty, "Items list is empty.");
            }
            if (!ModelState.IsValid)
            {
                ItemList = await _context.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToListAsync();

                return Page();
            }

            var existingOrder = await _context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(m => m.id == Order.id);

            if (existingOrder == null)
            {
                return NotFound();
            }

            existingOrder.Items.Clear();
            existingOrder.Items.AddRange(Order.Items);

            _context.Attach(existingOrder).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(Order.id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.id == id);
        }
    }
}
