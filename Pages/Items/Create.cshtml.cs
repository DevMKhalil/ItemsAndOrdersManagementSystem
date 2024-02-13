using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ItemsAndOrdersManagementSystem.Data;
using ItemsAndOrdersManagementSystem.Models;
using Microsoft.AspNetCore.Identity;

namespace ItemsAndOrdersManagementSystem.Pages.Items
{
    public class CreateModel : PageModelBase
    {
        private readonly ItemsAndOrdersManagementSystem.Data.AppDbContext _context;

        public CreateModel(ItemsAndOrdersManagementSystem.Data.AppDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Item Item { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Items.Add(Item);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
