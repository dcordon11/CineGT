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
    public partial class FormGestionPeliculas : Form
    {
        private SqlConnection conn;

        public FormGestionPeliculas()
        {
            InitializeComponent();
            conn = GetConnection();
            LoadPeliculas();
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings["CineDB"].ConnectionString);
        }


        private void LoadPeliculas()
        {
            try
            {
                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Pelicula", conn);
                DataTable table = new DataTable();
                adapter.Fill(table);
                dataGridViewPeliculas.DataSource = table;
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                conn.Close();  // Asegúrate de cerrar la conexión en el bloque finally
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Validación de longitud
            if (txtClasificacion.Text.Length > 50) // Reemplaza 50 con la longitud máxima de la columna
            {
                MessageBox.Show("La clasificación es demasiado larga. Máximo 50 caracteres.");
                return;
            }

            string query = "INSERT INTO Pelicula (nombre, clasificacion, duracion, descripcion) VALUES (@nombre, @clasificacion, @duracion, @descripcion)";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@nombre", txtNombre.Text);
                cmd.Parameters.AddWithValue("@clasificacion", txtClasificacion.Text);
                cmd.Parameters.AddWithValue("@duracion", int.Parse(txtDuracion.Text));
                cmd.Parameters.AddWithValue("@descripcion", txtDescripcion.Text);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Error al agregar película: " + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }

            LoadPeliculas();
        }
        


        private void btneditar_Click(object sender, EventArgs e)
        {
            string query = "UPDATE Pelicula SET nombre = @nombre, clasificacion = @clasificacion, duracion = @duracion, descripcion = @descripcion WHERE id_pelicula = @id";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@nombre", txtNombre.Text);
            cmd.Parameters.AddWithValue("@clasificacion", txtClasificacion.Text);
            cmd.Parameters.AddWithValue("@duracion", int.Parse(txtDuracion.Text));
            cmd.Parameters.AddWithValue("@descripcion", txtDescripcion.Text);
            cmd.Parameters.AddWithValue("@id", int.Parse(txtID.Text));

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            LoadPeliculas();
        }

        private void btneliminar_Click(object sender, EventArgs e)
        {
            string query = "DELETE FROM Pelicula WHERE id_pelicula = @id";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", int.Parse(txtID.Text));

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            LoadPeliculas();
        }
    }
}
