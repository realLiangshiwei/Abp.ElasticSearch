using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.ElasticSearch.Configuration;
using Elasticsearch.Net;
using Nest;

namespace Abp.ElasticSearch
{
    /// <summary>
    /// AbpElasticSearchPlug
    /// </summary>
    public class AbpElasticSearchManager : IElasticsearchManager
    {
        public IElasticClient EsClient { get; }

        private readonly IElasticSearchConfiguration _elasticSearchConfiguration;

        public AbpElasticSearchManager(IElasticSearchConfiguration elasticSearchConfiguration)
        {
            _elasticSearchConfiguration = elasticSearchConfiguration;
            EsClient = GetClient();
        }

        /// <summary>
        /// GetClient
        /// </summary>
        /// <returns></returns>
        private ElasticClient GetClient()
        {
            var str = _elasticSearchConfiguration.ConnectionString;
            var strArray = str.Split('|');
            var nodes = strArray.Select(s => new Uri(s)).ToList();
            var connectionPool = new StaticConnectionPool(nodes);
            var connectionString = new ConnectionSettings(connectionPool);
            connectionString.BasicAuthentication(_elasticSearchConfiguration.AuthUserName,
                _elasticSearchConfiguration.AuthPassWord);

            return new ElasticClient(connectionString);
        }

        /// <summary>
        /// CreateEsIndex Not Mapping
        /// Auto Set Alias alias is Input IndexName
        /// </summary>
        /// <param name="indexName"></param>
        /// <param name="shard"></param>
        /// <param name="numberOfReplicas"></param>
        /// <returns></returns>
        public virtual async Task CreateIndexAsync<T>(string indexName, int shard = 1, int numberOfReplicas = 1)
            where T : class
        {
            await CreateIndexAsync<T>(indexName,
                indexSettingsDescriptor =>
                    indexSettingsDescriptor.NumberOfShards(shard).NumberOfReplicas(numberOfReplicas));
        }

        public virtual async Task CreateIndexAsync<T>(string indexName,
            Func<IndexSettingsDescriptor, IndexSettingsDescriptor> selector)
            where T : class
        {
            var exits = await EsClient.Indices.AliasExistsAsync(indexName);

            if (exits.Exists)
                return;
            var newName = indexName + DateTime.Now.Ticks;
            var result = await EsClient
                .Indices.CreateAsync(newName,
                    ss =>
                        ss.Index(newName)
                            .Settings(indexSettingsDescriptor =>
                                selector(indexSettingsDescriptor).Setting("max_result_window", int.MaxValue))
                            .Map(m => m.AutoMap<T>()));

            if (result.Acknowledged)
            {
                await EsClient.Indices.PutAliasAsync(newName, indexName);
                return;
            }

            throw new ElasticSearchException($"Create Index {indexName} failed :" + result.ServerError.Error.Reason);
        }

        /// <summary>
        /// AddOrUpdate Document
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="indexName"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task AddOrUpdateAsync<T>(string indexName, T model) where T : class
        {
            var result = await EsClient.IndexAsync(model, ss => ss.Index(indexName));
            if (result.ServerError == null) return;
            throw new ElasticSearchException($"Index Document failed at index {indexName} :" +
                                             result.ServerError.Error.Reason);
        }

        /// <summary>
        /// Bulk AddOrUpdate Document
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="indexName"></param>
        /// <param name="list"></param>
        /// <param name="bulkNum">bulkNum</param>
        /// <returns></returns>
        public virtual async Task BulkAddOrUpdateAsync<T>(string indexName, List<T> list)
            where T : class
        {
            await BulkAddOrUpdate(indexName, list);
        }

        private async Task BulkAddOrUpdate<T>(string indexName, List<T> list) where T : class
        {
            var bulk = new BulkRequest(indexName)
            {
                Operations = new List<IBulkOperation>()
            };
            foreach (var item in list)
            {
                bulk.Operations.Add(new BulkIndexOperation<T>(item));
            }

            var response = await EsClient.BulkAsync(bulk);
            if (response.Errors)
                throw new ElasticSearchException(
                    $"Bulk InsertOrUpdate Document failed at index {indexName} :{response.ServerError.Error.Reason}");
        }

        private async Task BulkDelete<T>(string indexName, List<T> list) where T : class
        {
            var bulk = new BulkRequest(indexName)
            {
                Operations = new List<IBulkOperation>()
            };
            foreach (var item in list)
            {
                bulk.Operations.Add(new BulkDeleteOperation<T>(new Id(item)));
            }

            var response = await EsClient.BulkAsync(bulk);
            if (response.Errors)
                throw new ElasticSearchException(
                    $"Bulk Delete Document at index {indexName} :{response.ServerError.Error.Reason}");
        }

