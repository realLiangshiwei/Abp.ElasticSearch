using Abp.Configuration.Startup;

namespace Abp.ElasticSearch.Configuration
{
    public static class ElasticSearchConfigrationExtensions
    {
        public static IElasticSearchConfigration ElasticSearch(this IModuleConfigurations configurations)
        {
            return configurations.AbpConfiguration.Get<IElasticSearchConfigration>();
        }
    }
}
