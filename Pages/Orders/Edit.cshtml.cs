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
using AutoMapper;
using MediatR;
using ItemsAndOrdersManagementSystem.Aplication.Orders.Dtos;
using ItemsAndOrdersManagementSystem.Aplication.Orders.Queries.GetById;
using ItemsAndOrdersManagementSystem.Common.Helper;
using ItemsAndOrdersManagementSystem.Aplication.Items.Queries.GetList;
using ItemsAndOrdersManagementSystem.Aplication.Orders.Commands.CreateOrder;
using ItemsAndOrdersManagementSystem.Aplication.Orders.Commands.UpdateOrder;
using System.Security.Claims;

namespace ItemsAndOrdersManagementSystem.Pages.Orders
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
        public OrderDto Order { get; set; }
        public List<SelectListItem> ItemList { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var res = await _mediator.Send(new GetOrderByIdQuery { Id = id.Value });

            if (res.IsFailure)
                ModelState.AddErrors(res.Error);
            else
                Order = res.Value;

            await GetItemList();

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {

            var orderDto = _mapper.Map<UpdateOrderCommand>(Order);

            orderDto.UserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var res = await _mediator.Send(orderDto);

            ModelState.Clear();

            if (res.IsFailure)
                ModelState.AddErrors(res.Error);

            if (!ModelState.IsValid)
            {
                await GetItemList();
                return Page();
            }

            return RedirectToPage("./Index");
        }

        private async Task GetItemList()
        {
            var items = await _mediator.Send(new GetItemListQuery { });

            ItemList = items
                        .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                        .ToList();
        }
    }
}
