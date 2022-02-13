using System.Collections.Generic;
using BookStore.Roles.Dto;

namespace BookStore.Web.Models.Users
{
    public class UserListViewModel
    {
        public IReadOnlyList<RoleDto> Roles { get; set; }
    }
}
