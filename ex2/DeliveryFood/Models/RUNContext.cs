using Microsoft.EntityFrameworkCore;
using DeliveryFood.Models;
namespace DeliveryFood.Controllers
{
    internal class RUNContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<PromoCode> PromoCodes { get; set; }

        //public Context(DbContextOptions<RUNContext> options)
        //    : base(options)
        //{
        //    Database.EnsureCreated();
        //}
        
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    User adminUser = new User
        //    {
        //        Id = 1,
        //        // добавление промокода с регистрацией
        //        //присваивание роли админа
               
        //    };
        //    base.OnModelCreating(modelBuilder);
        //}
    }
}