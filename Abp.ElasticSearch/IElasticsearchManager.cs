﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Dependency;
using Nest;

namespace Abp.ElasticSearch
{
    /// <summary>
    /// 接口
    /// </summary>
    public interface IElasticsearchManager : ITransientDependency
    {
        /// <summary>
        /// CreateEsIndex auto Mapping T Property
        /// Auto Set Alias alias is Input IndexName
        /// </summary>
        /// <param name="indexName"></param>
        /// <param name="shard"></param>
        /// <param name="numberOfReplicas"></param>
        /// <returns></returns>
        Task CreateIndexAsync<T>(string indexName, int shard = 1, int numberOfReplicas = 1)
            where T: class;

        Task CreateIndexAsync<T>(string indexName, Func<IndexSettingsDescriptor, IndexSettingsDescriptor> selector)
            where T: class;

        /// <summary>
        /// ReIndex
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="indexName"></param>
        /// <returns></returns>
        Task ReIndex<T>(string indexName) where T : class;


        /// <summary>
        /// AddOrUpdate Document
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="indexName"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        Task AddOrUpdateAsync<T>(string indexName, T model) where T : class;


        /// <summary>
        /// Bulk AddOrUpdate Document,Default bulkNum is 1000
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="indexName"></param>
        /// <param name="list"></param>
        /// <param name="bulkNum">bulkNum</param>
        /// <returns></returns>
        Task BulkAddOrUpdateAsync<T>(string indexName, List<T> list, int bulkNum = 1000)
            where T : class;

        /// <summary>
        ///  Bulk Delete Document,Default bulkNum is 1000
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="indexName"></param>
        /// <param name="list"></param>
        /// <param name="bulkNum">bulkNum</param>
        /// <returns></returns>
        Task BulkDeleteAsync<T>(string indexName, List<T> list, int bulkNum = 1000) where T : class;

        /// <summary>
        /// Delete Document
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="indexName"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        Task DeleteAsync<T>(string indexName, T model) where T : class;

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
        /// <param name="indexName"></param>
        /// <returns></returns>
        Task ReBuild<T>(string indexName) where T : class;

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
        Task<ISearchResponse<T>> SearchAsync<T>(string indexName, SearchDescriptor<T> query,
            int skip, int size, string[] includeFields = null, string preTags = "<strong style=\"color: red;\">",
            string postTags = "</strong>", bool disableHigh = false, params string[] highField)
            where T : class;

        Task<CountResponse> CountAsync<T>(string indexName,
            Func<QueryContainerDescriptor<T>, QueryContainer> query) where T : class;
    }
}