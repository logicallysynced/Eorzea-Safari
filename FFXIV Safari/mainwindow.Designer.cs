namespace FFXIV_Safari
{
    partial class mainform
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mainform));
            this.btn_close = new System.Windows.Forms.Button();
            this.huntdata = new System.Windows.Forms.RichTextBox();
            this.btn_settings = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_close
            // 
            this.btn_close.BackColor = System.Drawing.Color.Tomato;
            this.btn_close.FlatAppearance.BorderSize = 0;
            this.btn_close.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_close.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.btn_close.Location = new System.Drawing.Point(374, 13);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(10, 10);
            this.btn_close.TabIndex = 1;
            this.btn_close.UseVisualStyleBackColor = false;
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // huntdata
            // 
            this.huntdata.BackColor = System.Drawing.SystemColors.ControlText;
            this.huntdata.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.huntdata.Cursor = System.Windows.Forms.Cursors.SizeAll;
            this.huntdata.DetectUrls = false;
            this.huntdata.Font = new System.Drawing.Font("Myriad Pro", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.huntdata.Location = new System.Drawing.Point(12, 13);
            this.huntdata.Name = "huntdata";
            this.huntdata.ReadOnly = true;
            this.huntdata.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.huntdata.Size = new System.Drawing.Size(372, 319);
            this.huntdata.TabIndex = 3;
            this.huntdata.TabStop = false;
            this.huntdata.Text = "";
            this.huntdata.TextChanged += new System.EventHandler(this.huntdata_TextChanged);
            this.huntdata.MouseDown += new System.Windows.Forms.MouseEventHandler(this.huntdata_MouseDown);
            this.huntdata.MouseMove += new System.Windows.Forms.MouseEventHandler(this.huntdata_MouseMove);
            this.huntdata.MouseUp += new System.Windows.Forms.MouseEventHandler(this.huntdata_MouseUp);
            // 
            // btn_settings
            // 
            this.btn_settings.BackColor = System.Drawing.Color.Turquoise;
            this.btn_settings.FlatAppearance.BorderSize = 0;
            this.btn_settings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_settings.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.btn_settings.Location = new System.Drawing.Point(364, 13);
            this.btn_settings.Name = "btn_settings";
            this.btn_settings.Size = new System.Drawing.Size(10, 10);
            this.btn_settings.TabIndex = 4;
            this.btn_settings.UseVisualStyleBackColor = false;
            this.btn_settings.Click += new System.EventHandler(this.btn_settings_Click);
            // 
            // mainform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Maroon;
            this.ClientSize = new System.Drawing.Size(400, 343);
            this.ControlBox = false;
            this.Controls.Add(this.btn_settings);
            this.Controls.Add(this.huntdata);
            this.Controls.Add(this.btn_close);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "mainform";
            this.Text = "Eorzea Safari";
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.Color.Maroon;
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btn_close;
        private System.Windows.Forms.RichTextBox huntdata;
        private System.Windows.Forms.Button btn_settings;
    }
}

