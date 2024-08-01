using Hisba.Data.Layers.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hisba.Data.Layers.EntitiesInfo
{
    public class OrderItemInfos
    {
        public int Id { get; set; }

        public int Code { get; set; }   

        public decimal Quantity { get; set; }

        public decimal DiscountPercentage { get; set; }

        public double Discount { get => (double)(DiscountPercentage / 100); }

        public double TVA { get => (double)(TVAPercentage / 100); }

        public decimal TVAPercentage {  get; set; }

        public decimal Price => PurchasePrice;

        public decimal PriceHT => SalePrice;

        public decimal Amount => Price * Quantity;

        public decimal AmountHT => PriceHT * Quantity;

        public decimal PriceTTC => PriceHT * (decimal)(1 + TVA);

        public decimal NetPriceHT => PriceHT * (decimal)(1 - Discount);

        public decimal NetAmountHT => NetPriceHT * Quantity;

        public decimal NetPriceTTC => PriceTTC * (decimal)(1 - Discount);

        public decimal NetAmountTTC => NetPriceTTC * Quantity;

        public decimal MarginPercentageHT => (decimal)(MarginHT * 100);

        public double MarginHT => (double)(NetPriceHT / PurchasePrice) - 1;

        public decimal MarginPercentageTTC => (decimal)(MarginTTC * 100);

        public double MarginTTC => (double)(NetPriceTTC / PurchasePrice) - 1;

        public int CreatorId { get; set; }

        public string Creator { get; set; }

        public DateTime Created { get; set; }

        public int ModifierId { get; set; }

        public string Modifier { get; set; }

        public DateTime Modified { get; set; }

        //-------------------------------

        public int OrderId { get; set; }

        public string OrderReference { get; set; }

        public int TierId { get; set; }

        public int TierCode { get; set; }

        public string Tier { get; set; }

        public string TierName { get; set; }

        public string OrderType { get; set; }

        public bool OrderTypeIsIn { get; set; }

        public short OrderTypeSign { get; set; }

        public int OrderTypeCode { get; set; }


        public DateTime Date { get; set; }

        public decimal TotalHT { get; set; }

        public decimal TotalTVA { get; set; }

        public decimal TotalTTC { get; set; }

        public bool Status { get; set; }

        //-------------------------

        public int ProductId { get; set; }

        public int ProductCode { get; set; }

        public string ProductReference { get; set; }

        public string ProductName { get; set; }

        public string Description { get; set; }

        public int CategoryId { get; set; }

        public string Category { get; set; }

        public decimal PurchasePrice { get; set; }

        public decimal SalePrice { get => PurchasePrice * (decimal)(1 + Margin); }

        public decimal SalePriceTTC { get => SalePrice * (decimal)(1 + productTVA); }

        public decimal MarginPercentage { get; set; }

        public double Margin { get => (double)(MarginPercentage / 100); }

        public decimal ProductTVA { get; set; }

        public double productTVA { get => (double)(ProductTVA / 100); }

        public decimal QuantityInStock { get; set; }

        //--------------------------------


    }
}
