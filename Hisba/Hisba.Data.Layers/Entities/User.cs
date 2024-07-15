using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hisba.Data.Layers.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Index(IsUnique = true)]
        [StringLength(1000)]
        public string Username { get; set; }

        public string Password { get; set; }

        public string FullName { get; set; }

        public DateTime BirthDate { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }

        public int? CreatorId { get; set; }

        public virtual User Creator { get; set; }

        public DateTime Created { get; set; }

        public int? ModifierId { get; set; }

        public virtual User Modifier { get; set; }

        public DateTime Modified { get; set; }

        public virtual ICollection<User> CreatedUsers { get; set; }
    }
}
