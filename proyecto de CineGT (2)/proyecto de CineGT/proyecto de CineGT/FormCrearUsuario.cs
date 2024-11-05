using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace proyecto_de_CineGT
{
    public partial class FormCrearUsuario : Form
    {
        private SqlConnection conn;

        public FormCrearUsuario()
        {
            InitializeComponent();
            conn = GetConnection();
            LoadRoles();
        }
        private SqlConnection GetConnection()
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings["CineDB"].ConnectionString);
        }

        private void LoadRoles()
        {
            // Añade los roles disponibles al ComboBox
            cmbRol.Items.Add("admin");
            cmbRol.Items.Add("vendedor");
            cmbRol.SelectedIndex = 0; // Selecciona el primer rol por defecto
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string nombreUsuario = txtNombreUsuario.Text;
            string contraseña = txtContraseña.Text;
            string rol = cmbRol.SelectedItem.ToString();

            if (string.IsNullOrEmpty(nombreUsuario) || string.IsNullOrEmpty(contraseña))
            {
                MessageBox.Show("Debe completar todos los campos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = "INSERT INTO Usuario (nombre_usuario, contraseña, rol) OUTPUT INSERTED.id_usuario VALUES (@nombre, @contraseña, @rol)";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@nombre", nombreUsuario);
                cmd.Parameters.AddWithValue("@contraseña", contraseña);
                cmd.Parameters.AddWithValue("@rol", rol);

                try
                {
                    conn.Open();
                    int nuevoIdUsuario = (int)cmd.ExecuteScalar(); // Captura el ID del usuario recién creado

                    // Mostrar solo el usuario recién creado
                    MostrarUsuarioRecienCreado(nuevoIdUsuario);
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Error al crear usuario: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    conn.Close();
                }
            }
        }


        private void MostrarUsuarioRecienCreado(int idUsuario)
        {
            // Consulta para obtener solo el usuario recién creado
            string query = "SELECT id_usuario, nombre_usuario, rol FROM Usuario WHERE id_usuario = @id";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@id", idUsuario);

                try
                {
                    if (conn.State == ConnectionState.Closed) // Verifica si la conexión está cerrada
                    {
                        conn.Open();
                    }
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    dataGridViewUsuario.DataSource = table; // Muestra solo el usuario recién creado en el DataGridView
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Error al mostrar usuario: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    conn.Close();
                }
            }
        }
    }
}
