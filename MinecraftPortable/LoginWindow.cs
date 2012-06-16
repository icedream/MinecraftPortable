using Microsoft.VisualBasic.Devices;
using Minecraft;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
namespace MinecraftPortable
{
	public partial class LoginWindow : Form
	{
        JarSwitcher jarSwitch = new JarSwitcher();

		private readonly char[] AllowedCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_".ToCharArray();
		private StartWindow sw = new StartWindow();
        private ComputerInfo computer = new ComputerInfo();

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.Button button1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label label2;
        private Button button2;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem itmJarSwitch;
        private System.Windows.Forms.TextBox txtPass;
		
		public LoginWindow()
		{
			this.InitializeComponent();
        }

        #region Window Design
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginWindow));
            this.label1 = new System.Windows.Forms.Label();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPass = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.itmJarSwitch = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // txtUser
            // 
            this.txtUser.AcceptsReturn = true;
            resources.ApplyResources(this.txtUser, "txtUser");
            this.txtUser.Name = "txtUser";
            this.txtUser.Enter += new System.EventHandler(this.txtUser_Enter);
            this.txtUser.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtUser_KeyPress);
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // txtPass
            // 
            this.txtPass.AcceptsReturn = true;
            resources.ApplyResources(this.txtPass, "txtPass");
            this.txtPass.Name = "txtPass";
            this.txtPass.UseSystemPasswordChar = true;
            // 
            // button2
            // 
            resources.ApplyResources(this.button2, "button2");
            this.button2.Name = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itmJarSwitch});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            resources.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
            // 
            // itmJarSwitch
            // 
            this.itmJarSwitch.Name = "itmJarSwitch";
            resources.ApplyResources(this.itmJarSwitch, "itmJarSwitch");
            this.itmJarSwitch.Click += new System.EventHandler(this.itmJarSwitch_Click);
            // 
            // LoginWindow
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button2);
            this.Controls.Add(this.txtPass);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtUser);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "LoginWindow";
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private void button1_Click(object sender, EventArgs e)
		{
			Program.LoginUsername = this.txtUser.Text;
			Program.LoginPasswort = this.txtPass.Text;
			if (string.IsNullOrEmpty(Program.LoginUsername))
			{
				MessageBox.Show("Dein Username darf nicht leer sein!", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return;
			}
			if (Program.LoginUsername.Length > 16)
			{
				MessageBox.Show("Dein Username darf nicht länger als 16 Zeichen sein!", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return;
			}
			string text = Program.LoginUsername;
			char[] allowedCharacters = this.AllowedCharacters;
			for (int i = 0; i < allowedCharacters.Length; i++)
			{
				char c = allowedCharacters[i];
				text = text.Replace(c.ToString(), "");
			}
			if (!string.IsNullOrEmpty(text))
			{
				MessageBox.Show("Dein Username enthält ungültige Zeichen:" + Environment.NewLine + Environment.NewLine + text, "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return;
			}
			if (string.IsNullOrEmpty(Program.LoginPasswort))
			{
				MessageBox.Show("Ohne ein gültiges Passwort kannst du nur auf Server im Offline-Modus!", "Achtung", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			this.sw.Show();
			this.backgroundWorker1.RunWorkerAsync(jarSwitch.SelectedJar);
		}
		private void java_ErrorDataReceived(object sender, DataReceivedEventArgs e)
		{
			MessageBox.Show("Error data: " + e.Data);
		}
		private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
		{
            string selectedJar = e.Argument.ToString();

			Authentication authentication = new Authentication();
			Control.CheckForIllegalCrossThreadCalls = false;
			base.Hide();
			Control.CheckForIllegalCrossThreadCalls = true;
			authentication.SessionId = "12345";
			if (!string.IsNullOrEmpty(Program.LoginPasswort))
			{
				this.sw.SetText("Login...");
				Control.CheckForIllegalCrossThreadCalls = false;
				this.sw.TopMost = false;
				Control.CheckForIllegalCrossThreadCalls = true;
				authentication.Username = Program.LoginUsername;
				authentication.Password = Program.LoginPasswort;
				authentication.LoginApiUri = new Uri("https://login.minecraft.net");
				if (!authentication.Login())
				{
					MessageBox.Show("Passwort oder Username falsch, du bekommst leider keinen Zugriff auf Premium-Server.");
				}
				else
				{
					this.sw.SetText("Login erfolgreich!");
				}
				Control.CheckForIllegalCrossThreadCalls = false;
				this.sw.TopMost = true;
				Control.CheckForIllegalCrossThreadCalls = true;
			}
			try
			{
				this.sw.SetText("Minecraft wird vorbereitet...");
				double num = this.computer.AvailablePhysicalMemory / 1048576uL;
				int num2 = -65535;
				int num3 = 0;
				bool flag = false;
				while (num2 != 0 && num3 < 20)
				{
					num *= 0.9;
					double num4 = num / 2.0;
					int num5 = (int)Math.Round(num / 4.0, 0) * 4;
					int num6 = (int)Math.Round(num4 / 4.0, 0) * 4;
					num3++;
					this.sw.SetText("Starte mit " + num5 + " MB RAM...");
					int num7 = 0;
					while (num7 < 2 && num2 != 0)
					{
						Process process = JavaPath.CreateJavaW(new string[]
						{
							"-Xms" + num6 + "M",
							"-Xmx" + num5 + "M",
							"-XX:UseSSE=3",
							"-Djava.library.path=" + Program.NativesPath,
							flag ? "-Xincgc" : null,
							"-cp",
							string.Join(";", new string[]
							{
								selectedJar,
								Path.Combine(Program.BinPath, "lwjgl.jar"),
								Path.Combine(Program.BinPath, "lwjgl_util.jar"),
								Path.Combine(Program.BinPath, "jinput.jar")
							}),
							"net.minecraft.client.Minecraft",
							Program.LoginUsername,
							"12345"
						});
						process.EnableRaisingEvents = true;
						process.StartInfo.EnvironmentVariables["AppData"] = Environment.CurrentDirectory;
						process.Start();
						while (!process.HasExited)
						{
							string text = process.StandardOutput.ReadLine();
							if (text != null)
							{
								this.sw.SetText(text);
								Control.CheckForIllegalCrossThreadCalls = false;
								base.TopMost = true;
								base.ControlBox = false;
								Control.CheckForIllegalCrossThreadCalls = true;
								if (text.ToLower().Contains("lwjgl"))
								{
									Control.CheckForIllegalCrossThreadCalls = false;
									this.sw.Hide();
									Control.CheckForIllegalCrossThreadCalls = true;
								}
							}
						}
						num2 = process.ExitCode;
						flag = !flag;
						num7++;
					}
				}
				if (num2 != 0)
				{
					MessageBox.Show("Java hat einen Fehler festgestellt. Fehlercode: " + num2, "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
			catch (JavaNotFoundException)
			{
				MessageBox.Show("Auf diesem Computer ist kein Java installiert. Bitte nachinstallieren!", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
			catch (Exception ex)
			{
				MessageBox.Show("Java konnte nicht gestartet werden: " + ex.Message, "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
		private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			base.Close();
		}
		private void txtUser_Enter(object sender, EventArgs e)
		{
			this.button1.NotifyDefault(true);
		}
		private void txtUser_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == '\r')
			{
				this.txtUser.Text = this.txtUser.Text.Trim();
				this.button1.PerformClick();
			}
		}

        private void itmJarSwitch_Click(object sender, EventArgs e)
        {
            //jarSwitch.Scan();
            jarSwitch.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            contextMenuStrip1.Show(this.Left + ((Control)sender).Left, this.Top + ((Control)sender).Top);
        }

	}
}
