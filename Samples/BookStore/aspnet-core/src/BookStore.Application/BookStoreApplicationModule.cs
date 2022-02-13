using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using BookStore.Authorization;

namespace BookStore
{
    [DependsOn(
        typeof(BookStoreCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class BookStoreApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<BookStoreAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(BookStoreApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)
            );
        }
    }
}
