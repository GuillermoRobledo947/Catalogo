using dominio;
using negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace TpFinal
{
    public partial class NuevoArticulo : Form
    {
        public NuevoArticulo()
        {
            InitializeComponent();
        }
        public NuevoArticulo(Articulo articulo)
        {
            InitializeComponent();
            this.articulo = articulo;
            Text = "Modificar Articulo";
        }

      
        private Articulo articulo = null;

        private void btnAceptar_Click(object sender, EventArgs e)
        {

            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                if (articulo == null)
                {
                    articulo= new Articulo();  
                }

               
                articulo.Codigo = txbCodigoArticulo.Text;
                articulo.Nombre = txbNombre.Text;
                articulo.Descripcion = txbDescripcion.Text;
                articulo.Marca = (Marcas)cboMarca.SelectedItem;
                articulo.Categoria = (Categorias)cboCategoria.SelectedItem;
                articulo.ImagenUrl = txbImagen.Text;
                articulo.Precio = decimal.Parse(txbPrecio.Text);
                
                
                if (articulo.Codigo != "" && articulo.Nombre != "" && articulo.Descripcion != "")
                {
                    if (articulo.Id != 0)
                    {
                        negocio.modificar(articulo);
                        MessageBox.Show("Modificado.");
                        Close();
                    }
                    else
                    {
                        negocio.agregar(articulo);
                        MessageBox.Show("Agregado.");
                        Close();
                    }
                }
                else
                {
                    MessageBox.Show("Por favor, rellene todos los campos");
                }

                
                

                

                

            }
            catch (System.FormatException)
            {

                MessageBox.Show("Por favor, ingrese valores validos");
            }

      
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void NuevoArticulo_Load(object sender, EventArgs e)
        {
            CategoriasNegocio categoriasNegocio = new CategoriasNegocio();
            MarcasNegocio marcasNegocio = new MarcasNegocio();

            try
            {
                cboCategoria.DataSource = categoriasNegocio.listar();
                cboCategoria.ValueMember = "Id";
                cboCategoria.DisplayMember = "Descripcion";
                cboMarca.DataSource = marcasNegocio.listar();
                cboMarca.ValueMember = "Id";
                cboMarca.DisplayMember = "Descripcion";

                if (articulo != null)
                {
                    txbCodigoArticulo.Text = articulo.Codigo;
                    txbDescripcion.Text = articulo.Descripcion;

                    if (articulo.ImagenUrl != "")
                    {
                        txbImagen.Text = articulo.ImagenUrl;
                    }
                    txbNombre.Text = articulo.Nombre;
                    txbPrecio.Text = articulo.Precio.ToString();
                    cboCategoria.SelectedValue = articulo.Categoria.Id;
                    cboMarca.SelectedValue = articulo.Marca.Id;
                }
                




            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
       


    }
    
}
