using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class TConfiguracion
    {
        [PrimaryKey, Identity]
        public int id { get; set; }
        public string TypeMoney { get; set; }
    }
}
