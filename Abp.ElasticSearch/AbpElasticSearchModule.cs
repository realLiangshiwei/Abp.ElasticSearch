using Abp.Dependency;
using Abp.ElasticSearch.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Abp.ElasticSearch
{
    [DependsOn(typeof(AbpKernelModule))]
    public class AbpElasticSearchModule : AbpModule
    {
        public override void PreInitialize()
        {
            IocManager.Register<IElasticSearchConfiguration, ElasticSearchConfiguration>();

            IocManager.Register<IElasticsearchManager,AbpElasticSearchManager>(DependencyLifeStyle.Transient);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AbpElasticSearchModule).GetAssembly());
        }
    }
}
