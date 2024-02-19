using AutoMapper;
using CSharpFunctionalExtensions;
using ItemsAndOrdersManagementSystem.Aplication.Orders.Dtos;
using ItemsAndOrdersManagementSystem.Resources;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ItemsAndOrdersManagementSystem.Aplication.Orders.Queries.GetById
{
    public class GetOrderByIdQuery : IRequest<Result<OrderDto>>
    {
        public int Id { get; set; }
    }

    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, Result<OrderDto>>
    {
        private readonly IAppDbContext _dbContext;
        private readonly IMapper _mapper;
        public GetOrderByIdQueryHandler(IAppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<Result<OrderDto>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var order = await _dbContext.Orders
                                .Include(x => x.Items)
                                .AsTracking()
                                .FirstOrDefaultAsync(x => x.id == request.Id, cancellationToken);

            if (order is null)
                return Result.Failure<OrderDto>(Messages.ItemNotFound);

            return _mapper.Map<OrderDto>(order);
        }
    }
}
