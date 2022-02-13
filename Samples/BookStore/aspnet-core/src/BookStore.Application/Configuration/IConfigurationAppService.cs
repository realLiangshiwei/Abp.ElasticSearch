using System.Threading.Tasks;
using BookStore.Configuration.Dto;

namespace BookStore.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
