using Hisba.Data.Layers;
using Hisba.Data.Layers.Entities;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Hisba.Data.Bll.Entities
{
    public class TierBll : SharedBll
    {
        public async static void Add(AppDbContext context, Tier Tier)
        {
            context.Tiers.Add(Tier);
            await context.SaveChangesAsync();
        }

        public async static void Update(AppDbContext context, Tier Tier)
        {
            var oldTier = await context.Tiers.FindAsync(Tier.Id);

            if (oldTier == null) return;

            oldTier.Id = Tier.Id;
            oldTier.Code = Tier.Code;
            oldTier.Reference = Tier.Reference;
            oldTier.FirstName = Tier.FirstName;
            oldTier.LastName = Tier.LastName;
            oldTier.BirthDate = Tier.BirthDate;
            oldTier.PhoneNumber = Tier.PhoneNumber;
            oldTier.Email = Tier.Email;
            oldTier.Address = Tier.Address;
            oldTier.City = Tier.City;
            oldTier.TierType = Tier.TierType;
            oldTier.Image = Tier.Image;
            oldTier.CreatorId = Tier.CreatorId;
            oldTier.Created = Tier.Created;
            oldTier.ModifierId = Tier.ModifierId;
            oldTier.Modified = Tier.Modified;

            await context.SaveChangesAsync();
        }

        public async static void Delete(Tier Tier)
        {
            using (var context = new AppDbContext())
            {
                var TierToDelete = await context.Tiers.FindAsync(Tier.Id);

                if (TierToDelete == null) return;

                context.Tiers.Remove(TierToDelete);
                await context.SaveChangesAsync();
            }
        }

        public static async Task<Tier> GetTierById(int id)
        {
            return await Db.Tiers.FindAsync(id);
        }

        public static async Task<Tier> GetTierByReference(string Reference)
        {
            return await Db.Tiers.FindAsync(Reference);
        }

        public static async Task<List<Tier>> GetAllTiers()
        {
            return await Db.Tiers.ToListAsync();
        }
    }
}
