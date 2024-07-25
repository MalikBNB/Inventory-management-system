using Hisba.Data.Layers;
using Hisba.Data.Layers.Entities;
using Hisba.Data.Layers.EntitiesInfo;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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

        //public static async Task<ProductCategory> GetProductCategoryById(int id)
        //{
        //    return await Db.ProductCategories.FindAsync(id);
        //}

        public static async Task<CategoryInfo> GetProductCategoryById(int id)
        {
            return await Db.ProductCategories.Select(item => new CategoryInfo
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name
            }).FirstOrDefaultAsync(pc => pc.Id == id);
        }

        public static async Task<ProductCategory> GetProductCategoryByName(string Name)
        {
            return await Db.ProductCategories.FirstOrDefaultAsync(c => c.Name == Name);
        }

        public static async Task<List<ProductCategory>> GetCategories()
        {
            return await Db.ProductCategories.ToListAsync();
        }

        public static async Task<List<CategoryInfo>> GetAllProductCategories()
        {
            return await Db.ProductCategories.Select(item => new CategoryInfo
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name
            }).ToListAsync();
        }
    }
}
