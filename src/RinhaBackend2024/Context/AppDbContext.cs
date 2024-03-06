using Microsoft.EntityFrameworkCore;
using RinhaBackend2024.Entities;

namespace RinhaBackend2024.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
    { }

    public DbSet<Cliente> Clientes { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>().HasKey(c => c.Id);
        modelBuilder.Entity<Cliente>().Property(c => c.Limite).IsRequired();
        modelBuilder.Entity<Cliente>().Property(c => c.Saldo).IsRequired();
        modelBuilder.Entity<Cliente>().Property<uint>("Version").IsRowVersion();
            
        base.OnModelCreating(modelBuilder);
        new DbInitializer(modelBuilder).Seed();
    }
}