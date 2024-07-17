using Hisba.Data.Layers;
using Hisba.Data.Layers.Entities;
using Hisba.Data.Layers.EntitiesInfo;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Hisba.Data.Bll.Entities
{
    public class OrderItemBll : SharedBll
    {
        public async static void Add(AppDbContext context, OrderItem OrderItem)
        {
            context.OrderItems.Add(OrderItem);
            await context.SaveChangesAsync();
        }

        public async static void Update(AppDbContext context, OrderItem OrderItem)
        {
            var oldOrderItem = await context.OrderItems.FindAsync(OrderItem.Id);

            if (oldOrderItem == null) return;

            oldOrderItem.Id = OrderItem.Id;
            oldOrderItem.ProductId = OrderItem.ProductId;
            oldOrderItem.OrderId = OrderItem.OrderId;
            oldOrderItem.Quantity = OrderItem.Quantity;
            oldOrderItem.TVAPercentage = OrderItem.TVAPercentage;
            oldOrderItem.DiscountPercentage = OrderItem.DiscountPercentage;
            oldOrderItem.CreatorId = OrderItem.CreatorId;
            oldOrderItem.Created = OrderItem.Created;
            oldOrderItem.ModifierId = OrderItem.ModifierId;
            oldOrderItem.Modified = OrderItem.Modified;

            await context.SaveChangesAsync();
        }

        public async static void Delete(OrderItem OrderItem)
        {
            using (var context = new AppDbContext())
            {
                var OrderItemToDelete = await context.OrderItems.FindAsync(OrderItem.Id);

                if (OrderItemToDelete == null) return;

                context.OrderItems.Remove(OrderItemToDelete);
                await context.SaveChangesAsync();
            }
        }

        public static async Task<OrderItem> GetOrderItemById(int id)
        {
            return await Db.OrderItems.FindAsync(id);
        }

        public static async Task<OrderItem> GetOrderItemByOrderId(int OrderId)
        {
            return await Db.OrderItems.FindAsync(OrderId);
        }

        public static async Task<OrderItem> GetOrderItemByProductId(int ProductId)
        {
            return await Db.OrderItems.FindAsync(ProductId);
        }

        public static async Task<List<OrderItemInfos>> GetAllOrderItems()
        {
            return await Db.OrderItems.Select(item => new OrderItemInfos
            {
                Id = item.Id,

                Quantity = item.Quantity,

                TVAPercentage = item.TVAPercentage,

                DiscountPercentage = item.DiscountPercentage,

                CreatorId = (int)item.CreatorId,

                Creator = item.Creator.Username,

                Created = item.Created,

                ModifierId = (int)item.ModifierId,

                Modifier = item.Modifier.Username,

                Modified = item.Modified,

                //-------------------------

                OrderId = (int)item.OrderId,

                OrderReference = item.Order.Reference,

                TierId = (int)item.Order.TierId,

                TierCode = item.Order.Tier.Code,

                Tier = item.Order.Tier.FirstName + " " + item.Order.Tier.LastName,

                OrderTypeId = (int)item.Order.OrderTypeId,

                OrderType = item.Order.OrderType.Label,

                Date = item.Order.Date,

                TotalHT = item.Order.TotalHT,

                TotalTVA = item.Order.TotalTVA,

                TotalTTC = item.Order.TotalTTC,

                Status = item.Order.Status,

                //-------------------------

                ProductId = (int)item.ProductId,

                Code = item.Product.Code,

                ProductReference = item.Product.Reference,

                ProductName = item.Product.Name,

                Description = item.Product.Description,

                CategoryId = (int)item.Product.CategoryId,

                Category = item.Product.Category.Name,

                PurchasePrice = item.Product.PurchasePrice,

                MarginPercentage = item.Product.MarginPercentage,

                QuantityInStock = item.Product.QuantityInStock,

                //--------------------------------

            }).ToListAsync();
        }
    }
}
