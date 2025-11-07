using actividad3;
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

namespace EjercicioBaseDeDatosWinForm
{
    // Cambiamos el nombre de la clase a Clientes (o el nombre de tu formulario de clientes)
    public partial class Clientes : Form
    {
        // TODO: Debes cambiar esta variable connectionString para que pueda conectarse a tu base de datos.
        // Se define una sola vez como constante para no repetirla.
        private const string connectionString = @"Data Source=DESKTOP-IJNOIL9;Initial Catalog=LenguajeProgrmacion;Integrated Security=True;";

        public Clientes()
        {
            InitializeComponent();
        }

        // Ya no necesitamos cargar un ComboBox de categorías, solo cargamos los clientes.
        private void cargarDatos()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Ajustamos la consulta para la tabla de clientes
                    string queryClientes = "SELECT ClienteID, NombreCompleto, Telefono, CorreoElectronico, Direccion FROM Clientes;";

                    using (SqlCommand cmd = new SqlCommand(queryClientes, connection))
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            // Ajustamos el nombre del DataGridView
                            dgClientes.DataSource = dt;
                        }
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Error al cargar datos de clientes: " + ex.Message, "Error SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            } // La conexión se cierra automáticamente gracias al 'using'
        }

        // Mantenemos el botón de cargar por si se usa
        private void btnCargar_Click(object sender, EventArgs e)
        {
            this.cargarDatos();
        }

        // ----------------------------------------------------------------------
        // OPERACIÓN: INSERTAR CLIENTE (btnAgregar_Click)
        // ----------------------------------------------------------------------
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            // Validaciones para evitar insertar datos erróneos.
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(textTelefonoProveedor.Text) ||
                string.IsNullOrWhiteSpace(textCorreoElectronicoProveedor.Text) ||
                string.IsNullOrWhiteSpace(IDProveedor.Text))
            {
                MessageBox.Show("Todos los campos (Nombre, Teléfono, Correo y Dirección) son obligatorios.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Usamos parámetros SQL para construir la consulta de forma segura.
            string queryInsertarClientes = @"INSERT INTO Clientes (NombreCompleto, Telefono, CorreoElectronico, Direccion)
                                            VALUES (@Nombre, @Telefono, @Correo, @Direccion)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(queryInsertarClientes, connection))
                {
                    // ❗ AÑADIMOS PARÁMETROS: Esto es lo más importante para corregir el error de inyección SQL ❗
                    cmd.Parameters.AddWithValue("@Nombre", txtNombre.Text);
                    cmd.Parameters.AddWithValue("@Telefono", textTelefonoProveedor.Text);
                    cmd.Parameters.AddWithValue("@Correo", textCorreoElectronicoProveedor.Text);
                    cmd.Parameters.AddWithValue("@Direccion", IDProveedor.Text);

                    try
                    {
                        connection.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Se ha insertado el cliente en la base de datos. ✔️");
                        }
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Error al insertar cliente: " + ex.Message, "Error SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            this.cargarDatos();
        }

        // ----------------------------------------------------------------------
        // OPERACIÓN: ELIMINAR CLIENTE (btnEliminar_Click)
        // ----------------------------------------------------------------------
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            // Asumo que tienes un control llamado txtClienteID para el ID a eliminar.
            if (string.IsNullOrWhiteSpace(txtIDEliminar.Text) || !int.TryParse(txtIDEliminar.Text, out _))
            {
                MessageBox.Show("Debe introducir un ID de cliente numérico y válido.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Usamos parámetros SQL para construir la consulta de forma segura.
            string queryEliminarCliente = @"DELETE FROM Clientes WHERE ClienteID = @ID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(queryEliminarCliente, connection))
                {
                    // ❗ AÑADIMOS PARÁMETROS: Protegemos el ID contra Inyección SQL ❗
                    cmd.Parameters.AddWithValue("@ID", txtIDEliminar.Text);

                    try
                    {
                        connection.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Se ha eliminado el cliente de la base de datos. 🗑️");
                        }
                        else
                        {
                            MessageBox.Show("No se encontró un cliente con ese ID.", "Información");
                        }
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Error al eliminar cliente: " + ex.Message, "Error SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            this.cargarDatos();
        }

        // ----------------------------------------------------------------------
        // OPERACIÓN: ACTUALIZAR CLIENTE (btnActualizar_Click)
        // ----------------------------------------------------------------------
        private void btnActualizar_Click(object sender, EventArgs e)
        {
            // Asumo que tienes un control llamado txtClienteIDActualizar para el ID a actualizar.
            if (string.IsNullOrWhiteSpace(txtIDActualizar.Text) || !int.TryParse(txtIDActualizar.Text, out _))
            {
                MessageBox.Show("Debe introducir un ID de cliente numérico y válido para actualizar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validaciones para los nuevos datos
            if (string.IsNullOrWhiteSpace(txtNombreActualizar.Text) ||
                string.IsNullOrWhiteSpace(textTelefonoActualizar.Text) ||
                string.IsNullOrWhiteSpace(textCorreoElectronicoActualizar.Text) ||
                string.IsNullOrWhiteSpace(textDireccionActualizar.Text))
            {
                MessageBox.Show("Todos los campos para actualizar son obligatorios.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Usamos parámetros SQL para construir la consulta de forma segura.
            string queryActualizarClientes = @"UPDATE Clientes 
                                             SET NombreCompleto = @Nombre, 
                                                 Telefono = @Telefono, 
                                                 CorreoElectronico = @Correo, 
                                                 Direccion = @Direccion 
                                             WHERE ClienteID = @ID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(queryActualizarClientes, connection))
                {
                    // ❗ AÑADIMOS PARÁMETROS para todos los valores ❗
                    cmd.Parameters.AddWithValue("@Nombre", txtNombreActualizar.Text);
                    cmd.Parameters.AddWithValue("@Telefono", textTelefonoActualizar.Text);
                    cmd.Parameters.AddWithValue("@Correo", textCorreoElectronicoActualizar.Text);
                    cmd.Parameters.AddWithValue("@Direccion", textDireccionActualizar.Text);
                    cmd.Parameters.AddWithValue("@ID", txtIDEliminar.Text);

                    try
                    {
                        connection.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Se ha actualizado el cliente en la base de datos. 🔄");
                        }
                        else
                        {
                            MessageBox.Show("No se encontró un cliente con ese ID para actualizar.", "Información");
                        }
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Error al actualizar cliente: " + ex.Message, "Error SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            this.cargarDatos();
        }

        // ----------------------------------------------------------------------
        // CARGA INICIAL
        // ----------------------------------------------------------------------
        private void Clientes_Load(object sender, EventArgs e)
        {
            // Solo cargamos los datos de los clientes al iniciar el formulario
            this.cargarDatos();
        }

        private void btnActualizar_Click_1(object sender, EventArgs e)
        {
            // Validación del ID
            if (string.IsNullOrWhiteSpace(txtIDActualizar.Text) || !int.TryParse(txtIDActualizar.Text, out int clienteID))
            {
                MessageBox.Show("Debe introducir un ID de cliente numérico y válido para actualizar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validaciones de los nuevos datos
            if (string.IsNullOrWhiteSpace(txtNombreActualizar.Text) ||
                string.IsNullOrWhiteSpace(textTelefonoActualizar.Text) ||
                string.IsNullOrWhiteSpace(textCorreoElectronicoActualizar.Text) ||
                string.IsNullOrWhiteSpace(textDireccionActualizar.Text))
            {
                MessageBox.Show("Todos los campos para actualizar son obligatorios.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Consulta con parámetros
            string queryActualizarClientes = @"UPDATE Clientes 
                                  SET NombreCompleto = @Nombre, 
                                      Telefono = @Telefono, 
                                      CorreoElectronico = @Correo, 
                                      Direccion = @Direccion 
                                  WHERE ClienteID = @ID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(queryActualizarClientes, connection))
                {
                    cmd.Parameters.AddWithValue("@Nombre", txtNombreActualizar.Text);
                    cmd.Parameters.AddWithValue("@Telefono", textTelefonoActualizar.Text);
                    cmd.Parameters.AddWithValue("@Correo", textCorreoElectronicoActualizar.Text);
                    cmd.Parameters.AddWithValue("@Direccion", textDireccionActualizar.Text);
                    cmd.Parameters.AddWithValue("@ID", clienteID); // ahora usamos el int validado

                    try
                    {
                        connection.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("✅ Cliente actualizado correctamente.");
                        }
                        else
                        {
                            MessageBox.Show("⚠ No se encontró un cliente con ese ID para actualizar.", "Información");
                        }
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("❌ Error al actualizar cliente: " + ex.Message, "Error SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            this.cargarDatos();
        }

        private void btnEliminar_Click_1(object sender, EventArgs e)
        {
            // Asumo que tienes un control llamado txtClienteID para el ID a eliminar.
            if (string.IsNullOrWhiteSpace(txtIDEliminar.Text) || !int.TryParse(txtIDEliminar.Text, out _))
            {
                MessageBox.Show("Debe introducir un ID de cliente numérico y válido.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Usamos parámetros SQL para construir la consulta de forma segura.
            string queryEliminarCliente = @"DELETE FROM Clientes WHERE ClienteID = @ID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(queryEliminarCliente, connection))
                {
                    // ❗ AÑADIMOS PARÁMETROS: Protegemos el ID contra Inyección SQL ❗
                    cmd.Parameters.AddWithValue("@ID", txtIDEliminar.Text);

                    try
                    {
                        connection.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Se ha eliminado el cliente de la base de datos. 🗑️");
                        }
                        else
                        {
                            MessageBox.Show("No se encontró un cliente con ese ID.", "Información");
                        }
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Error al eliminar cliente: " + ex.Message, "Error SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            this.cargarDatos();
        }
    }
    
}
