using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.MappingViews;
using System.Data.Entity.Migrations.Model;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DotNet.Data.Repositories;

//using MySql.Data.Entity;

namespace DotNet.Data
{
    //http://www.entityframeworktutorial.net/ ef官方示例 重点推荐
    //https://msdn.microsoft.com/en-us/data/ee712907 ef官方开发论坛
    //http://www.cnblogs.com/lonelyxmas/category/426666.html ef 博客园教程
    //[DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class EntityFrameworkDbContext:DbContext, IEntityFrameworkDbContext
    {
        static EntityFrameworkDbContext()
        {
            //http://www.cnblogs.com/dudu/archive/2011/12/27/entity_framework_sys_databases.html
            //让Entity Framework不再私闯sys.databases
            //Database.SetInitializer<EntityFrameworkDbContext>(null);
            //var db = new CreateDatabaseIfNotExists<EntityFrameworkDbContext>();
            //Database.SetInitializer<EntityFrameworkDbContext>(db);
            //https://efbulkinsert.codeplex.com/documentation
            //http://www.cnblogs.com/gaochundong/p/entity_framework_bulk_insert_extension.html
            //https://efbulkinsert.codeplex.com/discussions/549130
            
        }

        /// <summary>
        /// 一次性加载所有映射
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Assembly.GetExecutingAssembly()
            /*
            if (base.Database != null&&base.Database.Connection!=null &&base.Database.Connection.GetType().FullName== "MySql.Data.MySqlClient.MySqlConnection")
            {
                EntityFramework.BulkInsert.ProviderFactory.Register<MySqlBulkInsertProvider>("MySql.Data.MySqlClient.MySqlConnection");
            }
            */
            var typesToRegister = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(s=>s.FullName.StartsWith("DotNet.CloudFarm.Domain.Model")).GetTypes()
            .Where(type => !String.IsNullOrEmpty(type.Namespace))
            .Where(type => type.BaseType != null && type.BaseType.IsGenericType &&
                type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));
            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(configurationInstance);
            }

            base.OnModelCreating(modelBuilder);
        }

        public EntityFrameworkDbContext(string connectionString) : this(connectionString,true)
        {
            //this.database = base.Database;
            //this.dbConnection = this.Database.Connection;
            this.Database.Log = Log;
            
        }

        public EntityFrameworkDbContext(string connectionString, bool autoDetectChangesEnabled) : base(connectionString)
        {
            this.Configuration.AutoDetectChangesEnabled = autoDetectChangesEnabled;
        }

        /*
        public EntityFrameworkRepositoryContext(DbConnection dbConnection, bool contextOwnsConnection):base(dbConnection,contextOwnsConnection)
        {
            this.dbConnection = dbConnection;
            this.Database.Log = Log;
        }
        */

        /*
        public EntityFrameworkRepositoryContext(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        private readonly DbContext dbContext;

        public DbContext DbContext
        {
            get { return this.dbContext; }
        }

        */

        /*
        private IDbConnection dbConnection;

        private IDbTransaction dbTransaction;

        private Database database;

        private DbContext dbContext;

        public DbContext DbContext
        {
            get { return this.dbContext; } 
        }

        public Database Database
        {
            get { return this.database; }
        }
        */
        public new DbSet<T> Set<T>() where T : class
        {
            return base.Set<T>();
        }

        public new DbEntityEntry Entry<TEntity>(TEntity entity) where TEntity : class
        {
            return base.Entry<TEntity>(entity);
        }

        public IEnumerable<TEntity> SqlQuery<TEntity>(string sql, params object[] parameters)
        {
            
            //ObjectQuery<TEntity> objectQuery=new ObjectQuery<TEntity>(sql,this as ObjectContext);
            return base.Database.SqlQuery<TEntity>(sql, parameters);
        }

        public int ExecuteSqlCommand(string sql, params object[] parameters)
        {
            return base.Database.ExecuteSqlCommand(sql, parameters);
        }


        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="message"></param>
        public void Log(string message)
        {
            //Console.WriteLine(message);
        }

        /// <summary>
        /// 为了防止第一个人
        /// https://msdn.microsoft.com/en-us/data/dn469601 官方解释说明 Pre 
        /// </summary>
        public void PreInit()
        {
            var objectContext = ((IObjectContextAdapter)this).ObjectContext;
            var mappingCollection = (StorageMappingItemCollection)objectContext.MetadataWorkspace.GetItemCollection(DataSpace.CSSpace);
            mappingCollection.GenerateViews(new List<EdmSchemaError>());
        }

        void IDbContext.ExecuteSqlCommand(string sql, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        /*
        public DbMappingView GetView(EntitySetBase extent)
        {
            if (extent == null)
            {
                throw new ArgumentNullException("extent");
            }

            var extentName = extent.EntityContainer.Name + "." + extent.Name;

            return null;
        }

        */

        /*
        EntityFramework相关的官方资料
        Registering EF providers
        Config file registration
        Code-based registration
        https://msdn.microsoft.com/en-us/data/jj730568

        Code First Insert/Update/Delete Stored Procedures
        https://msdn.microsoft.com/en-us/data/dn468673
        
        Map an Entity to Multiple Tables (Entity Splitting)
        https://msdn.microsoft.com/en-us/data/jj715646
        */

        //public void BulkInsert<T>(IEnumerable<T> entities) where T : class
        //{
        //    this.BulkInsert(entities,new BulkInsertOptions() {BatchSize = 100});
        //}
    }
}
