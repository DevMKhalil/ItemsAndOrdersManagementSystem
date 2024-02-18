using AutoMapper;
using CSharpFunctionalExtensions;
using ItemsAndOrdersManagementSystem.Common.Helper;
using ItemsAndOrdersManagementSystem.Models;
using ItemsAndOrdersManagementSystem.Models.Dtos;
using ItemsAndOrdersManagementSystem.Resources;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Collections.Generic;


namespace ItemsAndOrdersManagementSystem.Aplication.Orders.Commands.CreateOrder
{
    public class CreateOrderCommand : IRequest<Result<int>>
    {
        public string UserId { get; set; } = null!;
        public List<int> OrderItemsDtoList { get; set; } = new();
    }

    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand,Result<int>>
    {
        private readonly IAppDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CreateOrderCommandHandler(IAppDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<int>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            string err = string.Empty;
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

            var createResult = Order.CreateOrder(orderDto);

            if (createResult.IsFailure)
                return Result.Failure<int>(createResult.Error);

            _dbContext.Orders.Add(createResult.Value);

            var saveResult = await _dbContext.SaveChangesAsync(cancellationToken);

            if (saveResult.IsFailure)
                return Result.Failure<int>(saveResult.Error);

            return Result.Success(createResult.Value.id);
        }
    }
}
