using AutoMapper;
using ItemsAndOrdersManagementSystem.Aplication.Items.Commands.CreateItem;
using ItemsAndOrdersManagementSystem.Aplication.Items.Commands.UpdateItem;
using ItemsAndOrdersManagementSystem.Common.Mapping;
using ItemsAndOrdersManagementSystem.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;

namespace ItemsAndOrdersManagementSystem.Aplication.Items.Dtos
{
    public record ItemDto() : IMapFrom<CreateItemCommand>, IMapFrom<UpdateItemCommand>, IMapFrom<Item>
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        [Column(TypeName = "decimal(7,4)")]
        public decimal Price { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<ItemDto, UpdateItemCommand>();
            profile.CreateMap<ItemDto, CreateItemCommand>();
            profile.CreateMap<Item, ItemDto>();
        }
    }
}
