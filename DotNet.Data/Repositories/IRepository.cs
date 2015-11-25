using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Data
{
    //http://www.entityframeworktutorial.net/
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T> where T:class
    {
        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Add(T entity);

        /// <summary>
        /// 批量添加实体
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="useTransaction">是否启用事物 默认不启用</param>
        /// <returns></returns>
        int AddRange(IEnumerable<T> entities,bool useTransaction=false);

        /// <summary>
        /// 更新实体集合
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Update(T entity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="useTransaction">是否启用事物</param>
        /// <param name="entities"></param>
        /// <returns></returns>
        int AddOrUpdate(bool useTransaction = false, params T[] entities);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="func"></param>
        /// <param name="useTransaction">是否启用事物</param>
        /// <param name="entities"></param>
        /// <returns></returns>
        int AddOrUpdate(Expression<Func<T, object>> func, bool useTransaction = false, params T[] entities);

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity"></param>
        int Delete(T entity);
        
        /// <summary>
        /// 根据条件删除实体
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        int DeleteByCondition(Expression<Func<T, bool>> func);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T GetById(long id);

        T GetById(string id);

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <returns></returns>
        IQueryable<T> GetList();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="whereFunc"></param>
        /// <param name="orderByFunc"></param>
        /// <param name="orderType"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IQueryable<T> GetPagedList(Expression<Func<T, bool>> whereFunc, Expression<Func<T, long>> orderByFunc,string orderType,int pageNumber, int pageSize); 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        void Log(string message);
    }
}
