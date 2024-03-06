using Microsoft.EntityFrameworkCore;
using RinhaBackend2024.Entities;

namespace RinhaBackend2024.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
    { }

    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Transacao> Transacoes { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>().ToTable("clientes");
        modelBuilder.Entity<Cliente>().HasKey(c => c.Id);
        modelBuilder.Entity<Cliente>().Property(c => c.Limite).IsRequired();
        modelBuilder.Entity<Cliente>().Property(c => c.Saldo).IsRequired();
        modelBuilder.Entity<Cliente>().Property<uint>("Version").IsRowVersion();
        
        modelBuilder.Entity<Transacao>().ToTable("transacoes");
        modelBuilder.Entity<Transacao>().HasKey(c => c.Id);
        modelBuilder.Entity<Transacao>().Property(c => c.Valor).IsRequired();
        modelBuilder.Entity<Transacao>().Property(c => c.Tipo).HasMaxLength(1).IsRequired();
        modelBuilder.Entity<Transacao>().Property(c => c.Descricao).HasMaxLength(10).IsRequired();
        modelBuilder.Entity<Transacao>()
            .HasOne(x => x.Cliente)
            .WithMany(x => x.Transacoes)
            .HasForeignKey(x => x.ClienteId)
            .IsRequired();
            
        base.OnModelCreating(modelBuilder);
        new DbInitializer(modelBuilder).Seed();
    }
}