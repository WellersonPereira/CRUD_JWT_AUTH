using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using CRUD_JWT_AUTH.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace CRUD_JWT_AUTH.Repositories
{
    public class Context :  IdentityDbContext<User, Role, int,
                            IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>,
                            IdentityRoleClaim<int>, IdentityUserToken<int>>
    {

        public Context(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserRole>(userRole => 
                {
                    userRole.HasKey(ur => new {ur.UserId, ur.RoleId});

                    userRole.HasOne(ur => ur.User)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);

                    userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);
                }
            );
        }

    }
}

//Acompanhando video https://youtu.be/_7Y21NKgSms?t=1475