using DevExpress.Xpf.Core;
using Hisba.Data.Layers;
using Hisba.Data.Layers.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Hisba.Data.Bll.Entities
{
    public class OrderBll
    {
        public async static Task<int> Add(Order order)
        {
            try
            {
                using(var context = new AppDbContext())
                {
                    context.Orders.Add(order);
                    await context.SaveChangesAsync();

                    return order.Id;
                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                return -1;
            }
        }

        public async static Task<bool> Update(Order order)
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    var oldOrder = await context.Orders.FindAsync(order.Id);

                    if (oldOrder == null) return false;

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

                    return true;
                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }           
        }

        public async static void Delete(Order order)
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    var OrderToDelete = await context.Orders.FindAsync(order.Id);

                    if (OrderToDelete == null) return;

                    context.Orders.Remove(OrderToDelete);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }           
        }

        public static async Task<Order> GetOrderById(int id)
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    var order = await context.Orders.FindAsync(id);
                    return order;
                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        public static async Task<Order> GetOrderByReference(string Reference)
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    var order = await context.Orders.FindAsync(Reference);
                    return order;
                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        public static async Task<string> GetLastReference()
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    var order = await context.Orders.OrderByDescending(o => o.Reference).Select(o => o.Reference).FirstOrDefaultAsync();
                    return order;
                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        public static async Task<string> GenerateNewReference()
        {
            try
            {
                var LastReference = await GetLastReference();
                int Ref;

                if (string.IsNullOrEmpty(LastReference))
                    Ref = 0;
                else
                    Ref = int.Parse(LastReference);

                Ref++;

                return Ref.ToString("00#####");
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }            
        }

        //public static async Task<decimal> CalculateTotalHT(int orderId)
        //{
        //    var items = await OrderItemBll.GetAllOrderItems(orderId);
        //    decimal total = 0;

        //    foreach (var item in items)
        //    {
        //        total += item.NetAmountHT;
        //    }

        //    return total;
        //}

        //public static async Task<decimal> CalculateTotalTVA(int orderId)
        //{
        //    var items = await OrderItemBll.GetAllOrderItems(orderId);
        //    decimal total = 0;

        //    foreach (var item in items)
        //    {
        //        total += item.TVAPercentage;
        //    }

        //    return total;
        //}

        //public static async Task<decimal> CalculateTotalTTC(int orderId)
        //{
        //    var items = await OrderItemBll.GetAllOrderItems(orderId);
        //    decimal total = 0;

        //    foreach (var item in items)
        //    {
        //        total += item.NetAmountTTC;
        //    }

        //    return total;
        //}

        public static async Task<List<Order>> GetAllOrders()
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    var orders = await context.Orders.ToListAsync();
                    return orders;
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
