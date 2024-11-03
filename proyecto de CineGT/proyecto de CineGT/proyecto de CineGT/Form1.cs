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

    public partial class FormMenu : Form
    {

        private SqlConnection GetConnection()
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings["CineDB"].ConnectionString);
        }

        private void TuMetodo()
        {
            using (SqlConnection connection = GetConnection())
            {
                try
                {
                    connection.Open();
                    // Aquí puedes ejecutar tus comandos SQL
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        public FormMenu()
        {
            InitializeComponent();
        }
        public static class DatabaseHelper
        {
            public static string ConnectionString = "Server=ALECABRERA;Database=proyecto_basesII;Trusted_Connection=True;";
        }


        private void button1_Click(object sender, EventArgs e)
        {
            new FormGestionPeliculas().ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new FormGestionSesiones().ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new FormVentaAsientos().ShowDialog();
        }

        private void btnanulartransaccion_Click(object sender, EventArgs e)
        {
            new FormAnularTransacciones().ShowDialog();
        }

        private void btnreportes_Click(object sender, EventArgs e)
        {
            new FormReportes().ShowDialog();
        }


        //falta aqui
        private void btnseguridad_Click(object sender, EventArgs e)
        {
            
        }

        private void btncopiaseguridad_Click(object sender, EventArgs e)
        {

        }
    }
}
