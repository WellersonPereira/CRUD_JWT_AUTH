using Microsoft.AspNetCore.Identity;

namespace CRUD_JWT_AUTH.Identity
{
    public class Role : IdentityRole<int>
    {
        public virtual List<UserRole> UserRoles { get; set; }
    }
}