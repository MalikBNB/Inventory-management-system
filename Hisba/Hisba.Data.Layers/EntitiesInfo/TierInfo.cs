using Hisba.Data.Layers.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hisba.Data.Layers.EntitiesInfo
{
    public class TierInfo
    {
        public int Id { get; set; }

        public int Code { get; set; }

        public string Reference { get; set; }

        public string Name { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName { get { return FirstName + " " + LastName; } }

        public DateTime BirthDate { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public string Wilaya { get; set; }

        public string Commune { get; set; }

        public string Email { get; set; }

        public TierType Type { get; set; }

        public int? CreatorId { get; set; }

        public string Creator { get; set; }

        public DateTime Created { get; set; }

        public int? ModifierId { get; set; }

        public string Modifier { get; set; }

        public DateTime Modified { get; set; }

    }

    public enum TierType
    {
        Client = 1,
        Provider = 2
    }
}
