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
        private string nombreUsuarioActivo;

        public FormVentaAsientos(string nombreUsuario)
        {
            this.nombreUsuarioActivo = nombreUsuario;
            InitializeComponent();
            InitializeConnection();
            InitializeDataGridView();
            conn = GetConnection(); // Método para obtener la conexión a la base de datos
            LoadComboBoxSesiones(); // Cargar sesiones en el ComboBox
            LoadSesiones();
            LoadAsientos();
            LoadAsientoTransaccion();
        }
        private void InitializeDataGridView()
        {
            // Definir las columnas
            dataGridViewAsientos.Columns.Clear();
            dataGridViewAsientos.Columns.Add("Asiento", "Asiento Seleccionado");

            // Configurar el DataGridView para AsientoTransaccion (dataGridViewTransacciones)
            dataGridViewTransacciones.Columns.Clear();
            dataGridViewTransacciones.Columns.Add("Transaccion", "Transacción Seleccionada");
        }



        private void LoadComboBoxSesiones()
        {
            string query = "SELECT s.id_sesion, s.id_sala, CONCAT('Película: ', p.nombre, ' - Sala: ', sa.nombre_sala, ' - Fecha: ', FORMAT(s.fecha_hora_inicio, 'g')) AS descripcion " +
               "FROM Sesion s " +
               "JOIN Pelicula p ON s.id_pelicula = p.id_pelicula " +
               "JOIN Sala sa ON s.id_sala = sa.id_sala " +
               "WHERE s.estado = 'activa'";

            using (SqlDataAdapter adapter = new SqlDataAdapter(query, conn))
            {
                DataTable table = new DataTable();
                try
                {
                    conn.Open();
                    adapter.Fill(table);
                    cmbSesiones.DataSource = table;
                    cmbSesiones.DisplayMember = "descripcion";
                    cmbSesiones.ValueMember = "id_sesion";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al cargar las sesiones: {ex.Message}");
                }
                finally
                {
                    conn.Close();
                }
            }
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
            string query = @"
    SELECT 
        a.id_asiento, 
        a.id_sala, 
        a.fila, 
        a.numero
    FROM Asiento a
    ORDER BY a.id_asiento DESC";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable table = new DataTable();
                try
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();
                    adapter.Fill(table);
                    dataGridViewAsientos.DataSource = table;

                    // Ajustar encabezados si es necesario
                    dataGridViewAsientos.Columns["id_asiento"].HeaderText = "ID Asiento";
                    dataGridViewAsientos.Columns["id_sala"].HeaderText = "ID Sala";
                    dataGridViewAsientos.Columns["fila"].HeaderText = "Fila";
                    dataGridViewAsientos.Columns["numero"].HeaderText = "Número";
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

        private void LoadAsientoTransaccion()
        {
            string query = @"
    SELECT 
        at.id_transaccion,
        at.id_usuario,
        at.id_sesion,
        at.id_asiento,
        at.fecha_hora,
        at.tipo_asignacion
    FROM AsientoTransaccion at
    ORDER BY at.id_transaccion DESC";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable table = new DataTable();
                try
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();
                    adapter.Fill(table);
                    dataGridViewTransacciones.DataSource = table;

                    // Ajustar encabezados si es necesario
                    dataGridViewTransacciones.Columns["id_transaccion"].HeaderText = "ID Transacción";
                    dataGridViewTransacciones.Columns["id_usuario"].HeaderText = "ID Usuario";
                    dataGridViewTransacciones.Columns["id_sesion"].HeaderText = "ID Sesión";
                    dataGridViewTransacciones.Columns["id_asiento"].HeaderText = "ID Asiento";
                    dataGridViewTransacciones.Columns["fecha_hora"].HeaderText = "Fecha y Hora";
                    dataGridViewTransacciones.Columns["tipo_asignacion"].HeaderText = "Tipo de Asignación";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al cargar los datos de transacción: {ex.Message}");
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
            int idSesion = (int)selectedSession["id_sesion"];
            int idSala = (int)selectedSession["id_sala"];
            int cantidadAsientos = (int)numericUpDownCantidadAsientos.Value;

            if (radioButtonAutomatico.Checked)
            {
                AsignarAsientosAutomatica(idSesion, idSala, cantidadAsientos);
            }
            else if (radioButtonManual.Checked)
            {
                AsignarAsientosManual(idSesion, idSala, cantidadAsientos);
            }
            else
            {
                MessageBox.Show("Seleccione un método de asignación.");
            }

            // Recargar ambas tablas para reflejar la compra
            LoadAsientos();
            LoadAsientoTransaccion();
        }
        
        
        
        // Crear formulario para ingresar fila y número manualmente
        private Form CrearFormularioAsientoManual(out TextBox txtFila, out NumericUpDown numNumero)
        {
            Form inputForm = new Form
            {
                Text = "Seleccionar Asiento Manualmente",
                Size = new Size(300, 250),
                StartPosition = FormStartPosition.CenterParent
            };

            Label lblFila = new Label() { Left = 20, Top = 40, Text = "Fila:" };
            txtFila = new TextBox() { Left = 150, Top = 30, Width = 100 };

            Label lblNumero = new Label() { Left = 20, Top = 80, Text = "Número:" };
            numNumero = new NumericUpDown() { Left = 150, Top = 80, Width = 100 };
            numNumero.Minimum = 1;
            numNumero.Maximum = 10; // Ajusta según el máximo de asientos por fila

            Button btnConfirmar = new Button() { Text = "Confirmar", Left = 100, Width = 100, Top = 140, DialogResult = DialogResult.OK };

            inputForm.Controls.Add(lblFila);
            inputForm.Controls.Add(txtFila);
            inputForm.Controls.Add(lblNumero);
            inputForm.Controls.Add(numNumero);
            inputForm.Controls.Add(btnConfirmar);

            return inputForm;
        }


        private int AgregarAsiento(int idSala, string fila, int numero)
        {
            // Verificar si el asiento ya existe en la tabla
            int idAsientoExistente = ObtenerIdAsiento(idSala, fila, numero);
            if (idAsientoExistente > 0)
            {
                // Si el asiento ya existe, retorna el ID del asiento sin modificar nada
                return idAsientoExistente;
            }

            // Si el asiento no existe, obtener el siguiente ID y agregar el nuevo asiento
            int idAsiento = ObtenerSiguienteIdAsiento();
            string query = @"
        INSERT INTO Asiento (id_asiento, id_sala, fila, numero) 
        VALUES (@idAsiento, @idSala, @fila, @numero)";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@idAsiento", idAsiento);
                cmd.Parameters.AddWithValue("@idSala", idSala);
                cmd.Parameters.AddWithValue("@fila", fila);
                cmd.Parameters.AddWithValue("@numero", numero);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al agregar el asiento: {ex.Message}");
                    conn.Close();
                    return 0;
                }
            }

            return idAsiento;
        }


        private void AgregarTransaccionAsiento(int idUsuario, int idSesion, int idAsiento, string tipoAsignacion)
        {
            string queryTransaccion = @"
        INSERT INTO AsientoTransaccion (id_usuario, id_sesion, id_asiento, fecha_hora, total_asientos, tipo_asignacion)
        VALUES (@idUsuario, @idSesion, @idAsiento, GETDATE(), 1, @tipoAsignacion)";

            using (SqlCommand cmdTransaccion = new SqlCommand(queryTransaccion, conn))
            {
                cmdTransaccion.Parameters.AddWithValue("@idUsuario", idUsuario);
                cmdTransaccion.Parameters.AddWithValue("@idSesion", idSesion);
                cmdTransaccion.Parameters.AddWithValue("@idAsiento", idAsiento);
                cmdTransaccion.Parameters.AddWithValue("@tipoAsignacion", tipoAsignacion);

                try
                {
                    conn.Open();
                    cmdTransaccion.ExecuteNonQuery();
                    conn.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al registrar la transacción de asiento: {ex.Message}");
                    conn.Close();
                }
            }
        }



        private int ObtenerIdUsuario()
        {
            string query = "SELECT id_usuario FROM Usuario WHERE nombre_usuario = @nombreUsuario";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@nombreUsuario", nombreUsuarioActivo);
                try
                {
                    conn.Open();
                    var result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al obtener ID del usuario: " + ex.Message);
                    return 0;
                }
                finally
                {
                    conn.Close();
                }
            }
        }


        private bool EsAsientoDisponible(int idSala, string fila, int numero, int idSesion)
        {
            string query = @"
        SELECT COUNT(*) 
        FROM Asiento a
        LEFT JOIN AsientoTransaccion at ON a.id_asiento = at.id_asiento AND at.id_sesion = @idSesion
        WHERE a.id_sala = @idSala 
        AND a.fila = @fila 
        AND a.numero = @numero 
        AND at.id_asiento IS NULL";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@idSala", idSala);
                cmd.Parameters.AddWithValue("@fila", fila);
                cmd.Parameters.AddWithValue("@numero", numero);
                cmd.Parameters.AddWithValue("@idSesion", idSesion);

                conn.Open();
                int count = (int)cmd.ExecuteScalar();
                conn.Close();

                return count == 1; // Si count es 1, el asiento está disponible
            }
        }


        private DataTable ObtenerAsientosDisponibles(int idSala, int idSesion)
        {
            string query = @"
        SELECT a.id_asiento, a.fila, a.numero
        FROM Asiento a
        LEFT JOIN AsientoTransaccion at ON a.id_asiento = at.id_asiento AND at.id_sesion = @idSesion
        WHERE a.id_sala = @idSala AND at.id_asiento IS NULL";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@idSala", idSala);
                cmd.Parameters.AddWithValue("@idSesion", idSesion);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable availableSeats = new DataTable();
                try
                {
                    conn.Open();
                    adapter.Fill(availableSeats);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al obtener los asientos disponibles: {ex.Message}");
                }
                finally
                {
                    conn.Close();
                }
                return availableSeats;
            }
        }


        private Form CrearFormularioAsientoManualConAsientosDisponibles(DataTable asientosDisponibles)
        {
            Form inputForm = new Form
            {
                Text = "Seleccionar Asiento Manualmente",
                Size = new Size(300, 250),
                StartPosition = FormStartPosition.CenterParent
            };

            Label lblFila = new Label() { Left = 20, Top = 40, Text = "Fila:" };
            ComboBox cmbFila = new ComboBox() { Left = 150, Top = 30, Width = 100, Name = "cmbFila" };

            Label lblNumero = new Label() { Left = 20, Top = 80, Text = "Número:" };
            ComboBox cmbNumero = new ComboBox() { Left = 150, Top = 80, Width = 100, Name = "cmbNumero" };

            // Llenar el ComboBox con las filas únicas de asientos disponibles
            foreach (DataRow row in asientosDisponibles.Rows)
            {
                string fila = row["fila"].ToString();
                if (!cmbFila.Items.Contains(fila))
                    cmbFila.Items.Add(fila);
            }

            // Actualizar los números de asiento disponibles cuando se selecciona una fila
            cmbFila.SelectedIndexChanged += (s, e) =>
            {
                cmbNumero.Items.Clear();
                string selectedFila = cmbFila.SelectedItem.ToString();
                foreach (DataRow row in asientosDisponibles.Select($"fila = '{selectedFila}'"))
                {
                    int numero = (int)row["numero"];
                    cmbNumero.Items.Add(numero);
                }
                if (cmbNumero.Items.Count > 0)
                    cmbNumero.SelectedIndex = 0;
            };

            if (cmbFila.Items.Count > 0)
                cmbFila.SelectedIndex = 0;

            Button btnConfirmar = new Button() { Text = "Confirmar", Left = 100, Width = 100, Top = 140, DialogResult = DialogResult.OK };

            inputForm.Controls.Add(lblFila);
            inputForm.Controls.Add(cmbFila);
            inputForm.Controls.Add(lblNumero);
            inputForm.Controls.Add(cmbNumero);
            inputForm.Controls.Add(btnConfirmar);

            return inputForm;
        }



        private void AsignarAsientosManual(int idSesion, int idSala, int cantidadAsientos)
        {
            DataTable asientosDisponibles = ObtenerAsientosDisponibles(idSala, idSesion);
            int idUsuario = ObtenerIdUsuario();

            if (asientosDisponibles.Rows.Count == 0)
            {
                MessageBox.Show("No hay asientos disponibles para esta sala y sesión.");
                return;
            }

            for (int i = 0; i < cantidadAsientos; i++)
            {
                asientosDisponibles = ObtenerAsientosDisponibles(idSala, idSesion); // Actualiza la lista cada vez
                using (Form inputForm = CrearFormularioAsientoManualConAsientosDisponibles(asientosDisponibles))
                {
                    if (inputForm.ShowDialog() == DialogResult.OK)
                    {
                        string fila = inputForm.Controls["cmbFila"].Text;
                        int numero = int.Parse(((ComboBox)inputForm.Controls["cmbNumero"]).SelectedItem.ToString());

                        // Agregar asiento a la tabla Asiento y obtener su ID
                        int idAsiento = AgregarAsiento(idSala, fila, numero); // Usa la función actualizada

                        if (idAsiento > 0 && EsAsientoDisponible(idSala, fila, numero, idSesion))
                        {
                            // Registrar la transacción en AsientoTransaccion
                            AgregarTransaccionAsiento(idUsuario, idSesion, idAsiento, "manual");
                        }
                        else
                        {
                            MessageBox.Show($"El asiento Fila: {fila}, Número: {numero} ya no está disponible.");
                        }
                    }
                }
            }
        }

        private void AsignarAsientosAutomatica(int idSesion, int idSala, int cantidadAsientos)
        {
            DataTable asientosDisponibles = ObtenerAsientosDisponibles(idSala, idSesion);

            if (asientosDisponibles.Rows.Count == 0)
            {
                MessageBox.Show("No hay asientos disponibles para esta sala y sesión.");
                return;
            }

            Random random = new Random();
            int asientosAsignados = 0;
            int idUsuario = ObtenerIdUsuario();

            while (asientosAsignados < cantidadAsientos && asientosDisponibles.Rows.Count > 0)
            {
                int index = random.Next(asientosDisponibles.Rows.Count);
                DataRow asientoRow = asientosDisponibles.Rows[index];
                string fila = asientoRow["fila"].ToString();
                int numero = Convert.ToInt32(asientoRow["numero"]);

                // Obtener el ID del asiento existente o crear uno nuevo si no existe
                int idAsiento = AgregarAsiento(idSala, fila, numero); // Usa la función actualizada

                if (idAsiento > 0)
                {
                    // Registrar la transacción en AsientoTransaccion
                    AgregarTransaccionAsiento(idUsuario, idSesion, idAsiento, "automatica");
                    asientosAsignados++;
                    asientosDisponibles.Rows.RemoveAt(index);
                }
                else
                {
                    MessageBox.Show($"No se pudo asignar el asiento Fila: {fila}, Número: {numero}.");
                }
            }

            if (asientosAsignados > 0)
            {
                LoadAsientos();
                LoadAsientoTransaccion();
            }
            else
            {
                MessageBox.Show("No se pudieron asignar asientos automáticamente.");
            }
        }



        private int ObtenerSiguienteIdAsiento()
        {
            string query = "SELECT ISNULL(MAX(id_asiento), 0) + 1 FROM Asiento";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                try
                {
                    conn.Open();
                    int siguienteId = Convert.ToInt32(cmd.ExecuteScalar());
                    conn.Close();
                    return siguienteId;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al obtener el siguiente ID de asiento: {ex.Message}");
                    conn.Close();
                    return 0;
                }
            }
        }


        private int ObtenerIdAsiento(int idSala, string fila, int numero)
        {
            string query = "SELECT id_asiento FROM Asiento WHERE id_sala = @idSala AND fila = @fila AND numero = @numero";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@idSala", idSala);
                cmd.Parameters.AddWithValue("@fila", fila);
                cmd.Parameters.AddWithValue("@numero", numero);

                try
                {
                    conn.Open();
                    var result = cmd.ExecuteScalar();
                    conn.Close();

                    return result != null ? Convert.ToInt32(result) : 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al obtener el ID del asiento: {ex.Message}");
                    conn.Close();
                    return 0;
                }
            }
        }


        private void btnCambiarAsiento_Click(object sender, EventArgs e)
        {
            if (dataGridViewAsientos.SelectedRows.Count > 0)
            {
                int idSesion = (int)cmbSesiones.SelectedValue;
                int idSala = Convert.ToInt32(dataGridViewAsientos.SelectedRows[0].Cells["id_sala"].Value);
                int idAsientoActual = Convert.ToInt32(dataGridViewAsientos.SelectedRows[0].Cells["id_asiento"].Value);
                string filaActual = dataGridViewAsientos.SelectedRows[0].Cells["fila"].Value.ToString();
                int numeroActual = Convert.ToInt32(dataGridViewAsientos.SelectedRows[0].Cells["numero"].Value);

                // Obtener la lista de asientos disponibles para la misma sala y sesión
                DataTable asientosDisponibles = ObtenerAsientosDisponibles(idSala, idSesion);

                if (asientosDisponibles.Rows.Count == 0)
                {
                    MessageBox.Show("No hay asientos disponibles para esta sala y sesión.");
                    return;
                }

                using (Form inputForm = CrearFormularioAsientoManualConAsientosDisponibles(asientosDisponibles))
                {
                    if (inputForm.ShowDialog() == DialogResult.OK)
                    {
                        string nuevaFila = inputForm.Controls["cmbFila"].Text;
                        int nuevoNumero = int.Parse(((ComboBox)inputForm.Controls["cmbNumero"]).SelectedItem.ToString());

                        // Verificar si el asiento seleccionado es el mismo que el actual
                        if (nuevaFila == filaActual && nuevoNumero == numeroActual)
                        {
                            MessageBox.Show("El asiento seleccionado es el mismo que el asiento actual. No se requiere cambio.");
                            return;
                        }

                        // Obtener el ID del nuevo asiento seleccionado
                        int idAsientoNuevo = ObtenerIdAsiento(idSala, nuevaFila, nuevoNumero);

                        if (idAsientoNuevo > 0)
                        {
                            // Actualizar el asiento en la tabla AsientoTransaccion
                            string updateQuery = @"
                    UPDATE AsientoTransaccion
                    SET id_asiento = @idAsientoNuevo, fecha_hora = GETDATE(), tipo_asignacion = 'manual'
                    WHERE id_asiento = @idAsientoActual AND id_sesion = @idSesion";

                            using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                            {
                                cmd.Parameters.AddWithValue("@idAsientoNuevo", idAsientoNuevo);
                                cmd.Parameters.AddWithValue("@idAsientoActual", idAsientoActual);
                                cmd.Parameters.AddWithValue("@idSesion", idSesion);

                                try
                                {
                                    if (conn.State == ConnectionState.Closed)
                                        conn.Open();
                                    int rowsAffected = cmd.ExecuteNonQuery();
                                    conn.Close();

                                    if (rowsAffected > 0)
                                    {
                                        MessageBox.Show("Asiento cambiado exitosamente.");

                                        // Recargar ambas tablas para reflejar el cambio de asiento
                                        LoadAsientos();
                                        LoadAsientoTransaccion();
                                    }
                                    else
                                    {
                                        MessageBox.Show("No se encontró el asiento actual en la transacción. Verifique que los datos son correctos.");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show($"Error al cambiar asiento: {ex.Message}");
                                }
                                finally
                                {
                                    conn.Close();
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Error al obtener el ID del nuevo asiento. Verifique que el asiento seleccionado esté disponible.");
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Seleccione un asiento para cambiar.");
            }
        }
    }
}
