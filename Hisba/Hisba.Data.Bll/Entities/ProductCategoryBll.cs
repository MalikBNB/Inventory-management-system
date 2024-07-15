using Hisba.Data.Layers;
using Hisba.Data.Layers.Entities;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Hisba.Data.Bll.Entities
{
    public class ProductCategoryBll : SharedBll
    {
        public async static void Add(AppDbContext context, ProductCategory ProductCategory)
        {
            context.ProductCategories.Add(ProductCategory);
            await context.SaveChangesAsync();
        }

        public async static void Update(AppDbContext context, ProductCategory ProductCategory)
        {
            var oldProductCategory = await context.ProductCategories.FindAsync(ProductCategory.Id);

            if (oldProductCategory == null) return;

            oldProductCategory.Id = ProductCategory.Id;
            oldProductCategory.Name = ProductCategory.Name;
            oldProductCategory.CreatorId = ProductCategory.CreatorId;
            oldProductCategory.Created = ProductCategory.Created;
            oldProductCategory.ModifierId = ProductCategory.ModifierId;
            oldProductCategory.Modified = ProductCategory.Modified;

            await context.SaveChangesAsync();
        }

        public async static void Delete(ProductCategory ProductCategory)
        {
            using (var context = new AppDbContext())
            {
                var ProductCategoryToDelete = await context.ProductCategories.FindAsync(ProductCategory.Id);

                if (ProductCategoryToDelete == null) return;

                context.ProductCategories.Remove(ProductCategoryToDelete);
                await context.SaveChangesAsync();
            }
        }

        public static async Task<ProductCategory> GetProductCategoryById(int id)
        {
            return await Db.ProductCategories.FindAsync(id);
        }

        public static async Task<ProductCategory> GetProductCategoryByName(string Name)
        {
            return await Db.ProductCategories.FindAsync(Name);
        }

        public static async Task<List<ProductCategory>> GetAllProductCategories()
        {
            return await Db.ProductCategories.ToListAsync();
        }
    }
}
