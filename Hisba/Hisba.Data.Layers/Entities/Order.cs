using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hisba.Data.Layers.Entities
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public string Reference { get; set; }

        public int? TierId { get; set; }

        public virtual Tier Tier { get; set; }

        public int? OrderTypeId { get; set; }

        public virtual OrderType OrderType { get; set; }

        public DateTime Date { get; set; }

        public decimal TotalHT { get; set; }

        public decimal TotalTVA { get; set; }

        public decimal TotalTTC { get; set; }

        public bool Status { get; set; }

        public int? CreatorId { get; set; }

        public virtual User Creator { get; set; }

        public DateTime Created { get; set; }

        public int? ModifierId { get; set; }

        public virtual User Modifier { get; set; }

        public DateTime Modified { get; set; }

        public ICollection<OrderItem> Items { get; set; }

        public ICollection<Deposit> Deposits { get; set; }

    }
}
