using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hisba.Data.Layers.Entities
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }

        public int? OrderId { get; set; }

        public virtual Order Order { get; set; }

        public int? ProductId { get; set; }

        public virtual Product Product { get; set; }

        public string Quantity { get; set; }

        public decimal PriceHT { get; set; }

        public float TVA { get; set; }

        public float Discount { get; set; }

        [NotMapped]
        public decimal PriceTTC { get; set; }

        public int? CreatorId { get; set; }

        public virtual User Creator { get; set; }

        public DateTime Created { get; set; }

        public int? ModifierId { get; set; }

        public virtual User Modifier { get; set; }

        public DateTime Modified { get; set; }
    }
}
