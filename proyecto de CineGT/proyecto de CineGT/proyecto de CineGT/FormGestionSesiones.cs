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
    public partial class FormGestionSesiones : Form
    {
        private SqlConnection conn;

        public FormGestionSesiones()
        {
            InitializeComponent();
            conn = GetConnection();
            LoadComboBoxes();
            LoadSesiones();
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings["CineDB"].ConnectionString);
        }

        private void LoadComboBoxes()
        {
            // Cargar películas en ComboBox
            SqlDataAdapter adapterPeliculas = new SqlDataAdapter("SELECT id_pelicula, nombre FROM Pelicula", conn);
            DataTable tablePeliculas = new DataTable();
            adapterPeliculas.Fill(tablePeliculas);
            cmbPeliculas.DataSource = tablePeliculas;
            cmbPeliculas.DisplayMember = "nombre";
            cmbPeliculas.ValueMember = "id_pelicula";

            // Cargar salas en ComboBox
            SqlDataAdapter adapterSalas = new SqlDataAdapter("SELECT id_sala, nombre_sala FROM Sala", conn);
            DataTable tableSalas = new DataTable();
            adapterSalas.Fill(tableSalas);
            cmbSalas.DataSource = tableSalas;
            cmbSalas.DisplayMember = "nombre_sala";
            cmbSalas.ValueMember = "id_sala";
        }

        private void LoadSesiones()
        {
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Sesion", conn);
            DataTable table = new DataTable();
            adapter.Fill(table);
            dataGridViewSesiones.DataSource = table;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Verificar que se seleccionó una película y una sala
            if (cmbPeliculas.SelectedValue != null && cmbSalas.SelectedValue != null)
            {
                // Consultar y usar el ID de la película directamente desde el ComboBox
                string query = "INSERT INTO Sesion (id_pelicula, id_sala, fecha_hora_inicio, fecha_hora_fin, estado) VALUES (@pelicula, @sala, @inicio, @fin, 'activa')";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@pelicula", cmbPeliculas.SelectedValue); // ID de la película
                    cmd.Parameters.AddWithValue("@sala", cmbSalas.SelectedValue); // ID de la sala
                    cmd.Parameters.AddWithValue("@inicio", dateTimePickerInicio.Value); // Fecha y hora de inicio
                    cmd.Parameters.AddWithValue("@fin", dateTimePickerInicio.Value.AddHours(2)); // Duración de 2 horas

                    conn.Open();
                    cmd.ExecuteNonQuery(); // Ejecutar la inserción
                    conn.Close();
                }
                LoadSesiones(); // Actualizar la lista de sesiones
            }
            else
            {
                MessageBox.Show("Por favor, selecciona una película y una sala."); // Mensaje de error
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Verificar que se ha seleccionado una sesión en el DataGridView
            if (dataGridViewSesiones.SelectedRows.Count > 0)
            {
                int idSesion = Convert.ToInt32(dataGridViewSesiones.SelectedRows[0].Cells["id_sesion"].Value);
                string query = "UPDATE Sesion SET estado = 'inactiva' WHERE id_sesion = @id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", idSesion); // ID de la sesión seleccionada

                    conn.Open();
                    cmd.ExecuteNonQuery(); // Ejecutar la actualización
                    conn.Close();
                }
                LoadSesiones(); // Actualizar la lista de sesiones
            }
            else
            {
                MessageBox.Show("Por favor, selecciona una sesión para desactivarla."); // Mensaje de error
            }
        }
    }
}
