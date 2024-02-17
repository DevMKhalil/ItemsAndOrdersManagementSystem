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
using ItemsAndOrdersManagementSystem.Aplication.Items.Commands.UpdateItem;
using ItemsAndOrdersManagementSystem.Common.Helper;
using MediatR;
using ItemsAndOrdersManagementSystem.Aplication.Items.Dtos;
using ItemsAndOrdersManagementSystem.Aplication.Items.Commands.CreateItem;
using AutoMapper;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using ItemsAndOrdersManagementSystem.Aplication.Items.Queries.GetById;

namespace ItemsAndOrdersManagementSystem.Pages.Items
{
    public class EditModel : PageModelBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public EditModel(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [BindProperty]
        public ItemDto Item { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id is null)
                return NotFound();

            var res = await _mediator.Send(new GetItemByIdQuery { ItemId = id.Value});

            if (res.IsFailure)
                ModelState.AddErrors(res.Error);

            if (!ModelState.IsValid)
                return Page();

            this.Item = res.Value;

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var command = _mapper.Map<UpdateItemCommand>(Item);

            var res = await _mediator.Send(command);

            if (res.IsFailure)
                ModelState.AddErrors(res.Error);

            if (!ModelState.IsValid)
                return Page();

            return RedirectToPage("./Index");
        }
    }
}
