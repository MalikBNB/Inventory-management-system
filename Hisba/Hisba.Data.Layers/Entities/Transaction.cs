using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hisba.Data.Layers.Entities
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }

        public decimal Amount { get; set; }

        public int? TierId { get; set; }

        public virtual Tier Tier { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public DateTime Date { get; set; }

        public int? TypeId { get; set; }

        public virtual TransactionType Type { get; set; }

        public int? CreatorId { get; set; }

        public virtual User Creator { get; set; }

        public DateTime Created { get; set; }

        public int? ModifierId { get; set; }

        public virtual User Modifier { get; set; }

        public DateTime Modified { get; set; }

        public ICollection<Deposit> Deposits { get; set; }
    }

    public enum PaymentMethod
    {
        Cash,
        Check
    }
}
