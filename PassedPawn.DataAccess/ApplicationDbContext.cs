using Microsoft.EntityFrameworkCore;
using PassedPawn.DataAccess.Entities;

namespace PassedPawn.DataAccess;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Student> Students { get; init; }
    
    public DbSet<Coach> Coaches { get; init; }
    
    public DbSet<Photo> Photos { get; init; }
    
    public DbSet<Nationality> Nationalities { get; init; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}