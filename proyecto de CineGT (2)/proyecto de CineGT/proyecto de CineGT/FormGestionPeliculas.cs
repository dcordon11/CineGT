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
            if (dataGridViewPeliculas.SelectedRows.Count > 0)
            {
                int idPelicula = Convert.ToInt32(dataGridViewPeliculas.SelectedRows[0].Cells["id_pelicula"].Value);

                // Si el campo está vacío, se obtiene el valor actual desde el DataGridView
                string nombre = string.IsNullOrWhiteSpace(txtNombre.Text) ?
                    dataGridViewPeliculas.SelectedRows[0].Cells["nombre"].Value.ToString() : txtNombre.Text;

                string clasificacion = string.IsNullOrWhiteSpace(txtClasificacion.Text) ?
                    dataGridViewPeliculas.SelectedRows[0].Cells["clasificacion"].Value.ToString() : txtClasificacion.Text;

                int duracion = string.IsNullOrWhiteSpace(txtDuracion.Text) ?
                    Convert.ToInt32(dataGridViewPeliculas.SelectedRows[0].Cells["duracion"].Value) : int.Parse(txtDuracion.Text);

                string descripcion = string.IsNullOrWhiteSpace(txtDescripcion.Text) ?
                    dataGridViewPeliculas.SelectedRows[0].Cells["descripcion"].Value.ToString() : txtDescripcion.Text;

                string query = "UPDATE Pelicula SET nombre = @nombre, clasificacion = @clasificacion, duracion = @duracion, descripcion = @descripcion WHERE id_pelicula = @id";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@nombre", nombre);
                    cmd.Parameters.AddWithValue("@clasificacion", clasificacion);
                    cmd.Parameters.AddWithValue("@duracion", duracion);
                    cmd.Parameters.AddWithValue("@descripcion", descripcion);
                    cmd.Parameters.AddWithValue("@id", idPelicula);

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Película actualizada correctamente.");
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Error al editar película: " + ex.Message);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }

                LoadPeliculas(); // Recarga el DataGridView para reflejar los cambios
            }
            else
            {
                MessageBox.Show("Seleccione una fila para editar.");
            }
        }

        private void btneliminar_Click(object sender, EventArgs e)
        {
            if (dataGridViewPeliculas.SelectedRows.Count > 0)
            {
                int idPelicula = Convert.ToInt32(dataGridViewPeliculas.SelectedRows[0].Cells["id_pelicula"].Value);
                string query = "DELETE FROM Pelicula WHERE id_pelicula = @id";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", idPelicula);

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Película eliminada correctamente.");
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Error al eliminar película: " + ex.Message);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }

                LoadPeliculas();
            }
            else
            {
                MessageBox.Show("Seleccione una fila para eliminar.");
            }
        }
        private void dataGridViewPeliculas_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewPeliculas.SelectedRows.Count > 0)
            {
                // Muestra los datos seleccionados en los campos de texto para editarlos
                txtNombre.Text = dataGridViewPeliculas.SelectedRows[0].Cells["nombre"].Value.ToString();
                txtClasificacion.Text = dataGridViewPeliculas.SelectedRows[0].Cells["clasificacion"].Value.ToString();
                txtDuracion.Text = dataGridViewPeliculas.SelectedRows[0].Cells["duracion"].Value.ToString();
                txtDescripcion.Text = dataGridViewPeliculas.SelectedRows[0].Cells["descripcion"].Value.ToString();
            }
        }
    }
}
