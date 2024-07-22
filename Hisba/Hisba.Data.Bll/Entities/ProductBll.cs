using Hisba.Data.Layers;
using Hisba.Data.Layers.Entities;
using Hisba.Data.Layers.EntitiesInfo;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Hisba.Data.Bll.Entities
{
    public class ProductBll : SharedBll
    {
        public async static void Add(Product Product)
        {
            using (var context = new AppDbContext())
            {
                context.Products.Add(Product);
                await context.SaveChangesAsync();
            }
        }

        public async static void Update(AppDbContext context, Product Product)
        {
            var oldProduct = await context.Products.FindAsync(Product.Id);

            if (oldProduct == null) return;

            oldProduct.Id = Product.Id;
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

        public async static void Delete(Product Product)
        {
            using (var context = new AppDbContext())
            {
                var ProductToDelete = await context.Products.FindAsync(Product.Id);

                if (ProductToDelete == null) return;

                context.Products.Remove(ProductToDelete);
                await context.SaveChangesAsync();
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

        public static bool IsCodeExist(int Code)
        {
            return Db.Products.FirstOrDefault(p => p.Code == Code) == null;
        }

        public static async Task<bool> IsReferenceExist(string Reference)
        {
            return await Db.Products.FindAsync(Reference) != null;
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

                Created = item.Created,

                Modifier = item.Modifier.Username,

                Modified = item.Modified,

            }).ToListAsync();
        }
    }
}
