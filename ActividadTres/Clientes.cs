using ActividadTres.LenguajeProgrmacion;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ClienteEntity = ActividadTres.LenguajeProgrmacion.Clientes;

namespace ActividadTres
{
    public partial class Clientes : Form
    {
        private readonly LenguajeProgrmacionEntities1 _context;

        public Clientes()
        {
            InitializeComponent();
            _context = new LenguajeProgrmacionEntities1();
            this.Load += Clientes_Load;
            this.FormClosed += (s, e) => _context.Dispose();
            if (dgClientes != null) dgClientes.AutoGenerateColumns = true;
        }

        private void Clientes_Load(object sender, EventArgs e)
        {
            cargarDatos();
            limpiarInsertar();
            limpiarActualizar();
            txtEliminar?.Clear();
        }

        // ============ UTILIDADES ============
        private void cargarDatos()
        {
            try
            {
                var lista = _context.Set<ClienteEntity>()
                    .Select(c => new
                    {
                        ID = c.ClienteID,
                        Nombre = c.NombreCompleto,
                        Correo = c.CorreoElectronico,
                        Telefono = c.Telefono,
                        Direccion = c.Direccion
                    })
                    .OrderBy(x => x.ID)
                    .ToList();

                dgClientes.DataSource = lista;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar clientes: " + ex.Message);
            }
        }

        private static bool EmailValido(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        private void limpiarInsertar()
        {
            // Si ClienteID es IDENTITY, no uses txtClienteID para insertar.
            txtClienteID?.Clear();
            txtNombreCompleto?.Clear();
            txtCorreoEletronico?.Clear();
            txtTelefono?.Clear();
            txtDireccion?.Clear();
        }

        private void limpiarActualizar()
        {
            textIDActualizar?.Clear();
            txtNombreCompletoActualizar?.Clear();
            txtCorreoelectronicoActualizar?.Clear();
            txtTelefonoActualizar?.Clear();
            txtDireccionActualizar?.Clear();
        }

        // ============ BOTONES ============
        private void btnCargar_Click(object sender, EventArgs e) => cargarDatos();

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombreCompleto.Text) ||
                string.IsNullOrWhiteSpace(txtCorreoEletronico.Text) ||
                string.IsNullOrWhiteSpace(txtTelefono.Text) ||
                string.IsNullOrWhiteSpace(txtDireccion.Text))
            {
                MessageBox.Show("Complete Nombre, Correo, Teléfono y Dirección.");
                return;
            }

            if (!EmailValido(txtCorreoEletronico.Text))
            {
                MessageBox.Show("Correo no válido.");
                return;
            }

            var cliente = new ClienteEntity
            {
                NombreCompleto = txtNombreCompleto.Text.Trim(),
                CorreoElectronico = txtCorreoEletronico.Text.Trim(),
                Telefono = txtTelefono.Text.Trim(),
                Direccion = txtDireccion.Text.Trim()
            };

            // ⚠️ Solo si NO es IDENTITY y deseas escribir el ID manual:
            // if (!int.TryParse(txtClienteID.Text, out int idManual)) { MessageBox.Show("ID inválido."); return; }
            // cliente.ClienteID = idManual;

            try
            {
                _context.Set<ClienteEntity>().Add(cliente);
                _context.SaveChanges();
                MessageBox.Show("Cliente agregado correctamente.");
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

            var cliente = _context.Set<ClienteEntity>().FirstOrDefault(c => c.ClienteID == id);
            if (cliente == null)
            {
                MessageBox.Show("Cliente no encontrado.");
                return;
            }

            if (MessageBox.Show("¿Eliminar este cliente?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            try
            {
                _context.Set<ClienteEntity>().Remove(cliente);
                _context.SaveChanges();
                MessageBox.Show("Cliente eliminado.");
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
            if (!int.TryParse(textIDActualizar.Text, out int id) ||
                string.IsNullOrWhiteSpace(txtNombreCompletoActualizar.Text) ||
                string.IsNullOrWhiteSpace(txtCorreoelectronicoActualizar.Text) ||
                string.IsNullOrWhiteSpace(txtTelefonoActualizar.Text) ||
                string.IsNullOrWhiteSpace(txtDireccionActualizar.Text))
            {
                MessageBox.Show("Complete ID y todos los campos.");
                return;
            }

            if (!EmailValido(txtCorreoelectronicoActualizar.Text))
            {
                MessageBox.Show("Correo no válido.");
                return;
            }

            var cliente = _context.Set<ClienteEntity>().FirstOrDefault(c => c.ClienteID == id);
            if (cliente == null)
            {
                MessageBox.Show("Cliente no encontrado.");
                return;
            }

            cliente.NombreCompleto = txtNombreCompletoActualizar.Text.Trim();
            cliente.CorreoElectronico = txtCorreoelectronicoActualizar.Text.Trim();
            cliente.Telefono = txtTelefonoActualizar.Text.Trim();
            cliente.Direccion = txtDireccionActualizar.Text.Trim();

            try
            {
                _context.SaveChanges();
                MessageBox.Show("Cliente actualizado.");
                cargarDatos();
                limpiarActualizar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar: " + ex.Message);
            }
        }

        // ============ (Opcional) Autollenar desde la grilla ============
        private void dgClientes_SelectionChanged(object sender, EventArgs e)
        {
            if (dgClientes.CurrentRow?.DataBoundItem == null) return;

            var row = dgClientes.CurrentRow;
            var id = row.Cells["ID"].Value?.ToString();
            var nombre = row.Cells["Nombre"].Value?.ToString();
            var correo = row.Cells["Correo"].Value?.ToString();
            var tel = row.Cells["Telefono"].Value?.ToString();
            var dir = row.Cells["Direccion"].Value?.ToString();

            textIDActualizar.Text = id;
            txtNombreCompletoActualizar.Text = nombre;
            txtCorreoelectronicoActualizar.Text = correo;
            txtTelefonoActualizar.Text = tel;
            txtDireccionActualizar.Text = dir;

            txtEliminar.Text = id;
        }
    }
}
