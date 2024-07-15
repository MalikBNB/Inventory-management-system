using Hisba.Data.Layers;
using Hisba.Data.Layers.Entities;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Hisba.Data.Bll.Entities
{
    public class UserBll : SharedBll
    {
        public async static void Add(AppDbContext context, User User)
        {
            context.Users.Add(User);
            await context.SaveChangesAsync();
        }

        public async static void Update(AppDbContext context, User User)
        {
            var oldUser = await context.Users.FindAsync(User.Id);

            if (oldUser == null) return;

            oldUser.Id = User.Id;
            oldUser.Username = User.Username;
            oldUser.Password = User.Password;
            oldUser.FullName = User.FullName;
            oldUser.BirthDate = User.BirthDate;
            oldUser.PhoneNumber = User.PhoneNumber;
            oldUser.Email = User.Email;
            oldUser.Address = User.Address;
            oldUser.CreatorId = User.CreatorId;
            oldUser.Created = User.Created;
            oldUser.ModifierId = User.ModifierId;
            oldUser.Modified = User.Modified;

            await context.SaveChangesAsync();
        }

        public async static void Delete(User User)
        {
            using (var context = new AppDbContext())
            {
                var UserToDelete = await context.Users.FindAsync(User.Id);

                if (UserToDelete == null) return;

                context.Users.Remove(UserToDelete);
                await context.SaveChangesAsync();
            }
        }

        public static async Task<User> GetUserById(int id)
        {
            return await Db.Users.FindAsync(id);
        }

        public static async Task<User> GetUserByUsername(string Username)
        {
            return await Db.Users.FindAsync(Username);
        }

        public static async Task<List<User>> GetAllUsers()
        {
            return await Db.Users.ToListAsync();
        }
    }
}
