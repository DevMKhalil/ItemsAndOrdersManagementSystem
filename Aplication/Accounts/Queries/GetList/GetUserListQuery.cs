using AutoMapper;
using AutoMapper.QueryableExtensions;
using ItemsAndOrdersManagementSystem.Aplication.Accounts.Dtos;
using ItemsAndOrdersManagementSystem.Aplication.Items.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ItemsAndOrdersManagementSystem.Aplication.Accounts.Queries.GetList
{
    public class GetUserListQuery : IRequest<List<UserDto>>
    {
    }

    public class GetItemListQueryHandler : IRequestHandler<GetUserListQuery, List<UserDto>>
    {
        private readonly IAppDbContext _dbContext;
        private readonly IMapper _mapper;
        public GetItemListQueryHandler(IAppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<List<UserDto>> Handle(GetUserListQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.ApplicationUsers
                .AsNoTracking()
                .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}
