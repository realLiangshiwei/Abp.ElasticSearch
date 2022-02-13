using System.Collections.Generic;
using BookStore.Roles.Dto;

namespace BookStore.Web.Models.Roles
{
    public class RoleListViewModel
    {
        public IReadOnlyList<PermissionDto> Permissions { get; set; }
    }
}
