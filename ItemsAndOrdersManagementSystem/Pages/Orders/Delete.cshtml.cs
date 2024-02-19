using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ItemsAndOrdersManagementSystem.Data;
using ItemsAndOrdersManagementSystem.Models;
using ItemsAndOrdersManagementSystem.Aplication.Orders.Queries.GetById;
using ItemsAndOrdersManagementSystem.Common.Helper;
using MediatR;
using ItemsAndOrdersManagementSystem.Aplication.Orders.Dtos;
using ItemsAndOrdersManagementSystem.Aplication.Orders.Commands.DeleteCommand;

namespace ItemsAndOrdersManagementSystem.Pages.Orders
{
    public class DeleteModel : PageModelBase
    {
        private readonly IMediator _mediator;

        public DeleteModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        [BindProperty]
        public OrderDto Order { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id is null)
                return NotFound();

            var res = await _mediator.Send(new GetOrderByIdQuery { Id = id.Value });

            if (res.IsFailure)
                ModelState.AddErrors(res.Error);
            else
                Order = res.Value;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id is null)
                return NotFound();

            var res = await _mediator.Send(new DeleteOrderCommand { Id = id.Value });

            if (res.IsFailure)
                ModelState.AddErrors(res.Error);

            return RedirectToPage("./Index");
        }
    }
}
