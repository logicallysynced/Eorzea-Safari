using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FFXIV_Safari
{
    public partial class settings : Form
    {
        public bool settingsclosed { get; private set; }
        public settings()
        {
            InitializeComponent();
            chk_aranks.Checked = FFXIV_Safari.Properties.Settings.Default.chk_aranks;
            chk_sranks.Checked = FFXIV_Safari.Properties.Settings.Default.chk_sranks;
            chk_transparent.Checked = FFXIV_Safari.Properties.Settings.Default.checktransparent;
            cb_server.SelectedItem = FFXIV_Safari.Properties.Settings.Default.serverselect;
            nm_interval.Value = FFXIV_Safari.Properties.Settings.Default.nm_interval;
            btn_backCol.BackColor = System.Drawing.ColorTranslator.FromHtml(FFXIV_Safari.Properties.Settings.Default.cd_backcol);
            btn_backCol.ForeColor = System.Drawing.ColorTranslator.FromHtml(FFXIV_Safari.Properties.Settings.Default.cd_backcol);
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            btn_save.Text = "Saving..";
            FFXIV_Safari.Properties.Settings.Default.chk_aranks = chk_aranks.Checked;
            FFXIV_Safari.Properties.Settings.Default.chk_sranks = chk_sranks.Checked;
            FFXIV_Safari.Properties.Settings.Default.checktransparent = chk_transparent.Checked;
            FFXIV_Safari.Properties.Settings.Default.serverselect = cb_server.SelectedItem.ToString();
            FFXIV_Safari.Properties.Settings.Default.nm_interval = nm_interval.Value;
            FFXIV_Safari.Properties.Settings.Default.Save();
            this.Close();
        }

        private void btn_backCol_Click(object sender, EventArgs e)
        {
            DialogResult result = cd_back.ShowDialog();
            if (result == DialogResult.OK)
            {
                btn_backCol.BackColor = cd_back.Color;
                btn_backCol.ForeColor = cd_back.Color;
                FFXIV_Safari.Properties.Settings.Default.cd_backcol = ColorTranslator.ToHtml(cd_back.Color);
            }
        }

        private void chk_transparent_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_transparent.Checked)
            {
                btn_backCol.Enabled = false;
                btn_backCol.BackColor = System.Drawing.SystemColors.InactiveBorder;
                btn_backCol.ForeColor = System.Drawing.SystemColors.InactiveBorder;
            }
            else
            {
                btn_backCol.Enabled = true;
                btn_backCol.BackColor = cd_back.Color;
                btn_backCol.ForeColor = cd_back.Color;
            }
        }

        private void richTextBox1_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.LinkText);
        }
    }
}
