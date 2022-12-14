namespace sims3_mod_enabler
{
    partial class MainForm
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
            this.inputTree = new System.Windows.Forms.TreeView();
            this.outputTree = new System.Windows.Forms.TreeView();
            this.enableButton = new System.Windows.Forms.Button();
            this.disableButton = new System.Windows.Forms.Button();
            this.allLabel = new System.Windows.Forms.Label();
            this.enabledLabel = new System.Windows.Forms.Label();
            this.saveButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // inputTree
            // 
            this.inputTree.Location = new System.Drawing.Point(9, 35);
            this.inputTree.Name = "inputTree";
            this.inputTree.Size = new System.Drawing.Size(463, 668);
            this.inputTree.TabIndex = 0;
            // 
            // outputTree
            // 
            this.outputTree.Location = new System.Drawing.Point(521, 35);
            this.outputTree.Name = "outputTree";
            this.outputTree.Size = new System.Drawing.Size(463, 669);
            this.outputTree.TabIndex = 1;
            // 
            // enableButton
            // 
            this.enableButton.Location = new System.Drawing.Point(477, 286);
            this.enableButton.Name = "enableButton";
            this.enableButton.Size = new System.Drawing.Size(39, 24);
            this.enableButton.TabIndex = 2;
            this.enableButton.Text = ">>";
            this.enableButton.UseVisualStyleBackColor = true;
            this.enableButton.Click += new System.EventHandler(this.enableButton_Click);
            // 
            // disableButton
            // 
            this.disableButton.Location = new System.Drawing.Point(477, 382);
            this.disableButton.Name = "disableButton";
            this.disableButton.Size = new System.Drawing.Size(39, 23);
            this.disableButton.TabIndex = 3;
            this.disableButton.Text = "<<";
            this.disableButton.UseVisualStyleBackColor = true;
            this.disableButton.Click += new System.EventHandler(this.disableButton_Click);
            // 
            // allLabel
            // 
            this.allLabel.AutoSize = true;
            this.allLabel.Location = new System.Drawing.Point(10, 15);
            this.allLabel.Name = "allLabel";
            this.allLabel.Size = new System.Drawing.Size(112, 15);
            this.allLabel.TabIndex = 4;
            this.allLabel.Text = "Available packages";
            // 
            // enabledLabel
            // 
            this.enabledLabel.AutoSize = true;
            this.enabledLabel.Location = new System.Drawing.Point(521, 15);
            this.enabledLabel.Name = "enabledLabel";
            this.enabledLabel.Size = new System.Drawing.Size(112, 15);
            this.enabledLabel.TabIndex = 5;
            this.enabledLabel.Text = "Enabled  packages";
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(462, 738);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(64, 24);
            this.saveButton.TabIndex = 6;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(993, 800);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.enabledLabel);
            this.Controls.Add(this.allLabel);
            this.Controls.Add(this.disableButton);
            this.Controls.Add(this.enableButton);
            this.Controls.Add(this.outputTree);
            this.Controls.Add(this.inputTree);
            this.Name = "MainForm";
            this.Text = "Sims 3 Mods Enabler";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TreeView inputTree;
        private TreeView outputTree;
        private Button enableButton;
        private Button disableButton;
        private Label allLabel;
        private Label enabledLabel;
        private Button saveButton;
    }
}