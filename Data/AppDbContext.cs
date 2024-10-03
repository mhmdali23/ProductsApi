using Microsoft.EntityFrameworkCore;
using WebAppApi.Controllers;
using WebAppApi.Models;

namespace WebAppApi.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions options):base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>().ToTable("products");
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<UserPermission>().ToTable("UserPermissions");

            modelBuilder.Entity<UserPermission>().HasKey(k=> new {k.UserId,k.PermissionId});
        }


    }
}
