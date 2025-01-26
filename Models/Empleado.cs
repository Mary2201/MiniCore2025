using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCore.Models
{
    public class Empleado
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        // Relación con Gasto
        public virtual List<Gasto> Gastos { get; set; } = new List<Gasto>();
    }
}
