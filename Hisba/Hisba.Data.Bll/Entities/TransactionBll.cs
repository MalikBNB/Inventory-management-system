using Hisba.Data.Layers;
using Hisba.Data.Layers.Entities;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Hisba.Data.Bll.Entities
{
    public class TransactionBll : SharedBll
    {
        public async static void Add(AppDbContext context, Transaction Transaction)
        {
            context.Transactions.Add(Transaction);
            await context.SaveChangesAsync();
        }

        public async static void Update(AppDbContext context, Transaction Transaction)
        {
            var oldTransaction = await context.Transactions.FindAsync(Transaction.Id);

            if (oldTransaction == null) return;

            oldTransaction.Id = Transaction.Id;
            oldTransaction.TierId = Transaction.TierId;
            oldTransaction.Amount = Transaction.Amount;
            oldTransaction.PaymentMethod = Transaction.PaymentMethod;
            oldTransaction.Date = Transaction.Date;
            oldTransaction.TypeId = Transaction.TypeId;
            oldTransaction.CreatorId = Transaction.CreatorId;
            oldTransaction.Created = Transaction.Created;
            oldTransaction.ModifierId = Transaction.ModifierId;
            oldTransaction.Modified = Transaction.Modified;

            await context.SaveChangesAsync();
        }

        public async static void Delete(Transaction Transaction)
        {
            using (var context = new AppDbContext())
            {
                var TransactionToDelete = await context.Transactions.FindAsync(Transaction.Id);

                if (TransactionToDelete == null) return;

                context.Transactions.Remove(TransactionToDelete);
                await context.SaveChangesAsync();
            }
        }

        public static async Task<Transaction> GetTransactionById(int id)
        {
            return await Db.Transactions.FindAsync(id);
        }

        public static async Task<Transaction> GetTransactionByTierId(int TierId)
        {
            return await Db.Transactions.FindAsync(TierId);
        }

        public static async Task<List<Transaction>> GetAllTransactions()
        {
            return await Db.Transactions.ToListAsync();
        }
    }
}