        /// <summary>
        ///  Bulk Delete Document
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="indexName"></param>
        /// <param name="list"></param>
        /// <param name="bulkNum">bulkNum</param>
        /// <returns></returns>
        public virtual async Task BulkDeleteAsync<T>(string indexName, List<T> list)
            where T : class
        {
            await BulkDelete<T>(indexName, list);
        }

        /// <summary>
        /// Delete Document
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="indexName"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task DeleteAsync<T>(string indexName, T model) where T : class
        {
            var response = await EsClient.DeleteAsync(new DeleteRequest(indexName, new Id(model)));
            if (response.ServerError == null) return;
            throw new Exception($"Delete Document at index {indexName} :{response.ServerError.Error.Reason}");
        }

        /// <summary>
        /// Delete Index
        /// </summary>
        /// <param name="indexName"></param>
        /// <returns></returns>
        public virtual async Task DeleteIndexAsync(string indexName)
        {
            var response = await EsClient.Indices.DeleteAsync(indexName);
            if (response.Acknowledged) return;
            throw new Exception($"Delete index {indexName} failed :{response.ServerError.Error.Reason}");
        }

        public virtual async Task ReIndex<T>(string indexName) where T : class
        {
            await DeleteIndexAsync(indexName);
            await CreateIndexAsync<T>(indexName);
        }

        /// <summary>
        /// Non-stop Update Documents
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="indexName"></param>
        /// <returns></returns>
        public virtual async Task ReBuild<T>(string indexName) where T : class
        {
            var result = await EsClient.Indices.GetAliasAsync(indexName);
            var oldName = result.Indices.Keys.First();
            //创建新的索引
            var newIndex = indexName + DateTime.Now.Ticks;
            var createResult = await EsClient.Indices.CreateAsync(newIndex,
                c =>
                    c.Index(newIndex)
                        .Map(m => m.AutoMap<T>()));
            
            if (!createResult.Acknowledged)
            {
                throw new Exception($"reBuild create newIndex {indexName} failed :{result.ServerError.Error.Reason}");
            }

            //重建索引数据
            var reResult = await EsClient.ReindexOnServerAsync(descriptor => descriptor
                .Source(source => source.Index(indexName))
                .Destination(dest => dest.Index(newIndex)));

            if (reResult.ServerError != null)
            {
                throw new Exception($"reBuild {indexName} data failed :{reResult.ServerError.Error.Reason}");
            }

            //删除旧索引
            var deleteResult = await EsClient.Indices.DeleteAsync(oldName);
            var reAliasResult = await EsClient.Indices.PutAliasAsync(newIndex, indexName);

            if (!deleteResult.Acknowledged)
            {
                throw new Exception(
                    $"reBuild delete old Index {oldName.Name} failed :{deleteResult.ServerError.Error.Reason}");
            }

            if (!reAliasResult.IsValid)
            {
                throw new Exception($"reBuild set Alias {indexName} failed :{reAliasResult.ServerError.Error.Reason}");
            }
        }

        /// <summary>
        /// search
        /// </summary>
        /// <typeparam name="T"></typeparam>
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
        public virtual async Task<ISearchResponse<T>> SearchAsync<T>(
            string indexName, 
            SearchDescriptor<T> query = null,
            int skip = 0, 
            int size = 20,
            string[] includeFields = null,
            string preTags = "<strong style=\"color: red;\">", string postTags = "</strong>", bool disableHigh = false,
            string[] highFields = null) where T : class
        {
            if (query == null)
            {
                query = new SearchDescriptor<T>();
            }
            
            query.Index(indexName);
            var highlight = new HighlightDescriptor<T>();
            if (disableHigh)
            {
                preTags = "";
                postTags = "";
            }

            highlight.PreTags(preTags).PostTags(postTags);

            var isHigh = highFields != null && highFields.Length > 0;

            var hfs = new List<Func<HighlightFieldDescriptor<T>, IHighlightField>>();

            //分页
            query.Skip(skip).Take(size);
            //关键词高亮
            if (isHigh)
            {
                foreach (var s in highFields)
                {
                    hfs.Add(f => f.Field(s));
                }

                highlight.Fields(hfs.ToArray());
                query.Highlight(h => highlight);
            }

            
            if (includeFields != null)
                query.Source(ss => ss.Includes(ff => ff.Fields(includeFields.ToArray())));
            var response = await EsClient.SearchAsync<T>(query);
            return response;
        }

        public virtual async Task<CountResponse> CountAsync<T>(string indexName)
            where T : class
        {
            return await EsClient.CountAsync<T>(c => c.Index(indexName));
        }

        public virtual async Task<CountResponse> CountAsync<T>(string indexName,
            Func<QueryContainerDescriptor<T>, QueryContainer> query)
            where T : class
        {
            return await EsClient.CountAsync<T>(c => c.Index(indexName).Query(query));
        }
    }
}
