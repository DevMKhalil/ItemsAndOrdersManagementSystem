using CSharpFunctionalExtensions;
using ItemsAndOrdersManagementSystem.Resources;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ItemsAndOrdersManagementSystem.Models
{
    public class OrderItems
    {
        private OrderItems()
        {
            
        }
        [Key]
        [Required]
        public int Id { get; internal set; }
        [ForeignKey(nameof(Order))]
        public int OrderId { get; internal set; }
        [ForeignKey(nameof(Item))]
        public int ItemId { get; internal set; }

        public Order Order { get; set; } = null!;
        public Item Item { get; set; } = null!;

        public static Result<OrderItems> CreateOrderItems(Maybe<Item> maybeItem)
        {
            if (maybeItem.HasNoValue)
                return Result.Failure<OrderItems>(Messages.ItemNotFound);

            return Result.Success( new OrderItems()
            {
                ItemId = maybeItem.Value.Id
            });
        }

        public Result<OrderItems> UpdateOrderItems(Maybe<Item> maybeItem)
        {
            if (maybeItem.HasNoValue)
                return Result.Failure<OrderItems>(Messages.ItemNotFound);

            this.ItemId = maybeItem.Value.Id;

            return Result.Success(this);
        }
    }
}
