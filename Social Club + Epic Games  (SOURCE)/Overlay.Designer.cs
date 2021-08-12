﻿
namespace EPICSC_GTAV_External_Overlay
{
    partial class Overlay
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Overlay));
            this.ProcessTimer = new System.Windows.Forms.Timer(this.components);
            this.WindowHookTimer = new System.Windows.Forms.Timer(this.components);
            this.MemoryTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // ProcessTimer
            // 
            this.ProcessTimer.Enabled = true;
            this.ProcessTimer.Tick += new System.EventHandler(this.ProcessTimer_Tick);
            // 
            // WindowHookTimer
            // 
            this.WindowHookTimer.Enabled = true;
            this.WindowHookTimer.Tick += new System.EventHandler(this.WindowHookTimer_Tick);
            // 
            // MemoryTimer
            // 
            this.MemoryTimer.Enabled = true;
            this.MemoryTimer.Tick += new System.EventHandler(this.MemoryTimer_Tick);
            // 
            // Overlay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(120, 100);
            this.ControlBox = false;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Overlay";
            this.Text = "NightFyreTV";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OverlayClose);
            this.Load += new System.EventHandler(this.Overlay_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.OverlayPaint);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer ProcessTimer;
        private System.Windows.Forms.Timer WindowHookTimer;
        private System.Windows.Forms.Timer MemoryTimer;
    }
}

