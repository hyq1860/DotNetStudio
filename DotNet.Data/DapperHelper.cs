using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Data
{
    public class DapperHelper
    {
        private static IDbConnection CreateConnection(string connectionString)
        {
            var conn = new SqlConnection(connectionString);
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }

            return conn;
        }

        /// <summary>
        /// 执行增、删、改方法
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string connectionString, string sql, object parameters = null)
        {
            using (IDbConnection conn = CreateConnection(connectionString))
            {
                return conn.Execute(sql, parameters);
            }
        }

        /// <summary>
        /// 得到单行单列
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static object ExecuteScalar(string connectionString, string sql, object parameters = null)
        {
            using (IDbConnection conn = CreateConnection(connectionString))
            {
                return conn.ExecuteScalar(sql, parameters);
            }
        }

        /// <summary>
        /// 单个数据集查询
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="connectionString"></param>
        /// <param name="sql"></param>
        /// <param name="where"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static List<TEntity> Query<TEntity>(string connectionString, string sql, Func<TEntity, bool> where, object parameters = null)
        {
            using (IDbConnection conn = CreateConnection(connectionString))
            {
                return conn.Query<TEntity>(sql, parameters).Where(where).ToList();
            }
        }

        /// <summary>
        /// 单个数据集查询
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="connectionString"></param>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static List<TEntity> Query<TEntity>(string connectionString, string sql, object parameters = null)
        {
            using (IDbConnection conn = CreateConnection(connectionString))
            {
                return conn.Query<TEntity>(sql, parameters).ToList();
            }
        }

        /// <summary>
        /// 多个数据集查询
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="sql"></param>
        /// <param name="parameters">DynamicParameters</param>
        /// <returns></returns>
        public static SqlMapper.GridReader MultyQuery(string connectionString, string sql, object parameters = null)
        {
            /*
            var p = new DynamicParameters();
            p.Add("a", 11);
            p.Add("r", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
            */
            using (IDbConnection conn = CreateConnection(connectionString))
            {
                return conn.QueryMultiple(sql, parameters);
            }
        }
    }
}
