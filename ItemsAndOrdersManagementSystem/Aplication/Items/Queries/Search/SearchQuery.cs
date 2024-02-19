using AutoMapper;
using AutoMapper.QueryableExtensions;
using CSharpFunctionalExtensions;
using ItemsAndOrdersManagementSystem.Aplication.Items.Dtos;
using ItemsAndOrdersManagementSystem.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ItemsAndOrdersManagementSystem.Aplication.Items.Queries.SearchQuery
{
    public class SearchQuery : IRequest<PagedList<ItemDto>>
    {
        public string Name { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; } = 10;
    }

    public class SearchQueryHandler : IRequestHandler<SearchQuery, PagedList<ItemDto>>
    {
        private readonly IAppDbContext _dbContext;
        private readonly IMapper _mapper;
        public SearchQueryHandler(IAppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<PagedList<ItemDto>> Handle(SearchQuery request, CancellationToken cancellationToken)
        {
            var query = _dbContext.Items
                        .AsNoTracking()
                        .ProjectTo<ItemDto>(_mapper.ConfigurationProvider);

            if (!string.IsNullOrEmpty(request.Name))
            {
                query = query.Where(x => x.Name.ToLower().Contains(request.Name.ToLower()));
            }

            return await PagedList<ItemDto>.ToPagedList(query, request.Skip, request.Take);
        }
    }
}
