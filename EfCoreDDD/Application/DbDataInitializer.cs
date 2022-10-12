using EfCoreDDD.Core;
using EfCoreDDD.Infrastructure;

namespace EfCoreDDD.Application
{
    public class DbDataInitializer : IDbDataInitializer
    {
        private readonly MyDbContext dbContext;

        public DbDataInitializer(MyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddData()
        {
            if (dbContext.User.Any())
                return;

            var user = User.CreateUser("User 1", new List<Account>
            {
                Account.CreateNewActiveAccount("Account 1"),
                Account.CreateNewInvitedAccount("Account 2"),
            });

            dbContext.User.Add(user);

            await dbContext.SaveChangesAsync();
        }
    }
}
