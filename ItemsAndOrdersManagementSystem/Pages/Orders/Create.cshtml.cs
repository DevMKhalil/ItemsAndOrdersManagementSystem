using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ItemsAndOrdersManagementSystem.Data;
using ItemsAndOrdersManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ItemsAndOrdersManagementSystem.Aplication.Orders.Dtos;
using ItemsAndOrdersManagementSystem.Aplication.Items.Queries.GetList;
using MediatR;
using ItemsAndOrdersManagementSystem.Aplication.Orders.Queries.GetById;
using ItemsAndOrdersManagementSystem.Common.Helper;
using AutoMapper;
using ItemsAndOrdersManagementSystem.Aplication.Orders.Commands.CreateOrder;
using ItemsAndOrdersManagementSystem.Aplication.Accounts.Queries.GetList;

namespace ItemsAndOrdersManagementSystem.Pages.Orders
{
    public class CreateModel : PageModelBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public List<SelectListItem> ItemList { get; set; } = new();
        public List<SelectListItem> UserList { get; set; } = new();
        [BindProperty]
        public List<int> NewItemDetailList { get; set; } = new();
        public CreateModel(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<IActionResult> OnGet()
        {
            await GetItemList();
            await GetUserList();

            return Page();
        }

        [BindProperty]
        public OrderDto Order { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            var orderDto = _mapper.Map<CreateOrderCommand>(Order);

            orderDto.UserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var res = await _mediator.Send(orderDto);

            ModelState.Clear();

            if (res.IsFailure)
                ModelState.AddErrors(res.Error);

            if (!ModelState.IsValid)
            {
                await GetItemList();
                await GetUserList();
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

        private async Task GetUserList()
        {
            var users = await _mediator.Send(new GetUserListQuery { });

            UserList = users
                        .Select(x => new SelectListItem(x.UserName, x.Id.ToString()))
                        .ToList();
        }
    }
}
