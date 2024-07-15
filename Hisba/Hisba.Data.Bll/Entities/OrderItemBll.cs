using Hisba.Data.Layers;
using Hisba.Data.Layers.Entities;
using System.Collections.Generic;
using System.Data.Entity;
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
            oldOrderItem.PriceHT = OrderItem.PriceHT;
            oldOrderItem.TVA = OrderItem.TVA;
            oldOrderItem.Discount = OrderItem.Discount;
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

        public static async Task<List<OrderItem>> GetAllOrderItems()
        {
            return await Db.OrderItems.ToListAsync();
        }
    }
}
