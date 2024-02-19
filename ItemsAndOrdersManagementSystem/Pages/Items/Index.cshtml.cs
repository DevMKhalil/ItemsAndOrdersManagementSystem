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
using ItemsAndOrdersManagementSystem.Aplication.Items.Queries.SearchQuery;

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
        public string FilterItemName { get; set; }
        public int PageIndex { get; set; } = 1;
        public int TotalPages { get; set; }
        public const int ItemsPerPage = 5;

        public async Task OnGetAsync(string? itemName, int? pageIndex)
        {
            FilterItemName = itemName ?? string.Empty;

            PageIndex = pageIndex ?? 1;

            var result = await _mediator.Send(new SearchQuery
                                {
                                    Name = FilterItemName,
                                    Skip = (PageIndex - 1) * ItemsPerPage,
                                    Take = ItemsPerPage
                                });

            TotalPages = (int)Math.Ceiling((double)result.TotalCount / ItemsPerPage);
            ItemList = result.Data;
        }
    }
}
