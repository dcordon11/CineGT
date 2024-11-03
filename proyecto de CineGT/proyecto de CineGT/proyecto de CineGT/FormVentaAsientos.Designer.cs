namespace proyecto_de_CineGT
{
    partial class FormVentaAsientos
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
            this.cmbSesiones = new System.Windows.Forms.ComboBox();
            this.numericUpDownCantidadAsientos = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.radioButtonAutomatico = new System.Windows.Forms.RadioButton();
            this.btnVenderAsientos = new System.Windows.Forms.Button();
            this.dataGridViewAsientos = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCantidadAsientos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAsientos)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbSesiones
            // 
            this.cmbSesiones.FormattingEnabled = true;
            this.cmbSesiones.Location = new System.Drawing.Point(234, 28);
            this.cmbSesiones.Name = "cmbSesiones";
            this.cmbSesiones.Size = new System.Drawing.Size(453, 24);
            this.cmbSesiones.TabIndex = 0;
            // 
            // numericUpDownCantidadAsientos
            // 
            this.numericUpDownCantidadAsientos.Location = new System.Drawing.Point(234, 155);
            this.numericUpDownCantidadAsientos.Name = "numericUpDownCantidadAsientos";
            this.numericUpDownCantidadAsientos.Size = new System.Drawing.Size(120, 22);
            this.numericUpDownCantidadAsientos.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 161);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(180, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "ingrese cantidad de asientos";
            // 
            // radioButtonAutomatico
            // 
            this.radioButtonAutomatico.AutoSize = true;
            this.radioButtonAutomatico.Location = new System.Drawing.Point(234, 222);
            this.radioButtonAutomatico.Name = "radioButtonAutomatico";
            this.radioButtonAutomatico.Size = new System.Drawing.Size(163, 20);
            this.radioButtonAutomatico.TabIndex = 3;
            this.radioButtonAutomatico.TabStop = true;
            this.radioButtonAutomatico.Text = "asignacion automatica";
            this.radioButtonAutomatico.UseVisualStyleBackColor = true;
            // 
            // btnVenderAsientos
            // 
            this.btnVenderAsientos.Location = new System.Drawing.Point(234, 293);
            this.btnVenderAsientos.Name = "btnVenderAsientos";
            this.btnVenderAsientos.Size = new System.Drawing.Size(131, 43);
            this.btnVenderAsientos.TabIndex = 4;
            this.btnVenderAsientos.Text = "confirmar compra";
            this.btnVenderAsientos.UseVisualStyleBackColor = true;
            this.btnVenderAsientos.Click += new System.EventHandler(this.button1_Click);
            // 
            // dataGridViewAsientos
            // 
            this.dataGridViewAsientos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewAsientos.Location = new System.Drawing.Point(32, 425);
            this.dataGridViewAsientos.Name = "dataGridViewAsientos";
            this.dataGridViewAsientos.RowHeadersWidth = 51;
            this.dataGridViewAsientos.RowTemplate.Height = 24;
            this.dataGridViewAsientos.Size = new System.Drawing.Size(1109, 241);
            this.dataGridViewAsientos.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(32, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 16);
            this.label2.TabIndex = 6;
            this.label2.Text = "sesiones";
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(234, 254);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(141, 20);
            this.radioButton1.TabIndex = 7;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "asignacion manual";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // FormVentaAsientos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1220, 701);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dataGridViewAsientos);
            this.Controls.Add(this.btnVenderAsientos);
            this.Controls.Add(this.radioButtonAutomatico);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numericUpDownCantidadAsientos);
            this.Controls.Add(this.cmbSesiones);
            this.Name = "FormVentaAsientos";
            this.Text = "FormVentaAsientos";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCantidadAsientos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAsientos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbSesiones;
        private System.Windows.Forms.NumericUpDown numericUpDownCantidadAsientos;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton radioButtonAutomatico;
        private System.Windows.Forms.Button btnVenderAsientos;
        private System.Windows.Forms.DataGridView dataGridViewAsientos;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton radioButton1;
    }
}