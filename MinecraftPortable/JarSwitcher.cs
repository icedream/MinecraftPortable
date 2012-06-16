using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Ionic.Zip;

namespace MinecraftPortable
{
    public partial class JarSwitcher : Form
    {
        List<string> jarList = new List<string>();

        public JarSwitcher()
        {
            InitializeComponent();
        }

        public string SelectedJar
        {
            get
            {
                for(int i = 0; i < jarSwitchPanel.Controls.Count; i++)
                    if(((RadioButton)jarSwitchPanel.Controls[i]).Checked)
                        return jarList[i];
                return null;
            }
        }

        public void Scan()
        {
            Clear();

            foreach (string f in Directory.GetFiles(Path.Combine(".minecraft", "bin")))
            {
                FileInfo fi = new FileInfo(f);
                if(fi.Extension.ToLower() == ".jar")
                    this.AddMinecraftJar(f);
            }
        }

        public void Clear()
        {
            foreach (Control c in this.jarSwitchPanel.Controls)
                c.Dispose();
            jarSwitchPanel.Controls.Clear();
            jarList.Clear();
        }

        public bool RemoveMinecraftJar(int index)
        {
            if (index < 0 || index >= jarList.Count)
                return false;

            jarList.RemoveAt(index);
            jarSwitchPanel.Controls[index].Dispose();
            jarSwitchPanel.Controls.RemoveAt(index);
            return true;
        }

        public bool RemoveMinecraftJar(string file)
        {
            int index = jarList.IndexOf(file);
            return RemoveMinecraftJar(index);
        }

        bool _GTIA()
        {
            return false;
        }

        public bool AddMinecraftJar(string file)
        {
            try
            {
                // Is it a ZIP (JAR) file?
                if (!ZipFile.IsZipFile(file))
                    return false;

                // Is it a consistent ZIP (JAR) file?
                if (!ZipFile.CheckZip(file))
                {
                    try
                    {
                        ZipFile.FixZipDirectory(file);
                    }
                    catch
                    {
                        return false;
                    }
                    if (!ZipFile.CheckZip(file))
                        return false;
                }
            }
            catch // zip errors are the most to occur here
            {
                return false;
            }

            // Does it have minecraft classes?
            ZipFile zf = new ZipFile(file);
            if (!zf.EntryFileNames.Contains("net/minecraft/client/Minecraft.class"))
                return false;


            List<string> modsList = new List<string>();

            // Minecraft Forge check
            if (zf.EntryFileNames.Contains("mod_MinecraftForge.class") || zf.EntryFileNames.Contains("forge/"))
                modsList.Add("Forge");

            // ModLoader check
            if(zf.EntryFileNames.Contains("ModLoader.class"))
                if(zf.EntryFileNames.Contains("fmlversion.properties"))
                    modsList.Add("Forge ModLoader");
                else
                    modsList.Add("ModLoader");

            // OptiFine check
            if(zf.EntryFileNames.Contains("GuiDetailSettingsOF.class"))
                modsList.Add("OptiFine");

            // TODO: ModLoader check

            // Convert to a static list
            string[] mods = modsList.ToArray();
            modsList = null;

            // Version string
            string VersionString = file + " ";
            VersionString += Environment.NewLine; // TODO: Minecraft version check
            VersionString += mods.Length > 0 ? "(modded with " + string.Join(", ", mods) + ")" : "";

            // Create a control to give the user the option to use that jar!
            RadioButton ctl = new RadioButton();
            ctl.Location = new Point(3,3);
            ctl.AutoSize = true;
            ctl.Text = VersionString;
            ctl.Name = "minecraft_" + jarSwitchPanel.Controls.Count;
            ctl.Checked = (ctl.TabIndex = jarSwitchPanel.Controls.Count) == 0;
            this.jarSwitchPanel.Controls.Add(ctl);
            this.jarSwitchPanel.SetFlowBreak(ctl, true);
            ctl.Click += new EventHandler(ctl_Click);
            // nice picture for minecraft.jar :D
            List<ZipEntry> zl = zf.SelectEntries("pack.png") as List<ZipEntry>;
            if (zl.Count > 0) 
            {
                Image x;

                // Extract image into memory
                using (MemoryStream ms = new MemoryStream())
                {
                    zl[0].Extract(ms);
                    ms.Seek(0, SeekOrigin.Begin);
                    x = Image.FromStream(ms);
                }

                // Resize
                float factor = 32f / x.Height;
                ctl.Image = x.GetThumbnailImage(
                        (int)((float)x.Width * factor), // width
                        (int)((float)x.Height * factor), // height
                        new Image.GetThumbnailImageAbort(_GTIA), // unneeded abort callback -_-
                        IntPtr.Zero // uhm...
                        );
                ctl.TextImageRelation = TextImageRelation.ImageBeforeText;
            }
            jarList.Add(file);

            return true;
        }

        void ctl_Click(object sender, EventArgs e)
        {
            Control me = ((Control)sender);
            foreach (Control c in jarSwitchPanel.Controls)
                if (c != me)
                    ((RadioButton)c).Checked = false;
        }

        private void JarSwitcher_Load(object sender, EventArgs e)
        {
            Scan();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
    }
}
