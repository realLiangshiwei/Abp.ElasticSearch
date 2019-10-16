namespace Abp.ElasticSearch.Configuration
{
    /// <summary>
    /// 连接配置
    /// </summary>
    public class ElasticSearchConfiguration : IElasticSearchConfiguration
    {
        /// <summary>
        /// 连接字符串支持多个节点主机 使用|进行分隔
        /// 例如 localhost:9200|localhost:8200
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 授权用户名
        /// </summary>
        public string AuthUserName { get; set; }

        /// <summary>
        /// 授权密码
        /// </summary>
        public string AuthPassWord { get; set; }
    }
}
