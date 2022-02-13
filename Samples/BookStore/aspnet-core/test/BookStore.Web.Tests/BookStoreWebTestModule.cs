using Abp.AspNetCore;
using Abp.AspNetCore.TestBase;
using Abp.Modules;
using Abp.Reflection.Extensions;
using BookStore.EntityFrameworkCore;
using BookStore.Web.Startup;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace BookStore.Web.Tests
{
    [DependsOn(
        typeof(BookStoreWebMvcModule),
        typeof(AbpAspNetCoreTestBaseModule)
    )]
    public class BookStoreWebTestModule : AbpModule
    {
        public BookStoreWebTestModule(BookStoreEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbContextRegistration = true;
        } 
        
        public override void PreInitialize()
        {
            Configuration.UnitOfWork.IsTransactional = false; //EF Core InMemory DB does not support transactions.
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(BookStoreWebTestModule).GetAssembly());
        }
        
        public override void PostInitialize()
        {
            IocManager.Resolve<ApplicationPartManager>()
                .AddApplicationPartsIfNotAddedBefore(typeof(BookStoreWebMvcModule).Assembly);
        }
    }
}