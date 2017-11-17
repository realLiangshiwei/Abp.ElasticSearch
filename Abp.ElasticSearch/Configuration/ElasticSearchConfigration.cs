namespace Abp.ElasticSearch.Configuration
{
    /// <summary>
    /// 连接配置
    /// </summary>
    public static class ElasticSearchConfiguration
    {
        /// <summary>
        /// 连接字符串支持多个节点主机 使用|进行分隔
        /// 例如 localhost:9200|localhost:8200
        /// </summary>
        public static string ConnectionString { get; set; }

        /// <summary>
        /// 授权用户名
        /// </summary>
        public static string AuthUserName { get; set; }

        /// <summary>
        /// 授权密码
        /// </summary>
        public static string AuthPassWord { get; set; }
    }
}
