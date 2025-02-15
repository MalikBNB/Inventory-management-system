﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Hisba.Data.Layers.Entities
{
    public class Product //: INotifyPropertyChanged
    {

        #region Public Properties
        [Key]
        public int Id { get; set; }

        public int Code { get; set; }

        public string Reference { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int? CategoryId { get; set; }

        public virtual ProductCategory Category { get; set; }

        public decimal PurchasePrice { get; set; }

        public decimal TVAPercentage { get; set; }

        public double TVA => (double)(TVAPercentage / 100);

        public decimal MarginPercentage { get; set; }

        public double Margin { get => (double)(MarginPercentage / 100); }
        
        public decimal SalePrice => PurchasePrice * (decimal)(1 + Margin);

        public decimal SalePriceTTC => SalePrice * (decimal)(1 + TVA);

        public decimal QuantityInStock { get; set; }

        public int? CreatorId { get; set; }

        public virtual User Creator { get; set; }

        public DateTime Created { get; set; }

        public int? ModifierId { get; set; }

        public virtual User Modifier { get; set; }

        public DateTime Modified { get; set; }

        public ICollection<OrderItem> Items { get; set; }

        #endregion
    }
}
