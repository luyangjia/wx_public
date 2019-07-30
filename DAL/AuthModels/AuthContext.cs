using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.DAL.AuthModels
{
    public class AuthContext : IdentityDbContext<ApplicationUser>
    {
        public AuthContext()
            : base("name=EmpContext")
        {
        }

        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>().ToTable("User.User").Property(p => p.Id);
            modelBuilder.Entity<IdentityUserRole>().ToTable("User.UserRoles");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("User.UserLogins");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("User.UserClaims");
            modelBuilder.Entity<IdentityRole>().ToTable("User.Role");
        }
    }
}
