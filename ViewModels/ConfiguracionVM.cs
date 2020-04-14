using LinqToDB;
using Models.Conexion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ViewModels
{
    public class ConfiguracionVM : Conexion 
    {
        private List<RadioButton> _radioButton;
        public static String Mony { get; set; }


        //Metodo Constructor vacio
        public ConfiguracionVM()
        {
            TypeMoney();
        }
        //Metodo Constructor
        public ConfiguracionVM(List<RadioButton> radioButton)
        {
            _radioButton = radioButton;
            radioButton[0].CheckedChanged += Pesos_CheckedChanged;
            radioButton[1].CheckedChanged += Dolar_CheckedChanged;
            TypeMoney();
        }

        //Metodo Generalizado
        private void TypeMoney()
        {
            var _tconfiguration = TConfiguracion.ToList();
            //Se verifica si el objeto contiene datos o no
            if (_tconfiguration.Count.Equals(0))
            {
                //Caso para cuando no tiene datos
                Mony = "Pesos";
                TConfiguracion.Value(u => u.TypeMoney, "Pesos")
                    .Insert();
            }
            else
            {
                //Caso para cuando ya tiene datos
                var data = _tconfiguration.Last();
                Mony = data.TypeMoney;
                switch (Mony)
                {
                    case "Pesos":
                        _radioButton[0].Checked = true;
                        break;

                    case "Dolar":
                        _radioButton[1].Checked = true;
                        break;
                }
            }
        }

        private void Dolar_CheckedChanged(object sender, EventArgs e)
        {
            TypeMoney("Dolar",_radioButton[1].Checked);
        }

        private void Pesos_CheckedChanged(object sender, EventArgs e)
        {
            TypeMoney("Pesos", _radioButton[0].Checked);
        }

        private void TypeMoney(string typeMoney, bool valor)
        {
            if (valor)
            {
                var _tconfiguration = TConfiguracion.ToList();
                //Se verifica si el objeto contiene datos o no
                if (_tconfiguration.Count.Equals(0))
                {
                    //Caso para cuando no tiene datos
                    Mony = typeMoney;
                    TConfiguracion.Value(u => u.TypeMoney, typeMoney)
                        .Insert();
                }
                else
                {
                    var data = _tconfiguration.Last();
                    if (data.TypeMoney.Equals(typeMoney))
                    {
                        Mony = typeMoney;
                    }
                    else
                    {
                        Mony = typeMoney;
                        TConfiguracion.Value(u => u.TypeMoney, typeMoney)
                            .Insert();
                    }
                }
            }
        }
    }
}
