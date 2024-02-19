using AutoMapper;
using AutoMapper.QueryableExtensions;
using ItemsAndOrdersManagementSystem.Aplication.Orders.Dtos;
using ItemsAndOrdersManagementSystem.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ItemsAndOrdersManagementSystem.Aplication.Orders.Queries.Search
{
    public class SearchQuery : IRequest<PagedList<OrderForList>>
    {
        public int Skip { get; set; }
        public int Take { get; set; } = 10;
    }

    public class SearchQueryHandler : IRequestHandler<SearchQuery, PagedList<OrderForList>> 
    {
        private readonly IAppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public SearchQueryHandler(IAppDbContext dbContext, IMapper mapper, IHttpContextAccessor httpContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _httpContextAccessor = httpContext;
        }

        public async Task<PagedList<OrderForList>> Handle(SearchQuery request, CancellationToken cancellationToken)
        {

            var query = _dbContext.Orders
                .AsNoTracking()
                .Where(x => x.UserId == _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier))
                .ProjectTo<OrderForList>(_mapper.ConfigurationProvider);

            var res = await PagedList<OrderForList>.ToPagedList(query, request.Skip, request.Take);

            //var res = await _dbContext.Orders
            //    .AsNoTracking()
            //    .Where(x => x.UserId == _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier))
            //    .ProjectTo<OrderForList>(_mapper.ConfigurationProvider)
            //    .ToListAsync(cancellationToken);

            if (res.Data.Count > default(int))
            {
                var userIds = res.Data.Select(z => z.UserId).ToList();

                var UserList = await _dbContext.ApplicationUsers
                                        .Where(x => userIds.Contains(x.Id))
                                        .Select(x => new { Name = x.UserName, Id = x.Id })
                                        .ToListAsync();

                res.Data.ForEach(x => x.UserName = UserList.FirstOrDefault(z => z.Id == x.UserId).Name); 
            }

            return res;
        }
    }
}
