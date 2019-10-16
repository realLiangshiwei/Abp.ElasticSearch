using Abp.Configuration.Startup;

namespace Abp.ElasticSearch.Configuration
{
    public static class ElasticSearchConfigurationExtensions
    {
        public static IElasticSearchConfiguration ElasticSearch(this IModuleConfigurations configurations)
        {
            return configurations.AbpConfiguration.Get<IElasticSearchConfiguration>();
        }
    }
}
