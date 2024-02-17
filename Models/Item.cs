using CSharpFunctionalExtensions;
using ItemsAndOrdersManagementSystem.Common.Helper;
using ItemsAndOrdersManagementSystem.Models.Dtos;
using ItemsAndOrdersManagementSystem.Resources;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ItemsAndOrdersManagementSystem.Models
{
    public class Item
    {
        [Key]
        [Required]
        public int Id { get; private set; }
        public string Name { get; private set; } = null!;
        public string Description { get; private set; } = null!;
        [Column(TypeName = "decimal(7,4)")]
        public decimal Price { get; private set; }

        public static Result<Item> CreateItem(ItemDto itemDto)
        {
            var res = ValidateItem(itemDto);

            if (res.IsFailure)
                return Result.Failure<Item>(res.Error);

            var item = new Item
            {
                Name = itemDto.Name,
                Price = itemDto.Price,
                Description = itemDto.Description
            };

            return Result.Success(item);
        }

        private static Result ValidateItem(ItemDto itemDto)
        {
            string err = string.Empty;

            if (string.IsNullOrEmpty(itemDto.Name) || string.IsNullOrWhiteSpace(itemDto.Name))
                err = err.ErrorAppendMessage(nameof(Item), nameof(Name), Messages.InsertItemName);

            if (string.IsNullOrEmpty(itemDto.Description) || string.IsNullOrWhiteSpace(itemDto.Description))
                err = err.ErrorAppendMessage(nameof(Item), nameof(Description), Messages.InsertItemDesc);

            if (itemDto.Price <= default(decimal))
                err = err.ErrorAppendMessage(nameof(Item), nameof(Price), Messages.InsertItemPrice);

            if (!string.IsNullOrEmpty(err))
                return Result.Failure<Item>(err);
            else
                return Result.Success();
        }

        public Result<Item> UpdateItem(ItemDto itemDto)
        {
            var res = ValidateItem(itemDto);

            if (res.IsFailure)
                return Result.Failure<Item>(res.Error);

            this.Name = itemDto.Name;
            this.Description = itemDto.Description;
            this.Price = itemDto.Price;

            return Result.Success(this);
        }
    }
}
