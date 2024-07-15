using System;
using System.ComponentModel.DataAnnotations;

namespace Hisba.Data.Layers.Entities
{
    public class Deposit
    {
        [Key]
        public int Id { get; set; }

        public int? OrderId { get; set; }

        public virtual Order Order { get; set; }

        public int? TransactionId { get; set; }

        public virtual Transaction Transaction { get; set; }

        public decimal Slice { get; set; }

        public int? CreatorId { get; set; }

        public virtual User Creator { get; set; }

        public DateTime Created { get; set; }

        public int? ModifierId { get; set; }

        public virtual User Modifier { get; set; }

        public DateTime Modified { get; set; }
    }
}
