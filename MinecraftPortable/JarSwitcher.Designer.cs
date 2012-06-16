namespace MinecraftPortable
{
    partial class JarSwitcher
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(JarSwitcher));
            this.jarSwitch = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.jarSwitchPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.jarSwitch.SuspendLayout();
            this.jarSwitchPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // jarSwitch
            // 
            this.jarSwitch.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.jarSwitch.Controls.Add(this.jarSwitchPanel);
            this.jarSwitch.Location = new System.Drawing.Point(13, 13);
            this.jarSwitch.Name = "jarSwitch";
            this.jarSwitch.Size = new System.Drawing.Size(373, 280);
            this.jarSwitch.TabIndex = 0;
            this.jarSwitch.TabStop = false;
            this.jarSwitch.Text = "Jar-Auswahl";
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(311, 300);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // jarSwitchPanel
            // 
            this.jarSwitchPanel.Controls.Add(this.checkBox1);
            this.jarSwitchPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.jarSwitchPanel.Location = new System.Drawing.Point(3, 16);
            this.jarSwitchPanel.Name = "jarSwitchPanel";
            this.jarSwitchPanel.Size = new System.Drawing.Size(367, 261);
            this.jarSwitchPanel.TabIndex = 0;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.jarSwitchPanel.SetFlowBreak(this.checkBox1, true);
            this.checkBox1.Location = new System.Drawing.Point(3, 3);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(80, 17);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "checkBox1";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // JarSwitcher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(398, 335);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.jarSwitch);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "JarSwitcher";
            this.Text = "Jar-Switching";
            this.Load += new System.EventHandler(this.JarSwitcher_Load);
            this.jarSwitch.ResumeLayout(false);
            this.jarSwitchPanel.ResumeLayout(false);
            this.jarSwitchPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox jarSwitch;
        private System.Windows.Forms.FlowLayoutPanel jarSwitchPanel;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox checkBox1;

    }
}