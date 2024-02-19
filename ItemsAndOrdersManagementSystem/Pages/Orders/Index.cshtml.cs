using MediatR;
using ItemsAndOrdersManagementSystem.Aplication.Orders.Dtos;
using ItemsAndOrdersManagementSystem.Aplication.Orders.Queries.GetList;
using ItemsAndOrdersManagementSystem.Aplication.Orders.Queries.Search;

namespace ItemsAndOrdersManagementSystem.Pages.Orders
{
    public class IndexModel : PageModelBase
    {
        private readonly IMediator _mediator;

        public IndexModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public IList<OrderForList> OrderList { get;set; } = default!;
        public int PageIndex { get; set; } = 1;
        public int TotalPages { get; set; }
        public const int ItemsPerPage = 5;

        public async Task OnGetAsync(int? pageIndex)
        {
            PageIndex = pageIndex ?? 1;

            var result = await _mediator.Send(new SearchQuery
                                {
                                    Skip = (PageIndex - 1) * ItemsPerPage,
                                    Take = ItemsPerPage
                                });

            TotalPages = (int)Math.Ceiling((double)result.TotalCount / ItemsPerPage);
            OrderList = result.Data;
        }
    }
}
