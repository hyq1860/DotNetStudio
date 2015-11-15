using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Data.Repositories
{
    public static class EntityFrameworkExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tracker"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetEntities<T>(this DbChangeTracker tracker)
        {
            var entities = tracker.Entries().Where(entry => entry.State != EntityState.Detached && entry.Entity != null)
                .Select(entry => entry.Entity).OfType<T>();
            return entities;
        }

        public static string GetTableName<T>(this DbContext context)
            where T : class
        {
            var entitySet = GetEntitySet<T>(context);
            if (entitySet == null)
                throw new Exception(string.Format("Unable to find entity set '{0}' in edm metadata",(typeof(T).Name)));
            var tableName = GetStringProperty(entitySet, "Schema") + "." + GetStringProperty(entitySet, "Table");
            return tableName;
        }

        private static EntitySet GetEntitySet<T>(this DbContext context)
        {
            var type = typeof(T);
            var entityName = type.Name;
            var metadata = ((IObjectContextAdapter)context).ObjectContext.MetadataWorkspace;

            IEnumerable<EntitySet> entitySets;
            entitySets = metadata.GetItemCollection(DataSpace.SSpace)
                             .GetItems<EntityContainer>()
                             .Single()
                             .BaseEntitySets
                             .OfType<EntitySet>()
                             .Where(s => !s.MetadataProperties.Contains("Type")
                                         || s.MetadataProperties["Type"].ToString() == "Tables");
            var entitySet = entitySets.FirstOrDefault(t => t.Name == entityName);
            return entitySet;
        }

        private static string GetStringProperty(MetadataItem entitySet, string propertyName)
        {
            MetadataProperty property;
            if (entitySet == null)
                throw new ArgumentNullException("entitySet");
            if (entitySet.MetadataProperties.TryGetValue(propertyName, false, out property))
            {
                string str = null;
                if (((property != null) &&
                    (property.Value != null)) &&
                    (((str = property.Value as string) != null) &&
                    !string.IsNullOrEmpty(str)))
                {
                    return str;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 向上遍历所有父节点
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="currentItem"></param>
        /// <param name="parentSelector"></param>
        /// <param name="isCurrentItem"></param>
        /// <returns></returns>
        public static IEnumerable<T> Traverse<T>(this T currentItem, Func<T, T> parentSelector,Func<T,bool> isCurrentItem)
        {
            if (currentItem == null)
            {
                yield break;
            }
            var stack = new Stack<T>();
            stack.Push(currentItem);
            while (stack.Any())
            {
                var item = stack.Pop();
                if (!isCurrentItem(item))
                {
                    yield return item;
                }
                
                // get item's parent
                var parent = parentSelector(item);
                if (parent != null)
                {
                    // push parent in stack
                    // find parent's parent later
                    stack.Push(parent);
                }
            }

        }

        /// <summary>
        /// 遍历子节点
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="currentItem"></param>
        /// <param name="childSelector"></param>
        /// <param name="isCurrentItem"></param>
        /// <returns></returns>
        public static IEnumerable<T> Traverse<T>(this T currentItem, Func<T, IEnumerable<T>> childSelector,Func<T,bool> isCurrentItem)
        {
            if (currentItem == null)
            {
                yield break;
            }
            var stack = new Stack<T>();
            stack.Push(currentItem);
            while (stack.Any())
            {
                var item = stack.Pop();
                if(!isCurrentItem(item))
                {
                    yield return item;
                }
                
                // get item's children
                foreach (var child in childSelector(item).Reverse())
                {
                    // push each child in stack
                    // find child's children later
                    stack.Push(child);
                }
            }
            
            /*
            if (items == null)
            {
                yield break;
            }

            foreach (var item in items)
            {
                yield return item;

                var children = Traverse(childSelector(item), childSelector);
                foreach (var childItem in children)
                {
                    yield return childItem;
                }
            }
            */
        }
    }
}
