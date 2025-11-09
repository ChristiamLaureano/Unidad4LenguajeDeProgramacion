using ActividadTres.LenguajeProgrmacion;
using System;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ProveedorEntity = ActividadTres.LenguajeProgrmacion.Proveedores;

namespace ActividadTres
{
    public partial class Proveedores : Form
    {
        // Usa SIEMPRE el mismo contexto del proyecto
        private readonly LenguajeProgrmacionEntities1 _context;

        public Proveedores()
        {
            InitializeComponent();
            _context = new LenguajeProgrmacionEntities1();
            this.Load += Proveedores_Load;
            this.FormClosed += (s, e) => _context.Dispose();
            dgProveedores.AutoGenerateColumns = true;
        }

        private void Proveedores_Load(object sender, EventArgs e)
        {
            cargarDatos();
            limpiarInsertar();
            limpiarActualizar();
            txtEliminar.Clear();
        }

        private void cargarDatos()
        {
            try
            {
                var lista = _context.Set<ProveedorEntity>()
                    .Select(p => new { ID = p.ProveedorID, Nombre = p.NombreProveedor, Telefono = p.Telefono, Correo = p.CorreoElectronico })
                    .OrderBy(x => x.ID)
                    .ToList();

                dgProveedores.DataSource = lista;
            }
            catch (Exception ex) { MostrarError(ex, "cargar proveedores"); }
        }

        private static bool EmailValido(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        private void limpiarInsertar()
        {
            txtProveedorID?.Clear(); // ignorado si es IDENTITY
            txtNombreProveedor.Clear();
            txtTelefono.Clear();
            txtCorreoElectronico.Clear();
        }

        private void limpiarActualizar()
        {
            textProveedorIDActualizar.Clear();
            txtProveedorActualizar.Clear();
            txtTelefonoActualizar.Clear();
            txtCorreoElectronicoActualizar.Clear();
        }

        // -------- Helpers de errores ----------
        private void MostrarError(Exception ex, string accion)
        {
            string msg = "Error al " + accion + ":\n" + ex.Message;
            var i = ex.InnerException;
            while (i != null) { msg += "\n→ " + i.Message; i = i.InnerException; }
            MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // -------- Botones ----------
        private void btnCargar_Click(object sender, EventArgs e) => cargarDatos();

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombreProveedor.Text) ||
                string.IsNullOrWhiteSpace(txtTelefono.Text) ||
                string.IsNullOrWhiteSpace(txtCorreoElectronico.Text))
            {
                MessageBox.Show("Complete Nombre, Teléfono y Correo.");
                return;
            }
            if (!EmailValido(txtCorreoElectronico.Text))
            {
                MessageBox.Show("Correo no válido."); return;
            }

            var proveedor = new ProveedorEntity
            {
                NombreProveedor = txtNombreProveedor.Text.Trim(),
                Telefono = txtTelefono.Text.Trim(),
                CorreoElectronico = txtCorreoElectronico.Text.Trim()
            };

            try
            {
                _context.Set<ProveedorEntity>().Add(proveedor);
                _context.SaveChanges();
                MessageBox.Show("Proveedor agregado.");
                cargarDatos();
                limpiarInsertar();
            }
            catch (Exception ex) { MostrarError(ex, "agregar proveedor"); }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtEliminar.Text, out int id))
            { MessageBox.Show("ID inválido."); return; }

            var proveedor = _context.Set<ProveedorEntity>().FirstOrDefault(p => p.ProveedorID == id);
            if (proveedor == null) { MessageBox.Show("Proveedor no encontrado."); return; }

            if (MessageBox.Show("¿Eliminar este proveedor?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            try
            {
                _context.Set<ProveedorEntity>().Remove(proveedor);
                _context.SaveChanges();
                MessageBox.Show("Proveedor eliminado.");
                cargarDatos();
                txtEliminar.Clear();
            }
            catch (Exception ex)
            {
                // Mensaje amigable si hay dependencias (FK)
                var full = ex.ToString();
                if (full.Contains("DELETE statement conflicted"))
                {
                    MessageBox.Show("No se puede eliminar: hay registros relacionados (Compras/Productos).", "Restricción de clave foránea", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                MostrarError(ex, "eliminar proveedor");
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(textProveedorIDActualizar.Text, out int id) ||
                string.IsNullOrWhiteSpace(txtProveedorActualizar.Text) ||
                string.IsNullOrWhiteSpace(txtTelefonoActualizar.Text) ||
                string.IsNullOrWhiteSpace(txtCorreoElectronicoActualizar.Text))
            {
                MessageBox.Show("Complete todos los campos correctamente.");
                return;
            }
            if (!EmailValido(txtCorreoElectronicoActualizar.Text))
            {
                MessageBox.Show("Correo no válido."); return;
            }

            var proveedor = _context.Set<ProveedorEntity>().FirstOrDefault(p => p.ProveedorID == id);
            if (proveedor == null) { MessageBox.Show("Proveedor no encontrado."); return; }

            proveedor.NombreProveedor = txtProveedorActualizar.Text.Trim();
            proveedor.Telefono = txtTelefonoActualizar.Text.Trim();
            proveedor.CorreoElectronico = txtCorreoElectronicoActualizar.Text.Trim();

            try
            {
                _context.SaveChanges();
                MessageBox.Show("Proveedor actualizado.");
                cargarDatos();
                limpiarActualizar();
            }
            catch (Exception ex) { MostrarError(ex, "actualizar proveedor"); }
        }

        private void dgProveedores_SelectionChanged(object sender, EventArgs e)
        {
            if (dgProveedores.CurrentRow?.DataBoundItem == null) return;

            var row = dgProveedores.CurrentRow;
            var id = row.Cells["ID"].Value?.ToString();
            var nombre = row.Cells["Nombre"].Value?.ToString();
            var tel = row.Cells["Telefono"].Value?.ToString();
            var correo = row.Cells["Correo"].Value?.ToString();

            textProveedorIDActualizar.Text = id;
            txtProveedorActualizar.Text = nombre;
            txtTelefonoActualizar.Text = tel;
            txtCorreoElectronicoActualizar.Text = correo;

            txtEliminar.Text = id;
        }
    }
}
