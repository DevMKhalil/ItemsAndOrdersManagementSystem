using CSharpFunctionalExtensions;
using ItemsAndOrdersManagementSystem.Aplication.Items.Dtos;
using ItemsAndOrdersManagementSystem.Common.Helper;
using ItemsAndOrdersManagementSystem.Models.Dtos;
using ItemsAndOrdersManagementSystem.Resources;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;
using System.Security.Claims;

namespace ItemsAndOrdersManagementSystem.Models
{
    public class Order
    {
        private Order()
        {
            
        }
        [Key]
        [Required]
        public int id { get; private set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; private set; } = null!;

        [Timestamp]
        public byte[]? Timestamp { get; private set; }

        public ApplicationUser User { get; set; }
        public List<OrderItems> Items { get; set; } = new();

        public static Result<Order> CreateOrder(OrderDto orderDto)
        {
            var validateResult = ValidateOrder(orderDto);

            if (validateResult.IsFailure)
                return Result.Failure<Order>(validateResult.Error);

            List<OrderItems> items = new();
            foreach (var item in orderDto.OrderItemList)
            {
                var orderItemsResult = OrderItems.CreateOrderItems(item);

                if (orderItemsResult.IsFailure)
                    return Result.Failure<Order>(orderItemsResult.Error);
                else
                    items.Add(orderItemsResult.Value);
            }

            return new Order()
            {
                UserId = orderDto.MaybeUser.Value.Id,
                Items = items
            };
        }

        private static Result ValidateOrder(OrderDto orderDto)
        {
            string err = string.Empty;

            if (orderDto.MaybeUser.HasNoValue)
                err = err.ErrorAppendMessage(nameof(Order), nameof(UserId), Messages.userIsNotAuthenticated);

            if (orderDto.OrderItemList.Count <= default(int))
                err = err.ErrorAppendMessage(Messages.ItemsListIsEmpty);

            if (orderDto.OrderItemList.Any(x => x.HasNoValue))
                err = err.ErrorAppendMessage(Messages.OrderItemNotFound);

            if (!orderDto.OrderItemList.Any(x => x.HasNoValue) && orderDto.OrderItemList.GroupBy(x => x.Value.Id).Any(x => x.Count() > 1))
                err = err.ErrorAppendMessage(Messages.TheItemIsDublicated);

            if (orderDto.HttpUser is null || orderDto.HttpUser.FindFirstValue(ClaimTypes.NameIdentifier) != orderDto.MaybeUser.Value.Id)
                err = err.ErrorAppendMessage(Messages.userIsNotAuthenticated);

            if (!string.IsNullOrEmpty(err))
                return Result.Failure(err);
            else
                return Result.Success();
        }

        public Result<Order> UpdateOrder(OrderDto orderDto)
        {
            var validateResult = ValidateOrder(orderDto);

            if (validateResult.IsFailure)
                return Result.Failure<Order>(validateResult.Error);

            this.Items.Clear();

            foreach (var item in orderDto.OrderItemList)
            {
                var orderItemsResult = OrderItems.CreateOrderItems(item);

                if (orderItemsResult.IsFailure)
                    return Result.Failure<Order>(orderItemsResult.Error);
                else
                    this.Items.Add(orderItemsResult.Value);
            }

            this.UserId = orderDto.MaybeUser.Value.Id;

            return Result.Success(this);
        }
    }
}
