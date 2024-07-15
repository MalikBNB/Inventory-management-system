using Hisba.Data.Layers;
using Hisba.Data.Layers.Entities;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Hisba.Data.Bll.Entities
{
    public class DepositBll : SharedBll
    {
        public async static void Add(AppDbContext context, Deposit Deposit)
        {
            context.Deposits.Add(Deposit);
            await context.SaveChangesAsync();
        }

        public async static void Update(AppDbContext context, Deposit Deposit)
        {
            var oldDeposit = await context.Deposits.FindAsync(Deposit.Id);

            if (oldDeposit == null) return;

            oldDeposit.Id = Deposit.Id;
            oldDeposit.OrderId = Deposit.OrderId;
            oldDeposit.TransactionId = Deposit.TransactionId;
            oldDeposit.Slice = Deposit.Slice;
            oldDeposit.CreatorId = Deposit.CreatorId;
            oldDeposit.Created = Deposit.Created;
            oldDeposit.ModifierId = Deposit.ModifierId;
            oldDeposit.Modified = Deposit.Modified;

            await context.SaveChangesAsync();
        }

        public async static void Delete(Deposit Deposit)
        {
            using (var context = new AppDbContext())
            {
                var DepositToDelete = await context.Deposits.FindAsync(Deposit.Id);

                if (DepositToDelete == null) return;

                context.Deposits.Remove(DepositToDelete);
                await context.SaveChangesAsync();
            }
        }

        public static async Task<Deposit> GetDepositById(int id)
        {
            return await Db.Deposits.FindAsync(id);
        }

        public static async Task<List<Deposit>> GetAllDeposits()
        {
            return await Db.Deposits.ToListAsync();
        }
    }
}
