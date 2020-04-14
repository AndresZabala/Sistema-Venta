using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ViewModels;
using ViewModels.Libreria;

namespace Sistema_Venta
{
    public partial class Form1 : Form
    {

        private ConfiguracionVM config;


        public Form1()
        {
            InitializeComponent();
            var radio = new List<RadioButton>();
            radio.Add(rdbDolar);
            radio.Add(rdbPesosCol);
            config = new ConfiguracionVM(radio);

        }

        private ClientesVM clientes;

        private void btnCliente_Click(object sender, EventArgs e)
        {
            var textBoxCliente = new List<TextBox>();
            textBoxCliente.Add(txtClienteNIt);
            textBoxCliente.Add(txtClienteNombre);
            textBoxCliente.Add(txtClienteApellido);
            textBoxCliente.Add(txtClienteEmail);
            textBoxCliente.Add(txtClienteTelefono);
            textBoxCliente.Add(txtClienteDireccion);

            var labelCliente = new List<Label>();
            labelCliente.Add(lblClienteNIt);
            labelCliente.Add(lblClienteNombre);
            labelCliente.Add(lblClienteApellido);
            labelCliente.Add(lblClienteEmail);
            labelCliente.Add(lblClienteTelefono);
            labelCliente.Add(lblClienteDireccion);
            labelCliente.Add(lblPaginasClientes);

            var labelReporte = new List<Label>();
            labelReporte.Add(lblReport_Nombre);
            labelReporte.Add(labelCliente_Deuda);
            labelReporte.Add(lblCliente_FechaDeuda);
            labelReporte.Add(lblCliente_pago);
            labelReporte.Add(lblCliente_fechaPago);
            labelReporte.Add(lblCliente_Ticket);
            labelReporte.Add(lblReport_Pagos);


            object[] objetos =
            {
                imagenCliente,
                checkBoxCliente_Credito,
                Properties.Resources.pensando,
                dataGridView_Clientes,
                numeric_PaginasClientes,
                dataGridViewCliente_Report
            };

            clientes = new ClientesVM(objetos, textBoxCliente, labelCliente, labelReporte);            
            tabControlPrincipal.SelectedIndex = 1;
            btnCliente.Enabled = false;
            btnConfiguracion.Enabled = true;
        }

        private void imagenCliente_Click(object sender, EventArgs e)
        {
            Objects.uploadimage.CargarImagen(imagenCliente);
        }

        private void txtClienteNIt_TextChanged(object sender, EventArgs e)
        {
            Objects.objEventos.validarCampos(txtClienteNIt.Text.Trim(),lblClienteNIt,"Nit");
        }
        private void txtClienteNIt_KeyPress(object sender, KeyPressEventArgs e)
        {
            Objects.objEventos.numberKeyPress(e);
        }
        private void txtClienteNombre_TextChanged(object sender, EventArgs e)
        {
            Objects.objEventos.validarCampos(txtClienteNombre.Text.Trim(),lblClienteNombre,"Nombre");
        }
        private void txtClienteNombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            Objects.objEventos.textKeyPress(e);
        }
        private void txtClienteApellido_TextChanged(object sender, EventArgs e)
        {
            Objects.objEventos.validarCampos(txtClienteApellido.Text.Trim(), lblClienteApellido, "Apellido");
        }
        private void txtClienteApellido_KeyPress(object sender, KeyPressEventArgs e)
        {
            Objects.objEventos.textKeyPress(e);
        }
        private void txtClienteEmail_TextChanged(object sender, EventArgs e)
        {
            Objects.objEventos.validarCampos(txtClienteEmail.Text.Trim(), lblClienteEmail, "Email");
        }
        private void txtClienteTelefono_TextChanged(object sender, EventArgs e)
        {
            Objects.objEventos.validarCampos(txtClienteTelefono.Text.Trim(), lblClienteTelefono, "Telefono");
        }
        private void txtClienteTelefono_KeyPress(object sender, KeyPressEventArgs e)
        {
            Objects.objEventos.numberKeyPress(e);
        }
        private void txtClienteDireccion_TextChanged(object sender, EventArgs e)
        {
            Objects.objEventos.validarCampos(txtClienteDireccion.Text.Trim(), lblClienteDireccion, "Direccion");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            clientes.guardarCliente();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            clientes.restablacer();            
        }

        private void dataGridView_Clientes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView_Clientes.Rows.Count > 0)
            {
                clientes.GetCliente();
            }                
        }
        private void dataGridView_Clientes_KeyUp(object sender, KeyEventArgs e)
        {
            if (dataGridView_Clientes.Rows.Count > 0)
            {
                clientes.GetCliente();
            }
        }

        private void btnCliente_Pagina1_Click(object sender, EventArgs e)
        {
            clientes.paginador("Primero");
        }

        private void btnCliente_Pagina2_Click(object sender, EventArgs e)
        {
            clientes.paginador("Anterior");
        }

        private void btnCliente_Pagina3_Click(object sender, EventArgs e)
        {
            clientes.paginador("Siguiente");
        }

        private void btnCliente_Pagina4_Click(object sender, EventArgs e)
        {
            clientes.paginador("Ultimo");
        }

        private void numeric_PaginasClientes_ValueChanged(object sender, EventArgs e)
        {
            clientes.registro_paginas();
        }

        private void txtClientesBuscar_TextChanged(object sender, EventArgs e)
        {
            clientes.searchClientes(txtClientesBuscar.Text.Trim());
        }

        private void tabControlCliente1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabControlCliente1.SelectedIndex)
            {
                case 0:
                    tabControlCliente2.SelectedIndex = 0;
                    clientes._seccion = 1;
                    break;

                case 1:
                    tabControlCliente2.SelectedIndex = 1;
                    clientes._seccion = 2;
                    break;
            }
            clientes.registro_paginas();
        }   

        private void tabControlCliente2_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabControlCliente2.SelectedIndex)
            {
                case 0:
                    tabControlCliente1.SelectedIndex = 0;
                    clientes._seccion = 1;
                    break;

                case 1:
                    tabControlCliente1.SelectedIndex = 1;
                    clientes._seccion = 2;
                    break;
            }
            clientes.registro_paginas();
        }

        private void dataGridViewCliente_Report_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewCliente_Report.Rows.Count != 0)
            {
                clientes.GetReportesCliente();
            }
        }
        private void dataGridViewCliente_Report_KeyUp(object sender, KeyEventArgs e)
        {
            if (dataGridViewCliente_Report.Rows.Count != 0)
            {
                clientes.GetReportesCliente();
            }
        }

       
        private void btnConfiguracion_Click(object sender, EventArgs e)
        {
            tabControlPrincipal.SelectedIndex = 2;
            btnConfiguracion.Enabled = false;
            btnCliente.Enabled = true;
        }
    }
}
