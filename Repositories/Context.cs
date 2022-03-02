using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using CRUD_JWT_AUTH.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace CRUD_JWT_AUTH.Repositories
{
    public class Context :  IdentityDbContext<User, IdentityRole<int>, int>
    {
        public Context(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

    }
}