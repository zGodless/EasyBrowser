﻿namespace EasyBrowser
{
    partial class FormURL
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
            this.textURL = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textURL
            // 
            this.textURL.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textURL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textURL.Location = new System.Drawing.Point(0, 0);
            this.textURL.Multiline = true;
            this.textURL.Name = "textURL";
            this.textURL.Size = new System.Drawing.Size(800, 19);
            this.textURL.TabIndex = 0;
            // 
            // FormURL
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 19);
            this.Controls.Add(this.textURL);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormURL";
            this.Opacity = 0.5D;
            this.Text = "Form2";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textURL;
    }
}