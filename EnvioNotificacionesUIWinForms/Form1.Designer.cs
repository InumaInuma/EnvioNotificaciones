﻿namespace EnvioNotificacionesUIWinForms
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            notifyIcon1 = new NotifyIcon(components);
            contextMenuStrip1 = new ContextMenuStrip(components);
            iniciarServicioToolStripMenuItem = new ToolStripMenuItem();
            detenerServicioToolStripMenuItem = new ToolStripMenuItem();
            salirToolStripMenuItem = new ToolStripMenuItem();
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // notifyIcon1
            // 
            notifyIcon1.ContextMenuStrip = contextMenuStrip1;
            notifyIcon1.Icon = (Icon)resources.GetObject("notifyIcon1.Icon");
            notifyIcon1.Text = "Control de Notificaciones de Citas";
            notifyIcon1.Visible = true;
            notifyIcon1.MouseDoubleClick += notifyIcon1_MouseDoubleClick;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { iniciarServicioToolStripMenuItem, detenerServicioToolStripMenuItem, salirToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(160, 70);
            // 
            // iniciarServicioToolStripMenuItem
            // 
            iniciarServicioToolStripMenuItem.Name = "iniciarServicioToolStripMenuItem";
            iniciarServicioToolStripMenuItem.Size = new Size(159, 22);
            iniciarServicioToolStripMenuItem.Text = "&Iniciar Servicio";
            iniciarServicioToolStripMenuItem.Click += iniciarServicioToolStripMenuItem_Click;
            // 
            // detenerServicioToolStripMenuItem
            // 
            detenerServicioToolStripMenuItem.Name = "detenerServicioToolStripMenuItem";
            detenerServicioToolStripMenuItem.Size = new Size(159, 22);
            detenerServicioToolStripMenuItem.Text = "&Detener Servicio";
            detenerServicioToolStripMenuItem.Click += detenerServicioToolStripMenuItem_Click;
            // 
            // salirToolStripMenuItem
            // 
            salirToolStripMenuItem.Name = "salirToolStripMenuItem";
            salirToolStripMenuItem.Size = new Size(159, 22);
            salirToolStripMenuItem.Text = "&Salir";
            salirToolStripMenuItem.Click += salirToolStripMenuItem_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(277, 190);
            Name = "Form1";
            Text = "Form1";
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            contextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private NotifyIcon notifyIcon1;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem iniciarServicioToolStripMenuItem;
        private ToolStripMenuItem detenerServicioToolStripMenuItem;
        private ToolStripMenuItem salirToolStripMenuItem;
    }
}
