using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Data.Entity
{
    public class BaseEntity<T>
    {
        public BaseEntity()
        {
            CreteTime = DateTime.Now;
        }
        /// <summary>
        /// pk id
        /// </summary>
        [DataMember]
        [Key]
        [Column("idx")]
        public T Id { get; set; }

        /// <summary>
        /// DB版号,Mysql详情参考;http://www.cnblogs.com/shanyou/p/6241612.html
        /// </summary>
        //[ConcurrencyCheck]
        //public DateTime RowVersion { get; set; }

        /// <summary>
        /// CreteTime
        /// </summary>
        [Column("create_time")]
        public DateTime CreteTime { get; set; }
    }
}
