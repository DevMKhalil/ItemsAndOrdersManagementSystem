using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ItemsAndOrdersManagementSystem.Data;
using ItemsAndOrdersManagementSystem.Models;
using ItemsAndOrdersManagementSystem.Aplication.Items.Queries.GetById;
using AutoMapper;
using MediatR;
using ItemsAndOrdersManagementSystem.Aplication.Items.Dtos;
using ItemsAndOrdersManagementSystem.Common.Helper;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using ItemsAndOrdersManagementSystem.Aplication.Items.Commands.DeleteCommand;

namespace ItemsAndOrdersManagementSystem.Pages.Items
{
    public class DeleteModel : PageModelBase
    {
        private readonly IMediator _mediator;

        public DeleteModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        [BindProperty]
        public ItemDto Item { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id is null)
                return NotFound();

            var res = await _mediator.Send(new GetItemByIdQuery { ItemId = id.Value });

            if (res.IsFailure)
                ModelState.AddErrors(res.Error);

            if (!ModelState.IsValid)
                return Page();

            this.Item = res.Value;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id is null)
                return NotFound();

            if (!ModelState.IsValid)
                return Page();

            var res = await _mediator.Send(new DeleteItemCommand { ItemId = id.Value});

            if (res.IsFailure)
                ModelState.AddErrors(res.Error);

            if (!ModelState.IsValid)
                return Page();

            return RedirectToPage("./Index");
        }
    }
}
