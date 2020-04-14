using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class TClientes
    {
        [PrimaryKey, Identity]
        public int ID { get; set; }
        public string Nid { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Fecha { get; set; }
        public bool Credito { get; set; }
        public byte[] Imagen { get; set; }
    }
}
