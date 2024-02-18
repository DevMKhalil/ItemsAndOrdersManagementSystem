using AutoMapper;
using ItemsAndOrdersManagementSystem.Common.Mapping;
using ItemsAndOrdersManagementSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace ItemsAndOrdersManagementSystem.Aplication.Orders.Dtos
{
    public record OrderForList : IMapFrom<Order>
    {
        public int id { get; set; }
        public string UserId { get; set; } = null!;
        public string UserName { get; set; } = null!;
        [Timestamp]
        public byte[] Timestamp { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Order, OrderForList>();
        }
    }
}
