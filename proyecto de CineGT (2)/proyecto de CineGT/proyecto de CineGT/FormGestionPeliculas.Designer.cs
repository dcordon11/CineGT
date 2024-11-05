namespace proyecto_de_CineGT
{
    partial class FormGestionPeliculas
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.txtDescripcion = new System.Windows.Forms.TextBox();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.txtClasificacion = new System.Windows.Forms.TextBox();
            this.txtDuracion = new System.Windows.Forms.TextBox();
            this.btnagregar = new System.Windows.Forms.Button();
            this.btneditar = new System.Windows.Forms.Button();
            this.btneliminar = new System.Windows.Forms.Button();
            this.dataGridViewPeliculas = new System.Windows.Forms.DataGridView();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPeliculas)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(44, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(184, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "ingrese nombre de la pelicula";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(44, 112);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(212, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "ingrese clasificacion de la pelicula";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(44, 178);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(204, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "ingrese la duracion de la pelicula";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(44, 245);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(245, 16);
            this.label4.TabIndex = 3;
            this.label4.Text = "ingrese una descripcion para la pelicula";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // txtDescripcion
            // 
            this.txtDescripcion.Location = new System.Drawing.Point(312, 239);
            this.txtDescripcion.Name = "txtDescripcion";
            this.txtDescripcion.Size = new System.Drawing.Size(318, 22);
            this.txtDescripcion.TabIndex = 5;
            // 
            // txtNombre
            // 
            this.txtNombre.Location = new System.Drawing.Point(312, 49);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(318, 22);
            this.txtNombre.TabIndex = 6;
            // 
            // txtClasificacion
            // 
            this.txtClasificacion.Location = new System.Drawing.Point(312, 106);
            this.txtClasificacion.Name = "txtClasificacion";
            this.txtClasificacion.Size = new System.Drawing.Size(318, 22);
            this.txtClasificacion.TabIndex = 7;
            // 
            // txtDuracion
            // 
            this.txtDuracion.Location = new System.Drawing.Point(312, 172);
            this.txtDuracion.Name = "txtDuracion";
            this.txtDuracion.Size = new System.Drawing.Size(318, 22);
            this.txtDuracion.TabIndex = 8;
            // 
            // btnagregar
            // 
            this.btnagregar.Location = new System.Drawing.Point(141, 329);
            this.btnagregar.Name = "btnagregar";
            this.btnagregar.Size = new System.Drawing.Size(107, 39);
            this.btnagregar.TabIndex = 9;
            this.btnagregar.Text = "agregar";
            this.btnagregar.UseVisualStyleBackColor = true;
            this.btnagregar.Click += new System.EventHandler(this.button1_Click);
            // 
            // btneditar
            // 
            this.btneditar.Location = new System.Drawing.Point(983, 272);
            this.btneditar.Name = "btneditar";
            this.btneditar.Size = new System.Drawing.Size(107, 39);
            this.btneditar.TabIndex = 10;
            this.btneditar.Text = "editar";
            this.btneditar.UseVisualStyleBackColor = true;
            this.btneditar.Click += new System.EventHandler(this.btneditar_Click);
            // 
            // btneliminar
            // 
            this.btneliminar.Location = new System.Drawing.Point(983, 80);
            this.btneliminar.Name = "btneliminar";
            this.btneliminar.Size = new System.Drawing.Size(107, 39);
            this.btneliminar.TabIndex = 11;
            this.btneliminar.Text = "eliminar";
            this.btneliminar.UseVisualStyleBackColor = true;
            this.btneliminar.Click += new System.EventHandler(this.btneliminar_Click);
            // 
            // dataGridViewPeliculas
            // 
            this.dataGridViewPeliculas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewPeliculas.Location = new System.Drawing.Point(97, 438);
            this.dataGridViewPeliculas.Name = "dataGridViewPeliculas";
            this.dataGridViewPeliculas.RowHeadersWidth = 51;
            this.dataGridViewPeliculas.RowTemplate.Height = 24;
            this.dataGridViewPeliculas.Size = new System.Drawing.Size(1240, 268);
            this.dataGridViewPeliculas.TabIndex = 12;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(916, 48);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(225, 16);
            this.label5.TabIndex = 13;
            this.label5.Text = "seleccione la fila que desea eliminar";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(804, 239);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(480, 16);
            this.label6.TabIndex = 14;
            this.label6.Text = "seleccione la fila que desea editar y en los textbox ingrese la nueva informacion" +
    "";
            // 
            // FormGestionPeliculas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1428, 801);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.dataGridViewPeliculas);
            this.Controls.Add(this.btneliminar);
            this.Controls.Add(this.btneditar);
            this.Controls.Add(this.btnagregar);
            this.Controls.Add(this.txtDuracion);
            this.Controls.Add(this.txtClasificacion);
            this.Controls.Add(this.txtNombre);
            this.Controls.Add(this.txtDescripcion);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "FormGestionPeliculas";
            this.Text = "FormGestionPeliculas";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPeliculas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.TextBox txtDescripcion;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.TextBox txtClasificacion;
        private System.Windows.Forms.TextBox txtDuracion;
        private System.Windows.Forms.Button btnagregar;
        private System.Windows.Forms.Button btneditar;
        private System.Windows.Forms.Button btneliminar;
        private System.Windows.Forms.DataGridView dataGridViewPeliculas;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
    }
}