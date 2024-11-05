namespace proyecto_de_CineGT
{
    partial class FormGestionSesiones
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
            this.cmbPeliculas = new System.Windows.Forms.ComboBox();
            this.dateTimePickerInicio = new System.Windows.Forms.DateTimePicker();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.dataGridViewSesiones = new System.Windows.Forms.DataGridView();
            this.cmbSalas = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnReactivarSesion = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.timePickerInicio = new System.Windows.Forms.DateTimePicker();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSesiones)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbPeliculas
            // 
            this.cmbPeliculas.FormattingEnabled = true;
            this.cmbPeliculas.Location = new System.Drawing.Point(167, 52);
            this.cmbPeliculas.Name = "cmbPeliculas";
            this.cmbPeliculas.Size = new System.Drawing.Size(121, 24);
            this.cmbPeliculas.TabIndex = 0;
            // 
            // dateTimePickerInicio
            // 
            this.dateTimePickerInicio.Location = new System.Drawing.Point(543, 50);
            this.dateTimePickerInicio.Name = "dateTimePickerInicio";
            this.dateTimePickerInicio.Size = new System.Drawing.Size(261, 22);
            this.dateTimePickerInicio.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(216, 195);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(150, 44);
            this.button1.TabIndex = 2;
            this.button1.Text = "crear sesion";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(402, 195);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(150, 44);
            this.button2.TabIndex = 3;
            this.button2.Text = "desactivar sesion";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // dataGridViewSesiones
            // 
            this.dataGridViewSesiones.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewSesiones.Location = new System.Drawing.Point(38, 324);
            this.dataGridViewSesiones.Name = "dataGridViewSesiones";
            this.dataGridViewSesiones.RowHeadersWidth = 51;
            this.dataGridViewSesiones.RowTemplate.Height = 24;
            this.dataGridViewSesiones.Size = new System.Drawing.Size(901, 263);
            this.dataGridViewSesiones.TabIndex = 4;
            // 
            // cmbSalas
            // 
            this.cmbSalas.FormattingEnabled = true;
            this.cmbSalas.Location = new System.Drawing.Point(366, 52);
            this.cmbSalas.Name = "cmbSalas";
            this.cmbSalas.Size = new System.Drawing.Size(121, 24);
            this.cmbSalas.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(199, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 16);
            this.label1.TabIndex = 6;
            this.label1.Text = "peliculas";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(399, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 16);
            this.label2.TabIndex = 7;
            this.label2.Text = "sala";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(592, 32);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(166, 16);
            this.label4.TabIndex = 10;
            this.label4.Text = "seleccionar fecha de inicio";
            // 
            // btnReactivarSesion
            // 
            this.btnReactivarSesion.Location = new System.Drawing.Point(595, 195);
            this.btnReactivarSesion.Name = "btnReactivarSesion";
            this.btnReactivarSesion.Size = new System.Drawing.Size(150, 44);
            this.btnReactivarSesion.TabIndex = 11;
            this.btnReactivarSesion.Text = "reactivar sesion";
            this.btnReactivarSesion.UseVisualStyleBackColor = true;
            this.btnReactivarSesion.Click += new System.EventHandler(this.btnReactivarSesion_Click);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(595, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(182, 16);
            this.label3.TabIndex = 0;
            this.label3.Text = "seleccionar hora de inicio";
            // 
            // timePickerInicio
            // 
            this.timePickerInicio.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.timePickerInicio.Location = new System.Drawing.Point(620, 98);
            this.timePickerInicio.Name = "timePickerInicio";
            this.timePickerInicio.Size = new System.Drawing.Size(125, 22);
            this.timePickerInicio.TabIndex = 13;
            // 
            // FormGestionSesiones
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(978, 815);
            this.Controls.Add(this.timePickerInicio);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnReactivarSesion);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbSalas);
            this.Controls.Add(this.dataGridViewSesiones);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dateTimePickerInicio);
            this.Controls.Add(this.cmbPeliculas);
            this.Name = "FormGestionSesiones";
            this.Text = "FormGestionSesiones";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSesiones)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbPeliculas;
        private System.Windows.Forms.DateTimePicker dateTimePickerInicio;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DataGridView dataGridViewSesiones;
        private System.Windows.Forms.ComboBox cmbSalas;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnReactivarSesion;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker timePickerInicio;
    }
}