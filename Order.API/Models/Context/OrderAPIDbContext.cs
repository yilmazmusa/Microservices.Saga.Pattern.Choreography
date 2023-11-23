using Microsoft.EntityFrameworkCore;



namespace Order.API.Models.Context
{
    public class OrderAPIDbContext : DbContext
    {
        public OrderAPIDbContext(DbContextOptions options) : base(options) //Dependency Injectından context'i kullanmak için burda DbContextOptions türünden
                                                                           //options'u constructorda oluşturmamız gerekiyor.
        {
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
    }
}
