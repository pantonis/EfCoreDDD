using EfCoreDDD.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace EfCoreDDD.Application
{
    public class UserService : IUserService
    {
        private readonly MyDbContext dbContext;

        public UserService(MyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task ActivateAccounts()
        {
            var user = await (from dbUser in dbContext.User.Include(x => x.Accounts).ThenInclude(x=> x.AccountStatus)
                              select dbUser).FirstOrDefaultAsync();

            foreach (var account in user.Accounts)
            {
                account.Activate();
            }

            await dbContext.SaveChangesAsync();
        }
    }
}
