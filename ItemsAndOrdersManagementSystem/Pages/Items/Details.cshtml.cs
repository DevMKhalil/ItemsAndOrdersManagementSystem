using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ItemsAndOrdersManagementSystem.Data;
using ItemsAndOrdersManagementSystem.Models;
using ItemsAndOrdersManagementSystem.Aplication.Items.Dtos;
using ItemsAndOrdersManagementSystem.Aplication.Items.Queries.GetById;
using MediatR;
using ItemsAndOrdersManagementSystem.Common.Helper;

namespace ItemsAndOrdersManagementSystem.Pages.Items
{
    public class DetailsModel : PageModelBase
    {
        private readonly IMediator _mediator;

        public DetailsModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public ItemDto Item { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id is null)
                return NotFound();

            var res = await _mediator.Send(new GetItemByIdQuery { ItemId = id.Value });

            if (res.IsFailure)
                ModelState.AddErrors(res.Error);
            else
                Item = res.Value;

            return Page();
        }
    }
}
