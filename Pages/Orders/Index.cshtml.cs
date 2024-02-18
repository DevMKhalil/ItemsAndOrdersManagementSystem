using MediatR;
using ItemsAndOrdersManagementSystem.Aplication.Orders.Dtos;
using ItemsAndOrdersManagementSystem.Aplication.Orders.Queries.GetList;

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

        public async Task OnGetAsync()
        {
            OrderList = await _mediator.Send(new GetOrderListQuery { });
        }
    }
}
