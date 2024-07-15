using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hisba.Data.Layers.Entities
{
    public class Tier
    {
        [Key]
        public int Id { get; set; }

        public int Code { get; set; }

        public string Reference { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName { get { return FirstName + " " + LastName; } }

        public DateTime BirthDate { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string Email { get; set; }

        public TierType TierType { get; set; }

        public byte[] Image { get; set; }

        public int? CreatorId { get; set; }

        public virtual User Creator { get; set; }

        public DateTime Created { get; set; }

        public int? ModifierId { get; set; }

        public virtual User Modifier { get; set; }

        public DateTime Modified { get; set; }


        public ICollection<Order> Orders { get; set; }

        public ICollection<Transaction> Transactions { get; set; }

    }

    public enum TierType
    {
        Client = 1,
        Supplier = 2
    }
}
