using System.ComponentModel.DataAnnotations;

namespace EfCoreDDD.Core
{
    public class Account
    {
        public long Id { get; private set; }
        public string Name { get; private set; }

        [Required]
        public AccountStatus AccountStatus { get; private set; }

        protected Account()
        {

        }

        private Account(string name, AccountStatus accountStatus)
        {
            Name = name;
            AccountStatus = accountStatus;
        }

        public static Account CreateNewActiveAccount(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new InvalidOperationException("No name passed.");

            return new Account(name, AccountStatus.Active);
        }

        public static Account CreateNewInvitedAccount(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new InvalidOperationException("No name passed.");

            return new Account(name, AccountStatus.Invited);
        }

        public void Activate()
        {
            if (AccountStatus != AccountStatus.Active)
                AccountStatus = AccountStatus.Active;
        }

        public void Deactivate()
        {
            if (AccountStatus != AccountStatus.Inactive)
                AccountStatus = AccountStatus.Inactive;
        }
    }

}
