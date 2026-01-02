using Microsoft.EntityFrameworkCore;

namespace ConfigurationStore.Data;

public class MainDbContext : DbContext
{
    public MainDbContext(DbContextOptions<MainDbContext> options)
        : base(options)
    {

    }

    public DbSet<User> Users { get; set; }
}