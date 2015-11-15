using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Data.Repositories
{
    public interface IEntityFrameworkDbContext:IDbContext
    {
        Database Database { get; }

        /// <summary>
        /// DbContext热身 缓解第一次使用ef缓慢的问题
        /// 人为第一次DbContext进行mapping views的生成操作，后续的DbContext共享生成的mapping views
        /// </summary>
        void PreInit();
    }
}
