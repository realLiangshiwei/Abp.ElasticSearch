using System.Collections.Generic;
using BookStore.Roles.Dto;

namespace BookStore.Web.Models.Common
{
    public interface IPermissionsEditViewModel
    {
        List<FlatPermissionDto> Permissions { get; set; }
    }
}