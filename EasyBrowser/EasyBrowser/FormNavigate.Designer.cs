namespace EasyBrowser
{
    partial class FormNavigate
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
            this.buttonNavigate = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonNavigate
            // 
            this.buttonNavigate.AllowDrop = true;
            this.buttonNavigate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonNavigate.Location = new System.Drawing.Point(0, 0);
            this.buttonNavigate.Name = "buttonNavigate";
            this.buttonNavigate.Size = new System.Drawing.Size(14, 14);
            this.buttonNavigate.TabIndex = 0;
            this.buttonNavigate.Text = "a";
            this.buttonNavigate.UseVisualStyleBackColor = true;
            // 
            // FormNavigate
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(14, 14);
            this.Controls.Add(this.buttonNavigate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormNavigate";
            this.Text = "Form2";
            this.Load += new System.EventHandler(this.FormNavigate_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonNavigate;
    }
}