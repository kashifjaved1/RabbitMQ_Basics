using Microsoft.EntityFrameworkCore;
using Producer.Data.Entities;

namespace Producer.Data
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) { }

        public DbSet<Order> Orders { get; set; }
    }
}
