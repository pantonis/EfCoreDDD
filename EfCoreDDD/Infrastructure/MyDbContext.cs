using EfCoreDDD.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EfCoreDDD.Infrastructure
{
    public class MyDbContext : DbContext
    {
        public MyDbContext()
        {
            ChangeTracker.LazyLoadingEnabled = false;
        }

        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Account> Account { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
#if (DEBUG)
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.UseLoggerFactory(GetLoggerFactory());
#endif

            var connectionString = "server=127.0.0.1;port=3306;database=EfCoreDDD;uid=username;pwd=password;";
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Id).ValueGeneratedOnAdd();

                entity.HasMany(p => p.Accounts).WithOne().HasForeignKey("UserId").IsRequired();
            });

            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Id).ValueGeneratedOnAdd();

                entity.HasOne(p => p.AccountStatus).WithMany().HasForeignKey("AccountStatusId").IsRequired();
            });

            modelBuilder.Entity<AccountStatus>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<AccountStatus>().HasData(AccountStatus.AllAccountStatuses.Values);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var enumerationEntries = ChangeTracker.Entries().Where(x => x.Entity is IEnumEntity);

            foreach (var enumerationEntry in enumerationEntries)
            {
                enumerationEntry.State = EntityState.Unchanged;
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        private ILoggerFactory GetLoggerFactory()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(builder => builder.AddConsole().AddFilter(DbLoggerCategory.Database.Command.Name, LogLevel.Information));
            return serviceCollection.BuildServiceProvider().GetService<ILoggerFactory>();
        }
    }
}
