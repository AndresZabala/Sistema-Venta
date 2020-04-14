using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class TReportes_clientes
    {
        [PrimaryKey, Identity]
        public int IdRegistro { get; set; }
        public Decimal UltimoPago { get; set; }
        public string FechaPago { get; set; }
        public Decimal DeudaActual { get; set; }
        public string FechaDeuda { get; set; }
        public string Ticket { get; set; }
        public string FechaLimite { get; set; }
        public int IdCliente { get; set; }
        
    }
}
