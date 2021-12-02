using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using FT_SpReport.CoreBusiness.Models;
using Microsoft.AspNetCore.Identity;
using FT_SpReport.CoreBusiness.Helpers;

namespace FT_ReportAuth.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<SpModel> SpModels { get; set; }
        public DbSet<SpParamModel> SpParamModels { get; set; }
        public DbSet<SpData> SpDatas { get; set; }
        public DbSet<SpModelRole> SpModelRoles { get; set; }
        public DbSet<LookupModel> LookupModels { get; set; }
        public DbSet<ReportModel> ReportModels { get; set; }
        public DbSet<ReportModelRole> ReportModelRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //seed admin role
            string adminid = "a18be9c0-aa65-4af8-bd17-00bd9344e575";
            string managerid = "a18be9c0-aa65-4af8-bd17-00bd9344e576";
            string userid = "a18be9c0-aa65-4af8-bd17-00bd9344e577";
            builder.Entity<IdentityRole>().HasData(
                    new IdentityRole
                    {
                        Id = adminid,
                        Name = StaticValues.AdministratorRole,
                        NormalizedName = StaticValues.AdministratorRole.ToUpper()
                    },
                    new IdentityRole
                    {
                        Id = managerid,
                        Name = StaticValues.ManagerRole,
                        NormalizedName = StaticValues.ManagerRole.ToUpper()
                    },
                    new IdentityRole
                    {
                        Id = userid,
                        Name = StaticValues.UserRole,
                        NormalizedName = StaticValues.UserRole.ToUpper()
                    }
                );

            //create user
            string userguid = "a18be9c0-aa65-4af8-bd17-00bd9344e578";
            string username = "ftadmin@myfastrack.net";
            var appUser = new IdentityUser
            {
                Id = userguid,
                Email = username,
                UserName = username,
                NormalizedEmail = username.ToUpper(),
                NormalizedUserName = username.ToUpper()
            };
            PasswordHasher<IdentityUser> ph = new PasswordHasher<IdentityUser>();
            //set user password
            appUser.PasswordHash = ph.HashPassword(appUser, "123456");

            //seed user
            builder.Entity<IdentityUser>().HasData(appUser);

            //set user role to admin
            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = adminid,
                UserId = userguid
            });
        }
    }
}
