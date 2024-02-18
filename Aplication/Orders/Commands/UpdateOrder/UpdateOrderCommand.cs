using AutoMapper;
using CSharpFunctionalExtensions;
using ItemsAndOrdersManagementSystem.Common.Helper;
using ItemsAndOrdersManagementSystem.Models;
using ItemsAndOrdersManagementSystem.Models.Dtos;
using ItemsAndOrdersManagementSystem.Resources;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ItemsAndOrdersManagementSystem.Aplication.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommand : IRequest<Result<int>>
    {
        public int id { get; set; }
        public string UserId { get; set; } = null!;
        public byte[] Timestamp { get; }
        public List<int> OrderItemsDtoList { get; set; } = new();
    }

    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, Result<int>>
    {
        private readonly IAppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UpdateOrderCommandHandler(IAppDbContext dbContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<int>> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            string err = string.Empty;
            Maybe<Order> maybeOrder = await _dbContext.Orders
                                                .Include(x => x.Items)
                                                .FirstOrDefaultAsync(x => x.id == request.id);

            if (maybeOrder.HasNoValue)
                return Result.Failure<int>(err.ErrorAppendMessage(Messages.OrderNotFound));

            Maybe<ApplicationUser> maybeUser = await _dbContext.ApplicationUsers.FindAsync(request.UserId, cancellationToken);

            if (maybeUser.HasNoValue)
                return Result.Failure<int>(err.ErrorAppendMessage(Messages.UserNotFound));

            List<Item> itemList = await _dbContext.Items.Where(x => request.OrderItemsDtoList.Contains(x.Id)).ToListAsync();
            List<int> itemListIds = itemList.Select(x => x.Id).ToList();

            List<Maybe<Item>> orderItemList = new();

            foreach (var item in request.OrderItemsDtoList)
            {
                orderItemList.Add(itemListIds.Contains(item) ? itemList.FirstOrDefault(x => x.Id == item) : Maybe.None);
            }

            OrderDto orderDto = new()
            {
                MaybeUser = maybeUser,
                HttpUser = _httpContextAccessor.HttpContext.User,
                OrderItemList = orderItemList
            };

            var updateResult = maybeOrder.Value.UpdateOrder(orderDto);

            if (updateResult.IsFailure)
                return Result.Failure<int>(updateResult.Error);

            var saveResult = await _dbContext.SaveChangesAsync(cancellationToken);

            if (saveResult.IsFailure)
                return Result.Failure<int>(saveResult.Error);

            return Result.Success(updateResult.Value.id);
        }
    }
}
