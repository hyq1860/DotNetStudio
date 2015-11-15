using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using MySql.Data.Entity;

namespace DotNet.Data.Repositories
{
    /// <summary>
    /// ef 自定义配置
    /// </summary>
    public class EnfityFrameworkDbConfiguration:DbConfiguration
    {
        public EnfityFrameworkDbConfiguration()
        {
            SetDatabaseLogFormatter((context, writeAction) => new EntityFrameworkSqlLogFormatter(context, writeAction));
            //DbInterception.Remove(new DbCommandInterceptor());
            //DbInterception.Add(new Log4NetCommandInterceptor());
        }
    }
}
