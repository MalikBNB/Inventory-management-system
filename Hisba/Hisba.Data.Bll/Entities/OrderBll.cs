using Hisba.Data.Layers;
using Hisba.Data.Layers.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace Hisba.Data.Bll.Entities
{
    public class OrderBll : SharedBll
    {
        public async static void Add(AppDbContext context, Order order)
        {
            context.Orders.Add(order);
            await context.SaveChangesAsync();
        }

        public async static void Update(AppDbContext context, Order order)
        {
            var oldOrder = await context.Orders.FindAsync(order.Id);

            if (oldOrder == null) return;

            oldOrder.Id = order.Id;
            oldOrder.Reference = order.Reference;
            oldOrder.TierId = order.TierId;
            oldOrder.OrderTypeId = order.OrderTypeId;
            oldOrder.Date = order.Date;
            oldOrder.TotalHT = order.TotalHT;
            oldOrder.TotalTVA = order.TotalTVA;
            oldOrder.TotalTTC = order.TotalTTC;
            oldOrder.Status = order.Status;
            oldOrder.CreatorId = order.CreatorId;
            oldOrder.Created = order.Created;
            oldOrder.ModifierId = order.ModifierId;
            oldOrder.Modified = order.Modified;

            await context.SaveChangesAsync();
        }

        public async static void Delete(Order order)
        {
            using (var context = new AppDbContext())
            {
                var OrderToDelete = await context.Orders.FindAsync(order.Id);

                if (OrderToDelete == null) return;

                context.Orders.Remove(OrderToDelete);
                await context.SaveChangesAsync();
            }
        }

        public static async Task<Order> GetOrderById(int id)
        {
            return await Db.Orders.FindAsync(id);
        }

        public static async Task<Order> GetOrderByReference(string Reference)
        {
            return await Db.Orders.FindAsync(Reference);
        }

        public static async Task<List<Order>> GetAllOrders()
        {
            return await Db.Orders.ToListAsync();
        }
    }
}
