using Microsoft.EntityFrameworkCore;
using MyRestaurant.Models;

namespace MyRestaurant.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<MyRestaurant.Models.MenuItem> MenuItem { get; set; }
        public DbSet<MyRestaurant.Models.OrderDetail> OrderDetail { get; set; }
      
    }
}
