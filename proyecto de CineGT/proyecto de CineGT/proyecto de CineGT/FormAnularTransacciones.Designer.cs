namespace proyecto_de_CineGT
{
    partial class FormAnularTransacciones
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
            this.btnAnularTransaccion = new System.Windows.Forms.Button();
            this.dgvTransacciones = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTransacciones)).BeginInit();
            this.SuspendLayout();
            // 
            // btnAnularTransaccion
            // 
            this.btnAnularTransaccion.Location = new System.Drawing.Point(328, 170);
            this.btnAnularTransaccion.Name = "btnAnularTransaccion";
            this.btnAnularTransaccion.Size = new System.Drawing.Size(148, 45);
            this.btnAnularTransaccion.TabIndex = 1;
            this.btnAnularTransaccion.Text = "anular transaccion";
            this.btnAnularTransaccion.UseVisualStyleBackColor = true;
            this.btnAnularTransaccion.Click += new System.EventHandler(this.button1_Click);
            // 
            // dgvTransacciones
            // 
            this.dgvTransacciones.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTransacciones.Location = new System.Drawing.Point(12, 288);
            this.dgvTransacciones.Name = "dgvTransacciones";
            this.dgvTransacciones.RowHeadersWidth = 51;
            this.dgvTransacciones.RowTemplate.Height = 24;
            this.dgvTransacciones.Size = new System.Drawing.Size(776, 201);
            this.dgvTransacciones.TabIndex = 2;
            // 
            // FormAnularTransacciones
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 501);
            this.Controls.Add(this.dgvTransacciones);
            this.Controls.Add(this.btnAnularTransaccion);
            this.Name = "FormAnularTransacciones";
            this.Text = "FormAnularTransacciones";
            ((System.ComponentModel.ISupportInitialize)(this.dgvTransacciones)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnAnularTransaccion;
        private System.Windows.Forms.DataGridView dgvTransacciones;
    }
}