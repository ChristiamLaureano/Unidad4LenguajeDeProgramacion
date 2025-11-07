using EjercicioBaseDeDatosWinForm;
using System;
using System.Windows.Forms;



namespace actividad3
{
    public partial class Form1 : Form
    {



        public Form1()
        {
            InitializeComponent();
            this.IsMdiContainer = true;   // ← imp
        }

        private void clientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clientes frm = new Clientes();
            frm.MdiParent = this;
            frm.Show();
        }

        // 🔹 Nombre corregido: debe coincidir con el que está en el diseñador
        private void categoriaToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Categorias frmCategorias = new Categorias();
            frmCategorias.MdiParent = this;
            frmCategorias.Show();
        }

        private void productosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Productos frmProducto = new Productos();
            frmProducto.MdiParent = this;
            frmProducto.Show();

        }

        private void proveedoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            proveedores frm = new proveedores();
            frm.MdiParent = this;
            frm.Show();


        }
    }
}
