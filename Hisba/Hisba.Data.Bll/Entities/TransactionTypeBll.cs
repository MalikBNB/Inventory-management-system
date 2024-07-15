using Hisba.Data.Layers;
using Hisba.Data.Layers.Entities;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Hisba.Data.Bll.Entities
{
    public class TransactionTypeBll : SharedBll
    {
        public async static void Add(AppDbContext context, TransactionType TransactionType)
        {
            context.TransactionTypes.Add(TransactionType);
            await context.SaveChangesAsync();
        }

        public async static void Update(AppDbContext context, TransactionType TransactionType)
        {
            var oldTransactionType = await context.TransactionTypes.FindAsync(TransactionType.Id);

            if (oldTransactionType == null) return;

            oldTransactionType.Id = TransactionType.Id;
            oldTransactionType.Code = TransactionType.Code;
            oldTransactionType.Label = TransactionType.Label;
            oldTransactionType.isIn = TransactionType.isIn;
            oldTransactionType.CreatorId = TransactionType.CreatorId;
            oldTransactionType.Created = TransactionType.Created;
            oldTransactionType.ModifierId = TransactionType.ModifierId;
            oldTransactionType.Modified = TransactionType.Modified;

            await context.SaveChangesAsync();
        }

        public async static void Delete(TransactionType TransactionType)
        {
            using (var context = new AppDbContext())
            {
                var TransactionTypeToDelete = await context.TransactionTypes.FindAsync(TransactionType.Id);

                if (TransactionTypeToDelete == null) return;

                context.TransactionTypes.Remove(TransactionTypeToDelete);
                await context.SaveChangesAsync();
            }
        }

        public static async Task<TransactionType> GetTransactionTypeById(int id)
        {
            return await Db.TransactionTypes.FindAsync(id);
        }

        public static async Task<List<TransactionType>> GetAllTransactionTypes()
        {
            return await Db.TransactionTypes.ToListAsync();
        }
    }
}
