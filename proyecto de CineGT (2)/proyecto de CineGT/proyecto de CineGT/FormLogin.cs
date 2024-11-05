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
            this.txtPassword.Location = new System.Drawing.Point(279, 121);
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

                    // Obtener el rol del usuario junto con la autenticación
                    string query = "SELECT rol FROM Usuario WHERE nombre_usuario = @usuario AND contraseña = @contraseña";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@usuario", txtUsuario.Text);
                    cmd.Parameters.AddWithValue("@contraseña", txtPassword.Text);

                    var rol = cmd.ExecuteScalar()?.ToString();
                    var nombreUsuario = txtUsuario.Text;


                    if (!string.IsNullOrEmpty(rol))
                    {
                        // Si las credenciales son correctas, pasa el rol al FormMenu
                        FormMenu formMenu = new FormMenu(rol, nombreUsuario);
                        formMenu.FormClosed += (s, args) => this.Close();
                        formMenu.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Usuario o contraseña incorrectos.", "Error de autenticación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al conectarse a la base de datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnCrearUsuario_Click(object sender, EventArgs e)
        {
            FormCrearUsuario formCrearUsuario = new FormCrearUsuario();
            formCrearUsuario.ShowDialog();
        }
    }
}
