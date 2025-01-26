using MiniCore.Models;
using System.ComponentModel.DataAnnotations.Schema;

public class Gasto
{
    public int Id { get; set; }
    public DateTime Fecha { get; set; }
    public string Descripcion { get; set; }
    public decimal Monto { get; set; }

    [Column("id_empleado")] // Mapea a la columna de la base de datos
    public int EmpleadoId { get; set; }

    [ForeignKey("EmpleadoId")]
    public virtual Empleado Empleado { get; set; }

    [Column("id_departamento")] // Mapea a la columna de la base de datos
    public int DepartamentoId { get; set; }

    [ForeignKey("DepartamentoId")]
    public virtual Departamento Departamento { get; set; }
}
