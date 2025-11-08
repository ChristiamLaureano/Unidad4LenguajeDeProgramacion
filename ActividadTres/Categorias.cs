using ActividadTres.LenguajeProgrmacion;
using System;
using System.Linq;
using System.Windows.Forms;
using CategoriaEntity = ActividadTres.LenguajeProgrmacion.Categorias;

namespace ActividadTres
{
    public partial class Categorias : Form
    {
        private readonly LenguajeProgrmacionEntities1 _context;

        public Categorias()
        {
            InitializeComponent();
            _context = new LenguajeProgrmacionEntities1();
            this.Load += Categorias_Load;
            this.FormClosed += (s, e) => _context.Dispose();

            if (dgCategorias != null) dgCategorias.AutoGenerateColumns = true;
        }

        private void Categorias_Load(object sender, EventArgs e)
        {
            cargarDatos();
            limpiarInsertar();
            limpiarActualizar();
            txtEliminar?.Clear();
        }

        // ====== UTILIDADES ======
        private void cargarDatos()
        {
            try
            {
                var lista = _context.Set<CategoriaEntity>()
                    .Select(c => new { ID = c.CategoriaID, Nombre = c.NombreCategoria })
                    .OrderBy(x => x.ID)
                    .ToList();

                dgCategorias.DataSource = lista;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar categorías: " + ex.Message);
            }
        }

        private void limpiarInsertar()
        {
            // Si tu ID ES IDENTITY, NO uses txtCategoriaID para insertar.
            txtCategoriaID?.Clear();
            txtNombreCategoria?.Clear();
        }

        private void limpiarActualizar()
        {
            txtCategoriaIDActualizar?.Clear();
            txtNombreCategoriaActualizar?.Clear();
        }

        // ====== BOTONES ======
        private void btnCargar_Click(object sender, EventArgs e) => cargarDatos();

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombreCategoria.Text))
            {
                MessageBox.Show("Ingrese el nombre de la categoría.");
                return;
            }

            var categoria = new CategoriaEntity
            {
                NombreCategoria = txtNombreCategoria.Text.Trim()
            };

            // ⚠️ Solo si tu ID NO es IDENTITY y quieres escribirlo manualmente, descomenta:
            // if (!int.TryParse(txtCategoriaID.Text, out int idManual)) { MessageBox.Show("ID inválido."); return; }
            // categoria.CategoriaID = idManual;

            try
            {
                _context.Set<CategoriaEntity>().Add(categoria);
                _context.SaveChanges();
                MessageBox.Show("Categoría agregada correctamente.");
                cargarDatos();
                limpiarInsertar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar: " + ex.Message);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtEliminar.Text, out int id))
            {
                MessageBox.Show("Debe ingresar un ID válido.");
                return;
            }

            var categoria = _context.Set<CategoriaEntity>().FirstOrDefault(c => c.CategoriaID == id);
            if (categoria == null)
            {
                MessageBox.Show("Categoría no encontrada.");
                return;
            }

            if (MessageBox.Show("¿Eliminar esta categoría?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            try
            {
                _context.Set<CategoriaEntity>().Remove(categoria);
                _context.SaveChanges();
                MessageBox.Show("Categoría eliminada.");
                cargarDatos();
                txtEliminar.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar: " + ex.Message);
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtCategoriaIDActualizar.Text, out int id) ||
                string.IsNullOrWhiteSpace(txtNombreCategoriaActualizar.Text))
            {
                MessageBox.Show("Complete ID y Nombre.");
                return;
            }

            var categoria = _context.Set<CategoriaEntity>().FirstOrDefault(c => c.CategoriaID == id);
            if (categoria == null)
            {
                MessageBox.Show("Categoría no encontrada.");
                return;
            }

            categoria.NombreCategoria = txtNombreCategoriaActualizar.Text.Trim();

            try
            {
                _context.SaveChanges();
                MessageBox.Show("Categoría actualizada correctamente.");
                cargarDatos();
                limpiarActualizar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar: " + ex.Message);
            }
        }

        // ====== (Opcional) Autollenar desde la grilla ======
        private void dgCategorias_SelectionChanged(object sender, EventArgs e)
        {
            if (dgCategorias.CurrentRow?.DataBoundItem == null) return;

            var row = dgCategorias.CurrentRow;
            var id = row.Cells["ID"].Value?.ToString();
            var nombre = row.Cells["Nombre"].Value?.ToString();

            txtCategoriaIDActualizar.Text = id;
            txtNombreCategoriaActualizar.Text = nombre;
            txtEliminar.Text = id;
        }
    }
}
