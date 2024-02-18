using AutoMapper;
using CSharpFunctionalExtensions;
using ItemsAndOrdersManagementSystem.Aplication.Orders.Commands.CreateOrder;
using ItemsAndOrdersManagementSystem.Aplication.Orders.Commands.UpdateOrder;
using ItemsAndOrdersManagementSystem.Common.Mapping;
using System.Security.Claims;

namespace ItemsAndOrdersManagementSystem.Models.Dtos
{
    public record OrderDto //: IMapFrom<CreateOrderCommand>, IMapFrom<UpdateOrderCommand>
    {
        public Maybe<ApplicationUser> MaybeUser { get; set; } = null!;
        public ClaimsPrincipal HttpUser { get; set; } = null!;
        public List<Maybe<Item>> OrderItemList { get; set; } = new();
        //public void Mapping(Profile profile)
        //{
        //    profile.CreateMap<CreateOrderCommand, OrderDto>();
        //    profile.CreateMap<UpdateOrderCommand, OrderDto>();
        //}
    }
}
