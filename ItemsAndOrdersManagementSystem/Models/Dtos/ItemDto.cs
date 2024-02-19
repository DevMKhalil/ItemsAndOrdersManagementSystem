using AutoMapper;
using ItemsAndOrdersManagementSystem.Aplication.Items.Commands.CreateItem;
using ItemsAndOrdersManagementSystem.Aplication.Items.Commands.UpdateItem;
using ItemsAndOrdersManagementSystem.Common.Mapping;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;

namespace ItemsAndOrdersManagementSystem.Models.Dtos
{
    public record ItemDto : IMapFrom<CreateItemCommand>, IMapFrom<UpdateItemCommand>
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        [Column(TypeName = "decimal(7,4)")]
        public decimal Price { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateItemCommand, ItemDto>();
            profile.CreateMap<UpdateItemCommand, ItemDto>();
        }
    }
}
