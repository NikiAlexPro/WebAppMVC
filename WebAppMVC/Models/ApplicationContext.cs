using Microsoft.EntityFrameworkCore;

namespace WebAppMVC.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Client> Client { get; set; }
        public DbSet<DetailClient> DetailClient { get; set; }
        public DbSet<ShopProduct> ShopProduct { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options) 
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
