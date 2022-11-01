namespace sims3_mod_enabler
{
    partial class LoadingForm
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
            this.loadingPackageBar = new System.Windows.Forms.ProgressBar();
            this.windowTextLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // loadingPackageBar
            // 
            this.loadingPackageBar.Location = new System.Drawing.Point(9, 31);
            this.loadingPackageBar.Margin = new System.Windows.Forms.Padding(2);
            this.loadingPackageBar.Name = "loadingPackageBar";
            this.loadingPackageBar.Size = new System.Drawing.Size(582, 19);
            this.loadingPackageBar.Step = 1;
            this.loadingPackageBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.loadingPackageBar.TabIndex = 0;
            // 
            // windowTextLabel
            // 
            this.windowTextLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.windowTextLabel.AutoSize = true;
            this.windowTextLabel.Location = new System.Drawing.Point(212, 64);
            this.windowTextLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.windowTextLabel.Name = "windowTextLabel";
            this.windowTextLabel.Size = new System.Drawing.Size(204, 15);
            this.windowTextLabel.TabIndex = 1;
            this.windowTextLabel.Text = "Loading package files, please wait...";
            // 
            // LoadingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 109);
            this.Controls.Add(this.windowTextLabel);
            this.Controls.Add(this.loadingPackageBar);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "LoadingForm";
            this.Text = "Loading data";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ProgressBar loadingPackageBar;
        private Label windowTextLabel;
    }
}