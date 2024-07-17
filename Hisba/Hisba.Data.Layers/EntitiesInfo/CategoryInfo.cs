using Hisba.Data.Layers.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hisba.Data.Layers.EntitiesInfo
{
    public class CategoryInfo
    {
        public int Id { get; set; }

        public int Code { get; set; }

        public string Name { get; set; }

        public int CreatorId { get; set; }

        public string Creator { get; set; }

        public DateTime Created { get; set; }

        public int ModifierId { get; set; }

        public string Modifier { get; set; }

        public DateTime Modified { get; set; }
    }
}
