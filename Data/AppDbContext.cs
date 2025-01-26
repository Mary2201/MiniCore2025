using Microsoft.EntityFrameworkCore;
using MiniCore.Models;

public class AppDbContext : DbContext
{
    public DbSet<Empleado> Empleados { get; set; }
    public DbSet<Gasto> Gastos { get; set; }
    public DbSet<Departamento> Departamentos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Configurar la conexión a MySQL
        optionsBuilder.UseMySql(
            "server=localhost;database=gestionPersonal;user=root;password=Pillin12;",
            new MySqlServerVersion(new Version(8, 0, 37)) 
        );
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configurar el nombre de la tabla para "Empleado"
        modelBuilder.Entity<Empleado>().ToTable("empleado");

        // Configurar el nombre de la tabla para "Departamento"
        modelBuilder.Entity<Departamento>().ToTable("departamento");

        // Configurar el nombre de la tabla para "Gasto"
        modelBuilder.Entity<Gasto>().ToTable("gasto");

        // Configuración de relaciones y restricciones
        modelBuilder.Entity<Gasto>()
            .HasOne(g => g.Empleado)
            .WithMany(e => e.Gastos)
            .HasForeignKey(g => g.EmpleadoId);

        modelBuilder.Entity<Departamento>()
            .Property(d => d.Nombre)
            .HasColumnName("dept");
        modelBuilder.Entity<Gasto>()
        .Property(g => g.EmpleadoId)
        .HasColumnName("id_empleado");

        modelBuilder.Entity<Gasto>()
            .Property(g => g.DepartamentoId)
            .HasColumnName("id_departamento");

        base.OnModelCreating(modelBuilder);
        // Configuración opcional adicional
    }
}
