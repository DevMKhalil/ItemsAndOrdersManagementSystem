using AutoMapper;
using ItemsAndOrdersManagementSystem.Aplication.Orders.Commands.CreateOrder;
using ItemsAndOrdersManagementSystem.Aplication.Orders.Commands.UpdateOrder;
using ItemsAndOrdersManagementSystem.Common.Mapping;
using ItemsAndOrdersManagementSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace ItemsAndOrdersManagementSystem.Aplication.Orders.Dtos
{
    public record OrderDto : IMapFrom<CreateOrderCommand>, IMapFrom<UpdateOrderCommand>, IMapFrom<Order>
    {
        public int id { get; set; }
        public string UserId { get; set; } = null!;
        [Timestamp]
        public byte[] Timestamp { get; set; }
        public List<int> OrderItemsDtoList { get; set; } = new();

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Order, OrderDto>()
                .ForMember(x => x.OrderItemsDtoList,opt => opt.MapFrom(src => src.Items.Select(z => z.ItemId)));

            profile.CreateMap<OrderDto, CreateOrderCommand>()
                .ForMember(x => x.OrderItemsDtoList, opt => opt.MapFrom(src => src.OrderItemsDtoList));

            profile.CreateMap<OrderDto, UpdateOrderCommand>()
                .ForMember(x => x.OrderItemsDtoList, opt => opt.MapFrom(src => src.OrderItemsDtoList));
        }
    }
}
