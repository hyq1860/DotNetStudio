using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Data
{
    public interface IDbContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        DbSet<T> Set<T>() where T : class;

        DbEntityEntry Entry<T>(T entity) where T : class;

        IEnumerable<T> SqlQuery<T>(string sql, params object[] parameters);

        void ExecuteSqlCommand(string sql, params object[] parameters);

        int SaveChanges();

        void Log(string message);
    }
}
