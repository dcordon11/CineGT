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
    public partial class FormAnularTransacciones : Form
    {
        private SqlConnection conn;

        public FormAnularTransacciones()
        {
            InitializeComponent();
            conn = GetConnection();
            CargarTransacciones();

        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings["CineDB"].ConnectionString);
        }

        private void CargarTransacciones()
        {
            using (SqlConnection con = GetConnection())
            {
                string query = "SELECT id_transaccion, id_usuario, id_sesion, fecha_hora, total_asientos, tipo_asignacion FROM Transaccion";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvTransacciones.DataSource = dt;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dgvTransacciones.SelectedRows.Count > 0)
            {
                int idTransaccion = Convert.ToInt32(dgvTransacciones.SelectedRows[0].Cells["id_transaccion"].Value);

                // Confirmación de anulación
                DialogResult result = MessageBox.Show("¿Está seguro de que desea anular esta transacción?", "Confirmar Anulación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    using (SqlConnection con = GetConnection())
                    {
                        con.Open();
                        SqlTransaction sqlTransaction = con.BeginTransaction();
                        try
                        {
                            // Eliminar los asientos asociados a la transacción
                            string deleteAsientoTransaccion = "DELETE FROM AsientoTransaccion WHERE id_transaccion = @idTransaccion";
                            using (SqlCommand cmdAsiento = new SqlCommand(deleteAsientoTransaccion, con, sqlTransaction))
                            {
                                cmdAsiento.Parameters.AddWithValue("@idTransaccion", idTransaccion);
                                cmdAsiento.ExecuteNonQuery();
                            }

                            // Eliminar la transacción
                            string deleteTransaccion = "DELETE FROM Transaccion WHERE id_transaccion = @idTransaccion";
                            using (SqlCommand cmdTransaccion = new SqlCommand(deleteTransaccion, con, sqlTransaction))
                            {
                                cmdTransaccion.Parameters.AddWithValue("@idTransaccion", idTransaccion);
                                cmdTransaccion.ExecuteNonQuery();
                            }

                            // Confirmar la transacción en la base de datos
                            sqlTransaction.Commit();

                            MessageBox.Show("Transacción anulada exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            CargarTransacciones(); // Recargar las transacciones en el DataGridView
                        }
                        catch (Exception ex)
                        {
                            sqlTransaction.Rollback();
                            MessageBox.Show("Error al anular la transacción: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, seleccione una transacción para anular.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
