using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebHalk.Data.Entities.Identity
{
    public class UserEntity : IdentityUser<int>
    {
        [StringLength(100)]
        public string? FirstName { get; set; }

        [StringLength(100)]
        public string? LastName { get; set; }

        public ICollection<UserRoleEntity>? UserRoles { get; set; }
    }
}
