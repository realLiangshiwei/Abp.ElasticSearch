using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace BookStore.Controllers
{
    public abstract class BookStoreControllerBase: AbpController
    {
        protected BookStoreControllerBase()
        {
            LocalizationSourceName = BookStoreConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
