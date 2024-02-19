using AutoMapper;
using CSharpFunctionalExtensions;
using ItemsAndOrdersManagementSystem.Aplication.Orders.Commands.CreateOrder;
using ItemsAndOrdersManagementSystem.Aplication.Orders.Commands.UpdateOrder;
using ItemsAndOrdersManagementSystem.Common.Mapping;
using System.Security.Claims;

namespace ItemsAndOrdersManagementSystem.Models.Dtos
{
    public record OrderDto
    {
        public Maybe<ApplicationUser> MaybeRequestUser { get; set; } = null!;
        public Maybe<ApplicationUser> MaybeOrderUser { get; set; } = null!;
        public ClaimsPrincipal HttpUser { get; set; } = null!;
        public List<Maybe<Item>> OrderItemList { get; set; } = new();
    }
}
