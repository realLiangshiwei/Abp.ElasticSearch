# AbpElasticSearch Module
aspnetboilerplate repository
> https://github.com/aspnetboilerplate/aspnetboilerplate

Nuget
> https://www.nuget.org/packages/Abp.ElasticSearch/


## Get Started 
in Visual Studio,using the Package Manager Console :
> Install-Package Abp.ElasticSearch

core Proejct open Module cs file

``` csharp
[DependsOn(typeof(AbpElasticSearchModule))]
public class CodeModule : AbpModule
{

    public override void PreInitialize()
    {
        Configuration.Modules.ElasticSearch().ConnectionString = "your connection string";
        Configuration.Modules.ElasticSearch().AuthUserName = "your auth username";
        Configuration.Modules.ElasticSearch().AuthPassWord = "your auth password";
        //
    }

    public override void Initialize()
    {
        // 
        IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
    }
}
```

### API List

``` csharp
/// <summary>
/// CreateEsIndex Not Mapping
/// Auto Set Alias alias is Input IndexName
/// </summary>
/// <param name="indexName"></param>
/// <param name="shard"></param>
/// <param name="numberOfReplicas"></param>
/// <returns></returns>
Task CrateIndexAsync(string indexName, int shard = 1, int numberOfReplicas = 1);

/// <summary>
/// CreateEsIndex auto Mapping T Property
/// Auto Set Alias alias is Input IndexName
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TKey"></typeparam>
/// <param name="indexName"></param>
/// <param name="shard"></param>
/// <param name="numberOfReplicas"></param>
/// <returns></returns>
Task CreateIndexAsync<T, TKey>(string indexName, int shard = 1, int numberOfReplicas = 1) where T : EntityDto<TKey>;

/// <summary>
/// ReIndex
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TKey"></typeparam>
/// <param name="indexName"></param>
/// <returns></returns>
Task ReIndex<T, TKey>(string indexName) where T : EntityDto<TKey>;


/// <summary>
/// AddOrUpdate Document
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TKey"></typeparam>
/// <param name="indexName"></param>
/// <param name="model"></param>
/// <returns></returns>
Task AddOrUpdateAsync<T, TKey>(string indexName, T model) where T : EntityDto<TKey>;


/// <summary>
/// Bulk AddOrUpdate Document,Default bulkNum is 1000
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TKey"></typeparam>
/// <param name="indexName"></param>
/// <param name="list"></param>
/// <param name="bulkNum">bulkNum</param>
/// <returns></returns>
Task BulkAddorUpdateAsync<T, TKey>(string indexName, List<T> list, int bulkNum = 1000) where T : EntityDto<TKey>;

/// <summary>
///  Bulk Delete Document,Default bulkNum is 1000
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TKey"></typeparam>
/// <param name="indexName"></param>
/// <param name="list"></param>
/// <param name="bulkNum">bulkNum</param>
/// <returns></returns>
Task BulkDeleteAsync<T, TKey>(string indexName, List<T> list, int bulkNum = 1000) where T : EntityDto<TKey>;

/// <summary>
/// Delete Document
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TKey"></typeparam>
/// <param name="indexName"></param>
/// <param name="typeName"></param>
/// <param name="model"></param>
/// <returns></returns>
Task DeleteAsync<T, TKey>(string indexName, T model) where T : EntityDto<TKey>;

/// <summary>
/// Delete Index
/// </summary>
/// <param name="indexName"></param>
/// <returns></returns>
Task DeleteIndexAsync(string indexName);


/// <summary>
/// Non-stop Update Documents
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TKey"></typeparam>
/// <param name="indexName"></param>
/// <returns></returns>
Task ReBuild<T, TKey>(string indexName) where T : EntityDto<TKey>;

/// <summary>
/// search
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TKey"></typeparam>
/// <param name="indexName"></param>
/// <param name="query"></param>
/// <param name="skip">skip num</param>
/// <param name="size">return document size</param>
/// <param name="includeFields">return fields</param>
/// <param name="preTags">Highlight tags</param>
/// <param name="postTags">Highlight tags</param>
/// <param name="disableHigh"></param>
/// <param name="highField">Highlight fields</param>
/// <returns></returns>
Task<ISearchResponse<T>> SearchAsync<T, TKey>(string indexName, SearchDescriptor<T> query,
    int skip, int size, string[] includeFields = null, string preTags = "<strong style=\"color: red;\">",
    string postTags = "</strong>", bool disableHigh = false, params string[] highField) where T : EntityDto<TKey>;

```
