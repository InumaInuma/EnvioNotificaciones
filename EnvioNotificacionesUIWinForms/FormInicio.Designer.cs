namespace EnvioNotificacionesUIWinForms
{
    partial class FormInicio
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
            components = new System.ComponentModel.Container();
            dataGridViewCitas = new DataGridView();
            btnAlternarVista = new Button();
            cmbEstadoNotificacion = new ComboBox();
            lblFiltroestado = new Label();
            btnObservados = new Button();
            timerActualizacionCitas = new System.Windows.Forms.Timer(components);
            dataGridViewObservados = new DataGridView();
            panelCitas = new Panel();
            lblFechaFin = new Label();
            lblFechaInicio = new Label();
            btnAplicarFiltros = new Button();
            dtpFechaFin = new DateTimePicker();
            dtpFechaInicio = new DateTimePicker();
            panelObservados = new Panel();
            btnDetenerServicio = new Button();
            btnIniciarServicio = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridViewCitas).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewObservados).BeginInit();
            panelCitas.SuspendLayout();
            panelObservados.SuspendLayout();
            SuspendLayout();
            // 
            // dataGridViewCitas
            // 
            dataGridViewCitas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCitas.Location = new Point(6, 70);
            dataGridViewCitas.Name = "dataGridViewCitas";
            dataGridViewCitas.Size = new Size(795, 475);
            dataGridViewCitas.TabIndex = 0;
            // 
            // btnAlternarVista
            // 
            btnAlternarVista.Location = new Point(463, 15);
            btnAlternarVista.Name = "btnAlternarVista";
            btnAlternarVista.Size = new Size(129, 31);
            btnAlternarVista.TabIndex = 1;
            btnAlternarVista.Text = "VER CITAS";
            btnAlternarVista.UseVisualStyleBackColor = true;
            btnAlternarVista.Click += btnAlternarVista_Click;
            // 
            // cmbEstadoNotificacion
            // 
            cmbEstadoNotificacion.FormattingEnabled = true;
            cmbEstadoNotificacion.Location = new Point(7, 33);
            cmbEstadoNotificacion.Name = "cmbEstadoNotificacion";
            cmbEstadoNotificacion.Size = new Size(149, 23);
            cmbEstadoNotificacion.TabIndex = 2;
            cmbEstadoNotificacion.SelectedIndexChanged += cmbEstadoNotificacion_SelectedIndexChanged;
            // 
            // lblFiltroestado
            // 
            lblFiltroestado.AutoSize = true;
            lblFiltroestado.Location = new Point(5, 15);
            lblFiltroestado.Name = "lblFiltroestado";
            lblFiltroestado.Size = new Size(122, 15);
            lblFiltroestado.TabIndex = 3;
            lblFiltroestado.Text = "FILTRAR POR ESTADO";
            // 
            // btnObservados
            // 
            btnObservados.Location = new Point(672, 33);
            btnObservados.Name = "btnObservados";
            btnObservados.Size = new Size(129, 31);
            btnObservados.TabIndex = 5;
            btnObservados.Text = "OBSERVADOS";
            btnObservados.UseVisualStyleBackColor = true;
            btnObservados.Click += btnObservados_Click;
            // 
            // timerActualizacionCitas
            // 
            timerActualizacionCitas.Interval = 120000;
            timerActualizacionCitas.Tick += timerActualizacionCitas_Tick;
            // 
            // dataGridViewObservados
            // 
            dataGridViewObservados.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewObservados.Location = new Point(6, 62);
            dataGridViewObservados.Name = "dataGridViewObservados";
            dataGridViewObservados.Size = new Size(1017, 485);
            dataGridViewObservados.TabIndex = 6;
            // 
            // panelCitas
            // 
            panelCitas.Controls.Add(lblFechaFin);
            panelCitas.Controls.Add(lblFechaInicio);
            panelCitas.Controls.Add(btnAplicarFiltros);
            panelCitas.Controls.Add(dtpFechaFin);
            panelCitas.Controls.Add(dtpFechaInicio);
            panelCitas.Controls.Add(btnObservados);
            panelCitas.Controls.Add(cmbEstadoNotificacion);
            panelCitas.Controls.Add(lblFiltroestado);
            panelCitas.Controls.Add(dataGridViewCitas);
            panelCitas.Location = new Point(2, 29);
            panelCitas.Name = "panelCitas";
            panelCitas.Size = new Size(1034, 574);
            panelCitas.TabIndex = 7;
            // 
            // lblFechaFin
            // 
            lblFechaFin.AutoSize = true;
            lblFechaFin.Location = new Point(897, 57);
            lblFechaFin.Name = "lblFechaFin";
            lblFechaFin.Size = new Size(57, 15);
            lblFechaFin.TabIndex = 10;
            lblFechaFin.Text = "Fecha Fin";
            // 
            // lblFechaInicio
            // 
            lblFechaInicio.AutoSize = true;
            lblFechaInicio.Location = new Point(891, 12);
            lblFechaInicio.Name = "lblFechaInicio";
            lblFechaInicio.Size = new Size(70, 15);
            lblFechaInicio.TabIndex = 9;
            lblFechaInicio.Text = "Fecha Inicio";
            // 
            // btnAplicarFiltros
            // 
            btnAplicarFiltros.Location = new Point(884, 105);
            btnAplicarFiltros.Name = "btnAplicarFiltros";
            btnAplicarFiltros.Size = new Size(75, 23);
            btnAplicarFiltros.TabIndex = 8;
            btnAplicarFiltros.Text = "FILTRAR";
            btnAplicarFiltros.UseVisualStyleBackColor = true;
            btnAplicarFiltros.Click += btnAplicarFiltros_Click;
            // 
            // dtpFechaFin
            // 
            dtpFechaFin.Location = new Point(824, 76);
            dtpFechaFin.Name = "dtpFechaFin";
            dtpFechaFin.Size = new Size(200, 23);
            dtpFechaFin.TabIndex = 7;
            // 
            // dtpFechaInicio
            // 
            dtpFechaInicio.Location = new Point(824, 30);
            dtpFechaInicio.Name = "dtpFechaInicio";
            dtpFechaInicio.Size = new Size(200, 23);
            dtpFechaInicio.TabIndex = 6;
            // 
            // panelObservados
            // 
            panelObservados.Controls.Add(btnAlternarVista);
            panelObservados.Controls.Add(dataGridViewObservados);
            panelObservados.Location = new Point(3, 29);
            panelObservados.Name = "panelObservados";
            panelObservados.Size = new Size(1033, 574);
            panelObservados.TabIndex = 5;
            panelObservados.Paint += Observados_Paint;
            // 
            // btnDetenerServicio
            // 
            btnDetenerServicio.Location = new Point(756, 6);
            btnDetenerServicio.Name = "btnDetenerServicio";
            btnDetenerServicio.Size = new Size(113, 23);
            btnDetenerServicio.TabIndex = 8;
            btnDetenerServicio.Text = "STOP SERVICIO";
            btnDetenerServicio.UseVisualStyleBackColor = true;
            btnDetenerServicio.Click += btnDetenerServicio_Click;
            // 
            // btnIniciarServicio
            // 
            btnIniciarServicio.Location = new Point(893, 6);
            btnIniciarServicio.Name = "btnIniciarServicio";
            btnIniciarServicio.Size = new Size(113, 23);
            btnIniciarServicio.TabIndex = 9;
            btnIniciarServicio.Text = "STAR SERVICIO";
            btnIniciarServicio.UseVisualStyleBackColor = true;
            btnIniciarServicio.Click += btnIniciarServicio_Click;
            // 
            // FormInicio
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1038, 605);
            Controls.Add(btnIniciarServicio);
            Controls.Add(btnDetenerServicio);
            Controls.Add(panelObservados);
            Controls.Add(panelCitas);
            Name = "FormInicio";
            Text = "FormInicio";
            FormClosing += FormInicio_FormClosing;
            Load += FormInicio_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridViewCitas).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewObservados).EndInit();
            panelCitas.ResumeLayout(false);
            panelCitas.PerformLayout();
            panelObservados.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dataGridViewCitas;
        private Button btnAlternarVista;
        private ComboBox cmbEstadoNotificacion;
        private Label lblFiltroestado;
        private Button btnObservados;
        private System.Windows.Forms.Timer timerActualizacionCitas;
        private DataGridView dataGridViewObservados;
        private Panel panelCitas;
        private Panel panelObservados;
        private Button btnAplicarFiltros;
        private DateTimePicker dtpFechaFin;
        private DateTimePicker dtpFechaInicio;
        private Label lblFechaFin;
        private Label lblFechaInicio;
        private Button btnDetenerServicio;
        private Button btnIniciarServicio;
    }
}