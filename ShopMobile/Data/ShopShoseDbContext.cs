using ShopMobile.Models;
using Microsoft.EntityFrameworkCore;


namespace ShopMobile.Data
{
    public class ShopShoseDbContext : DbContext
    {
        public ShopShoseDbContext(DbContextOptions<ShopShoseDbContext> options)
 : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; } 


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable(nameof(User));
            modelBuilder.Entity<Role>().ToTable(nameof(Role));
            modelBuilder.Entity<Product>().ToTable(nameof(Product));
            modelBuilder.Entity<Category>().ToTable(nameof(Category));



        }
    }
}
