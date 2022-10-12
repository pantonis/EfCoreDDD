namespace EfCoreDDD.Core
{
    public class User
    {
        public long Id { get; private set; }
        public string Name { get; private set; }

        protected User()
        {
            
        }

        private User(string name, List<Account> accounts)
        {
            Name = name;
            this.accounts = accounts.ToList();
        }

        private readonly List<Account> accounts;
        public virtual IReadOnlyCollection<Account> Accounts => accounts;

        public static User CreateUser(string name, List<Account> accounts)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new InvalidOperationException("No name passed.");

            if (accounts == null || accounts.Count == 0)
                throw new InvalidOperationException("No accounts passed.");

            return new User(name, accounts);
        }
    }
}