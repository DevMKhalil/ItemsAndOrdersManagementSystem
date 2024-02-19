using AutoMapper;
using AutoMapper.QueryableExtensions;
using ItemsAndOrdersManagementSystem.Aplication.Items.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ItemsAndOrdersManagementSystem.Aplication.Items.Queries.GetList
{
    public class GetItemListQuery : IRequest<List<ItemDto>>
    {
    }

    public class GetItemListQueryHandler : IRequestHandler<GetItemListQuery, List<ItemDto>>
    {
        private readonly IAppDbContext _dbContext;
        private readonly IMapper _mapper;
        public GetItemListQueryHandler(IAppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<List<ItemDto>> Handle(GetItemListQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.Items
                .AsNoTracking()
                .ProjectTo<ItemDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}
