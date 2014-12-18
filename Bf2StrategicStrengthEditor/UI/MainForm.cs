using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using Ionic.Zip;
using System.Diagnostics;
using System.Reflection;
using Battlefield2.Properties;

namespace Battlefield2
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// The current selected Tree Node
        /// </summary>
        protected TreeNode SelectedNode;

        /// <summary>
        /// The ObjectInfo.xml file object
        /// </summary>
        protected XmlDocument ObjectsXml;

        /// <summary>
        /// The selected objects XML node if it exists in the ObjectsXml
        /// </summary>
        protected XmlNode ItemXmlNode;

        /// <summary>
        /// Constructor
        /// </summary>
        public MainForm()
        {
            // Create form controls
            InitializeComponent();

            // Update Settings if we need to
            if (!Settings.Default.SettingsUpdated)
            {
                Settings.Default.Upgrade();
                Settings.Default.SettingsUpdated = true;
                Settings.Default.Save();
            }

            // Load the ObjectsInfo.xml
            ObjectsXml = new XmlDocument();
            string path = Path.Combine(Application.StartupPath, "ObjectInfo.xml");
            if (File.Exists(path))
            {
                ObjectsXml.Load(path);
            }
            else
            {
                // Load the embeded default version
                Assembly assembly = Assembly.GetExecutingAssembly();
                using (Stream stream = assembly.GetManifestResourceStream("Battlefield2.ObjectInfo.xml"))
                using (StreamReader reader = new StreamReader(stream))
                {
                    // Create the file
                    string result = reader.ReadToEnd();
                    File.WriteAllText(path, result);
                    Thread.Sleep(100);
                    ObjectsXml.Load(path);
                }
            }
        }

        #region Selection Events

        /// <summary>
        /// Tree Node event that is fired when a node is selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode Node = ((TreeView)sender).SelectedNode;

            // Make sure its a child node
            if (Node == null || Node.Nodes.Count != 0)
                return;

            // Get node path
            if (ObjectsXml != null)
            {
                try
                {
                    ItemXmlNode = ObjectsXml.SelectSingleNode("//object[@name='" + Node.Text.ToLowerInvariant() + "']");
                }
                catch (Exception)
                {
                    ItemXmlNode = null;
                }
            }


            // The list of AiFiles for this object are stored in the node tag
            SelectedNode = Node;
            Dictionary<AiFileType, AiFile> files = (Dictionary<AiFileType, AiFile>)Node.Tag;

            // Disable the Tab Control for now
            tabControl1.Enabled = false;
            tabControl1.SuspendLayout();

            // Load Kit data if we have it
            if (files.ContainsKey(AiFileType.Kit))
            {
                // Enable the second tab page
                if (!tabControl1.Controls.Contains(tabPage3))
                    tabControl1.TabPages.Insert(0, tabPage3);

                // Clear old object select and add new items
                KitSelect.Items.Clear();
                foreach (ObjectTemplate b in files[AiFileType.Kit].Objects.Values)
                {
                    KitSelect.Items.Add(b.Name);
                }

                // Set index to load
                KitSelect.SelectedIndex = 0;
                tabControl1.SelectedTab = tabPage3;
            }
            else
            {
                // Remove tab
                if (tabControl1.Controls.Contains(tabPage3))
                    tabControl1.Controls.Remove(tabPage3);
            }

            // Load weapon data too if we have it
            if (files.ContainsKey(AiFileType.Weapon))
            {
                // Enable the second tab page
                if (!tabControl1.Controls.Contains(tabPage2))
                    tabControl1.Controls.Add(tabPage2);

                // Clear old object select and add new items
                WeaponSelect.Items.Clear();
                foreach (ObjectTemplate b in files[AiFileType.Weapon].Objects.Values)
                {
                    WeaponSelect.Items.Add(b.Name);
                }

                // Set index to load
                WeaponSelect.SelectedIndex = 0;
                tabControl1.SelectedTab = tabPage2;
            }
            else
            {
                // Remove tab
                if (tabControl1.Controls.Contains(tabPage2))
                    tabControl1.Controls.Remove(tabPage2);
            }

            // Load Vehicle data if this is a vehicle
            if (files.ContainsKey(AiFileType.Vehicle))
            {
                // Enable the first tab page, use insert to set it to be first
                if (!tabControl1.Controls.Contains(tabPage1))
                    tabControl1.TabPages.Insert(0, tabPage1);

                // Clear old object select and add new items
                ObjectSelect.Items.Clear();
                foreach (ObjectTemplate b in files[AiFileType.Vehicle].Objects.Values.Where(x => x.TemplateType == TemplateType.AiTemplate))
                {
                    ObjectSelect.Items.Add(b.Name);
                }

                // Set index to load
                ObjectSelect.SelectedIndex = 0;
                tabControl1.SelectedTab = tabPage1;

                // Set vehicle data
                ObjectTemplate [] y = (
                    from x in files[AiFileType.Vehicle].Objects.Values 
                    where x.ObjectType == AiTemplatePluginType.Physical.ToString()
                    select x
                ).ToArray();

                StrTypeLabel.Text = (y.Length == 0) ? "None" : y[0].Properties["setStrType"][0].Values[0];
            }
            else
            {
                tabControl1.Controls.Remove(tabPage1);
            }

            // Re-enable the Tab Control for now
            tabControl1.ResumeLayout();
            tabControl1.Enabled = true;
        }

        private void ObjectSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Make sure its a child node
            if (SelectedNode == null || SelectedNode.Nodes.Count != 0)
                return;

            // The list of AiFiles for this object are stored in the node tag
            Dictionary<AiFileType, AiFile> files = (Dictionary<AiFileType, AiFile>)SelectedNode.Tag;
            AiTemplate template = (AiTemplate)files[AiFileType.Vehicle].Objects[ObjectSelect.SelectedItem.ToString()];

            // Vehicle strat. strengths are stored in the Unit plugin, so we load that too
            ObjectTemplate plugin = template.Plugins[AiTemplatePluginType.Unit];

            // [resultNumber( 0 => offense, 1 => defense )][Argument #]
            OffStartStrength.Value = (int)Double.Parse(plugin.Properties["setStrategicStrength"][0].Values[1]);
            DefStratStrength.Value = (int)Double.Parse(plugin.Properties["setStrategicStrength"][1].Values[1]);

            // [entry Name][line #][Argument #]
            ObjectBasicTemp.Value = (int)Int32.Parse(template.Properties["basicTemp"][0].Values[0]);
            Degeneration.Value = (int)Double.Parse(template.Properties["degeneration"][0].Values[0]);
            AllowedTimeDiff.Value = Decimal.Parse(template.Properties["allowedTimeDiff"][0].Values[0]);
        }

        private void WeaponSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Make sure its a child node
            if (SelectedNode == null || SelectedNode.Nodes.Count != 0)
                return;

            // The list of AiFiles for this object are stored in the node tag
            Dictionary<AiFileType, AiFile> files = (Dictionary<AiFileType, AiFile>)SelectedNode.Tag;
            ObjectTemplate template = files[AiFileType.Weapon].Objects[WeaponSelect.SelectedItem.ToString()];

            // Set form data
            MinRange.Value = Decimal.Parse(template.Properties["minRange"][0].Values[0]);
            MaxRange.Value = Decimal.Parse(template.Properties["maxRange"][0].Values[0]);
            WpnInfantryStrength.Value = Decimal.Parse(template.Properties["setStrength"][0].Values[1]);
            WpnLightStrength.Value = Decimal.Parse(template.Properties["setStrength"][1].Values[1]);
            WpnHeavyStrength.Value = Decimal.Parse(template.Properties["setStrength"][2].Values[1]);
            WpnNavalStrength.Value = Decimal.Parse(template.Properties["setStrength"][3].Values[1]);
            WpnHeliStrength.Value = Decimal.Parse(template.Properties["setStrength"][4].Values[1]);
            WpnAirPlaneStrength.Value = Decimal.Parse(template.Properties["setStrength"][5].Values[1]);
        }

        private void KitSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Make sure its a child node
            if (SelectedNode == null || SelectedNode.Nodes.Count != 0)
                return;

            // The list of AiFiles for this object are stored in the node tag
            Dictionary<AiFileType, AiFile> files = (Dictionary<AiFileType, AiFile>)SelectedNode.Tag;
            ObjectTemplate template = files[AiFileType.Kit].Objects[KitSelect.SelectedItem.ToString()];

            // Set form data
            KitOffStrength.Value = (int)Double.Parse(template.Properties["setStrategicStrength"][0].Values[1]);
            KitDefStrength.Value = (int)Double.Parse(template.Properties["setStrategicStrength"][1].Values[1]);
            KitInfantryStrength.Value = Decimal.Parse(template.Properties["setBattleStrength"][0].Values[1]);
            KitLightStrength.Value = Decimal.Parse(template.Properties["setBattleStrength"][1].Values[1]);
            KitHeavyStrength.Value = Decimal.Parse(template.Properties["setBattleStrength"][2].Values[1]);
            KitNavalStrength.Value = Decimal.Parse(template.Properties["setBattleStrength"][3].Values[1]);
            KitHeliStrength.Value = Decimal.Parse(template.Properties["setBattleStrength"][4].Values[1]);
            KitAirStrength.Value = Decimal.Parse(template.Properties["setBattleStrength"][5].Values[1]);
        }

        #endregion Selection Events

        #region Helper Methods

        /// <summary>
        /// Loads all of the *.ai files located in the SubDir name, into
        /// the ObjectManager
        /// </summary>
        /// <param name="Name"></param>
        protected void LoadTemplates(string Name)
        {
            string path = Path.Combine(Application.StartupPath, "Temp", Name);
            TreeNode topNode = new TreeNode(Name);

            // Make sure our tempalte directory exists
            if (!Directory.Exists(path))
                return;

            // Now loop through each object type
            foreach (string dir in Directory.EnumerateDirectories(path))
            {
                string dirName = dir.Remove(0, path.Length + 1);

                // Skip common folder
                if (dirName.ToLowerInvariant() == "common")
                    continue;

                TreeNode dirNode = new TreeNode(dirName);
                foreach (string subdir in Directory.EnumerateDirectories(dir))
                {
                    string subdirName = subdir.Remove(0, dir.Length + 1);

                    // Skip common folder
                    if (subdirName.ToLowerInvariant() == "common")
                        continue;

                    // Skip dirs that dont have an ai folder
                    if (!Directory.Exists(Path.Combine(subdir, "ai")))
                        continue;

                    TreeNode subNode = new TreeNode(subdirName);
                    Dictionary<AiFileType, AiFile> files = new Dictionary<AiFileType, AiFile>();

                    // Load the Objects.ai file if we have one
                    if (File.Exists(Path.Combine(subdir, "ai", "Objects.ai")))
                    {
                        TaskForm.UpdateStatus("Loading: " + Path.Combine(subdirName, "ai", "Object.ai"));
                        AiFile file = new AiFile(Path.Combine(subdir, "ai", "Objects.ai"), AiFileType.Vehicle);
                        if(ObjectManager.RegisterFileObjects(file))
                            files.Add(AiFileType.Vehicle, file);
                    }

                    // Load the Weapons.ai file if we have one
                    if (File.Exists(Path.Combine(subdir, "ai", "Weapons.ai")))
                    {
                        TaskForm.UpdateStatus("Loading: " + Path.Combine(subdirName, "ai", "Weapons.ai"));
                        AiFile file = new AiFile(Path.Combine(subdir, "ai", "Weapons.ai"), AiFileType.Weapon);
                        if(ObjectManager.RegisterFileObjects(file))
                            files.Add(AiFileType.Weapon, file);
                    }

                    if (files.Count > 0)
                    {
                        subNode.Tag = files;
                        dirNode.Nodes.Add(subNode);
                    }
                }

                if(dirNode.Nodes.Count > 0)
                    topNode.Nodes.Add(dirNode);

            }

            if (topNode.Nodes.Count > 0)
                treeView1.Nodes.Add(topNode);
        }

        /// <summary>
        /// Loads the KIT object template 
        /// </summary>
        protected void LoadKitTemplate()
        {
            string path = Path.Combine(Application.StartupPath, "Temp", "Kits", "ai", "Objects.ai");

            // Xpack doesnt have a kits folder!
            if (File.Exists(path))
            {
                TreeNode topNode = new TreeNode("Kits");
                Dictionary<AiFileType, AiFile> files = new Dictionary<AiFileType, AiFile>();
                AiFile file = new AiFile(path, AiFileType.Kit);
                ObjectManager.RegisterFileObjects(file);
                files.Add(AiFileType.Kit, file);
                topNode.Tag = files;
                treeView1.Nodes.Add(topNode);
            }
        }

        /// <summary>
        /// Takes a file path, and shortens its size to the specifed maxLength,
        /// while still maintaining readabilty
        /// </summary>
        /// <param name="path">the path we are shortening</param>
        /// <param name="maxLength">the max allowed characters to show (can be less depending on the string structure)</param>
        /// <returns></returns>
        public string ShrinkPath(string path, int maxLength)
        {
            // If the path isnt even long enough to reach the max length, just return it
            if (path.Length <= maxLength)
                return path;

            // Break path into parts, using the directory sperator as our delmiter
            List<string> parts = new List<string>(path.Replace('/', '\\').Split('\\'));

            // We always keep the top 2 directory parts, so add them to a new variable, then remove them from parts
            string start = parts[0] + @"\" + parts[1];
            parts.RemoveAt(1);
            parts.RemoveAt(0);

            // We always keep the last part, or filename, so store that as well and remove it from parts
            string end = parts[parts.Count - 1];
            parts.RemoveAt(parts.Count - 1);

            // Now we build the middle of the path using the remaining parts, adding each until we hit a limit of going over the max length
            parts.Insert(0, @"\...");
            while (parts.Count > 1 && (start.Length + end.Length + parts.Sum(p => p.Length) + parts.Count > maxLength))
                parts.RemoveAt(parts.Count - 1);

            // add the middle from the parts we just built
            string mid = "";
            parts.ForEach(p => mid += p + @"\");

            // combine everything and return it
            return start + mid + end;
        }

        /// <summary>
        /// Loads every .ai file and parses it for use within this form
        /// </summary>
        protected void ParseTemplates()
        {
            // Clear nodesand Globals!
            treeView1.Nodes.Clear();
            ObjectManager.ReleaseAll();

            // OPen task form if it isnt open
            if (!TaskForm.IsOpen)
                TaskForm.Show(this, "Parsing AI FIles", "Parsing AI Files", false);

            // Load kit Templates
            TaskForm.UpdateStatus("Loading Kits, Please Wait...");
            LoadKitTemplate();

            // Load Vehicle templates
            TaskForm.UpdateStatus("Loading Vehicles, Please Wait...");
            LoadTemplates("Vehicles");

            // Load Weapon templates
            TaskForm.UpdateInstructionText("Lodding Weapons, Please Wait...");
            LoadTemplates("Weapons");

            TaskForm.CloseForm();

            // Set desc
            DescLabel.Text = "Loaded Archive: " + ShrinkPath(Settings.Default.LastUsedZip, 95);

            // Enable action buttons
            SaveBtn.Enabled = true;
            RestoreBtn.Enabled = File.Exists(Settings.Default.LastUsedZip + ".original");
            RefreshBtn.Enabled = true;
        }

        /// <summary>
        /// Clears all form data
        /// </summary>
        protected void ClearTabValues()
        {
            foreach(TabPage page in tabControl1.TabPages)
            {
                foreach (Control C in page.Controls)
                {
                    if (C is NumericUpDown)
                    {
                        ((NumericUpDown)C).Value = 0;
                    }
                    else if (C is ComboBox)
                    {
                        ComboBox cmb = (ComboBox)C;
                        cmb.Items.Clear();
                        if (cmb.DataSource != null)
                        {
                            cmb.SelectedItem = string.Empty;
                            cmb.SelectedValue = 0;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Itterates through all of the Tree nodes, grabs the attached file tags,
        /// and saves each ai file
        /// </summary>
        /// <param name="Node"></param>
        public void SaveAiFiles(TreeNode Node)
        {
            if (Node.Nodes.Count > 0)
            {
                foreach (TreeNode subNode in Node.Nodes)
                {
                    SaveAiFiles(subNode);
                }
            }
            else
            {
                Dictionary<AiFileType, AiFile> files = (Dictionary<AiFileType, AiFile>)Node.Tag;
                foreach (KeyValuePair<AiFileType, AiFile> file in files)
                    file.Value.Save();
            }
        }

        #endregion Helper Methods

        #region Button Click Events

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            string serverZip = Settings.Default.LastUsedZip;
            string appPath = Path.Combine(Application.StartupPath, "Temp");

            // Show task form
            TaskForm.Show(this, "Saving Archive", "Saving Changes to Archive", false);

            // Get our AI files
            TaskForm.UpdateStatus("Fetching AI Files...");
            IEnumerable<string> files = (
                from x in Directory.GetFiles(appPath, "*.ai", SearchOption.AllDirectories) 
                let dir = x.Remove(0, appPath.Length + 1) 
                select dir
            );

            // Save all AI files
            TaskForm.UpdateStatus("Saving AI file changes...");
            foreach (TreeNode N in treeView1.Nodes)
                SaveAiFiles(N);

            // Create Backup!
            if (!File.Exists(serverZip + ".original"))
            {
                TaskForm.UpdateStatus("Creating Backup of server Archive...");
                File.Copy(serverZip, serverZip + ".original");
                RestoreBtn.Enabled = true;
            }

            // Remove readonly from zip
            FileInfo zipFile = new FileInfo(serverZip);
            zipFile.Attributes = FileAttributes.Normal;

            // Save files to zip
            using (ZipFile file = new ZipFile(serverZip))
            {
                foreach (string pth in files)
                {
                    TaskForm.UpdateStatus("Updating Entries...");

                    // Remove old entry
                    file.RemoveEntry(pth);

                    // Set file attributes to read only on the file, and add it to the zip
                    ZipEntry t = file.AddEntry(pth, File.OpenRead(Path.Combine(appPath, pth)));
                    t.Attributes = FileAttributes.ReadOnly;
                }

                file.Save();
            }

            zipFile.Attributes = FileAttributes.ReadOnly;
            TaskForm.CloseForm();
        }

        /// <summary>
        /// Event fired when the "Load Archive" button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewProfileBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog Dialog = new OpenFileDialog();
            Dialog.FileName = "Objects_Server.zip";
            Dialog.Filter = "Zip File|*.zip";

            // Set the initial search directory if we found an install path via registry
            if (!String.IsNullOrWhiteSpace(Settings.Default.LastUsedPath))
                Dialog.InitialDirectory = Settings.Default.LastUsedPath;

            // Show Dialog
            if (Dialog.ShowDialog() == DialogResult.OK)
            {
                // Save our last used directory
                string file = Dialog.FileName;
                Settings.Default.LastUsedPath = Path.GetDirectoryName(file);
                Settings.Default.LastUsedZip = file;
                Settings.Default.Save();

                // Show our task form
                TaskForm.Show(this, "Loading Archive", "Extracting Archive", false);
                TaskForm.UpdateStatus("Removing old temp files...");

                // Clear old temp data
                string targetDirectory = Path.Combine(Application.StartupPath, "Temp");
                if (Directory.Exists(targetDirectory))
                {
                    Directory.Delete(targetDirectory, true);
                    Thread.Sleep(250);
                }

                // Load zip
                TaskForm.UpdateStatus("Extracting Zip Contents...");
                using (ZipFile Zip = ZipFile.Read(file))
                {
                    // Extract just the files we need
                    ZipEntry[] Entries = Zip.Entries.Where(
                        x => (
                            x.FileName.StartsWith("Vehicles", StringComparison.InvariantCultureIgnoreCase) ||
                            x.FileName.StartsWith("Weapons", StringComparison.InvariantCultureIgnoreCase) ||
                            x.FileName.StartsWith("Kits", StringComparison.InvariantCultureIgnoreCase)
                        ) && x.FileName.EndsWith(".ai", StringComparison.InvariantCultureIgnoreCase)
                    ).ToArray();

                    // Extract entries
                    foreach (ZipEntry entry in Entries)
                    {
                        // Extract the entry
                        entry.Extract(targetDirectory, ExtractExistingFileAction.Throw);

                        // Get rid of read only attributes!
                        FileInfo F = new FileInfo(Path.Combine(targetDirectory, entry.FileName));
                        F.Attributes = FileAttributes.Normal;
                    }
                }

                // Prase the extracted ai files
                ParseTemplates();
            }
        }

        private void RestoreBtn_Click(object sender, EventArgs e)
        {
            string serverZip = Settings.Default.LastUsedZip;
            string backupZip = serverZip + ".original";

            // Make sure we have a backup to go off of
            if (!File.Exists(backupZip))
            {
                MessageBox.Show(
                    "Unable to located a backup zip file for this Server Archive.", 
                    "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information
                );
                return;
            }

            // Delete server zip (have to remove readonly first
            FileInfo zip = new FileInfo(serverZip);
            zip.Attributes = FileAttributes.Normal;
            zip.Delete();
            
            // Restore original
            File.Move(backupZip, serverZip);
            RestoreBtn.Enabled = false;

            // Alert User
            MessageBox.Show(
                String.Format("Successfully restored the \"{0}\" back to its original contents", Path.GetFileName(serverZip)),
                "Success", MessageBoxButtons.OK, MessageBoxIcon.Information
            );
        }

        private void RefreshBtn_Click(object sender, EventArgs e)
        {
            TaskForm.Show(this, "Parsing AI FIles", "Parsing AI Files", false);
            tabControl1.Enabled = false;
            ClearTabValues();
            ParseTemplates();
            SelectedNode = null;
        }

        #endregion Button Click Events

        #region Object Data Leave Events

        private void OffStartStrength_Leave(object sender, EventArgs e)
        {
            // The list of AiFiles for this object are stored in the node tag
            Dictionary<AiFileType, AiFile> files = (Dictionary<AiFileType, AiFile>)SelectedNode.Tag;
            AiTemplate template = (AiTemplate)files[AiFileType.Vehicle].Objects[ObjectSelect.SelectedItem.ToString()];

            // Vehicle strat. strengths are stored in the Unit plugin, so we load that too
            ObjectTemplate plugin = template.Plugins[AiTemplatePluginType.Unit];
            plugin.Properties["setStrategicStrength"][0].Values[1] = OffStartStrength.Value.ToString();
        }

        private void DefStratStrength_Leave(object sender, EventArgs e)
        {
            // The list of AiFiles for this object are stored in the node tag
            Dictionary<AiFileType, AiFile> files = (Dictionary<AiFileType, AiFile>)SelectedNode.Tag;
            AiTemplate template = (AiTemplate)files[AiFileType.Vehicle].Objects[ObjectSelect.SelectedItem.ToString()];

            // Vehicle strat. strengths are stored in the Unit plugin, so we load that too
            ObjectTemplate plugin = template.Plugins[AiTemplatePluginType.Unit];
            plugin.Properties["setStrategicStrength"][1].Values[1] = OffStartStrength.Value.ToString();
        }

        private void ObjectBasicTemp_Leave(object sender, EventArgs e)
        {
            // The list of AiFiles for this object are stored in the node tag
            Dictionary<AiFileType, AiFile> files = (Dictionary<AiFileType, AiFile>)SelectedNode.Tag;
            AiTemplate template = (AiTemplate)files[AiFileType.Vehicle].Objects[ObjectSelect.SelectedItem.ToString()];
            template.Properties["basicTemp"][0].Values[0] = ((int)ObjectBasicTemp.Value).ToString();
        }

        private void Degeneration_Leave(object sender, EventArgs e)
        {
            // The list of AiFiles for this object are stored in the node tag
            Dictionary<AiFileType, AiFile> files = (Dictionary<AiFileType, AiFile>)SelectedNode.Tag;
            AiTemplate template = (AiTemplate)files[AiFileType.Vehicle].Objects[ObjectSelect.SelectedItem.ToString()];
            template.Properties["degeneration"][0].Values[0] = ((int)Degeneration.Value).ToString();
        }

        private void AllowedTimeDiff_Leave(object sender, EventArgs e)
        {
            // The list of AiFiles for this object are stored in the node tag
            Dictionary<AiFileType, AiFile> files = (Dictionary<AiFileType, AiFile>)SelectedNode.Tag;
            AiTemplate template = (AiTemplate)files[AiFileType.Vehicle].Objects[ObjectSelect.SelectedItem.ToString()];
            template.Properties["allowedTimeDiff"][0].Values[0] = ((double)AllowedTimeDiff.Value).ToString();
        }
        #endregion Object Data Leave Events

        #region Weapon Data Leave Events

        private void MinRange_Leave(object sender, EventArgs e)
        {
            // The list of AiFiles for this object are stored in the node tag
            Dictionary<AiFileType, AiFile> files = (Dictionary<AiFileType, AiFile>)SelectedNode.Tag;
            ObjectTemplate template = files[AiFileType.Weapon].Objects[WeaponSelect.SelectedItem.ToString()];
            template.Properties["minRange"][0].Values[0] = MinRange.Value.ToString();
        }

        private void MaxRange_Leave(object sender, EventArgs e)
        {
            // The list of AiFiles for this object are stored in the node tag
            Dictionary<AiFileType, AiFile> files = (Dictionary<AiFileType, AiFile>)SelectedNode.Tag;
            ObjectTemplate template = files[AiFileType.Weapon].Objects[WeaponSelect.SelectedItem.ToString()];
            template.Properties["maxRange"][0].Values[0] = MaxRange.Value.ToString();
        }

        private void WpnInfantryStrength_Leave(object sender, EventArgs e)
        {
            // The list of AiFiles for this object are stored in the node tag
            Dictionary<AiFileType, AiFile> files = (Dictionary<AiFileType, AiFile>)SelectedNode.Tag;
            ObjectTemplate template = files[AiFileType.Weapon].Objects[WeaponSelect.SelectedItem.ToString()];
            template.Properties["setStrength"][0].Values[1] = WpnInfantryStrength.Value.ToString();
        }

        private void WpnLightStrength_Leave(object sender, EventArgs e)
        {
            // The list of AiFiles for this object are stored in the node tag
            Dictionary<AiFileType, AiFile> files = (Dictionary<AiFileType, AiFile>)SelectedNode.Tag;
            ObjectTemplate template = files[AiFileType.Weapon].Objects[WeaponSelect.SelectedItem.ToString()];
            template.Properties["setStrength"][1].Values[1] = WpnLightStrength.Value.ToString();
        }

        private void WpnHeavyStrength_Leave(object sender, EventArgs e)
        {
            // The list of AiFiles for this object are stored in the node tag
            Dictionary<AiFileType, AiFile> files = (Dictionary<AiFileType, AiFile>)SelectedNode.Tag;
            ObjectTemplate template = files[AiFileType.Weapon].Objects[WeaponSelect.SelectedItem.ToString()];
            template.Properties["setStrength"][2].Values[1] = WpnHeavyStrength.Value.ToString();
        }

        private void WpnNavalStrength_Leave(object sender, EventArgs e)
        {
            // The list of AiFiles for this object are stored in the node tag
            Dictionary<AiFileType, AiFile> files = (Dictionary<AiFileType, AiFile>)SelectedNode.Tag;
            ObjectTemplate template = files[AiFileType.Weapon].Objects[WeaponSelect.SelectedItem.ToString()];
            template.Properties["setStrength"][3].Values[1] = WpnNavalStrength.Value.ToString();
        }

        private void WpnHeliStrength_Leave(object sender, EventArgs e)
        {
            // The list of AiFiles for this object are stored in the node tag
            Dictionary<AiFileType, AiFile> files = (Dictionary<AiFileType, AiFile>)SelectedNode.Tag;
            ObjectTemplate template = files[AiFileType.Weapon].Objects[WeaponSelect.SelectedItem.ToString()];
            template.Properties["setStrength"][4].Values[1] = WpnHeliStrength.Value.ToString();
        }

        private void WpnAirPlaneStrength_Leave(object sender, EventArgs e)
        {
            // The list of AiFiles for this object are stored in the node tag
            Dictionary<AiFileType, AiFile> files = (Dictionary<AiFileType, AiFile>)SelectedNode.Tag;
            ObjectTemplate template = files[AiFileType.Weapon].Objects[WeaponSelect.SelectedItem.ToString()];
            template.Properties["setStrength"][5].Values[1] = WpnAirPlaneStrength.Value.ToString();
        }

        #endregion Weapon Data Leave Events

        #region Kit Data Leave Events

        private void KitOffStrength_Leave(object sender, EventArgs e)
        {
            // The list of AiFiles for this object are stored in the node tag
            Dictionary<AiFileType, AiFile> files = (Dictionary<AiFileType, AiFile>)SelectedNode.Tag;
            ObjectTemplate template = files[AiFileType.Kit].Objects[KitSelect.SelectedItem.ToString()];
            template.Properties["setStrategicStrength"][0].Values[1] = ((int)KitOffStrength.Value).ToString();
        }

        private void KitDefStrength_Leave(object sender, EventArgs e)
        {
            // The list of AiFiles for this object are stored in the node tag
            Dictionary<AiFileType, AiFile> files = (Dictionary<AiFileType, AiFile>)SelectedNode.Tag;
            ObjectTemplate template = files[AiFileType.Kit].Objects[KitSelect.SelectedItem.ToString()];
            template.Properties["setStrategicStrength"][1].Values[1] = ((int)KitDefStrength.Value).ToString();
        }

        private void KitInfantryStrength_Leave(object sender, EventArgs e)
        {
            // The list of AiFiles for this object are stored in the node tag
            Dictionary<AiFileType, AiFile> files = (Dictionary<AiFileType, AiFile>)SelectedNode.Tag;
            ObjectTemplate template = files[AiFileType.Kit].Objects[KitSelect.SelectedItem.ToString()];
            template.Properties["setBattleStrength"][0].Values[1] = KitInfantryStrength.Value.ToString();
        }

        private void KitLightStrength_Leave(object sender, EventArgs e)
        {
            // The list of AiFiles for this object are stored in the node tag
            Dictionary<AiFileType, AiFile> files = (Dictionary<AiFileType, AiFile>)SelectedNode.Tag;
            ObjectTemplate template = files[AiFileType.Kit].Objects[KitSelect.SelectedItem.ToString()];
            template.Properties["setBattleStrength"][1].Values[1] = KitLightStrength.Value.ToString();
        }

        private void KitHeavyStrength_Leave(object sender, EventArgs e)
        {
            // The list of AiFiles for this object are stored in the node tag
            Dictionary<AiFileType, AiFile> files = (Dictionary<AiFileType, AiFile>)SelectedNode.Tag;
            ObjectTemplate template = files[AiFileType.Kit].Objects[KitSelect.SelectedItem.ToString()];
            template.Properties["setBattleStrength"][2].Values[1] = KitHeavyStrength.Value.ToString();
        }

        private void KitNavalStrength_Leave(object sender, EventArgs e)
        {
            // The list of AiFiles for this object are stored in the node tag
            Dictionary<AiFileType, AiFile> files = (Dictionary<AiFileType, AiFile>)SelectedNode.Tag;
            ObjectTemplate template = files[AiFileType.Kit].Objects[KitSelect.SelectedItem.ToString()];
            template.Properties["setBattleStrength"][3].Values[1] = KitNavalStrength.Value.ToString();
        }

        private void KitHeliStrength_Leave(object sender, EventArgs e)
        {
            // The list of AiFiles for this object are stored in the node tag
            Dictionary<AiFileType, AiFile> files = (Dictionary<AiFileType, AiFile>)SelectedNode.Tag;
            ObjectTemplate template = files[AiFileType.Kit].Objects[KitSelect.SelectedItem.ToString()];
            template.Properties["setBattleStrength"][4].Values[1] = KitHeliStrength.Value.ToString();
        }

        private void KitAirStrength_Leave(object sender, EventArgs e)
        {
            // The list of AiFiles for this object are stored in the node tag
            Dictionary<AiFileType, AiFile> files = (Dictionary<AiFileType, AiFile>)SelectedNode.Tag;
            ObjectTemplate template = files[AiFileType.Kit].Objects[KitSelect.SelectedItem.ToString()];
            template.Properties["setBattleStrength"][5].Values[1] = KitAirStrength.Value.ToString();
        }

        #endregion

        #region General Main Form Events

        private void MainForm_Shown(object sender, EventArgs e)
        {
            string path = Path.Combine(Application.StartupPath, "Temp");
            if(Directory.Exists(path) && !String.IsNullOrEmpty(Settings.Default.LastUsedZip)) 
            {
                DialogResult Res = MessageBox.Show(
                    String.Format(
                        "We have detected that the temporary files from: \"{0}\" are already extracted, Would you like to continue editing these files?"
                        + "{1}{1}By clicking no, these files will remain until a new Zip archive is extracted.", 
                        ShrinkPath(Settings.Default.LastUsedZip, 60), Environment.NewLine
                    ), 
                    "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question
                );

                if (Res == DialogResult.No)
                {
                    return;
                }
                else if (Res == DialogResult.Yes)
                {
                    ParseTemplates();
                }
            }
        }

        private void CalculatorLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Dictionary<AiFileType, AiFile> files = (Dictionary<AiFileType, AiFile>)SelectedNode.Tag;
            CalculatorForm F = new CalculatorForm(files[AiFileType.Vehicle], SelectedNode.Text);
            F.ShowDialog();

            // Reload
            ObjectSelect_SelectedIndexChanged(this, default(EventArgs));
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                TreeNode N = treeView1.SelectedNode;
                if (N.Nodes.Count == 0 && N.Parent != null)
                    contextMenu.Show(Cursor.Position);
            }
        }

        private void DescMenuItem_Click(object sender, EventArgs e)
        {
            
            InfoPopupForm form = new InfoPopupForm(ItemXmlNode);
            form.ShowDialog();
        }

        private void NameMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode N = treeView1.SelectedNode;
            Clipboard.SetText(N.Text);
        }

        private void contextMenu_Opening(object sender, CancelEventArgs e)
        {
            contextMenu.Items[0].Enabled = (ItemXmlNode != null);
        }

        private void OpenFileMenuItem_Click(object sender, EventArgs e)
        {
            Dictionary<AiFileType, AiFile> files = (Dictionary<AiFileType, AiFile>)SelectedNode.Tag;

            switch (tabControl1.SelectedTab.Text)
            {
                case "Weapon Data":
                    Process.Start(files[AiFileType.Weapon].FilePath);
                    break;
                case "Vehicle Data":
                    Process.Start(files[AiFileType.Vehicle].FilePath);
                    break;
                case "Kit Data":
                    Process.Start(files[AiFileType.Kit].FilePath);
                    break;
            }
        }

        #endregion
    }
}
