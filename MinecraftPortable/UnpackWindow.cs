using Ionic.Zip;
using MinecraftPortable.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace MinecraftPortable
{
	public class UnpackWindow : Form
	{
		private delegate void T(string t);
		private delegate void I(int i);
		public string downloadServer = "https://s3.amazonaws.com/MinecraftDownload";
		private WebClient wc = new WebClient();
		private string[] units = new string[]
		{
			"B",
			"kB",
			"MB",
			"GB",
			"TB",
			"EB",
			"PB",
			"ZB"
		};
        private Label label1;
		private ProgressBar progressBar1;
		private BackgroundWorker backgroundWorker1;
        private PictureBox pictureBox1;
		private Label label2;
		public void SetText(string t)
		{
			if (base.InvokeRequired)
			{
				base.Invoke(new UnpackWindow.T(this.SetText), new object[]
				{
					t
				});
				return;
			}
			this.label2.Text = t;
		}
		public void SetPercent(int i)
		{
			if (base.InvokeRequired)
			{
				base.Invoke(new UnpackWindow.I(this.SetPercent), new object[]
				{
					i
				});
				return;
			}
			this.progressBar1.Value = i;
		}
		public UnpackWindow()
		{
			this.InitializeComponent();
		}
		private void DownloadFile(string src, string targ)
		{
            FileInfo fi = new FileInfo(targ);
			this.SetText("Download: " + fi.Name);
            if (fi.Exists)
            {
                this.wc.Headers.Add(HttpRequestHeader.IfNoneMatch, _generateFileMD5(targ));
            }
			this.wc.DownloadFileAsync(new Uri(src), targ + ".dl");
			while (this.wc.IsBusy)
			{
				Thread.Sleep(100);
			}
            if (this.wc.ResponseHeaders != null)
            {
                File.Delete(targ);
                File.Move(targ + ".dl", targ);
            }
            else
                File.Delete(targ + ".dl");
            this.wc.Headers.Clear();
		}

        private string _generateFileMD5(string path)
        {
            byte[] md5Hash = { };

            using (FileStream FileCheck = File.OpenRead(path))
            {
                MD5 md5 = new MD5CryptoServiceProvider();
                md5Hash = md5.ComputeHash(FileCheck);
            }

            return BitConverter.ToString(md5Hash).Replace("-", "").ToLower();
        }

		private void _UnpackWorker(object sender, DoWorkEventArgs e)
		{
			Program.MinecraftPath = Path.Combine(Environment.CurrentDirectory, ".minecraft");
			Program.BinPath = Path.Combine(Program.MinecraftPath, "bin");
			Program.NativesPath = Path.Combine(Program.BinPath, "natives");
			Directory.CreateDirectory(Program.NativesPath);
			bool flag = true;
			if (!File.Exists("minecraft.zip"))
			{
				using (Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("MinecraftPortable.minecraft.zip"))
				{
					if (manifestResourceStream == null)
					{
						this.SetText("Bereite Download vor...");
						this.wc.DownloadProgressChanged += new DownloadProgressChangedEventHandler(this.wc_DownloadProgressChanged);
						this.wc.DownloadString(this.downloadServer);
						this.DownloadFile(this.downloadServer + "/jinput.jar", Path.Combine(Program.BinPath, "jinput.jar"));
						this.DownloadFile(this.downloadServer + "/lwjgl_util.jar", Path.Combine(Program.BinPath, "lwjgl_util.jar"));
						this.DownloadFile(this.downloadServer + "/lwjgl.jar", Path.Combine(Program.BinPath, "lwjgl.jar"));
						this.DownloadFile(this.downloadServer + "/minecraft.jar", Path.Combine(Program.BinPath, "minecraft.jar"));
						this.DownloadFile(this.downloadServer + "/windows_natives.jar", Path.Combine(Program.BinPath, "windows_natives.jar"));
						this.SetText("Entpacke: windows_natives.jar...");
						using (ZipFile zipFile = new ZipFile(Path.Combine(Program.BinPath, "windows_natives.jar")))
						{
							zipFile.ExtractAll(Program.NativesPath, ExtractExistingFileAction.OverwriteSilently);
						}
						flag = false;
						return;
					}
					if (flag)
					{
						this.SetText("Entpacke Archiv...");
						using (FileStream fileStream = File.Create("minecraft.zip"))
						{
							byte[] array = new byte[4096];
							int count;
							while ((count = manifestResourceStream.Read(array, 0, array.Length)) > 0)
							{
								fileStream.Write(array, 0, count);
								fileStream.Flush();
								this.SetPercent((int)(100L * manifestResourceStream.Position / manifestResourceStream.Length));
							}
							fileStream.Close();
						}
					}
					manifestResourceStream.Close();
				}
			}
			if (flag)
			{
				this.SetPercent(0);
				this.SetText("Entpacke Dateien...");
				using (ZipFile zipFile2 = new ZipFile("minecraft.zip"))
				{
					zipFile2.ExtractProgress += new EventHandler<ExtractProgressEventArgs>(this.zf_ExtractProgress);
					zipFile2.ExtractAll(".", ExtractExistingFileAction.InvokeExtractProgressEvent);
				}
			}
		}
		private void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
		{
			this.SetPercent(e.ProgressPercentage);
		}
		private void zf_ExtractProgress(object sender, ExtractProgressEventArgs e)
		{
			switch (e.EventType)
			{
			case ZipProgressEventType.Extracting_BeforeExtractEntry:
				break;
			case ZipProgressEventType.Extracting_AfterExtractEntry:
				this.SetPercent(100 * e.EntriesExtracted / e.EntriesTotal);
				break;
			case ZipProgressEventType.Extracting_ExtractEntryWouldOverwrite:
				e.CurrentEntry.ExtractExistingFile = ((e.CurrentEntry.ModifiedTime > new FileInfo(e.CurrentEntry.FileName).LastWriteTime) ? ExtractExistingFileAction.OverwriteSilently : ExtractExistingFileAction.DoNotOverwrite);
				return;
			default:
				return;
			}
		}
		private string GetSize(double val)
		{
			int num = 0;
			while (val >= 1024.0)
			{
				val /= 1024.0;
				num++;
			}
			return Math.Round(val, 2).ToString() + " " + this.units[num];
		}
		private void UnpackWindow_Load(object sender, EventArgs e)
		{
			this.backgroundWorker1.RunWorkerAsync();
		}
		private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			base.Close();
		}
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UnpackWindow));
            this.label1 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // progressBar1
            // 
            resources.ApplyResources(this.progressBar1, "progressBar1");
            this.progressBar1.Name = "progressBar1";
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this._UnpackWorker);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.AutoEllipsis = true;
            this.label2.Name = "label2";
            this.label2.UseMnemonic = false;
            // 
            // pictureBox1
            // 
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.Image = global::MinecraftPortable.Properties.Resources.tnt1;
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            // 
            // UnpackWindow
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ControlBox = false;
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "UnpackWindow";
            this.Load += new System.EventHandler(this.UnpackWindow_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
	}
}
