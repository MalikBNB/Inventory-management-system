using System;

namespace Hisba.Data.Layers.EntitiesInfo
{
    public class ProductInfos
    {
        public int Id { get; set; }

        public int Code { get; set; }

        public string ProductReference { get; set; }

        public string ProductName { get; set; }

        public string Description { get; set; }

        public int CategoryId { get; set; }

        public string Category { get; set; }

        public decimal PurchasePrice { get; set; }

        public decimal SalePrice { get => PurchasePrice * (decimal)(1 + Margin); }

        public decimal SalePriceTTC { get => SalePrice * (decimal)(1 + TVA); }

        public decimal MarginPercentage { get; set; }

        public double Margin { get => (double)(MarginPercentage / 100); }

        public decimal TVAPercentage { get; set; }

        public double TVA { get => (double)(TVAPercentage / 100); }

        public decimal QuantityInStock { get; set; }

        public int CreatorId { get; set; }

        public string Creator { get; set; }

        public DateTime Created { get; set; }

        public int ModifierId { get; set; }

        public string Modifier { get; set; }

        public DateTime Modified { get; set; }

    }
}
