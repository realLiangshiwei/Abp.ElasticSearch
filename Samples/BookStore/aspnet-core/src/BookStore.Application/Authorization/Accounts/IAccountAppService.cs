using System.Threading.Tasks;
using Abp.Application.Services;
using BookStore.Authorization.Accounts.Dto;

namespace BookStore.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
