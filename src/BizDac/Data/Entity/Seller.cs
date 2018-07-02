using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entity
{
    [Table("io_seller")]
    public class Seller : BaseEntity <Int32>
    {
        /// <summary>
        /// store name
        /// </summary>
        [StringLength(20)]
        [Required]
        [Column("seller_code")]
        public string Code { get; set; }
        [StringLength(30)]
        [Required]
        [Column("seller_name")]
        public string Name { get; set; }
    }
}
