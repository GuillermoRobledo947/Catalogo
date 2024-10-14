using dominio;
using negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TpFinal
{
    public partial class Imagen : Form
    {
        private Articulo _articulo;

        public Imagen(Articulo articulo)
        {
            InitializeComponent();
            _articulo = articulo;
        }

        private void Detalle_Load(object sender, EventArgs e)
        {
            if (_articulo != null)
            {
                lblNombre.Text += _articulo.Nombre;
                lblDescripcion.Text += _articulo.Descripcion;
                lblPrecio.Text += _articulo.Precio.ToString("C2");
                lblCodigoArticulo.Text += _articulo.Codigo;
                lblMarca.Text += _articulo.Marca.Descripcion;
                lblCategoria.Text += _articulo.Categoria.Descripcion;
                cargarImagen(_articulo.ImagenUrl);

            }
        }
        private void cargarImagen(string imagen)
        {
            try
            {
                pbxDetalle.Load(imagen);
            }
            catch (Exception)
            {
                pbxDetalle.Load("https://libreria.xoc.uam.mx/portadas_art/sinimagen.png");
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

