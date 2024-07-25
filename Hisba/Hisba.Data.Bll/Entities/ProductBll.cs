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
    public class ProductBll : SharedBll
    {
        public async static void Add(Product Product)
        {
            using (var context = new AppDbContext())
            {               
                try
                {
                    context.Products.Add(Product);
                    await context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    DXMessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            
        }

        public async static void Update(Product Product)
        {
            using (var context = new AppDbContext())
            {
                try
                {
                    var oldProduct = await context.Products.FirstOrDefaultAsync(p => p.Reference == Product.Reference);

                    if (oldProduct == null) return;

                    oldProduct.Code = Product.Code;
                    oldProduct.Reference = Product.Reference;
                    oldProduct.Name = Product.Name;
                    oldProduct.Description = Product.Description;
                    oldProduct.CategoryId = Product.CategoryId;
                    oldProduct.PurchasePrice = Product.PurchasePrice;
                    oldProduct.MarginPercentage = Product.MarginPercentage;
                    oldProduct.QuantityInStock = Product.QuantityInStock;
                    oldProduct.CreatorId = Product.CreatorId;
                    oldProduct.Created = Product.Created;
                    oldProduct.ModifierId = Product.ModifierId;
                    oldProduct.Modified = Product.Modified;

                    await context.SaveChangesAsync();
                }
                catch(Exception ex)
                {
                    DXMessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
                
        }

        public async static Task<bool> Delete(int Id)
        {
            using (var context = new AppDbContext())
            {
                try
                {
                    var ProductToDelete = await context.Products.FindAsync(Id);

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
            return await Db.Products.FindAsync(id);
        }

        public static async Task<Product> GetProductByReference(string Reference)
        {
            return await Db.Products.FindAsync(Reference);
        }

        public static async Task<int> GenerateNewCode(CategoryInfo Category)
        {
            int NewCode, IncreaseCode;
            var LastCode = await Db.Products.Where(p => p.CategoryId == Category.Id)
                                           .OrderByDescending(pc => pc.Code).Select(pc => pc.Code).FirstOrDefaultAsync(); 
            
            if(LastCode.ToString().Length == 1)
                IncreaseCode = LastCode + 1;
            else
                IncreaseCode = int.Parse(LastCode.ToString().Substring(1)) + 1;

            NewCode = int.Parse(Category.Code.ToString() + IncreaseCode.ToString());

            return NewCode;
        }

        //public static async Task<bool> IsCodeExist(int Code)
        //{
        //    return await Db.Products.FirstOrDefaultAsync(p => p.Code == Code) == null;
        //}

        public static bool IsCodeExist(int Code)
        {
            var result = Task.Run(() => Db.Products.FirstOrDefaultAsync(p => p.Code == Code));

            return result.Result  == null;
        }

        public static async Task<bool> IsReferenceExist(string Reference)
        {
            return await Db.Products.FirstOrDefaultAsync(p => p.Reference == Reference) == null;
        }

        public static async Task<List<ProductInfos>> GetAllProducts()
        {
            return await Db.Products.Select(item => new ProductInfos
            {
                Id = item.Id,

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
        }
    }
}
