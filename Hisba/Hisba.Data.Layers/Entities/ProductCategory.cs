using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hisba.Data.Layers.Entities
{
    public class ProductCategory
    {
        [Key]
        public int Id { get; set; }

        [Index(IsUnique = true)]
        [StringLength(1000)]
        public string Name { get; set; }

        public int? CreatorId { get; set; }

        public virtual User Creator { get; set; }

        public DateTime Created { get; set; }

        public int? ModifierId { get; set; }

        public virtual User Modifier { get; set; }

        public DateTime Modified { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
