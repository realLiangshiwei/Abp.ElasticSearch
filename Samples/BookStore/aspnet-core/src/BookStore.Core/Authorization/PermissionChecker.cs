using Abp.Authorization;
using BookStore.Authorization.Roles;
using BookStore.Authorization.Users;

namespace BookStore.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
