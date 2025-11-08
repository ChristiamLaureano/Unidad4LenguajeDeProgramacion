using ActividadTres.LenguajeProgrmacion;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ProveedorEntity = ActividadTres.LenguajeProgrmacion.Proveedores;

namespace ActividadTres
{
    public partial class Proveedores : Form
    {
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

        // ====== UTILIDADES ======
        private void cargarDatos()
        {
            try
            {
                var lista = _context.Set<ProveedorEntity>()
                    .Select(p => new
                    {
                        ID = p.ProveedorID,
                        Nombre = p.NombreProveedor,
                        Telefono = p.Telefono,
                        Correo = p.CorreoElectronico
                    })
                    .OrderBy(x => x.ID)
                    .ToList();

                dgProveedores.DataSource = lista;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar proveedores: " + ex.Message);
            }
        }

        private static bool EmailValido(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            // Validación simple
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        private void limpiarInsertar()
        {
            // Si tu ID es IDENTITY, no uses txtProveedorID
            if (txtProveedorID != null) txtProveedorID.Clear();
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

        // ====== BOTONES ======
        private void btnCargar_Click(object sender, EventArgs e) => cargarDatos();

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            // VALIDACIONES
            if (string.IsNullOrWhiteSpace(txtNombreProveedor.Text) ||
                string.IsNullOrWhiteSpace(txtTelefono.Text) ||
                string.IsNullOrWhiteSpace(txtCorreoElectronico.Text))
            {
                MessageBox.Show("Complete Nombre, Teléfono y Correo.");
                return;
            }

            if (!EmailValido(txtCorreoElectronico.Text))
            {
                MessageBox.Show("Correo no válido.");
                return;
            }

            // Crear entidad
            var proveedor = new ProveedorEntity
            {
                NombreProveedor = txtNombreProveedor.Text.Trim(),
                Telefono = txtTelefono.Text.Trim(),
                CorreoElectronico = txtCorreoElectronico.Text.Trim()
            };

            // ⚠️ Si tu ProveedorID NO es IDENTITY y lo escribes manual:
            // if (!int.TryParse(txtProveedorID.Text, out int idManual)) { MessageBox.Show("ID inválido."); return; }
            // proveedor.ProveedorID = idManual;

            try
            {
                _context.Set<ProveedorEntity>().Add(proveedor);
                _context.SaveChanges();
                MessageBox.Show("Proveedor agregado.");
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
                MessageBox.Show("Debe introducir un ID válido.");
                return;
            }

            var proveedor = _context.Set<ProveedorEntity>().FirstOrDefault(p => p.ProveedorID == id);
            if (proveedor == null)
            {
                MessageBox.Show("Proveedor no encontrado.");
                return;
            }

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
                MessageBox.Show("Error al eliminar: " + ex.Message);
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
                MessageBox.Show("Correo no válido.");
                return;
            }

            var proveedor = _context.Set<ProveedorEntity>().FirstOrDefault(p => p.ProveedorID == id);
            if (proveedor == null)
            {
                MessageBox.Show("Proveedor no encontrado.");
                return;
            }

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
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar: " + ex.Message);
            }
        }

        // ====== (Opcional) Autollenar desde la grilla ======
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

            txtEliminar.Text = id; // práctico para borrar rápido
        }
    }
}
