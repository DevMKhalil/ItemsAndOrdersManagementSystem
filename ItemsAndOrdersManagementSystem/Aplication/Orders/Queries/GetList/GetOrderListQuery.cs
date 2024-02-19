using AutoMapper;
using AutoMapper.QueryableExtensions;
using ItemsAndOrdersManagementSystem.Aplication.Orders.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ItemsAndOrdersManagementSystem.Aplication.Orders.Queries.GetList
{
    public class GetOrderListQuery : IRequest<List<OrderForList>>
    {
    }

    public class GetOrderListQueryhandler : IRequestHandler<GetOrderListQuery, List<OrderForList>> 
    {
        private readonly IAppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public GetOrderListQueryhandler(IAppDbContext dbContext, IMapper mapper, IHttpContextAccessor httpContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _httpContextAccessor = httpContext;
        }

        public async Task<List<OrderForList>> Handle(GetOrderListQuery request, CancellationToken cancellationToken)
        {
            var res = await _dbContext.Orders
                .AsNoTracking()
                .Where(x => x.UserId == _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier))
                .ProjectTo<OrderForList>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            if (res.Count > default(int))
            {
                var userIds = res.Select(z => z.UserId).ToList();

                var UserList = await _dbContext.ApplicationUsers
                                        .Where(x => userIds.Contains(x.Id))
                                        .Select(x => new { Name = x.UserName, Id = x.Id })
                                        .ToListAsync();

                res.ForEach(x => x.UserName = UserList.FirstOrDefault(z => z.Id == x.UserId).Name); 
            }

            return res;
        }
    }
}
