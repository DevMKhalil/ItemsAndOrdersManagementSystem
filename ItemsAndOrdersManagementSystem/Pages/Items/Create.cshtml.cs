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
using ItemsAndOrdersManagementSystem.Aplication.Items.Commands.CreateItem;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ItemsAndOrdersManagementSystem.Common.Helper;
using ItemsAndOrdersManagementSystem.Aplication.Items.Dtos;
using Azure.Core;
using AutoMapper;

namespace ItemsAndOrdersManagementSystem.Pages.Items
{
    public class CreateModel : PageModelBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public CreateModel(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public ItemDto Item { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var command = _mapper.Map<CreateItemCommand>(Item);

            var res = await _mediator.Send(command);

            if (res.IsFailure)
                ModelState.AddErrors(res.Error);

            if (!ModelState.IsValid)
                return Page();

            return RedirectToPage("./Index");
        }
    }
}
