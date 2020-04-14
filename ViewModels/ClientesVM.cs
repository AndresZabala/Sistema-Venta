using LinqToDB;
using Models;
using Models.Conexion;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ViewModels.Libreria;

namespace ViewModels
{
    public class ClientesVM : Conexion
    {
        private List<TextBox> _textBoxCliente;
        private List<Label> _labelCliente, _labelReporte;
        private TextBoxEvent evento;
        private string _accion = "insert";
        private PictureBox _imagePictureBox;
        private CheckBox _checkBoxCredito;
        private Bitmap _imagBitmap;
        private static DataGridView _dataGridView1, _dataGridView2;
        private NumericUpDown _numericUpDown;
        private Paginador<TClientes> _paginadorClientes;
        private Paginador<TReportes_clientes> _paginadorReport;
        private int _reg_por_pagina = 10, _num_pagina = 1;
        private int _idCliente = 0;
        public int _seccion { get; set; }


        public ClientesVM(object[] objetos, List<TextBox> textBoxCliente, List<Label> labelCliente, List<Label> labelReporte)
        {
            _textBoxCliente = textBoxCliente;
            _labelCliente = labelCliente;
            _labelReporte = labelReporte;
            _imagePictureBox = (PictureBox)objetos[0];
            _checkBoxCredito = (CheckBox)objetos[1];
            _imagBitmap = (Bitmap)objetos[2];
            _dataGridView1 = (DataGridView)objetos[3];
            _numericUpDown = (NumericUpDown)objetos[4];
            _dataGridView2 = (DataGridView)objetos[5];
            evento = new TextBoxEvent();
            restablacer();
            restablecerReport();
        }

        //REGISTRO DE CLIENTES
        //Metodo que valida que los campos del formulario agregar Cliente se encuentren llenos
        public void guardarCliente()
        {
            for (int i=0; i < _textBoxCliente.Count; i++)
            {
                if (_textBoxCliente[i].Text.Trim().Equals(string.Empty))
                {
                    _labelCliente[i].Text = "Campo Requerido";
                    _labelCliente[i].ForeColor = Color.Red;
                    _labelCliente[i].Focus();

                    break;
                }
                else
                {
                    if (!evento.comprobarFormatoEmail(_textBoxCliente[i].Text.Trim()) && i==3)
                    {
                        _labelCliente[i].Text = "Email Invalido";
                        _labelCliente[i].ForeColor = Color.Red;
                        _labelCliente[i].Focus();

                        break;
                    }                    
                }        
            }

            var cliente1 = TClientes.Where(c => c.Nid.Equals(_textBoxCliente[0].Text.Trim())).ToList();
            var cliente2 = TClientes.Where(c => c.Email.Equals(_textBoxCliente[3].Text.Trim())).ToList();
            var list = cliente1.Union(cliente2).ToList();
            switch (_accion)
            {
                case "insert":
                    if (list.Count.Equals(0))
                    {
                        SaveData();
                    }
                    else
                    {
                        if (cliente1.Count > 0)
                        {
                            _labelCliente[0].Text = "El nid ya esta registrado";
                            _labelCliente[0].ForeColor = Color.Red;
                            _textBoxCliente[0].Focus();
                        }
                        if (cliente2.Count > 0)
                        {
                            _labelCliente[3].Text = "El email ya esta registrado";
                            _labelCliente[3].ForeColor = Color.Red;
                            _textBoxCliente[3].Focus();
                        }
                    }
                    break;
                case "update":
                    if (list.Count.Equals(2))//Se evalua el numero de identidad y el correo
                    {
                        if (cliente1[0].ID.Equals(_idCliente) && cliente2[0].ID.Equals(_idCliente))
                        {
                            SaveData();
                        }
                        else
                        {
                            if (cliente1[0].ID != _idCliente)
                            {
                                _labelCliente[0].Text = "El nid ya se encuentra registrado";
                                _labelCliente[0].ForeColor = Color.Red;
                                _textBoxCliente[0].Focus();
                            }
                            if (cliente2[0].ID != _idCliente)
                            {
                                _labelCliente[3].Text = "El email ya se encuentra registrado";
                                _labelCliente[3].ForeColor = Color.Red;
                                _textBoxCliente[3].Focus();
                            }
                        }                            
                    }
                    else
                    {
                        if (list.Count.Equals(0))//No se encuentra registrado el cliente
                        {
                            SaveData();
                        }
                        else
                        {
                            if (cliente1.Count != 0)
                            {
                                //Numero de indentidad ya se encuentra registrado
                                if (cliente1[0].ID.Equals(_idCliente))
                                {
                                    SaveData();
                                }
                                else
                                {
                                    if (cliente1[0].ID != _idCliente)
                                    {
                                        _labelCliente[0].Text = "El nid ya se encuentra registrado";
                                        _labelCliente[0].ForeColor = Color.Red;
                                        _textBoxCliente[0].Focus();
                                    }                                
                                }
                            }
                            if (cliente2.Count != 0)
                            {
                                if(cliente2[0].ID.Equals(_idCliente))
                                {
                                    SaveData();
                                }
                                else
                                {
                                    if (cliente1[0].ID != _idCliente)
                                    {
                                        _labelCliente[0].Text = "El nid ya se encuentra registrado";
                                        _labelCliente[0].ForeColor = Color.Red;
                                        _textBoxCliente[0].Focus();
                                    }
                                    if (cliente2[0].ID != _idCliente)
                                    {
                                        _labelCliente[3].Text = "El email ya se encuentra registrado";
                                        _labelCliente[3].ForeColor = Color.Red;
                                        _textBoxCliente[3].Focus();
                                    }
                                }
                            }
                        }
                    }
                    break;
            }
        }

