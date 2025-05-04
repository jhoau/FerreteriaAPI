using FerreteriaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FerreteriaAPI.Data;

public class FerreteriaContext : DbContext
{
    public FerreteriaContext(DbContextOptions<FerreteriaContext> options) : base(options)
    {
    }

    public DbSet<Empleado> Empleados { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<Factura> Facturas { get; set; }
    public DbSet<FacturaDetalle> FacturaDetalles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Factura>()
            .HasOne(f => f.Empleado)
            .WithMany()
            .HasForeignKey(f => f.EmpleadoId);

        modelBuilder.Entity<FacturaDetalle>()
            .HasOne(fd => fd.Factura)
            .WithMany(f => f.Detalles)
            .HasForeignKey(fd => fd.FacturaId);

        modelBuilder.Entity<FacturaDetalle>()
            .HasOne(fd => fd.Item)
            .WithMany()
            .HasForeignKey(fd => fd.ItemId);
    }
}
