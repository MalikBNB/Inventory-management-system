using DevExpress.Xpf.Core;
using Hisba.Data.Layers;
using Hisba.Data.Layers.Entities;
using Hisba.Data.Layers.EntitiesInfo;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Hisba.Data.Bll.Entities
{
    public class OrderItemBll
    {
        public async static Task<bool> Add(OrderItem OrderItem)
        {
            try
            {
                using(var context = new AppDbContext())
                {
                    context.OrderItems.Add(OrderItem);
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

        public async static Task<bool> Update(OrderItem OrderItem)
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    var oldOrderItem = await context.OrderItems.FindAsync(OrderItem.Id);

                    if (oldOrderItem == null) return false;

                    oldOrderItem.Id = OrderItem.Id;
                    oldOrderItem.Code = OrderItem.Code;
                    oldOrderItem.ProductId = OrderItem.ProductId;
                    oldOrderItem.OrderId = OrderItem.OrderId;
                    oldOrderItem.TVAPercentage = OrderItem.TVAPercentage;
                    oldOrderItem.Quantity = OrderItem.Quantity;
                    oldOrderItem.DiscountPercentage = OrderItem.DiscountPercentage;
                    oldOrderItem.CreatorId = OrderItem.CreatorId;
                    oldOrderItem.Created = OrderItem.Created;
                    oldOrderItem.ModifierId = OrderItem.ModifierId;
                    oldOrderItem.Modified = OrderItem.Modified;

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

        public async static Task<bool> Delete(int code)
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    var OrderItemToDelete = await context.OrderItems.FirstOrDefaultAsync(oi => oi.Code == code);

                    if (OrderItemToDelete == null) return false;

                    context.OrderItems.Remove(OrderItemToDelete);
                    await context.SaveChangesAsync();

                    return true;
                }

            }
            catch(Exception ex)
            {
                DXMessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;   
            }
        }

        public static async Task<OrderItem> GetOrderItemById(int id)
        {
            try
            {
                using(var context = new AppDbContext())
                {
                    return await context.OrderItems.FindAsync(id);
                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        public static async Task<OrderItemInfos> GetOrderItemByCode(int code)
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    return await context.OrderItems.Select(item => new OrderItemInfos
                    {
                        Id = item.Id,
                        Code = item.Code,
                        Quantity = item.Quantity,
                        DiscountPercentage = item.DiscountPercentage,
                        TVAPercentage = item.TVAPercentage,
                        Created = item.Created,
                        Creator = item.Creator.Username,
                        Modified = item.Modified,
                        Modifier = item.Modifier.Username,

                        OrderId = (int)item.OrderId,
                        OrderReference = item.Order.Reference,
                        Date = item.Order.Date,
                        TotalHT = item.Order.TotalHT,
                        TotalTTC = item.Order.TotalTTC,
                        TotalTVA = item.Order.TotalTVA,
                        Status = item.Order.Status,

                        OrderType = item.Order.OrderType.Label,
                        OrderTypeIsIn = item.Order.OrderType.isIn,
                        OrderTypeSign = item.Order.OrderType.Sign,
                        OrderTypeCode = (int)item.Order.OrderType.Code,

                        TierId = (int)item.Order.TierId,
                        TierCode = item.Order.Tier.Code,
                        Tier = item.Order.Tier.FirstName,
                        TierName = item.Order.Tier.Name,

                        ProductId = (int)item.ProductId,
                        ProductCode = item.Product.Code,
                        ProductName = item.Product.Name,
                        ProductReference = item.Product.Reference,
                        Description = item.Product.Description,
                        CategoryId = (int)item.Product.CategoryId,
                        Category = item.Product.Category.Name,
                        PurchasePrice = item.Product.PurchasePrice,
                        MarginPercentage = item.Product.MarginPercentage,
                        ProductTVA = item.Product.TVAPercentage,
                        QuantityInStock = item.Product.QuantityInStock,

                    }).FirstOrDefaultAsync(oi => oi.Code == code);
                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        public static async Task<OrderItem> GetOrderItemByOrderId(int OrderId)
        {
            try
            {
                using(var context = new AppDbContext())
                {
                    return await context.OrderItems.FindAsync(OrderId);
                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        public static async Task<OrderItem> GetOrderItemByProductId(int ProductId)
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    return await context.OrderItems.FindAsync(ProductId);
                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        public static async Task<int> GetLastCode()
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    return await context.OrderItems.OrderByDescending(o => o.Code).Select(o => o.Code).FirstOrDefaultAsync();
                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                return -1;
            }
        }

        public static async Task<int> GenerateNewCode()
        {           
            try
            {
                var LastCode = await GetLastCode();
                var newCode = LastCode + 1;

                return newCode;
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                return -1;
            }
        }

        public static async Task<List<OrderItemInfos>> GetAllOrderItems()
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    return await context.OrderItems.Select(item => new OrderItemInfos
                    {
                        Id = item.Id,
                        Code = item.Code,
                        Quantity = item.Quantity,
                        DiscountPercentage = item.DiscountPercentage,
                        TVAPercentage = item.TVAPercentage,
                        Created = item.Created,
                        Creator = item.Creator.Username,
                        Modified = item.Modified,
                        Modifier = item.Modifier.Username,

                        OrderId = (int)item.OrderId,
                        OrderReference = item.Order.Reference,
                        Date = item.Order.Date,
                        TotalHT = item.Order.TotalHT,
                        TotalTTC = item.Order.TotalTTC,
                        TotalTVA = item.Order.TotalTVA,
                        Status = item.Order.Status,

                        OrderType = item.Order.OrderType.Label,
                        OrderTypeIsIn = item.Order.OrderType.isIn,
                        OrderTypeSign = item.Order.OrderType.Sign,
                        OrderTypeCode = (int)item.Order.OrderType.Code,

                        TierId = (int)item.Order.TierId,
                        TierCode = item.Order.Tier.Code,
                        Tier = item.Order.Tier.FirstName,
                        TierName = item.Order.Tier.Name,

                        ProductId = (int)item.ProductId,
                        ProductCode = item.Product.Code,
                        ProductName = item.Product.Name,
                        ProductReference = item.Product.Reference,
                        Description = item.Product.Description,
                        CategoryId = (int)item.Product.CategoryId,
                        Category = item.Product.Category.Name,
                        PurchasePrice = item.Product.PurchasePrice,
                        MarginPercentage = item.Product.MarginPercentage,
                        ProductTVA = item.Product.TVAPercentage,
                        QuantityInStock = item.Product.QuantityInStock,

                    }).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
            
        }

        public static async Task<List<OrderItemInfos>> GetAllOrderItems(int orderId)
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    return await context.OrderItems.Select(item => new OrderItemInfos
                    {
                        Id = item.Id,
                        Code = item.Code,
                        Quantity = item.Quantity,
                        DiscountPercentage = item.DiscountPercentage,
                        TVAPercentage = item.TVAPercentage,
                        Created = item.Created,
                        Creator = item.Creator.Username,
                        Modified = item.Modified,
                        Modifier = item.Modifier.Username,

                        OrderId = (int)item.OrderId,
                        OrderReference = item.Order.Reference,
                        Date = item.Order.Date,
                        TotalHT = item.Order.TotalHT,
                        TotalTTC = item.Order.TotalTTC,
                        TotalTVA = item.Order.TotalTVA,
                        Status = item.Order.Status,

                        OrderType = item.Order.OrderType.Label,
                        OrderTypeIsIn = item.Order.OrderType.isIn,
                        OrderTypeSign = item.Order.OrderType.Sign,
                        OrderTypeCode = (int)item.Order.OrderType.Code,

                        TierId = (int)item.Order.TierId,
                        TierCode = item.Order.Tier.Code,
                        Tier = item.Order.Tier.FirstName,
                        TierName = item.Order.Tier.Name,

                        ProductId = (int)item.ProductId,
                        ProductCode = item.Product.Code,
                        ProductName = item.Product.Name,
                        ProductReference = item.Product.Reference,
                        Description = item.Product.Description,
                        CategoryId = (int)item.Product.CategoryId,
                        Category = item.Product.Category.Name,
                        PurchasePrice = item.Product.PurchasePrice,
                        MarginPercentage = item.Product.MarginPercentage,
                        ProductTVA = item.Product.TVAPercentage,
                        QuantityInStock = item.Product.QuantityInStock,

                    }).Where(oi => oi.OrderId == orderId).ToListAsync();
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
