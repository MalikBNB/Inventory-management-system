using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hisba.Data.Layers.Entities
{
    public class OrderType
    {
        [Key]
        public int Id { get; set; }

        public OrderTypeCode Code { get; set; }

        public string Label { get; set; }

        public bool isIn { get; set; }

        public byte Sign { get; set; }

        public int? CreatorId { get; set; }

        public virtual User Creator { get; set; }

        public DateTime Created { get; set; }

        public int? ModifierId { get; set; }

        public virtual User Modifier { get; set; }

        public DateTime Modified { get; set; }

        public ICollection<Order> Orders { get; set; }
    }

    public enum OrderTypeCode
    {
        GoodsReceipt,
        DeliveryNote,
        SaleReturnOrder,
        PurchaseReturnOrder
    }
}
