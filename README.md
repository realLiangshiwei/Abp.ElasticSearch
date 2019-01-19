# AbpElasticSearch Plug
aspnetboilerplate repository
> https://github.com/aspnetboilerplate/aspnetboilerplate

Nuget
> https://www.nuget.org/packages/Abp.ElasticSearch/


## Get Started 
in Visual Studio,using the Package Manager Console :
> Install-Package Abp.ElasticSearch

core Proejct open Module cs file

    [DependsOn(typeof(AbpElasticSearchModule))]
    public class CodeModule : AbpModule
    {

        public override void PreInitialize()
        {
            Configuration.Modules.ElasticSearch().ConnectionString = "your collectionstring";
            Configuration.Modules.ElasticSearch().AuthUserName = "your authusername";
            Configuration.Modules.ElasticSearch().AuthPassWord = "your authpassword";
            //
        }

        public override void Initialize()
        {
            // 
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
 
