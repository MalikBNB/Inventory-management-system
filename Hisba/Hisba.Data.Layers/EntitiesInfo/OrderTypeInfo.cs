using Hisba.Data.Layers.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hisba.Data.Layers.EntitiesInfo
{
    public class OrderTypeInfo
    {
        public int Id { get; set; }

        public OrderTypeCode Code { get; set; }

        public string Label { get; set; }

        public bool isIn { get; set; }

        public short Sign { get; set; }

        public int CreatorId { get; set; }

        public string Creator { get; set; }

        public DateTime Created { get; set; }

        public int ModifierId { get; set; }

        public string Modifier { get; set; }

        public DateTime Modified { get; set; }

        public ICollection<Order> Orders { get; set; }
    }

    public enum OrderTypeCode
    {
        GoodsReceipt = 1,
        PurchaseReturnOrder = 2,
        DeliveryNote = 3,
        SaleReturnOrder = 4,
    }
}

