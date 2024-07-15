using Hisba.Data.Layers;
using Hisba.Data.Layers.Entities;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Hisba.Data.Bll.Entities
{
    public class ProductBll : SharedBll
    {
        public async static void Add(AppDbContext context, Product Product)
        {
            context.Products.Add(Product);
            await context.SaveChangesAsync();
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
            oldProduct.Price = Product.Price;
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

        public static async Task<List<Product>> GetAllProducts()
        {
            return await Db.Products.ToListAsync();
        }
    }
}
