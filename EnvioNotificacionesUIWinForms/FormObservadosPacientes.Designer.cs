namespace EnvioNotificacionesUIWinForms
{
    partial class FormObservadosPacientes
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
            btnAtras = new Button();
            dataGridViewOrdenObser = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dataGridViewOrdenObser).BeginInit();
            SuspendLayout();
            // 
            // btnAtras
            // 
            btnAtras.Location = new Point(12, 12);
            btnAtras.Name = "btnAtras";
            btnAtras.Size = new Size(68, 40);
            btnAtras.TabIndex = 0;
            btnAtras.Text = "ATRAS";
            btnAtras.UseVisualStyleBackColor = true;
            btnAtras.Click += btnAtras_Click;
            // 
            // dataGridViewOrdenObser
            // 
            dataGridViewOrdenObser.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewOrdenObser.Location = new Point(12, 79);
            dataGridViewOrdenObser.Name = "dataGridViewOrdenObser";
            dataGridViewOrdenObser.Size = new Size(1014, 514);
            dataGridViewOrdenObser.TabIndex = 1;
            dataGridViewOrdenObser.CellContentClick += dataGridViewOrdenObser_CellContentClick;
            // 
            // FormObservadosPacientes
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1038, 605);
            Controls.Add(dataGridViewOrdenObser);
            Controls.Add(btnAtras);
            Name = "FormObservadosPacientes";
            Text = "FormObservadosPacientes";
            Load += FormObservadosPacientes_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridViewOrdenObser).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Button btnAtras;
        private DataGridView dataGridViewOrdenObser;
    }
}