using FitPlanner.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitPlanner.Infrastructure.DataAccess;

public class FitPlannerDbContext : DbContext
{
    public FitPlannerDbContext(DbContextOptions options) : base(options) {}
    
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FitPlannerDbContext).Assembly);
    }
}