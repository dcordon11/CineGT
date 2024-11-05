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
    public partial class FormReportes : Form
    {
        private SqlConnection conn;

        public FormReportes()
        {
            InitializeComponent();
            conn = GetConnection();

        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings["CineDB"].ConnectionString);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = GetConnection())
                {
                    con.Open(); // Asegura que la conexión esté abierta

                    DateTime fechaInicio = dtpFechaInicio.Value.Date;
                    DateTime fechaFin = dtpFechaFin.Value.Date.AddDays(1).AddSeconds(-1);

                    string query = @"
                SELECT Sesion.id_sesion, Pelicula.nombre AS Pelicula, Sala.nombre_sala AS Sala,
                       Sesion.fecha_hora_inicio, Sesion.fecha_hora_fin, Sesion.estado
                FROM Sesion
                INNER JOIN Pelicula ON Sesion.id_pelicula = Pelicula.id_pelicula
                INNER JOIN Sala ON Sesion.id_sala = Sala.id_sala
                WHERE Sesion.fecha_hora_inicio BETWEEN @fechaInicio AND @fechaFin";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                        cmd.Parameters.AddWithValue("@fechaFin", fechaFin);

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        // Verificación directa en el DataTable
                        foreach (DataRow row in dt.Rows)
                        {
                            Console.WriteLine($"ID Sesion: {row["id_sesion"]}, Estado en consulta: {row["estado"]}");
                        }

                        dgvReportes.DataSource = null;  // Limpia cualquier referencia previa
                        dgvReportes.DataSource = dt;    // Asigna el DataTable

                        if (dt.Rows.Count == 0)
                        {
                            MessageBox.Show("No se encontraron sesiones para el rango de fechas seleccionado.", "Sin Resultados", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnListadoTransacciones_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = GetConnection())
                {
                    string query = @"
                SELECT id_transaccion, Pelicula, Sala, Usuario, fecha_hora, total
                FROM Transaccion
                WHERE fecha_hora BETWEEN @fechaInicio AND @fechaFin";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@fechaInicio", dtpFechaInicio.Value);
                    cmd.Parameters.AddWithValue("@fechaFin", dtpFechaFin.Value);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvReportes.DataSource = dt;

                    // Mostrar mensaje si no hay resultados
                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show("No se encontraron resultados para el rango de fechas seleccionado.", "Sin Resultados", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
