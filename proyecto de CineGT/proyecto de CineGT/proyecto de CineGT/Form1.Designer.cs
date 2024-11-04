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
            this.btnpeliculas = new System.Windows.Forms.Button();
            this.btnsesiones = new System.Windows.Forms.Button();
            this.btnventaasientos = new System.Windows.Forms.Button();
            this.btnanulartransaccion = new System.Windows.Forms.Button();
            this.btnreportes = new System.Windows.Forms.Button();
            this.btnseguridad = new System.Windows.Forms.Button();
            this.btncopiaseguridad = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnpeliculas
            // 
            this.btnpeliculas.Location = new System.Drawing.Point(228, 212);
            this.btnpeliculas.Name = "btnpeliculas";
            this.btnpeliculas.Size = new System.Drawing.Size(129, 42);
            this.btnpeliculas.TabIndex = 0;
            this.btnpeliculas.Text = "Peliculas";
            this.btnpeliculas.UseVisualStyleBackColor = true;
            this.btnpeliculas.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnsesiones
            // 
            this.btnsesiones.Location = new System.Drawing.Point(550, 212);
            this.btnsesiones.Name = "btnsesiones";
            this.btnsesiones.Size = new System.Drawing.Size(129, 42);
            this.btnsesiones.TabIndex = 1;
            this.btnsesiones.Text = "Sesiones";
            this.btnsesiones.UseVisualStyleBackColor = true;
            this.btnsesiones.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnventaasientos
            // 
            this.btnventaasientos.Location = new System.Drawing.Point(228, 275);
            this.btnventaasientos.Name = "btnventaasientos";
            this.btnventaasientos.Size = new System.Drawing.Size(129, 42);
            this.btnventaasientos.TabIndex = 2;
            this.btnventaasientos.Text = "Venta de asientos";
            this.btnventaasientos.UseVisualStyleBackColor = true;
            this.btnventaasientos.Click += new System.EventHandler(this.button3_Click);
            // 
            // btnanulartransaccion
            // 
            this.btnanulartransaccion.Location = new System.Drawing.Point(550, 275);
            this.btnanulartransaccion.Name = "btnanulartransaccion";
            this.btnanulartransaccion.Size = new System.Drawing.Size(129, 42);
            this.btnanulartransaccion.TabIndex = 4;
            this.btnanulartransaccion.Text = "Anulación de transacciones";
            this.btnanulartransaccion.UseVisualStyleBackColor = true;
            this.btnanulartransaccion.Click += new System.EventHandler(this.btnanulartransaccion_Click);
            // 
            // btnreportes
            // 
            this.btnreportes.Location = new System.Drawing.Point(228, 343);
            this.btnreportes.Name = "btnreportes";
            this.btnreportes.Size = new System.Drawing.Size(129, 42);
            this.btnreportes.TabIndex = 5;
            this.btnreportes.Text = "Reportes";
            this.btnreportes.UseVisualStyleBackColor = true;
            this.btnreportes.Click += new System.EventHandler(this.btnreportes_Click);
            // 
            // btnseguridad
            // 
            this.btnseguridad.Location = new System.Drawing.Point(550, 343);
            this.btnseguridad.Name = "btnseguridad";
            this.btnseguridad.Size = new System.Drawing.Size(129, 42);
            this.btnseguridad.TabIndex = 6;
            this.btnseguridad.Text = "Seguridad";
            this.btnseguridad.UseVisualStyleBackColor = true;
            this.btnseguridad.Click += new System.EventHandler(this.btnseguridad_Click);
            // 
            // btncopiaseguridad
            // 
            this.btncopiaseguridad.Location = new System.Drawing.Point(397, 431);
            this.btncopiaseguridad.Name = "btncopiaseguridad";
            this.btncopiaseguridad.Size = new System.Drawing.Size(129, 42);
            this.btncopiaseguridad.TabIndex = 7;
            this.btncopiaseguridad.Text = "Copia de seguridad";
            this.btncopiaseguridad.UseVisualStyleBackColor = true;
            this.btncopiaseguridad.Click += new System.EventHandler(this.btncopiaseguridad_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 22.2F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(350, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(236, 42);
            this.label1.TabIndex = 8;
            this.label1.Text = "¡Bienvenido!";
            // 
            // FormMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(931, 578);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btncopiaseguridad);
            this.Controls.Add(this.btnseguridad);
            this.Controls.Add(this.btnreportes);
            this.Controls.Add(this.btnanulartransaccion);
            this.Controls.Add(this.btnventaasientos);
            this.Controls.Add(this.btnsesiones);
            this.Controls.Add(this.btnpeliculas);
            this.Name = "FormMenu";
            this.Text = "Form Menu";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnpeliculas;
        private System.Windows.Forms.Button btnsesiones;
        private System.Windows.Forms.Button btnventaasientos;
        private System.Windows.Forms.Button btnanulartransaccion;
        private System.Windows.Forms.Button btnreportes;
        private System.Windows.Forms.Button btnseguridad;
        private System.Windows.Forms.Button btncopiaseguridad;
        private System.Windows.Forms.Label label1;
    }
}

