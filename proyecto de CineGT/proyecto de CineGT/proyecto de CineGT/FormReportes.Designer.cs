namespace proyecto_de_CineGT
{
    partial class FormReportes
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dtpFechaInicio = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.btnListadoSesiones = new System.Windows.Forms.Button();
            this.btnListadoTransacciones = new System.Windows.Forms.Button();
            this.dgvReportes = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.dtpFechaFin = new System.Windows.Forms.DateTimePicker();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReportes)).BeginInit();
            this.SuspendLayout();
            // 
            // dtpFechaInicio
            // 
            this.dtpFechaInicio.Location = new System.Drawing.Point(539, 58);
            this.dtpFechaInicio.Name = "dtpFechaInicio";
            this.dtpFechaInicio.Size = new System.Drawing.Size(268, 22);
            this.dtpFechaInicio.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(271, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(157, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "seleccione la fecha inicio";
            // 
            // btnListadoSesiones
            // 
            this.btnListadoSesiones.Location = new System.Drawing.Point(337, 204);
            this.btnListadoSesiones.Name = "btnListadoSesiones";
            this.btnListadoSesiones.Size = new System.Drawing.Size(158, 52);
            this.btnListadoSesiones.TabIndex = 2;
            this.btnListadoSesiones.Text = "listado de sesiones";
            this.btnListadoSesiones.UseVisualStyleBackColor = true;
            this.btnListadoSesiones.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnListadoTransacciones
            // 
            this.btnListadoTransacciones.Location = new System.Drawing.Point(539, 204);
            this.btnListadoTransacciones.Name = "btnListadoTransacciones";
            this.btnListadoTransacciones.Size = new System.Drawing.Size(158, 52);
            this.btnListadoTransacciones.TabIndex = 3;
            this.btnListadoTransacciones.Text = "listado de transacciones";
            this.btnListadoTransacciones.UseVisualStyleBackColor = true;
            this.btnListadoTransacciones.Click += new System.EventHandler(this.btnListadoTransacciones_Click);
            // 
            // dgvReportes
            // 
            this.dgvReportes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReportes.Location = new System.Drawing.Point(12, 360);
            this.dgvReportes.Name = "dgvReportes";
            this.dgvReportes.RowHeadersWidth = 51;
            this.dgvReportes.RowTemplate.Height = 24;
            this.dgvReportes.Size = new System.Drawing.Size(1005, 214);
            this.dgvReportes.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(271, 97);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(139, 16);
            this.label2.TabIndex = 5;
            this.label2.Text = "seleccione la fecha fin";
            // 
            // dtpFechaFin
            // 
            this.dtpFechaFin.Location = new System.Drawing.Point(539, 97);
            this.dtpFechaFin.Name = "dtpFechaFin";
            this.dtpFechaFin.Size = new System.Drawing.Size(268, 22);
            this.dtpFechaFin.TabIndex = 6;
            // 
            // FormReportes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1045, 661);
            this.Controls.Add(this.dtpFechaFin);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dgvReportes);
            this.Controls.Add(this.btnListadoTransacciones);
            this.Controls.Add(this.btnListadoSesiones);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dtpFechaInicio);
            this.Name = "FormReportes";
            this.Text = "FormReportes";
            ((System.ComponentModel.ISupportInitialize)(this.dgvReportes)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dtpFechaInicio;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnListadoSesiones;
        private System.Windows.Forms.Button btnListadoTransacciones;
        private System.Windows.Forms.DataGridView dgvReportes;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtpFechaFin;
    }
}