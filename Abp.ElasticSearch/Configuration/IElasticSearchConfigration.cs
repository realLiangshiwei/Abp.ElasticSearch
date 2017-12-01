using System;
using System.Collections.Generic;
using System.Text;

namespace Abp.ElasticSearch.Configuration
{
    public interface IElasticSearchConfigration
    {

        /// <summary>
        /// 连接字符串支持多个节点主机 使用|进行分隔
        /// 例如 localhost:9200|localhost:8200
        /// </summary>
        string ConnectionString { get; set; }

        /// <summary>
        /// 授权用户名
        /// </summary>
        string AuthUserName { get; set; }

        /// <summary>
        /// 授权密码
        /// </summary>
        string AuthPassWord { get; set; }
    }
}
