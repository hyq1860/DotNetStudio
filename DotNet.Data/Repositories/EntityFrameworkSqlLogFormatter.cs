using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Data.Repositories
{
    public class EntityFrameworkSqlLogFormatter:DatabaseLogFormatter
    {
        public EntityFrameworkSqlLogFormatter(DbContext context, Action<string> writeAction)
            : base(context, writeAction)
        {   
        }

        public EntityFrameworkSqlLogFormatter(Action<string> writeAction) : base(writeAction)
        {
        }

        public override void LogCommand<T>(DbCommand command,DbCommandInterceptionContext<T> interceptionContext)
        {
            Write(string.Format("Context '{0}' is executing command '{1}' '{2}'",
                Context.GetType().Name,
                command.CommandText.Replace(Environment.NewLine,""),
                Environment.NewLine));
        }
    }
}
