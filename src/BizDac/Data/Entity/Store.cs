using System.ComponentModel.DataAnnotations;

namespace Data.Entity
{
    public class Store : BaseEntity<long>
    {
        /// <summary>
        /// store name
        /// </summary>
        [StringLength(30)]
        [Required]
        public string Name { get; set; }
    }
}
