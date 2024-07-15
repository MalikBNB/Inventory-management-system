using Hisba.Data.Layers;
using Hisba.Data.Layers.Entities;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Hisba.Data.Bll.Entities
{
    public class OrderTypeBll : SharedBll
    {
        public async static void Add(AppDbContext context, OrderType OrderType)
        {
            context.OrderTypes.Add(OrderType);
            await context.SaveChangesAsync();
        }

        public async static void Update(AppDbContext context, OrderType OrderType)
        {
            var oldOrderType = await context.OrderTypes.FindAsync(OrderType.Id);

            if (oldOrderType == null) return;

            oldOrderType.Id = OrderType.Id;
            oldOrderType.Code = OrderType.Code;
            oldOrderType.Label = OrderType.Label;
            oldOrderType.isIn = OrderType.isIn;
            oldOrderType.Sign = OrderType.Sign;
            oldOrderType.CreatorId = OrderType.CreatorId;
            oldOrderType.Created = OrderType.Created;
            oldOrderType.ModifierId = OrderType.ModifierId;
            oldOrderType.Modified = OrderType.Modified;

            await context.SaveChangesAsync();
        }

        public async static void Delete(OrderType OrderType)
        {
            using (var context = new AppDbContext())
            {
                var OrderTypeToDelete = await context.OrderTypes.FindAsync(OrderType.Id);

                if (OrderTypeToDelete == null) return;

                context.OrderTypes.Remove(OrderTypeToDelete);
                await context.SaveChangesAsync();
            }
        }

        public static async Task<OrderType> GetOrderTypeById(int id)
        {
            return await Db.OrderTypes.FindAsync(id);
        }

        public static async Task<List<OrderType>> GetAllOrderTypes()
        {
            return await Db.OrderTypes.ToListAsync();
        }
    }
}
