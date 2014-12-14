namespace Battlefield2
{
    partial class CalculatorForm
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
            this.components = new System.ComponentModel.Container();
            this.ObjectSelect = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.VehicleStrengthSelect = new System.Windows.Forms.ComboBox();
            this.label25 = new System.Windows.Forms.Label();
            this.IsDriverBox = new System.Windows.Forms.CheckBox();
            this.PosHasWeaponBox = new System.Windows.Forms.CheckBox();
            this.IsMobileBox = new System.Windows.Forms.CheckBox();
            this.ExposureSelect = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.DescLabel = new System.Windows.Forms.Label();
            this.TitleLabel = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.lineShape1 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.panel2 = new System.Windows.Forms.Panel();
            this.NumberSeats = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.PosIsEffective = new System.Windows.Forms.CheckBox();
            this.PosIsPowerfulBox = new System.Windows.Forms.CheckBox();
            this.IsStationaryBox = new System.Windows.Forms.CheckBox();
            this.CanTakeCPBox = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.DefStrength = new System.Windows.Forms.Label();
            this.OffStrength = new System.Windows.Forms.Label();
            this.BasicTemp = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.Label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.shapeContainer2 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.lineShape2 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.OpenFileBtn = new System.Windows.Forms.Button();
            this.SaveBtn = new System.Windows.Forms.Button();
            this.CloseBtn = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.CanFireWpnBox = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumberSeats)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ObjectSelect
            // 
            this.ObjectSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ObjectSelect.FormattingEnabled = true;
            this.ObjectSelect.Location = new System.Drawing.Point(122, 19);
            this.ObjectSelect.Name = "ObjectSelect";
            this.ObjectSelect.Size = new System.Drawing.Size(180, 21);
            this.ObjectSelect.TabIndex = 3;
            this.ObjectSelect.SelectedIndexChanged += new System.EventHandler(this.ObjectSelect_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Vehicle Position:";
            // 
            // VehicleStrengthSelect
            // 
            this.VehicleStrengthSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.VehicleStrengthSelect.FormattingEnabled = true;
            this.VehicleStrengthSelect.Items.AddRange(new object[] {
            "None",
            "Light Armour",
            "Heavy Armour",
            "Helicopter / AirPlane",
            "Stationary Weapon / Bi-pod"});
            this.VehicleStrengthSelect.Location = new System.Drawing.Point(146, 17);
            this.VehicleStrengthSelect.Name = "VehicleStrengthSelect";
            this.VehicleStrengthSelect.Size = new System.Drawing.Size(180, 21);
            this.VehicleStrengthSelect.TabIndex = 5;
            this.VehicleStrengthSelect.SelectedIndexChanged += new System.EventHandler(this.VehicleStrengthSelect_SelectedIndexChanged);
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(49, 20);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(91, 13);
            this.label25.TabIndex = 13;
            this.label25.Text = "Base Temp Type:";
            // 
            // IsDriverBox
            // 
            this.IsDriverBox.AutoSize = true;
            this.IsDriverBox.Location = new System.Drawing.Point(18, 79);
            this.IsDriverBox.Name = "IsDriverBox";
            this.IsDriverBox.Size = new System.Drawing.Size(139, 17);
            this.IsDriverBox.TabIndex = 14;
            this.IsDriverBox.Text = "Position is Driver or Pilot";
            this.IsDriverBox.UseVisualStyleBackColor = true;
            this.IsDriverBox.CheckedChanged += new System.EventHandler(this.IsDriverBox_CheckedChanged);
            // 
            // PosHasWeaponBox
            // 
            this.PosHasWeaponBox.AutoSize = true;
            this.PosHasWeaponBox.Location = new System.Drawing.Point(18, 102);
            this.PosHasWeaponBox.Name = "PosHasWeaponBox";
            this.PosHasWeaponBox.Size = new System.Drawing.Size(261, 17);
            this.PosHasWeaponBox.TabIndex = 15;
            this.PosHasWeaponBox.Text = "Position has Weapon (not soldier\'s handweapons)";
            this.PosHasWeaponBox.UseVisualStyleBackColor = true;
            this.PosHasWeaponBox.CheckedChanged += new System.EventHandler(this.PosHasWeaponBox_CheckedChanged);
            // 
            // IsMobileBox
            // 
            this.IsMobileBox.AutoSize = true;
            this.IsMobileBox.Location = new System.Drawing.Point(18, 214);
            this.IsMobileBox.Name = "IsMobileBox";
            this.IsMobileBox.Size = new System.Drawing.Size(231, 17);
            this.IsMobileBox.TabIndex = 16;
            this.IsMobileBox.Text = "Is Mobile Vehicle (pilot / driver / passenger)";
            this.toolTip1.SetToolTip(this.IsMobileBox, "Pilot or Passenger in a plane, or a Driver or Passenger in a Land Vehicle.");
            this.IsMobileBox.UseVisualStyleBackColor = true;
            this.IsMobileBox.CheckedChanged += new System.EventHandler(this.IsMobileBox_CheckedChanged);
            // 
            // ExposureSelect
            // 
            this.ExposureSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ExposureSelect.FormattingEnabled = true;
            this.ExposureSelect.Items.AddRange(new object[] {
            "No Protection",
            "Partially Protected",
            "Fully Protected"});
            this.ExposureSelect.Location = new System.Drawing.Point(122, 49);
            this.ExposureSelect.Name = "ExposureSelect";
            this.ExposureSelect.Size = new System.Drawing.Size(180, 21);
            this.ExposureSelect.TabIndex = 18;
            this.ExposureSelect.SelectedIndexChanged += new System.EventHandler(this.ExposureSelect_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(29, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "Soldier Exposure:";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.linkLabel1);
            this.panel1.Controls.Add(this.DescLabel);
            this.panel1.Controls.Add(this.TitleLabel);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.shapeContainer1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(584, 70);
            this.panel1.TabIndex = 21;
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(413, 38);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(88, 13);
            this.linkLabel1.TabIndex = 6;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Korbens weights.";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // DescLabel
            // 
            this.DescLabel.AutoSize = true;
            this.DescLabel.Location = new System.Drawing.Point(14, 38);
            this.DescLabel.Name = "DescLabel";
            this.DescLabel.Size = new System.Drawing.Size(401, 13);
            this.DescLabel.TabIndex = 4;
            this.DescLabel.Text = "A Calculator that suggests temperature, and strategic strength of vehicles  based" +
                " on";
            // 
            // TitleLabel
            // 
            this.TitleLabel.AutoSize = true;
            this.TitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TitleLabel.Location = new System.Drawing.Point(14, 20);
            this.TitleLabel.Name = "TitleLabel";
            this.TitleLabel.Size = new System.Drawing.Size(217, 13);
            this.TitleLabel.TabIndex = 3;
            this.TitleLabel.Text = "Vehicle Strategic Strength Calculator";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Battlefield2.Properties.Resources.calculate;
            this.pictureBox1.Location = new System.Drawing.Point(520, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(48, 48);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.lineShape1});
            this.shapeContainer1.Size = new System.Drawing.Size(584, 70);
            this.shapeContainer1.TabIndex = 5;
            this.shapeContainer1.TabStop = false;
            // 
            // lineShape1
            // 
            this.lineShape1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.lineShape1.Name = "lineShape1";
            this.lineShape1.X1 = -2;
            this.lineShape1.X2 = 595;
            this.lineShape1.Y1 = 69;
            this.lineShape1.Y2 = 69;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.Window;
            this.panel2.Controls.Add(this.NumberSeats);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.groupBox2);
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Controls.Add(this.VehicleStrengthSelect);
            this.panel2.Controls.Add(this.label25);
            this.panel2.Controls.Add(this.shapeContainer2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 70);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(584, 333);
            this.panel2.TabIndex = 22;
            // 
            // NumberSeats
            // 
            this.NumberSeats.Location = new System.Drawing.Point(462, 18);
            this.NumberSeats.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.NumberSeats.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NumberSeats.Name = "NumberSeats";
            this.NumberSeats.Size = new System.Drawing.Size(65, 20);
            this.NumberSeats.TabIndex = 29;
            this.NumberSeats.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NumberSeats.ValueChanged += new System.EventHandler(this.NumberSeats_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(354, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(102, 13);
            this.label4.TabIndex = 28;
            this.label4.Text = "Total Vehicle Seats:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.CanFireWpnBox);
            this.groupBox2.Controls.Add(this.ObjectSelect);
            this.groupBox2.Controls.Add(this.PosIsEffective);
            this.groupBox2.Controls.Add(this.IsMobileBox);
            this.groupBox2.Controls.Add(this.PosIsPowerfulBox);
            this.groupBox2.Controls.Add(this.IsStationaryBox);
            this.groupBox2.Controls.Add(this.PosHasWeaponBox);
            this.groupBox2.Controls.Add(this.IsDriverBox);
            this.groupBox2.Controls.Add(this.CanTakeCPBox);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.ExposureSelect);
            this.groupBox2.Location = new System.Drawing.Point(12, 55);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(330, 265);
            this.groupBox2.TabIndex = 27;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Position Information";
            // 
            // PosIsEffective
            // 
            this.PosIsEffective.AutoSize = true;
            this.PosIsEffective.Location = new System.Drawing.Point(42, 147);
            this.PosIsEffective.Name = "PosIsEffective";
            this.PosIsEffective.Size = new System.Drawing.Size(239, 17);
            this.PosIsEffective.TabIndex = 26;
            this.PosIsEffective.Text = "Effectively targets both Land and Air vehicles";
            this.toolTip1.SetToolTip(this.PosIsEffective, "* Pilot with Machinegun + bombs, \r\n* A land or stationary position w/ full rotate" +
                    ",\r\n* A multipurpose Machinegun or Cannon\r\n\r\n");
            this.PosIsEffective.UseVisualStyleBackColor = true;
            this.PosIsEffective.CheckedChanged += new System.EventHandler(this.PosIsEffective_CheckedChanged);
            // 
            // PosIsPowerfulBox
            // 
            this.PosIsPowerfulBox.AutoSize = true;
            this.PosIsPowerfulBox.Location = new System.Drawing.Point(42, 125);
            this.PosIsPowerfulBox.Name = "PosIsPowerfulBox";
            this.PosIsPowerfulBox.Size = new System.Drawing.Size(270, 17);
            this.PosIsPowerfulBox.TabIndex = 25;
            this.PosIsPowerfulBox.Text = "Is Powerful Anti-Vehicle Weapon (Cannons, Bombs)";
            this.toolTip1.SetToolTip(this.PosIsPowerfulBox, "* Tank cannon, \r\n* Plane bombs/missles\r\n* Anti-Aircraft weapon \r\n*** Stationary W" +
                    "eapons have defensive points only ***");
            this.PosIsPowerfulBox.UseVisualStyleBackColor = true;
            this.PosIsPowerfulBox.CheckedChanged += new System.EventHandler(this.PosIsPowerfulBox_CheckedChanged);
            // 
            // IsStationaryBox
            // 
            this.IsStationaryBox.AutoSize = true;
            this.IsStationaryBox.Location = new System.Drawing.Point(42, 169);
            this.IsStationaryBox.Name = "IsStationaryBox";
            this.IsStationaryBox.Size = new System.Drawing.Size(163, 17);
            this.IsStationaryBox.TabIndex = 24;
            this.IsStationaryBox.Text = "Is Mounted Weapon Position";
            this.toolTip1.SetToolTip(this.IsStationaryBox, "* Tank cannon, \r\n* Stationary MachineGun, \r\n* Anti-Aircraft weapon");
            this.IsStationaryBox.UseVisualStyleBackColor = true;
            this.IsStationaryBox.CheckedChanged += new System.EventHandler(this.IsStationaryBox_CheckedChanged);
            // 
            // CanTakeCPBox
            // 
            this.CanTakeCPBox.AutoSize = true;
            this.CanTakeCPBox.Location = new System.Drawing.Point(18, 236);
            this.CanTakeCPBox.Name = "CanTakeCPBox";
            this.CanTakeCPBox.Size = new System.Drawing.Size(136, 17);
            this.CanTakeCPBox.TabIndex = 23;
            this.CanTakeCPBox.Text = "Can Take Control Point";
            this.toolTip1.SetToolTip(this.CanTakeCPBox, "Foot Soldier or Land Vehicle Driver");
            this.CanTakeCPBox.UseVisualStyleBackColor = true;
            this.CanTakeCPBox.CheckedChanged += new System.EventHandler(this.CanTakeCPBox_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.DefStrength);
            this.groupBox1.Controls.Add(this.OffStrength);
            this.groupBox1.Controls.Add(this.BasicTemp);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.Label6);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(359, 55);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(209, 265);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Suggested Values:";
            // 
            // DefStrength
            // 
            this.DefStrength.AutoSize = true;
            this.DefStrength.Location = new System.Drawing.Point(164, 91);
            this.DefStrength.Name = "DefStrength";
            this.DefStrength.Size = new System.Drawing.Size(13, 13);
            this.DefStrength.TabIndex = 7;
            this.DefStrength.Text = "0";
            // 
            // OffStrength
            // 
            this.OffStrength.AutoSize = true;
            this.OffStrength.Location = new System.Drawing.Point(164, 65);
            this.OffStrength.Name = "OffStrength";
            this.OffStrength.Size = new System.Drawing.Size(13, 13);
            this.OffStrength.TabIndex = 6;
            this.OffStrength.Text = "0";
            // 
            // BasicTemp
            // 
            this.BasicTemp.AutoSize = true;
            this.BasicTemp.Location = new System.Drawing.Point(164, 38);
            this.BasicTemp.Name = "BasicTemp";
            this.BasicTemp.Size = new System.Drawing.Size(13, 13);
            this.BasicTemp.TabIndex = 5;
            this.BasicTemp.Text = "0";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(21, 91);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(118, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = "Def. Strategic Strength:";
            // 
            // Label6
            // 
            this.Label6.AutoSize = true;
            this.Label6.Location = new System.Drawing.Point(21, 65);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(115, 13);
            this.Label6.TabIndex = 3;
            this.Label6.Text = "Off. Strategic Strength:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 39);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Basic Temp:";
            // 
            // shapeContainer2
            // 
            this.shapeContainer2.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer2.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer2.Name = "shapeContainer2";
            this.shapeContainer2.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.lineShape2});
            this.shapeContainer2.Size = new System.Drawing.Size(584, 333);
            this.shapeContainer2.TabIndex = 30;
            this.shapeContainer2.TabStop = false;
            // 
            // lineShape2
            // 
            this.lineShape2.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.lineShape2.Name = "lineShape2";
            this.lineShape2.X1 = 0;
            this.lineShape2.X2 = 583;
            this.lineShape2.Y1 = 332;
            this.lineShape2.Y2 = 332;
            // 
            // OpenFileBtn
            // 
            this.OpenFileBtn.Location = new System.Drawing.Point(30, 411);
            this.OpenFileBtn.Name = "OpenFileBtn";
            this.OpenFileBtn.Size = new System.Drawing.Size(159, 29);
            this.OpenFileBtn.TabIndex = 23;
            this.OpenFileBtn.Text = "Open Vehicle\'s Object.ai File";
            this.OpenFileBtn.UseVisualStyleBackColor = true;
            this.OpenFileBtn.Click += new System.EventHandler(this.OpenFileBtn_Click);
            // 
            // SaveBtn
            // 
            this.SaveBtn.Location = new System.Drawing.Point(409, 411);
            this.SaveBtn.Name = "SaveBtn";
            this.SaveBtn.Size = new System.Drawing.Size(159, 29);
            this.SaveBtn.TabIndex = 24;
            this.SaveBtn.Text = "Apply Settings";
            this.SaveBtn.UseVisualStyleBackColor = true;
            this.SaveBtn.Click += new System.EventHandler(this.SaveBtn_Click);
            // 
            // CloseBtn
            // 
            this.CloseBtn.Location = new System.Drawing.Point(219, 411);
            this.CloseBtn.Name = "CloseBtn";
            this.CloseBtn.Size = new System.Drawing.Size(159, 29);
            this.CloseBtn.TabIndex = 25;
            this.CloseBtn.Text = "Cancel and Close";
            this.CloseBtn.UseVisualStyleBackColor = true;
            this.CloseBtn.Click += new System.EventHandler(this.CloseBtn_Click);
            // 
            // toolTip1
            // 
            this.toolTip1.AutomaticDelay = 2000;
            this.toolTip1.AutoPopDelay = 20000;
            this.toolTip1.InitialDelay = 200;
            this.toolTip1.ReshowDelay = 400;
            this.toolTip1.ShowAlways = true;
            // 
            // CanFireWpnBox
            // 
            this.CanFireWpnBox.AutoSize = true;
            this.CanFireWpnBox.Location = new System.Drawing.Point(18, 191);
            this.CanFireWpnBox.Name = "CanFireWpnBox";
            this.CanFireWpnBox.Size = new System.Drawing.Size(167, 17);
            this.CanFireWpnBox.TabIndex = 27;
            this.CanFireWpnBox.Text = "Can Fire Soldier Handweapon";
            this.CanFireWpnBox.UseVisualStyleBackColor = true;
            this.CanFireWpnBox.CheckedChanged += new System.EventHandler(this.CanFireWpnBox_CheckedChanged);
            // 
            // CalculatorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 452);
            this.Controls.Add(this.CloseBtn);
            this.Controls.Add(this.SaveBtn);
            this.Controls.Add(this.OpenFileBtn);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CalculatorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Selected Vehicle: ";
            this.toolTip1.SetToolTip(this, " - Pilot w/ mg + bombs,\r\n - land or stationary PCO position w/ full rotate \r\n - m" +
                    "ultipurpose MG/cannon\r\n\r\n");
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumberSeats)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox ObjectSelect;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox VehicleStrengthSelect;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.CheckBox IsDriverBox;
        private System.Windows.Forms.CheckBox PosHasWeaponBox;
        private System.Windows.Forms.CheckBox IsMobileBox;
        private System.Windows.Forms.ComboBox ExposureSelect;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label DescLabel;
        private System.Windows.Forms.Label TitleLabel;
        private System.Windows.Forms.PictureBox pictureBox1;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox CanTakeCPBox;
        private System.Windows.Forms.CheckBox IsStationaryBox;
        private System.Windows.Forms.CheckBox PosIsPowerfulBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label Label6;
        private System.Windows.Forms.Label DefStrength;
        private System.Windows.Forms.Label OffStrength;
        private System.Windows.Forms.Label BasicTemp;
        private System.Windows.Forms.CheckBox PosIsEffective;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown NumberSeats;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button OpenFileBtn;
        private System.Windows.Forms.Button SaveBtn;
        private System.Windows.Forms.Button CloseBtn;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer2;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape2;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox CanFireWpnBox;
    }
}