using Microsoft.EntityFrameworkCore;
using WebApplication1.Entities;

namespace WebApplication1.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<PC> PCs => Set<PC>();
    public DbSet<Component> Components => Set<Component>();
    public DbSet<ComponentType> ComponentTypes => Set<ComponentType>();
    public DbSet<ComponentManufacturer> ComponentManufacturers => Set<ComponentManufacturer>();
    public DbSet<PCComponent> PCComponents => Set<PCComponent>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PCComponent>(e =>
        {
            e.HasKey(pc => new { pc.PCId, pc.ComponentCode });

            e.Property(pc => pc.ComponentCode)
                .HasColumnType("char(10)")
                .HasMaxLength(10)
                .IsRequired();

            e.HasOne(pc => pc.PC)
                .WithMany(p => p.PCComponents)
                .HasForeignKey(pc => pc.PCId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(pc => pc.Component)
                .WithMany(c => c.PCComponents)
                .HasForeignKey(pc => pc.ComponentCode)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Component>(e =>
        {
            e.HasOne(c => c.Manufacturer)
                .WithMany(m => m.Components)
                .HasForeignKey(c => c.ComponentManufacturersId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(c => c.Type)
                .WithMany(t => t.Components)
                .HasForeignKey(c => c.ComponentTypesId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        SeedData(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ComponentType>().HasData(
            new ComponentType { Id = 1, Abbreviation = "CPU", Name = "Processor" },
            new ComponentType { Id = 2, Abbreviation = "GPU", Name = "Graphics Card" },
            new ComponentType { Id = 3, Abbreviation = "RAM", Name = "Memory" }
        );

        modelBuilder.Entity<ComponentManufacturer>().HasData(
            new ComponentManufacturer { Id = 1, Abbreviation = "AMD", FullName = "Advanced Micro Devices", FoundationDate = new DateTime(1969, 5, 1) },
            new ComponentManufacturer { Id = 2, Abbreviation = "NV", FullName = "NVIDIA Corporation", FoundationDate = new DateTime(1993, 4, 5) },
            new ComponentManufacturer { Id = 3, Abbreviation = "COR", FullName = "Corsair Gaming Inc.", FoundationDate = new DateTime(1994, 1, 1) }
        );

        modelBuilder.Entity<Component>().HasData(
            new Component
            {
                Code = "CPU0000001",
                Name = "Ryzen 7 7800X3D",
                Description = "8-core gaming processor",
                ComponentManufacturersId = 1,
                ComponentTypesId = 1
            },
            new Component
            {
                Code = "GPU0000001",
                Name = "RTX 4080 Super",
                Description = "High-end gaming graphics card",
                ComponentManufacturersId = 2,
                ComponentTypesId = 2
            },
            new Component
            {
                Code = "RAM0000001",
                Name = "Corsair Vengeance DDR5 16GB",
                Description = "DDR5 RAM module 16GB",
                ComponentManufacturersId = 3,
                ComponentTypesId = 3
            }
        );

        modelBuilder.Entity<PC>().HasData(
            new PC
            {
                Id = 1,
                Name = "Gaming Beast X",
                Weight = 12.5,
                Warranty = 36,
                CreatedAt = new DateTime(2026, 5, 8, 9, 0, 0),
                Stock = 5
            },
            new PC
            {
                Id = 2,
                Name = "Office Mini Pro",
                Weight = 4.2,
                Warranty = 24,
                CreatedAt = new DateTime(2026, 4, 15, 13, 30, 0),
                Stock = 12
            },
            new PC
            {
                Id = 3,
                Name = "Workstation Pro 5000",
                Weight = 15.8,
                Warranty = 48,
                CreatedAt = new DateTime(2026, 3, 20, 10, 15, 0),
                Stock = 3
            }
        );

        modelBuilder.Entity<PCComponent>().HasData(
            new PCComponent { PCId = 1, ComponentCode = "CPU0000001", Amount = 1 },
            new PCComponent { PCId = 1, ComponentCode = "GPU0000001", Amount = 1 },
            new PCComponent { PCId = 1, ComponentCode = "RAM0000001", Amount = 2 },
            new PCComponent { PCId = 2, ComponentCode = "CPU0000001", Amount = 1 },
            new PCComponent { PCId = 2, ComponentCode = "RAM0000001", Amount = 1 },
            new PCComponent { PCId = 3, ComponentCode = "GPU0000001", Amount = 2 },
            new PCComponent { PCId = 3, ComponentCode = "RAM0000001", Amount = 4 }
        );
    }
}
