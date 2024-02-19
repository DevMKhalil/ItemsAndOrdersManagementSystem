using AutoMapper;
using CSharpFunctionalExtensions;
using ItemsAndOrdersManagementSystem.Aplication.Items.Dtos;
using ItemsAndOrdersManagementSystem.Models;
using ItemsAndOrdersManagementSystem.Resources;
using MediatR;

namespace ItemsAndOrdersManagementSystem.Aplication.Items.Queries.GetById
{
    public class GetItemByIdQuery : IRequest<Result<ItemDto>>
    {
        public int ItemId { get; set; }
    }

    public class GetItemByIdQueryHandler : IRequestHandler<GetItemByIdQuery, Result<ItemDto>>
    {
        private readonly IAppDbContext _dbContext;
        private readonly IMapper _mapper;
        public GetItemByIdQueryHandler(IAppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<Result<ItemDto>> Handle(GetItemByIdQuery request, CancellationToken cancellationToken)
        {
            var item = await _dbContext.Items.FindAsync(request.ItemId, cancellationToken);

            if (item is null) 
                return Result.Failure<ItemDto>(Messages.ItemNotFound);

            return _mapper.Map<ItemDto>(item);
        }
    }
}
