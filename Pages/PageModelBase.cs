using ItemsAndOrdersManagementSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ItemsAndOrdersManagementSystem.Pages
{
    public class PageModelBase : PageModel
    {
        public PageModelBase()
        {
        }

        public override async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
        {
            if (User is null || User.Identity is null || !User.Identity.IsAuthenticated)
            {
                context.Result = new RedirectToPageResult("/Account/Login");
            }
            await base.OnPageHandlerExecutionAsync(context, next);
        }
    }
}
