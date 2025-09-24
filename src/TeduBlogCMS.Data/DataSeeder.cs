using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using TeduBlogCMS.Core.Domain.Identity;

namespace TeduBlogCMS.Data
{
    public class DataSeeder
    {
        public async Task SeedAsync(TeduBlogConText context)
        {
            var passwordHasher = new PasswordHasher<AppUser>();
            var rootAdminRoleId = Guid.NewGuid();
            if (!context.Roles.Any())
            {
                await context.Roles.AddAsync(
                    new AppRole()
                    {
                        Id = rootAdminRoleId,
                        Name = "Admin",
                        NormalizedName = "ADMIN",
                        DisplayName = "Quản trị viên",
                    }
                );
                await context.SaveChangesAsync();
            }
            if (!context.Users.Any())
            {
                var userId = Guid.NewGuid();
                var user = new AppUser()
                {
                    Id = userId,
                    FirstName = "Phạm",
                    LastName = " Nhất Thống",
                    Email = "phamnhatthong1712@gmail.com",
                    NormalizedEmail = "ADMIN@FPT.EDU.VN",
                    UserName = "admin",
                    NormalizedUserName = "Admin",
                    IsActive = true,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    LockoutEnabled = false,
                    DateCreated = DateTime.Now,
                };
                user.PasswordHash = passwordHasher.HashPassword(user, "Admin@123$");
                await context.AddAsync(user);
                await context.UserRoles.AddAsync(
                    new IdentityUserRole<Guid>() { RoleId = rootAdminRoleId, UserId = userId }
                );
                await context.SaveChangesAsync();
            }
        }
    }
}
