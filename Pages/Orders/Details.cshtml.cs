using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ItemsAndOrdersManagementSystem.Data;
using ItemsAndOrdersManagementSystem.Models;
using MediatR;
using ItemsAndOrdersManagementSystem.Aplication.Orders.Dtos;
using ItemsAndOrdersManagementSystem.Aplication.Items.Queries.GetById;
using ItemsAndOrdersManagementSystem.Common.Helper;
using ItemsAndOrdersManagementSystem.Aplication.Orders.Queries.GetById;
using ItemsAndOrdersManagementSystem.Aplication.Items.Queries.GetList;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ItemsAndOrdersManagementSystem.Pages.Orders
{
    public class DetailsModel : PageModelBase
    {
        private readonly IMediator _mediator;

        public DetailsModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public OrderDto Order { get; set; } = default!;
        public List<SelectListItem> ItemList { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id is null)
                return NotFound();

            var res = await _mediator.Send(new GetOrderByIdQuery { Id = id.Value });

            if (res.IsFailure)
                ModelState.AddErrors(res.Error);
            else
                Order = res.Value;

            var items = await _mediator.Send(new GetItemListQuery { });

            ItemList = items
                        .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                        .ToList();

            return Page();
        }
    }
}
