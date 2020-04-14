using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ViewModels.Libreria
{
    public class TextBoxEvent
    {
        //Metodo que valida solo 
        public void textKeyPress(KeyPressEventArgs  e)
        {
            if (char.IsLetter(e.KeyChar))
            {
                e.Handled = false;
            }
            else if(e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                e.Handled = false;
            }
            else if (char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (char.IsSeparator(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        //Metodo que solo valida los numeros
        public void numberKeyPress(KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (e.KeyChar == Convert.ToChar(Keys.Enter))
            { 
                e.Handled = false;
            }
            else if (char.IsLetter(e.KeyChar))
            {
                e.Handled = true;
            }
            else if (char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else if(char.IsSeparator(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        //Metodo que verifica si se encuentra llenos los campos
        public void validarCampos(string campoTexto, Label label, string valor)
        {
            if (campoTexto.Equals(string.Empty))
            {
                label.ForeColor = Color.LightSlateGray;
            }
            else
            {
                label.Text = valor;
                label.ForeColor = Color.Green;
            }
        }


        public bool comprobarFormatoEmail(string email)
        {
            return new EmailAddressAttribute().IsValid(email);
        } 
    }
}
