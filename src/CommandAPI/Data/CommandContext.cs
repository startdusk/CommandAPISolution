using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using CommandAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace CommandAPI.Data
{
    // 使用ASPNETCore的身份验证组件
    // 安装 Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    // 为了让数据能支持身份认证：继承 IdentityDbContext<IdentityUser>，这里我们用了继承了IdentityUser进行拓展用户信息
    // Identity 指的是身份认证的数据库结构，AspNetCore会自动生成数据结构
    public class CommandContext : IdentityDbContext<ApplicationUser> //DbContext
    {
        public CommandContext(DbContextOptions<CommandContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // 初始化用户与角色的种子数据
            // 1. 更新用户与角色的外键关系
            builder.Entity<ApplicationUser>(b =>
            {
                b.HasMany(x => x.UserRoles)
                .WithOne()
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();
            });

            // 2. 添加角色
            var adminRoleId = "308660dc-ae51-480f-824d-7dca6714c3e2"; // guid 
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = adminRoleId,
                    Name = "Admin",
                    NormalizedName = "Admin".ToUpper()
                }
            );

            // 3. 添加用户 
            var adminUserId = "e7497add-5127-4ef2-858d-5f77371ef554";
            ApplicationUser adminUser = new ApplicationUser
            {
                Id = adminUserId,
                UserName = "admin@admin.com",
                NormalizedUserName = "admin@admin.com".ToUpper(),
                Email = "admin@admin.com",
                NormalizedEmail = "admin@admin.com".ToUpper(),
                TwoFactorEnabled = false,
                EmailConfirmed = true,
                PhoneNumber = "123456789",
                PhoneNumberConfirmed = false
            };
            PasswordHasher<ApplicationUser> ph = new PasswordHasher<ApplicationUser>();
            adminUser.PasswordHash = ph.HashPassword(adminUser, "admin");
            builder.Entity<ApplicationUser>().HasData(adminUser);

            // 4. 给用户加入管理员权限
            // 通过使用 linking table：IdentityUserRole
            builder.Entity<IdentityUserRole<string>>()
                .HasData(new IdentityUserRole<string>()
                {
                    RoleId = adminRoleId,
                    UserId = adminUserId
                });

            base.OnModelCreating(builder);
        }


        public DbSet<Command> CommandItems { get; set; }
    }
}