using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
//using System.Transactions;
using DotNet.Data.Repositories;
//using EntityFramework.Extensions;

namespace DotNet.Data
{
    public class GenericRepository<T> :IRepository<T> where T:class 
    {
        private IEntityFrameworkDbContext context;

        private bool isNoTracking;

        /// <summary>
        /// 获取实体集合
        /// </summary>
        private DbSet<T> Entities
        {
            get
            {
                return context.Set<T>();
            }
        }

        public GenericRepository(IEntityFrameworkDbContext context,bool isNoTracking = true)
        {
            this.context = context;
            this.isNoTracking = isNoTracking;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Add(T entity)
        {
            Entities.Add(entity);
            return context.SaveChanges();
        }

        /// <summary>
        /// https://msdn.microsoft.com/en-us/data/dn456843
        /// http://stackoverflow.com/questions/10585478/one-dbcontext-per-web-request-why
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="useTransaction"></param>
        /// <returns></returns>
        public int AddRange(IEnumerable<T> entities,bool useTransaction = false)
        {
            int result = 0;
            if (useTransaction)
            {
                using (var dbConnection = context.Database.Connection)
                {
                    if (dbConnection.State == ConnectionState.Closed)
                    {
                        dbConnection.Open();
                    }
                    context.Database.UseTransaction(null);
                    using (var dbTransaction = dbConnection.BeginTransaction())
                    {
                        try
                        {
                            context.Database.UseTransaction(dbTransaction);
                            foreach (var entity in entities)
                            {
                                this.Entities.Add(entity);
                            }
                            result = context.SaveChanges();
                            dbTransaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            dbTransaction.Rollback();
                        }
                        return result;
                    }
                }
            }
            else
            {
                foreach (var entity in entities)
                {
                    this.Entities.Add(entity);
                }
                result = context.SaveChanges();
                return result;
            }


            /*
            www.codeproject.com/Articles/690136/All-About-TransactionScope
            using (TransactionScope transaction = new TransactionScope())
            {
                foreach (var entity in entities)
                {
                    this.Entities.Add(entity);
                }
                result = context.SaveChanges();
                //提交事物
                transaction.Complete();
                return result;
            }
            */
        }

        public int Update(T entity)
        {
            if (!isNoTracking)
            {
                return this.context.SaveChanges();
            }
            else
            {
                this.context.Entry(entity).State = EntityState.Modified;
            }
                
            return this.context.SaveChanges();
        }

        public int AddOrUpdate(bool useTransaction = false,params T[] entities)
        {
            if (useTransaction)
            {
                var result = 0;
                using (var dbConnection = context.Database.Connection)
                {
                    if (dbConnection.State == ConnectionState.Closed)
                    {
                        dbConnection.Open();
                    }
                    context.Database.UseTransaction(null);
                    using (var dbTransaction = dbConnection.BeginTransaction())
                    {
                        try
                        {
                            context.Database.UseTransaction(dbTransaction);
                            foreach (var entity in entities)
                            {
                                this.Entities.AddOrUpdate(entity);
                            }
                            result = context.SaveChanges();
                            dbTransaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            dbTransaction.Rollback();
                        }
                        return result;
                    }
                }
            }
            else
            {
                Entities.AddOrUpdate(entities);
                return this.context.SaveChanges();
            }
        }

        /// <summary>
        /// AddOrUpdate(p => p.Id, people)
        /// AddOrUpdate(p => new { p.FirstName, p.LastName }, people)
        /// </summary>
        /// <param name="func"></param>
        /// <param name="entities"></param>
        /// <returns></returns>
        public int AddOrUpdate(Expression < Func<T, object>> func, bool useTransaction = false, params T[] entities)
        {
            if (useTransaction)
            {
                var result = 0;
                using (var dbConnection = context.Database.Connection)
                {
                    if (dbConnection.State == ConnectionState.Closed)
                    {
                        dbConnection.Open();
                    }
                    context.Database.UseTransaction(null);
                    using (var dbTransaction = dbConnection.BeginTransaction())
                    {
                        try
                        {
                            context.Database.UseTransaction(dbTransaction);
                            foreach (var entity in entities)
                            {
                                this.Entities.AddOrUpdate(func,entity);
                            }
                            result = context.SaveChanges();
                            dbTransaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            dbTransaction.Rollback();
                        }
                        return result;
                    }
                }
            }
            else
            {
                Entities.AddOrUpdate(func, entities);
                return this.context.SaveChanges();
            }
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity"></param>
        public int Delete(T entity)
        {
            if (!isNoTracking)
            {
                this.Entities.Remove(entity);
            }
            else
            {
                this.Entities.Attach(entity);
            }

            this.Entities.Remove(entity);
            return this.context.SaveChanges();
        }

        public int DeleteByCondition(Expression<Func<T, bool>> func)
        {
            var list = GetList().Where(func).ToList();
            list.ForEach(e =>
            {
                if (!isNoTracking)
                {
                    this.Entities.Remove(e);
                }
                else
                {
                    this.Entities.Attach(e);
                }
                this.Entities.Remove(e);
            });

            return this.context.SaveChanges();
        }

        public T GetById(long id)
        {
            return Entities.Find(id);
        }

        public T GetById(string id)
        {
            return Entities.Find(id);
        }

        public IQueryable<T> GetList()
        {
            if (!isNoTracking)
            {
                return this.Entities.AsQueryable();
            }
            else
            {
                return this.Entities.AsNoTracking().AsQueryable();
            }   
        }

        //http://www.cnblogs.com/xishuai/p/repository-return-iqueryable-or-ienumerable.html
        //http://stackoverflow.com/questions/8190480/how-to-pass-multiple-expressions-to-orderby-for-ef
        public IQueryable<T> GetPagedList(Expression<Func<T,bool>> whereFunc, Expression<Func<T, long>> orderByFunc,string orderType,int pageNumber, int pageSize)
        {
            /*
            using (var t = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions
                {
                    IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                }))
            {
                
            }
            */

            if (pageNumber <= 0)
                throw new ArgumentOutOfRangeException("pageNumber", pageNumber, "The pageNumber is one-based and should be larger than zero.");
            if (pageSize <= 0)
                throw new ArgumentOutOfRangeException("pageSize", pageSize, "The pageSize is one-based and should be larger than zero.");

            IQueryable<T> queryable = this.Entities;
            if (whereFunc != null)
            {
                queryable = queryable.Where(whereFunc);
            }
            if (orderByFunc != null)
            {
                if (string.IsNullOrEmpty(orderType) || orderType.ToLower() == "desc")
                {
                    queryable = queryable.OrderByDescending(orderByFunc);
                }
                else
                {
                    queryable = queryable.OrderBy(orderByFunc);
                }
            }
            else
            {
                throw new Exception("orderByFunc can be null");
            }

            return queryable.Skip(pageSize * (pageNumber - 1)).Take(pageSize);
        }



        public void Log(string message)
        {
            
        }
    }
}
