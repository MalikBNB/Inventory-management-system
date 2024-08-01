using DevExpress.Xpf.Core;
using Hisba.Data.Layers;
using Hisba.Data.Layers.Entities;
using Hisba.Data.Layers.EntitiesInfo;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;

namespace Hisba.Data.Bll.Entities
{
    public class ProductBll
    {
        public async static void Add(Product Product)
        {
            try
            {
                using(var context = new AppDbContext())
                {
                    context.Products.Add(Product);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async static void Update(Product Product)
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    var oldProduct = await context.Products.FirstOrDefaultAsync(p => p.Id == Product.Id);

                    if (oldProduct == null) return;

                    oldProduct.Code = Product.Code;
                    oldProduct.Reference = Product.Reference;
                    oldProduct.Name = Product.Name;
                    oldProduct.Description = Product.Description;
                    oldProduct.CategoryId = Product.CategoryId;
                    oldProduct.PurchasePrice = Product.PurchasePrice;
                    oldProduct.MarginPercentage = Product.MarginPercentage;
                    oldProduct.TVAPercentage = Product.TVAPercentage;
                    oldProduct.QuantityInStock = Product.QuantityInStock;
                    oldProduct.CreatorId = Product.CreatorId;
                    oldProduct.Created = Product.Created;
                    oldProduct.ModifierId = Product.ModifierId;
                    oldProduct.Modified = Product.Modified;

                    await context.SaveChangesAsync();
                }
                    
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async static Task<bool> Delete(string reference)
        {
            using (var context = new AppDbContext())
            {
                try
                {
                    var ProductToDelete = await context.Products.FindAsync(reference);

                    if (ProductToDelete != null)
                    {
                        context.Products.Remove(ProductToDelete);
                        await context.SaveChangesAsync();

                        return true;
                    }

                    return false;
                }
                catch (Exception ex)
                {
                    DXMessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
        }

        public static async Task<Product> GetProductById(int id)
        {
            try
            {
                using(var context = new AppDbContext())
                {
                    var product = await context.Products.FindAsync(id);
                    return product;
                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        public static async Task<Product> GetProductByReference(string Reference)
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    var product = await context.Products.FirstOrDefaultAsync(p => p.Reference == Reference);
                    return product;
                }                    
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        public static async Task<int> GenerateNewCode(CategoryInfo Category)
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    int NewCode, IncreaseCode;
                    var LastCode = await context.Products.Where(p => p.CategoryId == Category.Id)
                                                   .OrderByDescending(pc => pc.Code).Select(pc => pc.Code).FirstOrDefaultAsync();

                    if (LastCode.ToString().Length == 1)
                        IncreaseCode = LastCode + 1;
                    else
                        IncreaseCode = int.Parse(LastCode.ToString().Substring(1)) + 1;

                    NewCode = int.Parse(Category.Code.ToString() + IncreaseCode.ToString());

                    return NewCode;
                }
     
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                return -1;
            }
        }

        public static bool IsCodeExist(int Code)
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    var result = Task.Run(() => context.Products.FirstOrDefaultAsync(p => p.Code == Code));
                    return result.Result == null;
                }  
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public static async Task<bool> IsReferenceExist(string Reference)
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    var isExist = await context.Products.FirstOrDefaultAsync(p => p.Reference == Reference) == null;
                    return isExist;
                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public static async Task<List<ProductInfos>> GetAllProducts()
        {
            using (var context = new AppDbContext())
            {
                var products = await context.Products.Select(item => new ProductInfos
                {

                    Code = item.Code,

                    ProductReference = item.Reference,

                    ProductName = item.Name,

                    Description = item.Description,

                    CategoryId = (int)item.CategoryId,

                    Category = item.Category.Name,

                    PurchasePrice = item.PurchasePrice,

                    MarginPercentage = item.MarginPercentage,

                    TVAPercentage = item.TVAPercentage,

                    QuantityInStock = item.QuantityInStock,

                    Creator = item.Creator.Username,

                    CreatorId = item.Creator.Id,

                    Created = item.Created,

                    Modifier = item.Modifier.Username,

                    ModifierId = item.Modifier.Id,

                    Modified = item.Modified,

                }).ToListAsync();
                return products;
            } 
        }

        public static async Task<List<Product>> GetProductsList()
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    var products = await context.Products.ToListAsync();
                    return products;
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
