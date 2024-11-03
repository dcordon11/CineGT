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
    public partial class FormVentaAsientos : Form
    {
        private SqlConnection conn;

        public FormVentaAsientos()
        {
            InitializeComponent();
           
            InitializeConnection();
            InitializeDataGridView();
            conn = GetConnection(); // Método para obtener la conexión a la base de datos
            LoadComboBoxSesiones(); // Cargar sesiones en el ComboBox

            // Verificar la conexión
            try
            {
                conn.Open();
                MessageBox.Show("Conexión exitosa.");
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al conectar: {ex.Message}");
            }

            LoadSesiones();
            LoadAsientos();
        }
        private void InitializeDataGridView()
        {
            // Definir las columnas
            dataGridViewAsientos.Columns.Clear();
            dataGridViewAsientos.Columns.Add("Asiento", "Asiento Seleccionado");
        }



        private void LoadComboBoxSesiones()
        {
            // Cargar sesiones en ComboBox
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT id_sesion, CONCAT('Película: ', (SELECT nombre FROM Pelicula WHERE id_pelicula = Sesion.id_pelicula), ' - Sala: ', (SELECT nombre_sala FROM Sala WHERE id_sala = Sesion.id_sala), ' - Fecha: ', fecha_hora_inicio) AS descripcion FROM Sesion WHERE estado = 'activa'", conn);
            DataTable table = new DataTable();
            adapter.Fill(table);
            cmbSesiones.DataSource = table;
            cmbSesiones.DisplayMember = "descripcion";
            cmbSesiones.ValueMember = "id_sesion";
        }


        private void LoadSesiones()
        {
            string query = @"
        SELECT 
            s.id_sesion, 
            sa.id_sala, 
            p.nombre AS pelicula_nombre, 
            sa.nombre_sala AS sala_nombre, 
            s.fecha_hora_inicio 
        FROM Sesion s 
        JOIN Sala sa ON s.id_sala = sa.id_sala 
        JOIN Pelicula p ON s.id_pelicula = p.id_pelicula";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable table = new DataTable();
                try
                {
                    conn.Open(); // Abre la conexión aquí
                    adapter.Fill(table);
                    cmbSesiones.DataSource = table;

                    // Establecer la visualización del ComboBox
                    cmbSesiones.DisplayMember = "pelicula_nombre"; // Mostrar nombre de la película
                    cmbSesiones.ValueMember = "id_sesion"; // Usar ID de la sesión

                    // Opcional: Modificar la forma en que se visualizan los elementos
                    foreach (DataRow row in table.Rows)
                    {
                        row["pelicula_nombre"] = $"{row["pelicula_nombre"]} - Sala: {row["sala_nombre"]} - Fecha y Hora: {((DateTime)row["fecha_hora_inicio"]).ToString("g")}";
                    }
                    cmbSesiones.DisplayMember = "pelicula_nombre"; // Volver a establecer después de modificar
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al cargar las sesiones: {ex.Message}");
                }
                finally
                {
                    conn.Close(); // Asegúrate de cerrar la conexión
                }
            }
        }


        private void LoadAsientos()
        {
            if (conn == null)
            {
                MessageBox.Show("La conexión no ha sido inicializada.");
                return;
            }

            string query = "SELECT id_sala, fila, id_asiento, numero FROM Asiento";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable table = new DataTable();
                try
                {
                    conn.Open();
                    adapter.Fill(table);
                    dataGridViewAsientos.DataSource = table;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al cargar los asientos: {ex.Message}");
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings["CineDB"].ConnectionString);
        }

        private void InitializeConnection()
        {
            conn = GetConnection(); // Inicializa la conexión
            if (conn == null)
            {
                MessageBox.Show("Error al establecer la conexión con la base de datos.");
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            var selectedSession = (DataRowView)cmbSesiones.SelectedItem;
            int idSesion = (int)selectedSession["id_sesion"]; // ID de la sesión
            int idSala = (int)selectedSession["id_sala"]; // ID de la sala
            int cantidadAsientos = (int)numericUpDownCantidadAsientos.Value;

            // Verifica que el usuario haya seleccionado la opción de asignación manual
            if (radioButtonAutomatico.Checked)
            {
                AsignarAsientosAutomatica(idSesion, idSala, cantidadAsientos);
            }
            else
            {
                MessageBox.Show("Por favor, elija la opción de asignación automatica.");
            }

            // Actualiza el DataGridView después de confirmar la compra
            LoadAsientos(); // Re-carga los asientos para mostrar los cambios
        }


        private void AsignarAsientosAutomatica(int idSesion, int idSala, int cantidadAsientos)
        {
            const int asientosPorFila = 10; // Cambia esto según tu configuración
            int filaActual = 0; // Inicializa la fila
            int asientosAsignados = 0; // Contador de asientos asignados

            while (asientosAsignados < cantidadAsientos)
            {
                // Calcular el número del asiento en esta fila
                for (int numeroAsiento = 1; numeroAsiento <= asientosPorFila; numeroAsiento++)
                {
                    // Calcular la letra de la fila (ejemplo: 'A', 'B', 'C', ...)
                    string fila = ((char)('A' + filaActual)).ToString();

                    // Verificar si el asiento ya existe
                    string checkQuery = "SELECT COUNT(*) FROM Asiento WHERE id_sala = @idSala AND fila = @fila AND numero = @numero";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@idSala", idSala);
                        checkCmd.Parameters.AddWithValue("@fila", fila);
                        checkCmd.Parameters.AddWithValue("@numero", numeroAsiento);

                        conn.Open();
                        int count = (int)checkCmd.ExecuteScalar();
                        conn.Close();

                        if (count == 0) // Si el asiento está disponible
                        {
                            // Inserción en la base de datos
                            string query = "INSERT INTO Asiento (id_sala, fila, numero) VALUES (@idSala, @fila, @numero)";
                            using (SqlCommand cmd = new SqlCommand(query, conn))
                            {
                                cmd.Parameters.AddWithValue("@idSala", idSala);
                                cmd.Parameters.AddWithValue("@fila", fila);
                                cmd.Parameters.AddWithValue("@numero", numeroAsiento); // Número de asiento

                                conn.Open();
                                cmd.ExecuteNonQuery();
                                conn.Close();
                            }

                            asientosAsignados++; // Incrementar el contador de asientos asignados

                            // Si hemos asignado todos los asientos necesarios, salir del bucle
                            if (asientosAsignados >= cantidadAsientos)
                            {
                                break;
                            }
                        }
                    }
                }

                filaActual++; // Pasar a la siguiente fila si no se asignaron suficientes asientos
            }

            // Después de insertar todos los asientos, recarga el DataGridView
            LoadAsientos();
        }
    }
}
