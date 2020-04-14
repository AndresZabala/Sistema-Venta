using LinqToDB;
using LinqToDB.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Conexion
{
    public class Conexion : DataConnection
    {

        public Conexion() : base("MICONEXION1") { }//la palabra reservada "base" indica que heredamos el metodo constructor de la Clase heredada es decir "DataConecction"
        public ITable<TClientes> TClientes { get { return GetTable<TClientes>(); } }
        public ITable<TReportes_clientes> TReportes_clientes { get { return GetTable<TReportes_clientes>(); } }
        public ITable<TConfiguracion> TConfiguracion { get { return GetTable<TConfiguracion>(); } }
    }
}
