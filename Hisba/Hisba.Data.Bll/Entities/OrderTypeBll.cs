using DevExpress.Xpf.Core;
using Hisba.Data.Layers;
using Hisba.Data.Layers.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;
using System.Windows;

namespace Hisba.Data.Bll.Entities
{
    public class OrderTypeBll
    {
        public async static void Add(OrderType OrderType)
        {
            try
            {
                using(var context = new AppDbContext())
                {
                    context.OrderTypes.Add(OrderType);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async static void Update(OrderType OrderType)
        {
            try
            {
                using (var context = new AppDbContext())
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
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }        
        }

        public async static void Delete(OrderType OrderType)
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    var OrderTypeToDelete = await context.OrderTypes.FindAsync(OrderType.Id);

                    if (OrderTypeToDelete == null) return;

                    context.OrderTypes.Remove(OrderTypeToDelete);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }           
        }

        public static async Task<OrderType> GetOrderTypeById(int id)
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    var orderType = await context.OrderTypes.FindAsync(id);
                    return orderType;
                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        public static async Task<OrderType> GetOrderType(OrderTypeCode Type)
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    var orderType = await context.OrderTypes.AsNoTracking().FirstOrDefaultAsync(ot => ot.Code == Type);
                    return orderType;
                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        public static async Task<List<OrderType>> GetAllOrderTypes()
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    var orderType = await context.OrderTypes.ToListAsync();
                    return orderType;
                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }
    }
}
