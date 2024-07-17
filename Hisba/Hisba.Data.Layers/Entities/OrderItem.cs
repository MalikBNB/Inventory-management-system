using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hisba.Data.Layers.Entities
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }

        public int? OrderId { get; set; }

        public virtual Order Order { get; set; }

        public int? ProductId { get; set; }

        public virtual Product Product { get; set; }

        public decimal Quantity { get; set; }

        public double TVA { get => (double)(TVAPercentage / 100); }

        public decimal TVAPercentage { get; set; }

        public double Discount { get => (double)(DiscountPercentage / 100); }

        public decimal DiscountPercentage { get; set; }

        public decimal PriceHT => Product.SalePrice;

        public decimal PriceTTC { get { return PriceHT * (decimal)(1 + TVA); } }

        public decimal NetPriceHT => PriceHT * (decimal)(1 - Discount);

        public decimal NetPriceTTC => PriceTTC * (decimal)(1 - Discount);

        public decimal MarginPercentageHT => (decimal)(MarginHT * 100);

        public double MarginHT => (double)(NetPriceHT / Product.PurchasePrice) - 1;

        public decimal MarginPercentageTTC => (decimal)(MarginTTC * 100);

        public double MarginTTC => (double)(NetPriceTTC / Product.PurchasePrice) - 1;

        public int? CreatorId { get; set; }

        public virtual User Creator { get; set; }

        public DateTime Created { get; set; }

        public int? ModifierId { get; set; }

        public virtual User Modifier { get; set; }

        public DateTime Modified { get; set; }
    }
}
