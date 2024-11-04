using Microsoft.EntityFrameworkCore;
using PassedPawn.DataAccess.Entities;

namespace PassedPawn.DataAccess;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Student> Students { get; set; }
    
    public DbSet<Coach> Coaches { get; set; }
    
    public DbSet<Photo> Photos { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}