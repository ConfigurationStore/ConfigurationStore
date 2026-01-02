using Microsoft.EntityFrameworkCore;

namespace ConfigurationStore.Data;

public class MainDbContext : DbContext
{
    public MainDbContext(DbContextOptions<MainDbContext> options)
        : base(options)
    {

    }

    public DbSet<User> Users { get; set; }
    public DbSet<UserGroup> UserGroups { get; set; }

    public DbSet<Project> Projects { get; set; }
    public DbSet<ProjectEnvironment> ProjectEnvironments { get; set; }
}