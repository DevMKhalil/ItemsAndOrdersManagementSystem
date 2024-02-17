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
using ItemsAndOrdersManagementSystem.Aplication.Items.Queries.GetList;

namespace ItemsAndOrdersManagementSystem.Pages.Items
{
    public class IndexModel : PageModelBase
    {
        private readonly IMediator _mediator;

        public IndexModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public IList<ItemDto> ItemList { get;set; } = default!;

        public async Task OnGetAsync()
        {
            ItemList = await _mediator.Send(new GetItemListQuery { });
            //Item = await _context.Items.ToListAsync();
        }
    }
}
