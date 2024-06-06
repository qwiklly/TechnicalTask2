using WebApplication5.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApplication5.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<MealRecord> MealRecords { get; set; }


    }
}
