using ItemsAndOrdersManagementSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ItemsAndOrdersManagementSystem.Pages;

public class IndexModel : PageModelBase
{
    private readonly ILogger<IndexModel> _logger;
    private readonly UserManager<ApplicationUser> _userManager;

    public IndexModel(ILogger<IndexModel> logger,
        UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        this._userManager = userManager;
    }

    public async Task<IActionResult> OnGet()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Redirect("/Identity/Account/Login");
        }

        return Page();
    }
}
