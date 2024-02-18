using AutoMapper;
using ItemsAndOrdersManagementSystem.Aplication.Items.Commands.CreateItem;
using ItemsAndOrdersManagementSystem.Aplication.Items.Commands.UpdateItem;
using ItemsAndOrdersManagementSystem.Common.Mapping;
using ItemsAndOrdersManagementSystem.Models;

namespace ItemsAndOrdersManagementSystem.Aplication.Accounts.Dtos
{
    public record UserDto : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }
        public string UserName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ApplicationUser, UserDto>();
        }
    }
}
