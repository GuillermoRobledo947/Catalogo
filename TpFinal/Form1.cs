using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using dominio;
using negocio;

namespace TpFinal
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
        }
        private List<Articulo> listaArticulos;

        

        private void Form1_Load(object sender, EventArgs e)
        {
            cargar();
            cboCampo.Items.Add("Precio");
            cboCampo.Items.Add("Nombre");
            cboCampo.Items.Add("Descripción");
        }
        private void ocultarColumnas()
        {
            dgvDatos.Columns["ImagenUrl"].Visible = false;
            dgvDatos.Columns["Id"].Visible = false;
            dgvDatos.Columns["IdMarca"].Visible = false;
            dgvDatos.Columns["IdCategoria"].Visible = false;

        }

        private void dgvDatos_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDatos.CurrentRow != null)
            {
                Articulo seleccionado = (Articulo)dgvDatos.CurrentRow.DataBoundItem;
                cargarImagen(seleccionado.ImagenUrl);
            }
            
        }
        private void cargarImagen(string imagen)
        {
            try
            {
                pbxImagen.Load(imagen);
            }
            catch (Exception)
            {
                pbxImagen.Load("https://libreria.xoc.uam.mx/portadas_art/sinimagen.png");
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            NuevoArticulo nuevoArticulo = new NuevoArticulo();
            nuevoArticulo.ShowDialog();
            cargar();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvDatos.CurrentRow != null)
            {
                eliminar();
            }
            else
            {
                MessageBox.Show("No se pudo eliminar. Seleccione un articulo");
            }         
        }
        private void eliminar()
        {
            ArticuloNegocio articuloNegocio = new ArticuloNegocio();
            Articulo seleccionado;

            try
            {

                DialogResult respuesta = MessageBox.Show("¿Realmente desea eliminar este articulo?", "Eliminando", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);


                if (respuesta == DialogResult.Yes)
                {
                    seleccionado = (Articulo)dgvDatos.CurrentRow.DataBoundItem;
                    articuloNegocio.eliminar(seleccionado.Id);
                    cargar();
                    

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void cargar()
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                listaArticulos = negocio.listar();
                dgvDatos.DataSource = listaArticulos;
                ocultarColumnas();
                cargarImagen(listaArticulos[0].ImagenUrl);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            Articulo seleccionado;
            try
            {
                if (dgvDatos.CurrentRow != null)
                {
                    seleccionado = (Articulo)dgvDatos.CurrentRow.DataBoundItem;

                    NuevoArticulo modificar = new NuevoArticulo(seleccionado);
                    modificar.ShowDialog();
                    cargar();
                }
                else
                {
                    MessageBox.Show("Seleccione un articulo a modificar");
                }
                
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
            
            
                
        }

        private void btnDetalle_Click(object sender, EventArgs e)
        {
            verDetalle();

            

        }

        public Articulo verDetalle()
        {
            if (dgvDatos.SelectedRows.Count > 0)
            {
                Articulo articulo = new Articulo();
                DataGridViewRow filaSeleccionada = dgvDatos.SelectedRows[0];

                articulo.Marca = new Marcas();
                articulo.Categoria = new Categorias();

                articulo.Nombre = filaSeleccionada.Cells["Nombre"].Value.ToString();
                articulo.Descripcion = filaSeleccionada.Cells["Descripcion"].Value.ToString();
                articulo.Precio = Convert.ToDecimal(filaSeleccionada.Cells["Precio"].Value);
                articulo.Codigo = filaSeleccionada.Cells["Codigo"].Value.ToString();
                articulo.ImagenUrl = filaSeleccionada.Cells["ImagenUrl"].Value.ToString();

                articulo.Marca.Descripcion = filaSeleccionada.Cells["Marca"].Value.ToString();
                articulo.Categoria.Descripcion = filaSeleccionada.Cells["Categoria"].Value.ToString();

                // Pasar el artículo al formulario Detalle
                Imagen detalle = new Imagen(articulo);
                detalle.ShowDialog();

                return articulo;
            }
            else
            {
                MessageBox.Show("Por favor, selecciona una fila.");
                return null;
            }
        }

       

        private void txbBuscador_TextChanged(object sender, EventArgs e)
        {
            List<Articulo> ListaFiltrada;
            string filtro = txbBuscador.Text;

            if (filtro.Length>=2)
            {
                ListaFiltrada = listaArticulos.FindAll(x => x.Nombre.ToUpper().Contains(filtro.ToUpper()) || x.Descripcion.ToUpper().Contains(filtro.ToUpper()));
            }
            else
            {
                ListaFiltrada = listaArticulos;
            }
            dgvDatos.DataSource = null;
            dgvDatos.DataSource = ListaFiltrada;
            ocultarColumnas();
        }

        private void cboCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string opcion = cboCampo.SelectedItem.ToString();

            if (opcion == "Precio")
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Mayor a");
                cboCriterio.Items.Add("Menor a");
                cboCriterio.Items.Add("Igual a");
            }
            else
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Empieza con");
                cboCriterio.Items.Add("Termina con");
                cboCriterio.Items.Add("Contiene");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                if (validarFiltro())
                {
                return;
                }
                
                string campo = cboCampo.SelectedItem.ToString();
                string criterio = cboCriterio.SelectedItem.ToString();
                string filtro = cboFiltro.Text;
                dgvDatos.DataSource = negocio.filtrar(campo, criterio, filtro);
            
                
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
        private bool validarFiltro()
        {
            if (cboCampo.SelectedIndex < 0)
            {
                MessageBox.Show("Seleccione campo a filtrar");
                return true;
            }
            if (cboCriterio.SelectedIndex < 0)
            {
                MessageBox.Show("Seleccione criterio a filtrar");
                return true;
            }
            if (cboCampo.SelectedItem.ToString() == "Precio")
            {
                if (!(soloNumeros(cboFiltro.Text)))
                {
                    MessageBox.Show("Ingrese un número");
                    return true;
                }
                
            }

            return false;
        }
        private bool soloNumeros(string cadena)
        {
            foreach (char caracter in cadena)
            {
                if (!(char.IsNumber(caracter)))
                {
                    return false;
                }
            }
            return true;
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            cboCampo.Items.Clear();
            cboCriterio.Items.Clear();
            cboFiltro.Clear();
            cboCampo.Items.Add("Precio");
            cboCampo.Items.Add("Nombre");
            cboCampo.Items.Add("Descripción");
        }
    }
} 


