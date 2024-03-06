using Microsoft.EntityFrameworkCore;
using RinhaBackend2024.Entities;

namespace RinhaBackend2024.Context;

public class DbInitializer
{
    private readonly ModelBuilder modelBuilder;

    public DbInitializer(ModelBuilder modelBuilder)
    {
        this.modelBuilder = modelBuilder;
    }

    public void Seed()
    {
        modelBuilder.Entity<Cliente>().HasData(
            new Cliente(1,  100_000, 0 ),
            new Cliente(2,  80_000,  0 ),
            new Cliente(3,  1_000_000,  0 ),
            new Cliente(4,  10_000_000,  0 ),
            new Cliente(5, 500_000,  0 )
        );
    }
}