using Microsoft.AspNetCore.Identity;

namespace CRUD_JWT_AUTH.Identity
{
    public class User : IdentityUser<int>
    {
        public string FullName { get; set; }
    }
}