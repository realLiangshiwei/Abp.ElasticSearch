using System.Threading.Tasks;
using Abp.Application.Services;
using BookStore.Sessions.Dto;

namespace BookStore.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
