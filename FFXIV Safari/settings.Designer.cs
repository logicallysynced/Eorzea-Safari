namespace FFXIV_Safari
{
    partial class settings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(settings));
            this.cb_server = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chk_aranks = new System.Windows.Forms.CheckBox();
            this.chk_sranks = new System.Windows.Forms.CheckBox();
            this.chk_transparent = new System.Windows.Forms.CheckBox();
            this.nm_interval = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.btn_save = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.cd_back = new System.Windows.Forms.ColorDialog();
            this.btn_backCol = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nm_interval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // cb_server
            // 
            this.cb_server.FormattingEnabled = true;
            this.cb_server.Items.AddRange(new object[] {
            "Adamantoise",
            "Aegis",
            "Alexander",
            "Anima",
            "Asura",
            "Atomos",
            "Bahamut",
            "Balmung",
            "Behemoth",
            "Belias",
            "Brynhildr",
            "Cactuar",
            "Carbuncle",
            "Cerberus",
            "Chocobo",
            "Coeurl",
            "Diabolos",
            "Durandal",
            "Excalibur",
            "Exodus",
            "Faerie",
            "Famfrit",
            "Fenrir",
            "Garuda",
            "Gilgamesh",
            "Goblin",
            "Gungnir",
            "Hades",
            "Hyperion",
            "Ifrit",
            "Ixion",
            "Jenova",
            "Kujata",
            "Lamia",
            "Leviathan",
            "Lich",
            "Malboro",
            "Mandragora",
            "Masamune",
            "Mateus",
            "Midgardsormr",
            "Moogle",
            "Odin",
            "Pandaemonium",
            "Phoenix",
            "Ragnarok",
            "Ramuh",
            "Ridill",
            "Sargatanas",
            "Shinryu",
            "Shiva",
            "Siren",
            "Tiamat",
            "Titan",
            "Tonberry",
            "Typhon",
            "Ultima",
            "Ultros",
            "Unicorn",
            "Valefor",
            "Yojimbo",
            "Zalera",
            "Zeromus",
            "Zodiark"});
            this.cb_server.Location = new System.Drawing.Point(195, 29);
            this.cb_server.Name = "cb_server";
            this.cb_server.Size = new System.Drawing.Size(158, 21);
            this.cb_server.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(109, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Server Select";
            // 
            // chk_aranks
            // 
            this.chk_aranks.AutoSize = true;
            this.chk_aranks.Checked = global::FFXIV_Safari.Properties.Settings.Default.chk_aranks;
            this.chk_aranks.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_aranks.Location = new System.Drawing.Point(112, 110);
            this.chk_aranks.Name = "chk_aranks";
            this.chk_aranks.Size = new System.Drawing.Size(104, 17);
            this.chk_aranks.TabIndex = 4;
            this.chk_aranks.Text = "Display A Ranks";
            this.chk_aranks.UseVisualStyleBackColor = true;
            // 
            // chk_sranks
            // 
            this.chk_sranks.AutoSize = true;
            this.chk_sranks.Checked = global::FFXIV_Safari.Properties.Settings.Default.chk_sranks;
            this.chk_sranks.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_sranks.Location = new System.Drawing.Point(255, 110);
            this.chk_sranks.Name = "chk_sranks";
            this.chk_sranks.Size = new System.Drawing.Size(104, 17);
            this.chk_sranks.TabIndex = 5;
            this.chk_sranks.Text = "Display S Ranks";
            this.chk_sranks.UseVisualStyleBackColor = true;
            // 
            // chk_transparent
            // 
            this.chk_transparent.AutoSize = true;
            this.chk_transparent.Location = new System.Drawing.Point(255, 149);
            this.chk_transparent.Name = "chk_transparent";
            this.chk_transparent.Size = new System.Drawing.Size(83, 17);
            this.chk_transparent.TabIndex = 6;
            this.chk_transparent.Text = "Transparent";
            this.chk_transparent.UseVisualStyleBackColor = true;
            this.chk_transparent.CheckedChanged += new System.EventHandler(this.chk_transparent_CheckedChanged);
            // 
            // nm_interval
            // 
            this.nm_interval.Location = new System.Drawing.Point(195, 64);
            this.nm_interval.Name = "nm_interval";
            this.nm_interval.Size = new System.Drawing.Size(74, 20);
            this.nm_interval.TabIndex = 7;
            this.nm_interval.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(98, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Refresh Interval";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(273, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "seconds";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(77, 243);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(64, 64);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 10;
            this.pictureBox1.TabStop = false;
            // 
            // richTextBox1
            // 
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.Font = new System.Drawing.Font("Myriad Pro", 9.7F);
            this.richTextBox1.Location = new System.Drawing.Point(158, 243);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.richTextBox1.Size = new System.Drawing.Size(224, 64);
            this.richTextBox1.TabIndex = 11;
            this.richTextBox1.Text = "Eorzea Safari\nVersion 1.2\nData provided by http://ffxivhunt.com/\nCopyright © Roxa" +
    "s Keyheart";
            this.richTextBox1.LinkClicked += richTextBox1_LinkClicked;
            // 
            // btn_save
            // 
            this.btn_save.Location = new System.Drawing.Point(191, 185);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(75, 23);
            this.btn_save.TabIndex = 12;
            this.btn_save.Text = "Save";
            this.btn_save.UseVisualStyleBackColor = true;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(100, 150);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Background";
            // 
            // cd_back
            // 
            this.cd_back.AnyColor = true;
            this.cd_back.FullOpen = true;
            // 
            // btn_backCol
            // 
            this.btn_backCol.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btn_backCol.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btn_backCol.Location = new System.Drawing.Point(202, 146);
            this.btn_backCol.Name = "btn_backCol";
            this.btn_backCol.Size = new System.Drawing.Size(44, 23);
            this.btn_backCol.TabIndex = 14;
            this.btn_backCol.UseVisualStyleBackColor = false;
            this.btn_backCol.Click += new System.EventHandler(this.btn_backCol_Click);
            // 
            // settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(456, 312);
            this.ControlBox = false;
            this.Controls.Add(this.btn_backCol);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btn_save);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.nm_interval);
            this.Controls.Add(this.chk_transparent);
            this.Controls.Add(this.chk_sranks);
            this.Controls.Add(this.chk_aranks);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cb_server);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "settings";
            this.Text = "Eorzea Safari Config";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.nm_interval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cb_server;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chk_aranks;
        private System.Windows.Forms.CheckBox chk_sranks;
        private System.Windows.Forms.CheckBox chk_transparent;
        private System.Windows.Forms.NumericUpDown nm_interval;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button btn_save;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ColorDialog cd_back;
        private System.Windows.Forms.Button btn_backCol;

        public bool checkaranks
        {
            get { return chk_aranks.Checked; }
            set { chk_aranks.Checked = value; }
        }
        public bool checksranks
        {
            get { return chk_sranks.Checked; }
            set { chk_sranks.Checked = value; }
        }
        public bool checktransparent
        {
            get { return chk_transparent.Checked; }
            set { chk_transparent.Checked = value; }
        }
        public object serverselect
        {
            get { return cb_server.SelectedItem; }
            set { cb_server.SelectedItem = value; }
        }
        public decimal interval
        {
            get { return nm_interval.Value; }
            set { nm_interval.Value = value; }
        }
    }
}