        //Metodo
        public void SaveData()
        {
            BeginTransactionAsync();
            try
            {
                var srcImage = Objects.uploadimage.ResizeImage(_imagePictureBox.Image, 165, 100);
                var image = Objects.uploadimage.ImageToByte(srcImage);

                switch (_accion)
                {
                    case "insert":
                        TClientes.Value(c => c.Nid, _textBoxCliente[0].Text)
                            .Value(c => c.Nombre, _textBoxCliente[1].Text)
                            .Value(c => c.Apellido, _textBoxCliente[2].Text)
                            .Value(c => c.Email, _textBoxCliente[3].Text)
                            .Value(c => c.Telefono, _textBoxCliente[4].Text)
                            .Value(c => c.Direccion, _textBoxCliente[5].Text)
                            .Value(c => c.Credito, _checkBoxCredito.Checked)
                            .Value(c => c.Fecha, DateTime.Now.ToString("dd/MM/yyyy"))
                            .Value(c => c.Imagen, image)
                            .Insert();


                        var cliente = TClientes.ToList().Last();

                        TReportes_clientes.Value(u => u.UltimoPago, 0)
                            .Value(u => u.FechaPago, "--/--/--")
                            .Value(u => u.DeudaActual, 0)
                            .Value(u => u.FechaDeuda, "--/--/--")
                            .Value(u => u.Ticket, "0000000000")
                            .Value(u => u.FechaLimite, "--/--/--")
                            .Value(u => u.IdCliente, cliente.ID)
                            .Insert();

                        break;

                    case "update":
                        TClientes.Where(u => u.ID.Equals(_idCliente))
                            .Set(u => u.Nid, _textBoxCliente[0].Text)
                            .Set(u => u.Nombre, _textBoxCliente[1].Text)
                            .Set(u => u.Apellido, _textBoxCliente[2].Text)
                            .Set(u => u.Email, _textBoxCliente[3].Text)
                            .Set(u => u.Telefono, _textBoxCliente[4].Text)
                            .Set(u => u.Direccion, _textBoxCliente[5].Text)
                            .Set(u => u.Credito, _checkBoxCredito.Checked)
                            .Set(u => u.Imagen, image)
                            .Update();
                        break;
                }

                CommitTransaction();
                restablacer();
            }
            catch (Exception ex)
            {
                RollbackTransaction();
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        //Metodo que traera la informacion de los clientes de la tabla y cargara la informacion 
        //en el DataGridView
        public void searchClientes(string campo)
        {
            List<TClientes> query = new List<TClientes>();
            int inicio = (_num_pagina - 1) * _reg_por_pagina;
            if (campo.Equals(string.Empty))
            {
                query = TClientes.ToList();
            }
            else
            {
                query = TClientes.Where(c => c.Nid.StartsWith(campo) || c.Nombre.StartsWith(campo)
                || c.Apellido.StartsWith(campo)).ToList();
            }

            if (query.Count > 0)
            {
                _dataGridView1.DataSource = query.Skip(inicio).Take(_reg_por_pagina).ToList();
                _dataGridView1.Columns[0].Visible = false;
                _dataGridView1.Columns[7].Visible = false;
                _dataGridView1.Columns[9].Visible = false;
                _dataGridView1.Columns[1].DefaultCellStyle.BackColor = Color.WhiteSmoke;
                _dataGridView1.Columns[3].DefaultCellStyle.BackColor = Color.WhiteSmoke;
                _dataGridView1.Columns[5].DefaultCellStyle.BackColor = Color.WhiteSmoke;
                _dataGridView1.Columns[7].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            }
            else
            {
                _dataGridView1.DataSource = query;
            }            
        }

        public void GetCliente()
        {
            _accion = "update";
            _idCliente = Convert.ToInt16(_dataGridView1.CurrentRow.Cells[0].Value);
            _textBoxCliente[0].Text = Convert.ToString(_dataGridView1.CurrentRow.Cells[1].Value);
            _textBoxCliente[1].Text = Convert.ToString(_dataGridView1.CurrentRow.Cells[2].Value);
            _textBoxCliente[2].Text = Convert.ToString(_dataGridView1.CurrentRow.Cells[3].Value);
            _textBoxCliente[3].Text = Convert.ToString(_dataGridView1.CurrentRow.Cells[4].Value);
            _textBoxCliente[4].Text = Convert.ToString(_dataGridView1.CurrentRow.Cells[5].Value);
            _textBoxCliente[5].Text = Convert.ToString(_dataGridView1.CurrentRow.Cells[6].Value);

            try
            {
                byte[] arrayImage = (byte[])_dataGridView1.CurrentRow.Cells[9].Value;
                _imagePictureBox.Image = Objects.uploadimage.byteArrayToImage(arrayImage);
            }
            catch(Exception)
            {
                _imagePictureBox.Image = _imagBitmap;
            }

            
            _checkBoxCredito.Checked = Convert.ToBoolean(_dataGridView1.CurrentRow.Cells[8].Value);
            _checkBoxCredito.ForeColor = _checkBoxCredito.Checked ? Color.Green : Color.Red;
        } 

        public void restablacer()
        {
            _seccion = 1;
            _accion = "insert";
            _num_pagina = 1;
            _imagePictureBox.Image = _imagBitmap;
            _textBoxCliente[0].Text = string.Empty;
            _textBoxCliente[1].Text = string.Empty;
            _textBoxCliente[2].Text = string.Empty;
            _textBoxCliente[3].Text = string.Empty;
            _textBoxCliente[4].Text = string.Empty;
            _textBoxCliente[5].Text = string.Empty;
            _checkBoxCredito.Checked = false;
            _checkBoxCredito.ForeColor = Color.LightSlateGray;
            _labelCliente[0].Text = "Nid";
            _labelCliente[0].ForeColor = Color.LightSlateGray;
            _labelCliente[1].Text = "Nombre";
            _labelCliente[1].ForeColor = Color.LightSlateGray;
            _labelCliente[2].Text = "Apellido";
            _labelCliente[2].ForeColor = Color.LightSlateGray;
            _labelCliente[3].Text = "Email";
            _labelCliente[3].ForeColor = Color.LightSlateGray;
            _labelCliente[4].Text = "Telefono";
            _labelCliente[4].ForeColor = Color.LightSlateGray;
            _labelCliente[5].Text = "Direccion";
            _labelCliente[5].ForeColor = Color.LightSlateGray;
            searchClientes(""); //Metodo que agrega registro al 

            listCliente = TClientes.ToList();
            if (listCliente.Count > 0)
            {
                _paginadorClientes = new Paginador<TClientes>(listCliente, _labelCliente[6], _reg_por_pagina);
            }
        }

        //PAGOS Y REPORTES

        //METODO QUE GENERA LOS REPORTES
        public void GetReportes(String metodo)
        {
            int inicio = (_num_pagina - 1) * _reg_por_pagina;
            if (metodo.Equals(""))
            {
                var query = TClientes.Join(TReportes_clientes, 
                    z => z.ID,
                    a => a.IdCliente,
                    (z,a)=> new { 
                        z.ID,
                        z.Nid,
                        z.Nombre,
                        z.Apellido,
                        a.IdRegistro,
                        a.DeudaActual,
                        a.FechaDeuda,
                        a.UltimoPago,
                        a.FechaPago,
                        a.Ticket,
                        a.FechaLimite
                    }).ToList();
                _dataGridView2.DataSource = query.Skip(inicio).Take(_reg_por_pagina).ToList();
            }
            else
            {
                var query = TClientes.Join(TReportes_clientes,
                    z => z.ID,
                    a => a.IdCliente,
                    (z, a) => new {
                        z.ID,
                        z.Nid,
                        z.Nombre,
                        z.Apellido,
                        a.IdRegistro,
                        a.DeudaActual,
                        a.FechaDeuda,
                        a.UltimoPago,
                        a.FechaPago,
                        a.Ticket,
                        a.FechaLimite
                    }).Where( c => c.Nombre.StartsWith(metodo) || c.Apellido.StartsWith(metodo) || c.Nid.StartsWith(metodo)).ToList();
                _dataGridView2.DataSource = query.Skip(inicio).Take(_reg_por_pagina).ToList();
            }

            _dataGridView2.Columns[0].Visible = false;
            _dataGridView2.Columns[4].Visible = false;
            _dataGridView2.Columns[1].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            _dataGridView2.Columns[3].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            _dataGridView2.Columns[6].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            _dataGridView2.Columns[8].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            _dataGridView2.Columns[10].DefaultCellStyle.BackColor = Color.WhiteSmoke;
        }

        private Decimal _deuda;
        private String _ticket;
        private int _idReport = 0;
        public void GetReportesCliente()
        {
            _idCliente = Convert.ToInt16(_dataGridView2.CurrentRow.Cells[0].Value);

            var nombre = Convert.ToString(_dataGridView2.CurrentRow.Cells[2].Value);
            var apellido = Convert.ToString(_dataGridView2.CurrentRow.Cells[3].Value);
            _idReport = Convert.ToInt16(_dataGridView2.CurrentRow.Cells[4].Value);
            _labelReporte[0].Text = $"{nombre} {apellido}";
            _labelReporte[1].Text = /*_mony +*/ Convert.ToString(_dataGridView2.CurrentRow.Cells[5].Value);
            _deuda = Convert.ToDecimal(_dataGridView2.CurrentRow.Cells[5].Value);
            _labelReporte[2].Text = Convert.ToString(_dataGridView2.CurrentRow.Cells[6].Value);
            _labelReporte[3].Text = /*_mony +*/ Convert.ToString(_dataGridView2.CurrentRow.Cells[7].Value);
            _labelReporte[4].Text = Convert.ToString(_dataGridView2.CurrentRow.Cells[8].Value);
            _ticket = Convert.ToString(_dataGridView2.CurrentRow.Cells[9].Value);
            _labelReporte[5].Text = _ticket;
            _labelReporte[6].Text = "Ingresar pago";
            _labelReporte[6].ForeColor = Color.LightSlateGray;
        }


        public void restablecerReport()
        {
            //_seccion = 2;
            GetReportes("");
            listReport = TReportes_clientes.ToList();
            if (listReport.Count > 0)
            {
                _paginadorReport = new Paginador<TReportes_clientes>(listReport,
                _labelCliente[6], _reg_por_pagina);
            }
            
        }



        private List<TClientes> listCliente;
        private List<TReportes_clientes> listReport;
        public void paginador(string metodo)
        {
            switch (metodo)
            {
                case "Primero":
                    switch (_seccion)
                    {
                        case 1:
                            if (listCliente.Count > 0)
                            {
                                _num_pagina = _paginadorClientes.primero();
                            }
                            break;
                        case 2:
                            if (listReport.Count > 0)
                            {
                                _num_pagina = _paginadorClientes.primero();
                            }
                            break;
                    }
                               
                    break;

                case "Anterior":
                    switch (_seccion)
                    {
                        case 1:
                            if (listCliente.Count > 0)
                            {
                                _num_pagina = _paginadorClientes.anterior();
                            }
                            break;
                        case 2:
                            if (listReport.Count > 0)
                            {
                                _num_pagina = _paginadorClientes.anterior();
                            }
                            break;
                    }
                    break;

                case "Siguiente":
                    switch (_seccion)
                    {
                        case 1:
                            if (listCliente.Count > 0)
                            {
                                _num_pagina = _paginadorClientes.siguiente();
                            }
                            break;
                        case 2:
                            if (listReport.Count > 0)
                            {
                                _num_pagina = _paginadorClientes.siguiente();
                            }
                            break;
                    }
                    break;

                case "Ultimo":
                    switch (_seccion)
                    {
                        case 1:
                            if (listCliente.Count > 0)
                            {
                                _num_pagina = _paginadorClientes.ultimo();
                            }
                            break;
                        case 2:
                            if (listReport.Count > 0)
                            {
                                _num_pagina = _paginadorClientes.ultimo();
                            }
                            break;
                    }
                    break;
            }
            switch (_seccion)
            {
                case 1:
                    searchClientes("");
                    break;
                case 2:
                    GetReportes("");
                    break;
            }            
        }


        public void registro_paginas()
        {
            _num_pagina = 1;
            _reg_por_pagina = (int)_numericUpDown.Value;
            switch (_seccion)
            {
                case 1:
                    listCliente = TClientes.ToList();
                    if (listCliente.Count > 0)
                    {
                        _paginadorClientes = new Paginador<TClientes>(listCliente, _labelCliente[6], _reg_por_pagina);
                    }
                    searchClientes("");
                    break;
                case 2:                    
                    listReport = TReportes_clientes.ToList();
                    if (listReport.Count > 0)
                    {
                        _paginadorReport = new Paginador<TReportes_clientes>(listReport,
                        _labelCliente[6], _reg_por_pagina);
                        GetReportes("");
                    }
                    break;
            }
           
        }
    }
}
