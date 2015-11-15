using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFramework.BulkInsert.Extensions;
using EntityFramework.BulkInsert.Helpers;
using EntityFramework.BulkInsert.Providers;
using EntityFramework.MappingAPI;
using MySql.Data.MySqlClient;

namespace DotNet.Data.Repositories
{
    public class MySqlBulkInsertProvider: ProviderBase<MySqlConnection, MySqlTransaction>
    {
        private const string FieldTerminator = "\t";
        private const string LineTerminator = "\n";

        public override object GetSqlGeography(string wkt, int srid)
        {
            throw new NotImplementedException();
        }

        protected override MySqlConnection CreateConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        protected override string ConnectionString
        {
            get { return DbConnection.ConnectionString; }
        }

        public override void Run<T>(IEnumerable<T> entities, MySqlTransaction transaction, BulkInsertOptions options)
        {
            var keepIdentity = (SqlBulkCopyOptions.KeepIdentity & options.SqlBulkCopyOptions) > 0;

            using (var reader = new MappedDataReader<T>(entities, this))
            {
                var csvPath = AppDomain.CurrentDomain.BaseDirectory+System.Guid.NewGuid().ToString()+".csv";

                Dictionary<int, IPropertyMap> propertyMaps = reader.Cols
                    .Where(x => !x.Value.IsIdentity || keepIdentity)
                    .ToDictionary(x => x.Key, x => x.Value);

                var csv = new StringBuilder();

                while (reader.Read())
                {
                    foreach (var kvp in propertyMaps)
                    {
                        var value = reader.GetValue(kvp.Key);
                        if (value == null)
                        {
                            csv
                                .Append("null")
                                .Append(FieldTerminator);
                        }
                        else
                        {
                            // todo - escape "'"
                            csv
                                .AppendFormat("{0}", value)
                                .Append(FieldTerminator);
                        }

                    }
                    csv.Append(LineTerminator);
                }

                // todo - save csv
                File.WriteAllText(csvPath,csv.ToString());

                // upload csv
                var mySqlBulkLoader = new MySqlBulkLoader(transaction.Connection);
                //mySqlBulkLoader.TableName = string.Format("[{0}].[{1}]", reader.SchemaName, reader.TableName);
                mySqlBulkLoader.TableName = reader.TableName;
                mySqlBulkLoader.FieldTerminator = FieldTerminator;
                mySqlBulkLoader.LineTerminator = LineTerminator;
                mySqlBulkLoader.FileName = csvPath;

                foreach (var kvp in propertyMaps)
                {
                    mySqlBulkLoader.Columns.Add(kvp.Value.ColumnName);
                }

                int count = mySqlBulkLoader.Load();

                try
                {
                    File.Delete(csvPath);
                }
                catch (Exception exception)
                {
                }
            }
        }
    }
}
