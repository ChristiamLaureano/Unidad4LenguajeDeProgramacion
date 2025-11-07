using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace actividad3
{
    public partial class proveedores : Form
    {
        
        private const string connectionString = @"Data Source=DESKTOP-IJNOIL9;Initial Catalog=LenguajeProgrmacion;Integrated Security=True;";
       
        public proveedores()
        {
            InitializeComponent();
        }

        private void btnCargar_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT ProveedorID, NombreProveedor, Telefono, CorreoELectronico FROM PROVEEDORES;";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            dgClientes.DataSource = dt;
                        }
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Error al cargar categorías: " + ex.Message,
                        "Error SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnAgregar_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombreProveedor.Text))
            {
                MessageBox.Show("El nombre de la categoría es obligatorio.",
                    "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = "INSERT INTO Proveedors (NombreProveedor) VALUES (@Nombre)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Nombre", txtNombreProveedor.Text);

                    try
                    {
                        connection.Open();
                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                            MessageBox.Show("Categoría insertada correctamente ✔️");
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Error al insertar categoría: " + ex.Message,
                            "Error SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(txtNombreProveedor.Text))
            {
                MessageBox.Show("El nombre del proveedor es obligatorio.", "Advertencia",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string sql = @"INSERT INTO Proveedores (ProveedorID, NombreProveedor, Telefono, CorreoElectronico) 
                   VALUES (@id, @nombre, @telefono, @correo)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    // Si ProveedorID es autoincrement (IDENTITY), elimina esta línea y el parámetro @id
                    cmd.Parameters.AddWithValue("@id", int.Parse(txtProveedorID.Text));
                    cmd.Parameters.AddWithValue("@nombre", txtNombreProveedor.Text);
                    cmd.Parameters.AddWithValue("@telefono", textTelefono.Text);
                    cmd.Parameters.AddWithValue("@correo", textCorreoElectronico.Text);

                    try
                    {
                        connection.Open();
                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            MessageBox.Show("Proveedor agregado correctamente ✔️");
                            
                        }
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Error al insertar proveedor: " + ex.Message,
                            "Error SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(txtNombreProveedor.Text))
            {
                MessageBox.Show("El nombre del proveedor es obligatorio.", "Advertencia",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string sql = @"INSERT INTO Proveedores (ProveedorID, NombreProveedor, Telefono, CorreoElectronico) 
                   VALUES (@id, @nombre, @telefono, @correo)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    // Si ProveedorID es autoincrement (IDENTITY), elimina esta línea y el parámetro @id
                    cmd.Parameters.AddWithValue("@id", int.Parse(txtProveedorID.Text));
                    cmd.Parameters.AddWithValue("@nombre", txtNombreProveedor.Text);
                    cmd.Parameters.AddWithValue("@telefono", textTelefono.Text);
                    cmd.Parameters.AddWithValue("@correo", textCorreoElectronico.Text);

                    try
                    {
                        connection.Open();
                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            MessageBox.Show("Proveedor agregado correctamente ✔️");
                            
                        }
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Error al insertar proveedor: " + ex.Message,
                            "Error SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtIDEliminar.Text))
            {
                MessageBox.Show("Debes seleccionar un proveedor para eliminar.", "Advertencia",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtIDEliminar.Text, out int proveedorId))
            {
                MessageBox.Show("El ID del proveedor no es válido.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult result = MessageBox.Show(
                "¿Seguro que deseas eliminar este proveedor?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                string sql = "DELETE FROM Proveedores WHERE ProveedorID = @id";

                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@id", proveedorId);

                    try
                    {
                        connection.Open();
                        int rows = cmd.ExecuteNonQuery();

                        if (rows > 0)
                        {
                            MessageBox.Show("Proveedor eliminado correctamente ✔️");
                            CargarProveedores(); // refresca el DataGridView
                        }
                        else
                        {
                            MessageBox.Show("No se encontró el proveedor con ese ID.", "Aviso",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Error al eliminar proveedor: " + ex.Message,
                            "Error SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }


        private void dgClientes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // evitar cabeceras
            {
                DataGridViewRow fila = dgClientes.Rows[e.RowIndex];

                txtProveedorID.Text = fila.Cells["ProveedorID"].Value.ToString();
                txtNombreProveedor.Text = fila.Cells["NombreProveedor"].Value.ToString();
                textTelefono.Text = fila.Cells["Telefono"].Value.ToString();
                textCorreoElectronico.Text = fila.Cells["CorreoElectronico"].Value.ToString();
            }
        }


        private void CargarProveedores()
        {
            throw new NotImplementedException();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            
        
            // Validar que el ID no esté vacío
            if (string.IsNullOrWhiteSpace(txtIDActualizar.Text))
            {
                MessageBox.Show("Debes seleccionar un proveedor para actualizar.", "Advertencia",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validar que el ID sea un número válido
            if (!int.TryParse(txtIDActualizar.Text, out int proveedorId))
            {
                MessageBox.Show("El ID del proveedor no es válido.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Validar nombre obligatorio
            if (string.IsNullOrWhiteSpace(txtNombreActualizado.Text))
            {
                MessageBox.Show("El nombre del proveedor es obligatorio.", "Advertencia",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Consulta SQL de actualización
            string sql = @"UPDATE Proveedores
                   SET NombreProveedor = @nombre,
                       Telefono = @telefono,
                       CorreoElectronico = @correo
                   WHERE ProveedorID = @id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(sql, connection))
            {
                // Parámetros correctos con los textbox de actualización
                cmd.Parameters.AddWithValue("@nombre", txtNombreActualizado.Text);
                cmd.Parameters.AddWithValue("@telefono", textTelefonoActualizado.Text);
                cmd.Parameters.AddWithValue("@correo", textCorreoElectronicoActualizado.Text);
                cmd.Parameters.AddWithValue("@id", proveedorId);

                try
                {
                    connection.Open();
                    int rows = cmd.ExecuteNonQuery();

                    if (rows > 0)
                    {
                        MessageBox.Show("Proveedor actualizado correctamente ✔️");
                        CargarProveedores(); // Refresca el DataGridView después de actualizar
                    }
                    else
                    {
                        MessageBox.Show("No se encontró el proveedor con ese ID.", "Aviso",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Error al actualizar proveedor: " + ex.Message,
                        "Error SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

    }
}