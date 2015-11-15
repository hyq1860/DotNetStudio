using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNet.Common.Logging;

namespace DotNet.Data.Repositories
{
    public class Log4NetCommandInterceptor:IDbCommandInterceptor
    {
        public void NonQueryExecuting(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            LogIfNonAsync(command, interceptionContext);
        }

        public void NonQueryExecuted(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            LogIfError(command, interceptionContext);
        }

        public void ReaderExecuting(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            LogIfNonAsync(command, interceptionContext);
        }

        public void ReaderExecuted(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            LogIfError(command, interceptionContext);
        }

        public void ScalarExecuting(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            LogIfNonAsync(command, interceptionContext);
        }

        public void ScalarExecuted(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            LogIfError(command, interceptionContext);
        }

        private void LogIfNonAsync<TResult>(
        DbCommand command, DbCommandInterceptionContext<TResult> interceptionContext)
        {
            if (!interceptionContext.IsAsync)
            {
                Logger.Log(string.Format("Non-async command used: {0}", command.CommandText));
            }
        }

        private void LogIfError<TResult>(
            DbCommand command, DbCommandInterceptionContext<TResult> interceptionContext)
        {
            if (interceptionContext.Exception != null)
            {
                Logger.Log(string.Format("Command {0} failed with exception {1}",
                    command.CommandText, interceptionContext.Exception));
            }
        }
    }
}
