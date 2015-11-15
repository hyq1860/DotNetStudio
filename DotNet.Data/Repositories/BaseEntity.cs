using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace DotNet.Data.Repositories
{
    /// <summary>
    /// 实体基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseEntity<T>
    {
        public T Id { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
