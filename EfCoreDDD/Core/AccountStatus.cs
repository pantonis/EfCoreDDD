using CSharpFunctionalExtensions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EfCoreDDD.Core
{
    public class AccountStatus : IEnumEntity
    {
        public static readonly AccountStatus Invited = new(1, "Invited");
        public static readonly AccountStatus Inactive = new(2, "Inactive");
        public static readonly AccountStatus Active = new(3, "Active");

        public static readonly Dictionary<int, AccountStatus> AllAccountStatuses = new Dictionary<int, AccountStatus>
        {
            { Invited.Id,  Invited },
            { Inactive.Id,  Inactive },
            { Active.Id,  Active }
        };

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; }

        [Required]
        [MaxLength(32)]
        public string Name { get; set; }

        protected AccountStatus()
        {

        }

        private AccountStatus(int id, string name)
        {
            Id = id;
            Name = (name ?? string.Empty).Trim();
        }

        public static Result<AccountStatus> FromId(int id)
        {
            bool exists = AllAccountStatuses.TryGetValue(id, out AccountStatus value);
            if (!exists || value == null)
                return Result.Failure<AccountStatus>("Invalid user Status.");

            return Result.Success(value);
        }
    }
}
