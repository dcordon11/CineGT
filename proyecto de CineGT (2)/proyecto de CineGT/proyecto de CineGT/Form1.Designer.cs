namespace proyecto_de_CineGT
{
    partial class FormMenu
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnPeliculas = new System.Windows.Forms.Button();
            this.btnsesiones = new System.Windows.Forms.Button();
            this.btnventaasientos = new System.Windows.Forms.Button();
            this.btnanulartransaccion = new System.Windows.Forms.Button();
            this.btnreportes = new System.Windows.Forms.Button();
            this.btncopiaseguridad = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnPeliculas
            // 
            this.btnPeliculas.Location = new System.Drawing.Point(238, 93);
            this.btnPeliculas.Name = "btnPeliculas";
            this.btnPeliculas.Size = new System.Drawing.Size(129, 42);
            this.btnPeliculas.TabIndex = 0;
            this.btnPeliculas.Text = "peliculas";
            this.btnPeliculas.UseVisualStyleBackColor = true;
            this.btnPeliculas.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnsesiones
            // 
            this.btnsesiones.Location = new System.Drawing.Point(414, 93);
            this.btnsesiones.Name = "btnsesiones";
            this.btnsesiones.Size = new System.Drawing.Size(129, 42);
            this.btnsesiones.TabIndex = 1;
            this.btnsesiones.Text = "sesiones";
            this.btnsesiones.UseVisualStyleBackColor = true;
            this.btnsesiones.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnventaasientos
            // 
            this.btnventaasientos.Location = new System.Drawing.Point(238, 162);
            this.btnventaasientos.Name = "btnventaasientos";
            this.btnventaasientos.Size = new System.Drawing.Size(129, 42);
            this.btnventaasientos.TabIndex = 2;
            this.btnventaasientos.Text = "ventas de asientos";
            this.btnventaasientos.UseVisualStyleBackColor = true;
            this.btnventaasientos.Click += new System.EventHandler(this.button3_Click);
            // 
            // btnanulartransaccion
            // 
            this.btnanulartransaccion.Location = new System.Drawing.Point(414, 162);
            this.btnanulartransaccion.Name = "btnanulartransaccion";
            this.btnanulartransaccion.Size = new System.Drawing.Size(129, 42);
            this.btnanulartransaccion.TabIndex = 4;
            this.btnanulartransaccion.Text = "anulacion de transacciones";
            this.btnanulartransaccion.UseVisualStyleBackColor = true;
            this.btnanulartransaccion.Click += new System.EventHandler(this.btnanulartransaccion_Click);
            // 
            // btnreportes
            // 
            this.btnreportes.Location = new System.Drawing.Point(238, 234);
            this.btnreportes.Name = "btnreportes";
            this.btnreportes.Size = new System.Drawing.Size(129, 42);
            this.btnreportes.TabIndex = 5;
            this.btnreportes.Text = "reportes";
            this.btnreportes.UseVisualStyleBackColor = true;
            this.btnreportes.Click += new System.EventHandler(this.btnreportes_Click);
            // 
            // btncopiaseguridad
            // 
            this.btncopiaseguridad.Location = new System.Drawing.Point(414, 234);
            this.btncopiaseguridad.Name = "btncopiaseguridad";
            this.btncopiaseguridad.Size = new System.Drawing.Size(129, 42);
            this.btncopiaseguridad.TabIndex = 7;
            this.btncopiaseguridad.Text = "copias de seguridad";
            this.btncopiaseguridad.UseVisualStyleBackColor = true;
            this.btncopiaseguridad.Click += new System.EventHandler(this.btncopiaseguridad_Click);
            // 
            // FormMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btncopiaseguridad);
            this.Controls.Add(this.btnreportes);
            this.Controls.Add(this.btnanulartransaccion);
            this.Controls.Add(this.btnventaasientos);
            this.Controls.Add(this.btnsesiones);
            this.Controls.Add(this.btnPeliculas);
            this.Name = "FormMenu";
            this.Text = "Form Menu";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnPeliculas;
        private System.Windows.Forms.Button btnsesiones;
        private System.Windows.Forms.Button btnventaasientos;
        private System.Windows.Forms.Button btnanulartransaccion;
        private System.Windows.Forms.Button btnreportes;
        private System.Windows.Forms.Button btncopiaseguridad;
    }
}

