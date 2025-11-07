using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace actividad3
{
    public partial class Productos : Form
    {
        private const string connectionString = @"Data Source=DESKTOP-IJNOIL9;Initial Catalog=LenguajeProgrmacion;Integrated Security=True;";
        private int filas;

        public Productos()
        {
            InitializeComponent();
        }

        // ======================================================
        // Cargar productos en el DataGridView
        // ======================================================
        private void cargarDatosProductos()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = @"
                        SELECT p.ProductoID, p.NombreProducto, p.Descripcion, p.Precio, p.Stock,
                               c.NombreCategoria
                        FROM Productos p
                        LEFT JOIN Categorias c ON p.CategoriaID = c.CategoriaID;";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dgClientes.DataSource = dt; // tu DataGridView
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Error al cargar productos: " + ex.Message);
                }
            }
        }

        // ======================================================
        // Evento Load del formulario
        // ======================================================
        private void Productos_Load(object sender, EventArgs e)
        {
            cargarDatosProductos();
        }

        // ======================================================
        // Botón refrescar
        // ======================================================
        private void btnCargar_Click(object sender, EventArgs e)
        {
            cargarDatosProductos();
        }

        // ======================================================
        // Botón Agregar Producto
        // ======================================================
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            // Validaciones
            if (string.IsNullOrWhiteSpace(textNombreProducto.Text) ||
                string.IsNullOrWhiteSpace(textDescripcion.Text) ||
                string.IsNullOrWhiteSpace(textPrecio.Text) ||
                string.IsNullOrWhiteSpace(textStock.Text) ||
                string.IsNullOrWhiteSpace(textCategoriaID.Text))
            {
                MessageBox.Show("Todos los campos son obligatorios (Nombre, Descripción, Precio, Stock y Categoría).",
                                "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(textPrecio.Text, out decimal precio))
            {
                MessageBox.Show("El precio debe ser un número decimal válido.");
                return;
            }

            if (!int.TryParse(textStock.Text, out int stock))
            {
                MessageBox.Show("El stock debe ser un número entero válido.");
                return;
            }

            if (!int.TryParse(textCategoriaID.Text, out int categoriaId))
            {
                MessageBox.Show("El ID de la categoría debe ser un número entero válido.");
                return;
            }

            string query = @"
                INSERT INTO Productos (NombreProducto, Descripcion, Precio, Stock, CategoriaID)
                VALUES (@Nombre, @Descripcion, @Precio, @Stock, @CategoriaID)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@Nombre", textNombreProducto.Text.Trim());
                cmd.Parameters.AddWithValue("@Descripcion", textDescripcion.Text.Trim());
                cmd.Parameters.AddWithValue("@Precio", precio);
                cmd.Parameters.AddWithValue("@Stock", stock);
                cmd.Parameters.AddWithValue("@CategoriaID", categoriaId);

                try
                {
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Producto insertado correctamente ✔️");
                    cargarDatosProductos();

                    // limpiar campos
                    textNombreProducto.Clear();
                    textDescripcion.Clear();
                    textPrecio.Clear();
                    textStock.Clear();
                    textCategoriaID.Clear();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Error al insertar producto: " + ex.Message);
                }
            }
        }

        // ======================================================
        // Botón Actualizar Producto
        // ======================================================
        private void btnActualizar_Click(object sender, EventArgs e)
        {
            if (dgClientes.CurrentRow == null)
            {
                MessageBox.Show("Debes seleccionar un producto para actualizar.");
                return;
            }

            if (!int.TryParse(dgClientes.CurrentRow.Cells["ProductoID"].Value.ToString(), out int productoId))
            {
                MessageBox.Show("Selecciona un producto válido.");
                return;
            }

            // Validaciones de campos
            if (!decimal.TryParse(textPrecio.Text, out decimal precio) ||
                !int.TryParse(textStock.Text, out int stock) ||
                !int.TryParse(textCategoriaID.Text, out int categoriaId))
            {
                MessageBox.Show("Revisa los valores de Precio, Stock y CategoriaID.");
                return;
            }

            string query = @"
                UPDATE Productos
                SET NombreProducto=@Nombre, Descripcion=@Descripcion, Precio=@Precio, Stock=@Stock, CategoriaID=@CategoriaID
                WHERE ProductoID=@ProductoID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@ProductoID", productoId);
                cmd.Parameters.AddWithValue("@Nombre", textNombreProducto.Text.Trim());
                cmd.Parameters.AddWithValue("@Descripcion", textDescripcion.Text.Trim());
                cmd.Parameters.AddWithValue("@Precio", precio);
                cmd.Parameters.AddWithValue("@Stock", stock);
                cmd.Parameters.AddWithValue("@CategoriaID", categoriaId);

                try
                {
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Producto actualizado correctamente ✔️");
                    cargarDatosProductos();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Error al actualizar producto: " + ex.Message);
                }
            }
        }

        // ======================================================
        // Botón Eliminar Producto
        // ======================================================
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgClientes.CurrentRow == null)
            {
                MessageBox.Show("Debes seleccionar un producto para eliminar.");
                return;
            }

            int productoId = Convert.ToInt32(dgClientes.CurrentRow.Cells["ProductoID"].Value);

            string query = "DELETE FROM Productos WHERE ProductoID=@ProductoID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@ProductoID", productoId);

                try
                {
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Producto eliminado correctamente ✔️");
                    cargarDatosProductos();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Error al eliminar producto: " + ex.Message);
                }
            }
        }

        private void btnEliminar_Click_1(object sender, EventArgs e)
        {
            if (dgClientes.CurrentRow == null)
            {
                MessageBox.Show("Debes seleccionar un producto para eliminar.");
                return;
            }

            int productoId = Convert.ToInt32(dgClientes.CurrentRow.Cells["ProductoID"].Value);

            string query = "DELETE FROM Productos WHERE ProductoID=@ProductoID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@ProductoID", productoId);

                try
                {
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Producto eliminado correctamente ✔️");
                    cargarDatosProductos();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Error al eliminar producto: " + ex.Message);
                }
            }
        }

        private void btnActualizar_Click_1(object sender, EventArgs e)
        {
            try
            {
                // 1) Validar ID del producto
                if (string.IsNullOrWhiteSpace(txtProductoID.Text))
                {
                    MessageBox.Show("Debes seleccionar un producto para actualizar.", "Aviso",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (!int.TryParse(txtProductoID.Text, out int productoId))
                {
                    MessageBox.Show("El ID de producto no es válido.", "Aviso",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 2) Validar numéricos: Precio, Stock y CategoriaID
                if (!decimal.TryParse(TxtPrecioActulizado.Text, out decimal precio))
                {
                    MessageBox.Show("El precio no es válido.", "Aviso",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!int.TryParse(textStockActualizado.Text, out int stock))
                {
                    MessageBox.Show("El stock debe ser un número entero.", "Aviso",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!int.TryParse(txtCategoriaIDActualizado.Text, out int categoriaId))
                {
                    MessageBox.Show("El ID de categoría debe ser un número entero.", "Aviso",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 3) Armar SQL
                string sql = @"
            UPDATE Productos 
               SET NombreProducto = @nombre,
                   Descripcion   = @desc,
                   Precio        = @precio,
                   Stock         = @stock,
                   CategoriaID   = @cat
             WHERE ProductoID    = @id;";

                // 4) Parámetros
                var parametros = new SqlParameter[]
                {
            new SqlParameter("@nombre", txtNombreActualizado.Text),
            new SqlParameter("@desc",   textDescripcionActulizada.Text),
            new SqlParameter("@precio", precio),
            new SqlParameter("@stock",  stock),
            new SqlParameter("@cat",    categoriaId),
            new SqlParameter("@id",     productoId)
                };

                // 5) Ejecutar UPDATE usando tu helper
                

                if (filas > 0)
                {
                    MessageBox.Show("✅ Producto actualizado correctamente.", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                }
                else
                {
                    MessageBox.Show("⚠️ No se encontró el producto a actualizar.", "Aviso",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnActualizar_Click_2(object sender, EventArgs e)
        {
            try
            {
                // 1) Validar ID del producto
                if (string.IsNullOrWhiteSpace(txtProductoID.Text))
                {
                    MessageBox.Show("Debes seleccionar un producto para actualizar.", "Aviso",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (!int.TryParse(txtProductoID.Text, out int productoId))
                {
                    MessageBox.Show("El ID de producto no es válido.", "Aviso",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 2) Validar numéricos: Precio, Stock y CategoriaID
                if (!decimal.TryParse(TxtPrecioActulizado.Text, out decimal precio))
                {
                    MessageBox.Show("El precio no es válido.", "Aviso",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!int.TryParse(textStockActualizado.Text, out int stock))
                {
                    MessageBox.Show("El stock debe ser un número entero.", "Aviso",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!int.TryParse(txtCategoriaIDActualizado.Text, out int categoriaId))
                {
                    MessageBox.Show("El ID de categoría debe ser un número entero.", "Aviso",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 3) Armar SQL
                string sql = @"
            UPDATE Productos 
               SET NombreProducto = @nombre,
                   Descripcion   = @desc,
                   Precio        = @precio,
                   Stock         = @stock,
                   CategoriaID   = @cat
             WHERE ProductoID    = @id;";

                // 4) Parámetros
                var parametros = new SqlParameter[]
                {
            new SqlParameter("@nombre", txtNombreActualizado.Text),
            new SqlParameter("@desc",   textDescripcionActulizada.Text),
            new SqlParameter("@precio", precio),
            new SqlParameter("@stock",  stock),
            new SqlParameter("@cat",    categoriaId),
            new SqlParameter("@id",     productoId)
                };

                // 5) Ejecutar UPDATE usando tu helper


                if (filas > 0)
                {
                    MessageBox.Show("✅ Producto actualizado correctamente.", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                else
                {
                    MessageBox.Show("⚠️ No se encontró el producto a actualizar.", "Aviso",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




    


         private void dgClientes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Evitar cabeceras
            {
                DataGridViewRow fila = dgClientes.Rows[e.RowIndex];

                txtProductoID.Text = fila.Cells["ProductoID"].Value.ToString();
                txtNombreActualizado.Text = fila.Cells["NombreProducto"].Value.ToString();
                textDescripcionActulizada.Text = fila.Cells["Descripcion"].Value?.ToString();
                TxtPrecioActulizado.Text = fila.Cells["Precio"].Value.ToString();
                textStockActualizado.Text = fila.Cells["Stock"].Value.ToString();
                txtCategoriaIDActualizado.Text = fila.Cells["CategoriaID"].Value.ToString();
            }
            }




    }

}





