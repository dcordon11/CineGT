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
            // Modifica la consulta para ordenar las sesiones por id_sesion en orden descendente
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Sesion ORDER BY id_sesion DESC", conn);
            DataTable table = new DataTable();
            adapter.Fill(table);
            dataGridViewSesiones.DataSource = table;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (cmbPeliculas.SelectedValue != null && cmbSalas.SelectedValue != null)
            {
                int idPelicula = (int)cmbPeliculas.SelectedValue;
                int idSala = (int)cmbSalas.SelectedValue;

                // Obtener la fecha y hora de inicio combinada
                DateTime fechaInicio = dateTimePickerInicio.Value.Date + timePickerInicio.Value.TimeOfDay;

                // Consultar la duración de la película seleccionada
                int duracionMinutos;
                using (SqlCommand cmd = new SqlCommand("SELECT duracion FROM Pelicula WHERE id_pelicula = @idPelicula", conn))
                {
                    cmd.Parameters.AddWithValue("@idPelicula", idPelicula);
                    conn.Open();
                    duracionMinutos = (int)cmd.ExecuteScalar();
                    conn.Close();
                }

                // Convertir duración a horas y minutos y calcular la hora de fin
                DateTime fechaFin = fechaInicio.AddMinutes(duracionMinutos);

                // Verificar si existe una sesión en la misma sala y que no se solape
                // Verificar si existe una sesión en la misma sala y que no se solape respetando los 15 minutos entre sesiones
                string queryCheck = @"
    SELECT COUNT(*)
    FROM Sesion
    WHERE id_sala = @idSala 
      AND estado = 'activa'
      AND NOT (@fechaFin <= DATEADD(MINUTE, -15, fecha_hora_inicio) 
               OR @fechaInicio >= DATEADD(MINUTE, 15, fecha_hora_fin))";

                using (SqlCommand cmdCheck = new SqlCommand(queryCheck, conn))
                {
                    cmdCheck.Parameters.AddWithValue("@idSala", idSala);
                    cmdCheck.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                    cmdCheck.Parameters.AddWithValue("@fechaFin", fechaFin);
                    conn.Open();
                    int count = (int)cmdCheck.ExecuteScalar();
                    conn.Close();

                    if (count > 0)
                    {
                        MessageBox.Show("No se puede crear la sesión debido a un conflicto de horarios en la misma sala.");
                        return;
                    }
                }

                // Insertar la nueva sesión en la base de datos
                string queryInsert = "INSERT INTO Sesion (id_pelicula, id_sala, fecha_hora_inicio, fecha_hora_fin, estado) VALUES (@pelicula, @sala, @inicio, @fin, 'activa')";
                using (SqlCommand cmdInsert = new SqlCommand(queryInsert, conn))
                {
                    cmdInsert.Parameters.AddWithValue("@pelicula", idPelicula);
                    cmdInsert.Parameters.AddWithValue("@sala", idSala);
                    cmdInsert.Parameters.AddWithValue("@inicio", fechaInicio);
                    cmdInsert.Parameters.AddWithValue("@fin", fechaFin);

                    conn.Open();
                    cmdInsert.ExecuteNonQuery();
                    conn.Close();
                }

                LoadSesiones(); // Actualizar la lista de sesiones
            }
            else
            {
                MessageBox.Show("Por favor, selecciona una película y una sala.");
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

        private void btnReactivarSesion_Click(object sender, EventArgs e)
        {
            // Verificar que se ha seleccionado una sesión en el DataGridView
            if (dataGridViewSesiones.SelectedRows.Count > 0)
            {
                int idSesion = Convert.ToInt32(dataGridViewSesiones.SelectedRows[0].Cells["id_sesion"].Value);
                string query = "UPDATE Sesion SET estado = 'activa' WHERE id_sesion = @id";
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
                MessageBox.Show("Por favor, selecciona una sesión para reactivarla."); // Mensaje de error
            }
        }
    }
}
