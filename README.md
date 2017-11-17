# AbpElasticSearch Plug
aspnetboilerplate repository
> https://github.com/aspnetboilerplate/aspnetboilerplate

Nuget
> https://www.nuget.org/packages/Abp.ElasticSearch/


## Get Started 
in Visual Studio,using the Package Manager Console :
> Install-Package Abp.ElasticSearch

core Proejct open Module cs file

<code>


  [DependsOn(typeof(AbpElasticSearchModule))]
  
  
   public class CodeModule : AbpModule
    {
        public override void PreInitialize()
        {
            ElasticSearchConfiguration.ConnectionString = "xxx";
            ElasticSearchConfiguration.AuthUserName = "xxx";
            ElasticSearchConfiguration.AuthPassWord = "xxx";
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            //
        }

        public override void Initialize()
        {
            // 
        }
    }
</code>
