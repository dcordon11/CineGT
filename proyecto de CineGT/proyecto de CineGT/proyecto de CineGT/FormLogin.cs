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
    public partial class FormLogin : Form
    {
        private SqlConnection conn;

        public FormLogin()
        {
            InitializeComponent();
            // txtPassword
            this.txtPassword.Location = new System.Drawing.Point(318, 239);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(100, 22);
            this.txtPassword.TabIndex = 3;
            this.txtPassword.PasswordChar = '*'; // Oculta el texto de la contraseña

            conn = GetConnection();
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings["CineDB"].ConnectionString);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = GetConnection())
            {
                try
                {
                    con.Open();

                    // Consulta para verificar si el usuario y la contraseña son correctos
                    string query = "SELECT COUNT(1) FROM Usuario WHERE nombre_usuario = @usuario AND contraseña = @contraseña";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@usuario", txtUsuario.Text);
                    cmd.Parameters.AddWithValue("@contraseña", txtPassword.Text);

                    int count = Convert.ToInt32(cmd.ExecuteScalar());

                    if (count == 1)
                    {
                        // Si las credenciales son correctas, abre el formulario de menú
                        FormMenu formMenu = new FormMenu();
                        formMenu.FormClosed += (s, args) => this.Close(); // Cierra FormLogin cuando FormMenu se cierra
                        formMenu.Show();
                        this.Hide();
                    }
                    else
                    {
                        // Si las credenciales son incorrectas, muestra un mensaje de error
                        MessageBox.Show("Usuario o contraseña incorrectos.", "Error de autenticación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al conectarse a la base de datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
