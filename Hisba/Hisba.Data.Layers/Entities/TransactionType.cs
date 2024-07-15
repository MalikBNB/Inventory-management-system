using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hisba.Data.Layers.Entities
{
    public class TransactionType
    {
        [Key]
        public int Id { get; set; }

        public TransactionTypeCode Code { get; set; }

        public string Label { get; set; }

        public bool isIn { get; set; }

        public int? CreatorId { get; set; }

        public virtual User Creator { get; set; }

        public DateTime Created { get; set; }

        public int? ModifierId { get; set; }

        public virtual User Modifier { get; set; }

        public DateTime Modified { get; set; }

        public ICollection<Transaction> Transactions { get; set; }

    }

    public enum TransactionTypeCode
    {
        Collection,
        Disbursement
    }
}
