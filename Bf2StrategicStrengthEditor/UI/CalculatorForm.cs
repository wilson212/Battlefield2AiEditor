using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace Battlefield2
{
    public partial class CalculatorForm : Form
    {
        /// <summary>
        /// An array of each seats values, so we dont lose data when changing seats
        /// </summary>
        protected PcoObject[] PcoValueArray;

        /// <summary>
        /// If True, value changes will NOT update the calculator texts
        /// </summary>
        protected bool SuppressUpdate = true;

        /// <summary>
        /// The AI file that contains this vehicle's data (Vehicle.ai)
        /// </summary>
        protected AiFile WorkingFile;

        /// <summary>
        /// Creates a new instance of Calculator form.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="vehicleName"></param>
        public CalculatorForm(AiFile file, string vehicleName)
        {
            InitializeComponent();

            // Add vehicle name to window title
            this.Text += vehicleName;
            this.WorkingFile = file;

            // Set default index
            VehicleStrengthSelect.SelectedIndex = 0;

            // Get an array of PCO positions
            AiTemplate[] Pcos = (
                from x in file.Objects 
                where x.Value.TemplateType == TemplateType.AiTemplate 
                select x.Value as AiTemplate
            ).ToArray();

            // Create PCO Values array, making a slot for each template
            PcoValueArray = new PcoObject[Pcos.Length];

            // Current vehicle index
            int index = 0;

            // Add each vehicle position to the seat selector
            foreach (AiTemplate template in Pcos)
            {
                // Add PCO position
                ObjectSelect.Items.Add(template);

                // Create PCO values
                PcoValueArray[index++] = new PcoObject();

                // Only the main vehicle will have an armor type
                if (template.Plugins.ContainsKey(AiTemplatePluginType.Physical))
                {
                    string T = template.Plugins[AiTemplatePluginType.Physical].Properties["setStrType"][0].Values[0];
                    switch (T.ToLowerInvariant())
                    {
                        case "lightarmour":
                            VehicleStrengthSelect.SelectedIndex = 1;
                            break;
                        case "heavyarmour":
                            VehicleStrengthSelect.SelectedIndex = 2;
                            break;
                        case "helicopter":
                        case "airplane":
                            VehicleStrengthSelect.SelectedIndex = 3;
                            break;
                    }
                }
            }

            // Set main set as index, which will also remove the Suppression of events
            ObjectSelect.SelectedIndex = 0;
        }

        /// <summary>
        /// Reads the values from a PCO object and calculates the basicTemp, Offensive
        /// strategic strength, and Defensive strategic strength
        /// </summary>
        /// <param name="Pco"></param>
        /// <param name="basicTemp"></param>
        /// <param name="offStr"></param>
        /// <param name="defStr"></param>
        private void CalculateValues(PcoObject Pco, out int basicTemp, out int offStr, out int defStr)
        {
            // Reset Base values to 0
            basicTemp = offStr = defStr = 0;

            // Set base temp based on vehicle type
            switch (VehicleStrengthSelect.SelectedIndex)
            {
                case 1: basicTemp = 16; break;
                case 2: basicTemp = 24; break;
                case 3: basicTemp = 20; break;
                case 4: basicTemp = 12; break;
            }

            // Soldier Exposure
            basicTemp += Pco.SoldierExposureIndex;
            if (Pco.IsDriver) basicTemp += 9;

            if (Pco.HasWeapon)
            {
                basicTemp += 5;
                offStr += 1;
                defStr += 2;

                // Is a a powerul cannon?
                if (Pco.WeaponIsPowerful)
                {
                    offStr += 5;
                    defStr += 5;
                }

                // Effective at killing other Armor's
                if (Pco.WeaponIsEffective)
                    defStr += 2;
            }
            else if (Pco.CanFireHandWeapon)
            {
                // Only offensive if we arent stationary!
                if (VehicleStrengthSelect.SelectedIndex < 4)
                    offStr += 1;

                defStr += 2;
            }

            if (Pco.IsMobile)
            {
                basicTemp += 1;
                offStr += 5;
            }

            if (Pco.CanTakeCP)
            {
                offStr += 1;
                defStr += 2;
            }

            if (Pco.IsMountedWeapon) defStr += 1;

            // Do Seat calculation
            basicTemp -= ((int)Math.Floor(NumberSeats.Value / 2));
        }

        /// <summary>
        /// Updates the Calculator values on the form with the selected form values
        /// </summary>
        private void UpdateValues()
        {
            // Dont calculate if we are supressed
            if (SuppressUpdate) return;

            // Calculate
            int basicTemp = 0, offStr = 0, defStr = 0;
            CalculateValues(PcoValueArray[ObjectSelect.SelectedIndex], out basicTemp, out offStr, out defStr);

            // Set texts
            BasicTemp.Text = basicTemp.ToString();
            OffStrength.Text = offStr.ToString();
            DefStrength.Text = defStr.ToString();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://www.battlefieldsingleplayer.com/forum/index.php?showtopic=12257");   
        }

        #region Button Clicks

        /// <summary>
        /// Open Ai File Button Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenFileBtn_Click(object sender, EventArgs e)
        {
            Process.Start(Path.Combine(Application.StartupPath, "Temp", WorkingFile.FilePath));
        }

        /// <summary>
        /// Close Button Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Save Button Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveBtn_Click(object sender, EventArgs e)
        {
            // Get an array of PCO positions
            AiTemplate[] Pcos = (
                from x in WorkingFile.Objects
                where x.Value.TemplateType == TemplateType.AiTemplate
                select x.Value as AiTemplate
            ).ToArray();

            // Set the values of each PCO
            int i = 0;
            foreach(AiTemplate Template in Pcos)
            {
                // Calculate
                int basicTemp = 0, offStr = 0, defStr = 0;
                CalculateValues(PcoValueArray[i++], out basicTemp, out offStr, out defStr);

                // Vehicle strat. strengths are stored in the Unit plugin, so we load that too
                ObjectTemplate plugin = Template.Plugins[AiTemplatePluginType.Unit];

                // Set new values
                plugin.Properties["setStrategicStrength"][0].Values[1] = offStr.ToString();
                plugin.Properties["setStrategicStrength"][1].Values[1] = defStr.ToString();
                Template.Properties["basicTemp"][0].Values[0] = basicTemp.ToString();
            }

            // Inform User
            MessageBox.Show("Successfully applied settings of all positions!", "Success", MessageBoxButtons.OK);
        }

        #endregion Button Clicks

        #region Form Change Events

        private void VehicleStrengthSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateValues();
        }

        private void ObjectSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Suppress so calculator doesnt run multiple times here
            this.SuppressUpdate = true;

            // Load pco
            PcoObject pco = PcoValueArray[ObjectSelect.SelectedIndex];
            ExposureSelect.SelectedIndex = pco.SoldierExposureIndex;
            IsDriverBox.Checked = pco.IsDriver;
            PosHasWeaponBox.Checked = pco.HasWeapon;
            PosIsPowerfulBox.Checked = pco.WeaponIsPowerful;
            PosIsEffective.Checked = pco.WeaponIsEffective;
            IsMobileBox.Checked = pco.IsMobile;
            IsStationaryBox.Checked = pco.IsMountedWeapon;
            CanTakeCPBox.Checked = pco.CanTakeCP;

            this.SuppressUpdate = false;
            UpdateValues();
        }

        private void ExposureSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            PcoValueArray[ObjectSelect.SelectedIndex].SoldierExposureIndex = ExposureSelect.SelectedIndex;
            UpdateValues();
        }

        private void IsDriverBox_CheckedChanged(object sender, EventArgs e)
        {
            PcoValueArray[ObjectSelect.SelectedIndex].IsDriver = IsDriverBox.Checked;
            UpdateValues();
        }

        private void PosHasWeaponBox_CheckedChanged(object sender, EventArgs e)
        {
            // Set PCO value
            PcoValueArray[ObjectSelect.SelectedIndex].HasWeapon = PosHasWeaponBox.Checked;

            // Suppress so calculator doesnt run multiple times here
            this.SuppressUpdate = true;
            if (!PosHasWeaponBox.Checked)
            {
                // Disable sub boxes
                PosIsEffective.Enabled = PosIsEffective.Checked = false;
                PosIsPowerfulBox.Enabled = PosIsPowerfulBox.Checked = false;
                CanFireWpnBox.Enabled = true;
                
            }
            else
            {
                // Enable sub boxes
                PosIsEffective.Enabled = true;
                PosIsPowerfulBox.Enabled = true;
                CanFireWpnBox.Enabled = CanFireWpnBox.Checked = false;
            }

            // Allow Calculation again
            this.SuppressUpdate = false;
            UpdateValues();
        }

        private void CanFireWpnBox_CheckedChanged(object sender, EventArgs e)
        {
            // Set PCO value
            PcoValueArray[ObjectSelect.SelectedIndex].CanFireHandWeapon = CanFireWpnBox.Checked;

            // Cant cross fire these. DO this last to prevent calculator from running alot
            if (CanFireWpnBox.Checked && PosHasWeaponBox.Checked)
                PosHasWeaponBox.Checked = false;
            else
                UpdateValues();

            // Switch!
            PosHasWeaponBox.Enabled = !CanFireWpnBox.Checked;
        }

        private void PosIsPowerfulBox_CheckedChanged(object sender, EventArgs e)
        {
            PcoValueArray[ObjectSelect.SelectedIndex].WeaponIsPowerful = PosIsPowerfulBox.Checked;
            UpdateValues();
        }

        private void PosIsEffective_CheckedChanged(object sender, EventArgs e)
        {
            PcoValueArray[ObjectSelect.SelectedIndex].WeaponIsEffective = PosIsEffective.Checked;
            UpdateValues();
        }

        private void IsMobileBox_CheckedChanged(object sender, EventArgs e)
        {
            PcoValueArray[ObjectSelect.SelectedIndex].IsMobile = IsMobileBox.Checked;
            UpdateValues();
        }

        private void IsStationaryBox_CheckedChanged(object sender, EventArgs e)
        {
            PcoValueArray[ObjectSelect.SelectedIndex].IsMountedWeapon = IsStationaryBox.Checked;
            UpdateValues();
        }

        private void CanTakeCPBox_CheckedChanged(object sender, EventArgs e)
        {
            PcoValueArray[ObjectSelect.SelectedIndex].CanTakeCP = CanTakeCPBox.Checked;
            UpdateValues();
        }

        private void NumberSeats_ValueChanged(object sender, EventArgs e)
        {
            UpdateValues();
        }

        #endregion Form Change Events

        protected internal struct PcoObject
        {
            public int SoldierExposureIndex;

            public bool IsDriver;

            public bool HasWeapon;

            public bool WeaponIsPowerful;

            public bool WeaponIsEffective;

            public bool CanFireHandWeapon;

            public bool IsMobile;

            public bool IsMountedWeapon;

            public bool CanTakeCP;

        }
    }
}
