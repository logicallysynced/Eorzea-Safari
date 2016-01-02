using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using HtmlAgilityPack;
using Lexicons;
using System.Net.Http;
using System.Net;
using System.IO;

namespace FFXIV_Safari
{
    public partial class mainform : Form
    {
        Form mainformA;
        settings settings = new settings();
        private bool dragging;
        private Point pointClicked;
        Lexicon<string, string> elitemarksA = new Lexicon<string, string>();
        Lexicon<string, string> elitemarksS = new Lexicon<string, string>();
        Dictionary<string, string> locations = new Dictionary<string, string>();
        SortedLexicon<TimeSpan, string> finaloutputA = new SortedLexicon<TimeSpan, string>();
        SortedLexicon<TimeSpan, string> finaloutputS = new SortedLexicon<TimeSpan, string>();
        TimeSpan force = TimeSpan.Parse("02:30");
        TimeSpan first = TimeSpan.Parse("00:10");
        TimeSpan miss = TimeSpan.Parse("99:00:00");
        TimeSpan S1 = TimeSpan.Parse("01.00:00:00");
        TimeSpan S2 = TimeSpan.Parse("02.00:00:00");
        System.Timers.Timer refresh;
        public int interval;

        [DllImport("user32.dll", EntryPoint = "ShowCaret")]
        public static extern long ShowCaret(IntPtr hwnd);
        [DllImport("user32.dll", EntryPoint = "HideCaret")]
        public static extern long HideCaret(IntPtr hwnd);

        public mainform()
        {
            InitializeComponent();

            interval = Decimal.ToInt32(FFXIV_Safari.Properties.Settings.Default.nm_interval * 1000);

            locations.Add("Forneus", "Central Shroud");
            locations.Add("Girtab", "North Shroud");
            locations.Add("Melt", "East Shroud");
            locations.Add("Ghede Ti Malice", "South Shroud");
            locations.Add("Laideronnette", "Central Shroud");
            locations.Add("Thousand-cast Theda", "North Shroud");
            locations.Add("Wulgaru", "East Shroud");
            locations.Add("Mindflayer", "South Shroud");
            locations.Add("Sabotender Bailarina", "Central Thanalan");
            locations.Add("Alectyron", "Western Thanalan");
            locations.Add("Zanig'oh", "Southern Thanalan");
            locations.Add("Dalvag's Final Flame", "Northern Thanalan");
            locations.Add("Maahes", "Eastern Thanalan");
            locations.Add("Brontes", "Central Thanalan");
            locations.Add("Zona Seeker", "Western Thanalan");
            locations.Add("Nunyunuwi", "Southern Thanalan");
            locations.Add("Minhocao", "Northern Thanalan");
            locations.Add("Lampalagua", "Eastern Thanalan");
            locations.Add("Hellsclaw", "Eastern La Noscea");
            locations.Add("Unktehi", "Lower La Noscea");
            locations.Add("Vogaal Ja", "Middle La Noscea");
            locations.Add("Cornu", "Outer La Noscea");
            locations.Add("Marberry", "Upper La Noscea");
            locations.Add("Nahn", "Western La Noscea");
            locations.Add("Garlok", "Eastern La Noscea");
            locations.Add("Croakadile", "Lower La Noscea");
            locations.Add("Croque-Mitaine", "Middle La Noscea");
            locations.Add("Mahisha", "Outer La Noscea");
            locations.Add("Nandi", "Upper La Noscea");
            locations.Add("Bonnacon", "Western La Noscea");
            locations.Add("Mirka", "Coerthas Western Highlands");
            locations.Add("Lyuba", "Coerthas Western Highlands");
            locations.Add("Marraco", "Coerthas Central Highlands");
            locations.Add("Kurrea", "Mor Dhona");
            locations.Add("Kaiser Behemoth", "Coerthas Western Highlands");
            locations.Add("Safat", "Coerthas Central Highlands");
            locations.Add("Agrippa the Mighty", "Mor Dhona");
            locations.Add("Pylraster", "Dravanian Forelands");
            locations.Add("Lord of the Wyverns", "Dravanian Forelands");
            locations.Add("Slipkinx Steeljoints", "Dravanian Hinterlands");
            locations.Add("Stolas", "Dravanian Hinterlands");
            locations.Add("Bune", "Churning Mists");
            locations.Add("Agathos", "Churning Mists");
            locations.Add("Senmurv", "Dravanian Forelands");
            locations.Add("The Pale Rider", "Dravanian Hinterlands");
            locations.Add("Gandarewa", "Churning Mists");
            locations.Add("Campacti", "Azys Lla");
            locations.Add("Stench Blossom", "Azys Lla");
            locations.Add("Enkelados", "The Sea of Clouds");
            locations.Add("Sisiutl", "The Sea of Clouds");
            locations.Add("Leucrotta", "Azys Lla");
            locations.Add("Bird of Paradise", "The Sea of Clouds");

            mainformA = this;
            btn_close.BringToFront();
            btn_settings.BringToFront();
            huntdata.BackColor = System.Drawing.ColorTranslator.FromHtml(FFXIV_Safari.Properties.Settings.Default.cd_backcol);
            Page_Load();

            string currentVersion = "1.2.0".Replace(".", string.Empty);
            string newVersion = currentVersion;
            var webRequest = WebRequest.Create(@"http://thejourneynetwork.net/eorzeasafari/update/version.txt");

            using (var response = webRequest.GetResponse())
            using (var content = response.GetResponseStream())
            using (var reader = new StreamReader(content))
            {
                string newVersionA = reader.ReadToEnd();
                newVersion = newVersionA.Replace(".", string.Empty);
            }

            int cV = int.Parse(currentVersion);
            int nV = int.Parse(newVersion);

            if (nV > cV)
            {
                DialogResult result = MessageBox.Show("There is an updated version of Eorzea Safari. Would you like to download it now?", "New Version", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start("http://www.google.com");
                }
            }

            refresh = new System.Timers.Timer();
            refresh.Elapsed += (source, e) => { Page_Load(); };
            refresh.Interval = interval;
            refresh.Enabled = true;
        }

        private delegate void BlinkDelegate();
        protected async void Page_Load()
        {
            HttpClient client = new HttpClient();
            var doc = new HtmlAgilityPack.HtmlDocument();
            var html = await client.GetStringAsync("http://ffxivhunt.com/" + FFXIV_Safari.Properties.Settings.Default.serverselect.ToLower());
            doc.LoadHtml(html);

            if (this.InvokeRequired)
            {
                BlinkDelegate del = new BlinkDelegate(Page_Load);
                this.Invoke(del);
            }
            else
            {
                huntdata.Suspend();
                elitemarksA.Clear();
                elitemarksS.Clear();
                finaloutputA.Clear();
                finaloutputS.Clear();
                huntdata.Clear();

                elitemarksA.Add("Forneus", doc.DocumentNode.SelectNodes("/html/body/div[@id='timer']/div[@class='row breath']/div[@class='col-md-3 col-sm-6'][2]/div[@class='panel panel-info'][1]/table[@class='table table-condensed table-hover'][1]/tbody/tr[@class='hot-toggle'][1]/td[@class='t140']/a[@id='mob_35']/span[@class='time']")[0].InnerText);
                elitemarksA.Add("Girtab", doc.DocumentNode.SelectNodes("/html/body/div[@id='timer']/div[@class='row breath']/div[@class='col-md-3 col-sm-6'][2]/div[@class='panel panel-info'][1]/table[@class='table table-condensed table-hover'][3]/tbody/tr[@class='hot-toggle'][1]/td[@class='t140']/a[@id='mob_38']/span[@class='time']")[0].InnerText);
                elitemarksA.Add("Melt", doc.DocumentNode.SelectNodes("/html/body/div[@id='timer']/div[@class='row breath']/div[@class='col-md-3 col-sm-6'][2]/div[@class='panel panel-info'][1]/table[@class='table table-condensed table-hover'][2]/tbody/tr[@class='hot-toggle'][1]/td[@class='t140']/a[@id='mob_5']/span[@class='time']")[0].InnerText);
                elitemarksA.Add("Ghede Ti Malice", doc.DocumentNode.SelectNodes("/html/body/div[@id='timer']/div[@class='row breath']/div[@class='col-md-3 col-sm-6'][2]/div[@class='panel panel-info'][1]/table[@class='table table-condensed table-hover'][4]/tbody/tr[@class='hot-toggle'][1]/td[@class='t140']/a[@id='mob_32']/span[@class='time']")[0].InnerText);
                elitemarksS.Add("Laideronnette", doc.DocumentNode.SelectNodes("/html/body/div[@id='timer']/div[@class='row breath']/div[@class='col-md-3 col-sm-6'][2]/div[@class='panel panel-info'][1]/table[@class='table table-condensed table-hover'][1]/tbody/tr[@class='hot-toggle'][2]/td[@class='t140']/a[@id='mob_36']/span[@class='time']")[0].InnerText);
                elitemarksS.Add("Thousand-cast Theda", doc.DocumentNode.SelectNodes("/html/body/div[@id='timer']/div[@class='row breath']/div[@class='col-md-3 col-sm-6'][2]/div[@class='panel panel-info'][1]/table[@class='table table-condensed table-hover'][3]/tbody/tr[@class='hot-toggle'][2]/td[@class='t140']/a[@id='mob_39']/span[@class='time']")[0].InnerText);
                elitemarksS.Add("Wulgaru", doc.DocumentNode.SelectNodes("/html/body/div[@id='timer']/div[@class='row breath']/div[@class='col-md-3 col-sm-6'][2]/div[@class='panel panel-info'][1]/table[@class='table table-condensed table-hover'][2]/tbody/tr[@class='hot-toggle'][2]/td[@class='t140']/a[@id='mob_6']/span[@class='time']")[0].InnerText);
                elitemarksS.Add("Mindflayer", doc.DocumentNode.SelectNodes("/html/body/div[@id='timer']/div[@class='row breath']/div[@class='col-md-3 col-sm-6'][2]/div[@class='panel panel-info'][1]/table[@class='table table-condensed table-hover'][4]/tbody/tr[@class='hot-toggle'][2]/td[@class='t140']/a[@id='mob_33']/span[@class='time']")[0].InnerText);
                elitemarksA.Add("Sabotender Bailarina", doc.DocumentNode.SelectNodes("/html/body/div[@id='timer']/div[@class='row breath']/div[@class='col-md-3 col-sm-6'][3]/div[@class='panel panel-info'][1]/table[@class='table table-condensed table-hover'][1]/tbody/tr[@class='hot-toggle'][1]/td[@class='t140']/a[@id='mob_50']/span[@class='time']")[0].InnerText);
                elitemarksA.Add("Alectyron", doc.DocumentNode.SelectNodes("/html/body/div[@id='timer']/div[@class='row breath']/div[@class='col-md-3 col-sm-6'][3]/div[@class='panel panel-info'][1]/table[@class='table table-condensed table-hover'][5]/tbody/tr[@class='hot-toggle'][1]/td[@class='t140']/a[@id='mob_8']/span[@class='time']")[0].InnerText);
                elitemarksA.Add("Zanig'oh", doc.DocumentNode.SelectNodes("/html/body/div[@id='timer']/div[@class='row breath']/div[@class='col-md-3 col-sm-6'][3]/div[@class='panel panel-info'][1]/table[@class='table table-condensed table-hover'][4]/tbody/tr[@class='hot-toggle'][1]/td[@class='t140']/a[@id='mob_44']/span[@class='time']")[0].InnerText);
                elitemarksA.Add("Dalvag's Final Flame", doc.DocumentNode.SelectNodes("/html/body/div[@id='timer']/div[@class='row breath']/div[@class='col-md-3 col-sm-6'][3]/div[@class='panel panel-info'][1]/table[@class='table table-condensed table-hover'][3]/tbody/tr[@class='hot-toggle'][1]/td[@class='t140']/a[@id='mob_47']/span[@class='time']")[0].InnerText);
                elitemarksA.Add("Maahes", doc.DocumentNode.SelectNodes("/html/body/div[@id='timer']/div[@class='row breath']/div[@class='col-md-3 col-sm-6'][3]/div[@class='panel panel-info'][1]/table[@class='table table-condensed table-hover'][2]/tbody/tr[@class='hot-toggle'][1]/td[@class='t140']/a[@id='mob_41']/span[@class='time']")[0].InnerText);
                elitemarksS.Add("Brontes", doc.DocumentNode.SelectNodes("/html/body/div[@id='timer']/div[@class='row breath']/div[@class='col-md-3 col-sm-6'][3]/div[@class='panel panel-info'][1]/table[@class='table table-condensed table-hover'][1]/tbody/tr[@class='hot-toggle'][2]/td[@class='t140']/a[@id='mob_51']/span[@class='time']")[0].InnerText);
                elitemarksS.Add("Zona Seeker", doc.DocumentNode.SelectNodes("/html/body/div[@id='timer']/div[@class='row breath']/div[@class='col-md-3 col-sm-6'][3]/div[@class='panel panel-info'][1]/table[@class='table table-condensed table-hover'][5]/tbody/tr[@class='hot-toggle'][2]/td[@class='t140']/a[@id='mob_9']/span[@class='time']")[0].InnerText);
                elitemarksS.Add("Nunyunuwi", doc.DocumentNode.SelectNodes("/html/body/div[@id='timer']/div[@class='row breath']/div[@class='col-md-3 col-sm-6'][3]/div[@class='panel panel-info'][1]/table[@class='table table-condensed table-hover'][4]/tbody/tr[@class='hot-toggle'][2]/td[@class='t140']/a[@id='mob_45']/span[@class='time']")[0].InnerText);
                elitemarksS.Add("Minhocao", doc.DocumentNode.SelectNodes("/html/body/div[@id='timer']/div[@class='row breath']/div[@class='col-md-3 col-sm-6'][3]/div[@class='panel panel-info'][1]/table[@class='table table-condensed table-hover'][3]/tbody/tr[@class='hot-toggle'][2]/td[@class='t140']/a[@id='mob_48']/span[@class='time']")[0].InnerText);
                elitemarksS.Add("Lampalagua", doc.DocumentNode.SelectNodes("/html/body/div[@id='timer']/div[@class='row breath']/div[@class='col-md-3 col-sm-6'][3]/div[@class='panel panel-info'][1]/table[@class='table table-condensed table-hover'][2]/tbody/tr[@class='hot-toggle'][2]/td[@class='t140']/a[@id='mob_42']/span[@class='time']")[0].InnerText);
                elitemarksA.Add("Hellsclaw", doc.DocumentNode.SelectNodes("/html/body/div[@id='timer']/div[@class='row breath']/div[@class='col-md-3 col-sm-6'][1]/div[@class='panel panel-info']/table[@class='table table-condensed table-hover'][1]/tbody/tr[@class='hot-toggle'][1]/td[@class='t140']/a[@id='mob_20']/span[@class='time']")[0].InnerText);
                elitemarksA.Add("Unktehi", doc.DocumentNode.SelectNodes("/html/body/div[@id='timer']/div[@class='row breath']/div[@class='col-md-3 col-sm-6'][1]/div[@class='panel panel-info']/table[@class='table table-condensed table-hover'][2]/tbody/tr[@class='hot-toggle'][1]/td[@class='t140']/a[@id='mob_23']/span[@class='time']")[0].InnerText);
                elitemarksA.Add("Vogaal Ja", doc.DocumentNode.SelectNodes("/html/body/div[@id='timer']/div[@class='row breath']/div[@class='col-md-3 col-sm-6'][1]/div[@class='panel panel-info']/table[@class='table table-condensed table-hover'][3]/tbody/tr[@class='hot-toggle'][1]/td[@class='t140']/a[@id='mob_17']/span[@class='time']")[0].InnerText);
                elitemarksA.Add("Cornu", doc.DocumentNode.SelectNodes("/html/body/div[@id='timer']/div[@class='row breath']/div[@class='col-md-3 col-sm-6'][1]/div[@class='panel panel-info']/table[@class='table table-condensed table-hover'][4]/tbody/tr[@class='hot-toggle'][1]/td[@class='t140']/a[@id='mob_29']/span[@class='time']")[0].InnerText);
                elitemarksA.Add("Marberry", doc.DocumentNode.SelectNodes("/html/body/div[@id='timer']/div[@class='row breath']/div[@class='col-md-3 col-sm-6'][1]/div[@class='panel panel-info']/table[@class='table table-condensed table-hover'][5]/tbody/tr[@class='hot-toggle'][1]/td[@class='t140']/a[@id='mob_26']/span[@class='time']")[0].InnerText);
                elitemarksA.Add("Nahn", doc.DocumentNode.SelectNodes("/html/body/div[@id='timer']/div[@class='row breath']/div[@class='col-md-3 col-sm-6'][1]/div[@class='panel panel-info']/table[@class='table table-condensed table-hover'][6]/tbody/tr[@class='hot-toggle'][1]/td[@class='t140']/a[@id='mob_2']/span[@class='time']")[0].InnerText);
                elitemarksS.Add("Garlok", doc.DocumentNode.SelectNodes("/html/body/div[@id='timer']/div[@class='row breath']/div[@class='col-md-3 col-sm-6'][1]/div[@class='panel panel-info']/table[@class='table table-condensed table-hover'][1]/tbody/tr[@class='hot-toggle'][2]/td[@class='t140']/a[@id='mob_21']/span[@class='time']")[0].InnerText);
                elitemarksS.Add("Croakadile", doc.DocumentNode.SelectNodes("/html/body/div[@id='timer']/div[@class='row breath']/div[@class='col-md-3 col-sm-6'][1]/div[@class='panel panel-info']/table[@class='table table-condensed table-hover'][2]/tbody/tr[@class='hot-toggle'][2]/td[@class='t140']/a[@id='mob_24']/span[@class='time']")[0].InnerText);
                elitemarksS.Add("Croque-Mitaine", doc.DocumentNode.SelectNodes("/html/body/div[@id='timer']/div[@class='row breath']/div[@class='col-md-3 col-sm-6'][1]/div[@class='panel panel-info']/table[@class='table table-condensed table-hover'][3]/tbody/tr[@class='hot-toggle'][2]/td[@class='t140']/a[@id='mob_18']/span[@class='time']")[0].InnerText);
                elitemarksS.Add("Mahisha", doc.DocumentNode.SelectNodes("/html/body/div[@id='timer']/div[@class='row breath']/div[@class='col-md-3 col-sm-6'][1]/div[@class='panel panel-info']/table[@class='table table-condensed table-hover'][4]/tbody/tr[@class='hot-toggle'][2]/td[@class='t140']/a[@id='mob_30']/span[@class='time']")[0].InnerText);
                elitemarksS.Add("Nandi", doc.DocumentNode.SelectNodes("/html/body/div[@id='timer']/div[@class='row breath']/div[@class='col-md-3 col-sm-6'][1]/div[@class='panel panel-info']/table[@class='table table-condensed table-hover'][5]/tbody/tr[@class='hot-toggle'][2]/td[@class='t140']/a[@id='mob_27']/span[@class='time']")[0].InnerText);
                elitemarksS.Add("Bonnacon", doc.DocumentNode.SelectNodes("/html/body/div[@id='timer']/div[@class='row breath']/div[@class='col-md-3 col-sm-6'][1]/div[@class='panel panel-info']/table[@class='table table-condensed table-hover'][6]/tbody/tr[@class='hot-toggle'][2]/td[@class='t140']/a[@id='mob_3']/span[@class='time']")[0].InnerText);
                elitemarksA.Add("Mirka", doc.DocumentNode.SelectNodes("/html/body/div[@id='timer']/div[@class='row breath']/div[@class='col-md-3 col-sm-6'][2]/div[@class='panel panel-info'][2]/table[@class='table table-condensed table-hover'][2]/tbody/tr[@class='hot-toggle'][1]/td[@class='t140']/a[@id='mob_53']/span[@class='time']")[0].InnerText);
                elitemarksA.Add("Lyuba", doc.DocumentNode.SelectNodes("/html/body/div[@id='timer']/div[@class='row breath']/div[@class='col-md-3 col-sm-6'][2]/div[@class='panel panel-info'][2]/table[@class='table table-condensed table-hover'][2]/tbody/tr[@class='hot-toggle'][2]/td[@class='t140']/a[@id='mob_68']/span[@class='time']")[0].InnerText);
                elitemarksA.Add("Marraco", doc.DocumentNode.SelectNodes("/html/body/div[@id='timer']/div[@class='row breath']/div[@class='col-md-3 col-sm-6'][2]/div[@class='panel panel-info'][2]/table[@class='table table-condensed table-hover'][1]/tbody/tr[@class='hot-toggle'][1]/td[@class='t140']/a[@id='mob_11']/span[@class='time']")[0].InnerText);
                elitemarksA.Add("Kurrea", doc.DocumentNode.SelectNodes("/html/body/div[@id='timer']/div[@class='row breath']/div[@class='col-md-3 col-sm-6'][3]/div[@class='panel panel-info'][2]/table[@class='table table-condensed table-hover']/tbody/tr[@class='hot-toggle'][1]/td[@class='t140']/a[@id='mob_14']/span[@class='time']")[0].InnerText);
                elitemarksS.Add("Kaiser Behemoth", doc.DocumentNode.SelectNodes("/html/body/div[@id='timer']/div[@class='row breath']/div[@class='col-md-3 col-sm-6'][3]/div[@class='panel panel-info'][2]/table[@class='table table-condensed table-hover']/tbody/tr[@class='hot-toggle'][1]/td[@class='t140']/a[@id='mob_14']/span[@class='time']")[0].InnerText);
                elitemarksS.Add("Safat", doc.DocumentNode.SelectNodes("/html/body/div[@id='timer']/div[@class='row breath']/div[@class='col-md-3 col-sm-6'][2]/div[@class='panel panel-info'][2]/table[@class='table table-condensed table-hover'][1]/tbody/tr[@class='hot-toggle'][2]/td[@class='t140']/a[@id='mob_12']/span[@class='time']")[0].InnerText);
                elitemarksS.Add("Agrippa the Mighty", doc.DocumentNode.SelectNodes("/html/body/div[@id='timer']/div[@class='row breath']/div[@class='col-md-3 col-sm-6'][3]/div[@class='panel panel-info'][2]/table[@class='table table-condensed table-hover']/tbody/tr[@class='hot-toggle'][2]/td[@class='t140']/a[@id='mob_15']/span[@class='time']")[0].InnerText);
                elitemarksA.Add("Pylraster", doc.DocumentNode.SelectNodes("/html/body/div[@id='timer']/div[@class='row breath']/div[@class='col-md-3 col-sm-6'][4]/div[@class='panel panel-info'][1]/table[@class='table table-condensed table-hover'][2]/tbody/tr[@class='hot-toggle'][1]/td[@class='t140']/a[@id='mob_59']/span[@class='time']")[0].InnerText);
                elitemarksA.Add("Lord of the Wyverns", doc.DocumentNode.SelectNodes("/html/body/div[@id='timer']/div[@class='row breath']/div[@class='col-md-3 col-sm-6'][4]/div[@class='panel panel-info'][1]/table[@class='table table-condensed table-hover'][2]/tbody/tr[@class='hot-toggle'][2]/td[@class='t140']/a[@id='mob_75']/span[@class='time']")[0].InnerText);
                elitemarksA.Add("Slipkinx Steeljoints", doc.DocumentNode.SelectNodes("/html/body/div[@id='timer']/div[@class='row breath']/div[@class='col-md-3 col-sm-6'][4]/div[@class='panel panel-info'][1]/table[@class='table table-condensed table-hover'][3]/tbody/tr[@class='hot-toggle'][1]/td[@class='t140']/a[@id='mob_62']/span[@class='time']")[0].InnerText);
                elitemarksA.Add("Stolas", doc.DocumentNode.SelectNodes("/html/body/div[@id='timer']/div[@class='row breath']/div[@class='col-md-3 col-sm-6'][4]/div[@class='panel panel-info'][1]/table[@class='table table-condensed table-hover'][3]/tbody/tr[@class='hot-toggle'][2]/td[@class='t140']/a[@id='mob_77']/span[@class='time']")[0].InnerText);
                elitemarksA.Add("Bune", doc.DocumentNode.SelectNodes("/html/body/div[@id='timer']/div[@class='row breath']/div[@class='col-md-3 col-sm-6'][4]/div[@class='panel panel-info'][1]/table[@class='table table-condensed table-hover'][1]/tbody/tr[@class='hot-toggle'][1]/td[@class='t140']/a[@id='mob_65']/span[@class='time']")[0].InnerText);
                elitemarksA.Add("Agathos", doc.DocumentNode.SelectNodes("/html/body/div[@id='timer']/div[@class='row breath']/div[@class='col-md-3 col-sm-6'][4]/div[@class='panel panel-info'][1]/table[@class='table table-condensed table-hover'][1]/tbody/tr[@class='hot-toggle'][2]/td[@class='t140']/a[@id='mob_81']/span[@class='time']")[0].InnerText);
                elitemarksS.Add("Senmurv", doc.DocumentNode.SelectNodes("/html/body/div[@id='timer']/div[@class='row breath']/div[@class='col-md-3 col-sm-6'][4]/div[@class='panel panel-info'][1]/table[@class='table table-condensed table-hover'][2]/tbody/tr[@class='hot-toggle'][3]/td[@class='t140']/a[@id='mob_60']/span[@class='time']")[0].InnerText);
                elitemarksS.Add("The Pale Rider", doc.DocumentNode.SelectNodes("/html/body/div[@id='timer']/div[@class='row breath']/div[@class='col-md-3 col-sm-6'][4]/div[@class='panel panel-info'][1]/table[@class='table table-condensed table-hover'][3]/tbody/tr[@class='hot-toggle'][3]/td[@class='t140']/a[@id='mob_63']/span[@class='time']")[0].InnerText);
                elitemarksS.Add("Gandarewa", doc.DocumentNode.SelectNodes("/html/body/div[@id='timer']/div[@class='row breath']/div[@class='col-md-3 col-sm-6'][4]/div[@class='panel panel-info'][1]/table[@class='table table-condensed table-hover'][1]/tbody/tr[@class='hot-toggle'][3]/td[@class='t140']/a[@id='mob_66']/span[@class='time']")[0].InnerText);
                elitemarksA.Add("Campacti", doc.DocumentNode.SelectNodes("/html/body/div[@id='timer']/div[@class='row breath']/div[@class='col-md-3 col-sm-6'][4]/div[@class='panel panel-info'][2]/table[@class='table table-condensed table-hover'][1]/tbody/tr[@class='hot-toggle'][1]/td[@class='t140']/a[@id='mob_71']/span[@class='time']")[0].InnerText);
                elitemarksA.Add("Stench Blossom", doc.DocumentNode.SelectNodes("/html/body/div[@id='timer']/div[@class='row breath']/div[@class='col-md-3 col-sm-6'][4]/div[@class='panel panel-info'][2]/table[@class='table table-condensed table-hover'][1]/tbody/tr[@class='hot-toggle'][2]/td[@class='t140']/a[@id='mob_72']/span[@class='time']")[0].InnerText);
                elitemarksA.Add("Enkelados", doc.DocumentNode.SelectNodes("/html/body/div[@id='timer']/div[@class='row breath']/div[@class='col-md-3 col-sm-6'][4]/div[@class='panel panel-info'][2]/table[@class='table table-condensed table-hover'][2]/tbody/tr[@class='hot-toggle'][1]/td[@class='t140']/a[@id='mob_56']/span[@class='time']")[0].InnerText);
                elitemarksA.Add("Sisiutl", doc.DocumentNode.SelectNodes("/html/body/div[@id='timer']/div[@class='row breath']/div[@class='col-md-3 col-sm-6'][4]/div[@class='panel panel-info'][2]/table[@class='table table-condensed table-hover'][2]/tbody/tr[@class='hot-toggle'][2]/td[@class='t140']/a[@id='mob_79']/span[@class='time']")[0].InnerText);
                elitemarksS.Add("Leucrotta", doc.DocumentNode.SelectNodes("/html/body/div[@id='timer']/div[@class='row breath']/div[@class='col-md-3 col-sm-6'][4]/div[@class='panel panel-info'][2]/table[@class='table table-condensed table-hover'][1]/tbody/tr[@class='hot-toggle'][3]/td[@class='t140']/a[@id='mob_73']/span[@class='time']")[0].InnerText);
                elitemarksS.Add("Bird of Paradise", doc.DocumentNode.SelectNodes("/html/body/div[@id='timer']/div[@class='row breath']/div[@class='col-md-3 col-sm-6'][4]/div[@class='panel panel-info'][2]/table[@class='table table-condensed table-hover'][2]/tbody/tr[@class='hot-toggle'][3]/td[@class='t140']/a[@id='mob_57']/span[@class='time']")[0].InnerText);

                printData();
            }
        }

        private void printData()
        {

            if (FFXIV_Safari.Properties.Settings.Default.chk_aranks == true)
            {
                huntdata.SelectionColor = Color.Tomato;
                huntdata.SelectionFont = new Font("Myriad Pro", 11, FontStyle.Bold);
                huntdata.AppendText("Currently Open A Ranks \r\n");


                foreach (KeyValuePair<string, string> huntA in elitemarksA)
                {
                    string[] detect = huntA.Value.Split(' ');
                    var loc = locations[huntA.Key];
                    string outputA;
                    string outputB;
                    TimeSpan ts;

                    if (detect[0] == "open")
                    {
                        if (detect[2].Contains("min"))
                        {
                            outputA = detect[2].Remove(detect[2].Length - 3);
                            if (outputA.Length == 1)
                            {
                                outputB = "00:0" + outputA;
                                ts = TimeSpan.Parse(outputB);
                            }
                            else
                            {
                                outputB = "00:" + outputA;
                                ts = TimeSpan.Parse(outputB);
                            }
                        }
                        else
                        {
                            string[] time = detect[2].Split(':');
                            double outputC = Convert.ToDouble(time[0]);
                            double outputD = Convert.ToDouble(time[1]);
                            string timestampA;
                            string timestampB;

                            if (outputC.ToString().Length == 1)
                            {
                                timestampA = "0" + outputC.ToString();
                            }
                            else
                            {
                                timestampA = outputC.ToString();
                            }

                            if (outputD.ToString().Length == 1)
                            {
                                timestampB = outputD.ToString() + "0";
                            }
                            else
                            {
                                timestampB = outputD.ToString();
                            }
                            outputB = timestampA + ":" + timestampB;

                            ts = TimeSpan.FromHours(outputC) + TimeSpan.FromMinutes(outputD);
                        }


                        if (ts >= force)
                        {
                            finaloutputA.Add(ts, loc + " (" + huntA.Key + ") - " + outputB + " Forced \r\n");
                        }
                        else
                        {
                            finaloutputA.Add(ts, loc + " (" + huntA.Key + ") - " + outputB + "\r\n");
                        }

                    }
                    else if (detect[0] == "no")
                    {
                        finaloutputA.Add(miss, loc + " (" + huntA.Key + ") - Missing \r\n");
                    }

                }
                

                foreach (KeyValuePair<TimeSpan, string> finalA in finaloutputA.Reverse())
                {
                    if (finalA.Key == miss)
                    {
                        huntdata.SelectionColor = Color.Gold;
                        huntdata.SelectionFont = new Font("Myriad Pro", 10, FontStyle.Regular);
                        huntdata.AppendText(finalA.Value);
                    }
                    else if (finalA.Key >= force)
                    {
                        huntdata.SelectionColor = Color.DodgerBlue;
                        huntdata.SelectionFont = new Font("Myriad Pro", 10, FontStyle.Regular);
                        huntdata.AppendText(finalA.Value);
                    }
                    else if (finalA.Key < first)
                    {
                        huntdata.SelectionColor = Color.MediumSpringGreen;
                        huntdata.SelectionFont = new Font("Myriad Pro", 10, FontStyle.Regular);
                        huntdata.AppendText(finalA.Value);
                    }
                    else
                    {
                        huntdata.SelectionColor = Color.LimeGreen;
                        huntdata.SelectionFont = new Font("Myriad Pro", 10, FontStyle.Regular);
                        huntdata.AppendText(finalA.Value);
                    }
                }
                if (FFXIV_Safari.Properties.Settings.Default.chk_sranks == false)
                {
                    huntdata.Select(0, 0);
                }
            }

            if (FFXIV_Safari.Properties.Settings.Default.chk_sranks == true)
            {
                huntdata.SelectionColor = Color.Tomato;
                huntdata.SelectionFont = new Font("Myriad Pro", 11, FontStyle.Bold);
                if (FFXIV_Safari.Properties.Settings.Default.chk_aranks == false)
                {
                    huntdata.AppendText("Currently Open S Ranks \r\n");
                }
                else
                {
                    huntdata.AppendText(Environment.NewLine + "Currently Open S Ranks \r\n");
                }

                foreach (KeyValuePair<string, string> huntS in elitemarksS)
                {
                    string[] detect = huntS.Value.Split(' ');
                    var loc = locations[huntS.Key];
                    string outputA;
                    string outputB;
                    TimeSpan ts;

                    if (detect[0] == "open")
                    {
                        if (detect[2].Contains("min"))
                        {
                            outputA = detect[2].Remove(detect[2].Length - 3);
                            if (outputA.Length == 1)
                            {
                                outputB = "00:0" + outputA;
                                ts = TimeSpan.Parse(outputB);
                            }
                            else
                            {
                                outputB = "00:" + outputA;
                                ts = TimeSpan.Parse(outputB);
                            }
                        }
                        else
                        {
                            string[] time = detect[2].Split(':');
                            double outputC = Convert.ToDouble(time[0]);
                            double outputD = Convert.ToDouble(time[1]);
                            string timestampA;
                            string timestampB;

                            if (outputC.ToString().Length == 1)
                            {
                                timestampA = "0" + outputC.ToString();
                            }
                            else
                            {
                                timestampA = outputC.ToString();
                            }

                            if (outputD.ToString().Length == 1)
                            {
                                timestampB = outputD.ToString() + "0";
                            }
                            else
                            {
                                timestampB = outputD.ToString();
                            }
                            outputB = timestampA + ":" + timestampB;

                            ts = TimeSpan.FromHours(outputC) + TimeSpan.FromMinutes(outputD);
                        }


                        if (ts >= force)
                        {
                            finaloutputS.Add(ts, loc + " (" + huntS.Key + ") - " + outputB + "\r\n");
                        }
                        else
                        {
                            finaloutputS.Add(ts, loc + " (" + huntS.Key + ") - " + outputB + "\r\n");
                        }

                    }
                    else if (detect[0] == "no")
                    {
                        finaloutputS.Add(miss, loc + " (" + huntS.Key + ") - Missing \r\n");
                    }

                }


                foreach (KeyValuePair<TimeSpan, string> finalS in finaloutputS.Reverse())
                {
                    if (finalS.Key == miss)
                    {
                        huntdata.SelectionColor = Color.Gold;
                        huntdata.SelectionFont = new Font("Myriad Pro", 10, FontStyle.Regular);
                        huntdata.AppendText(finalS.Value);
                    }
                    else if (finalS.Key >= S2)
                    {
                        huntdata.SelectionColor = Color.DodgerBlue;
                        huntdata.SelectionFont = new Font("Myriad Pro", 10, FontStyle.Regular);
                        huntdata.AppendText(finalS.Value);
                    }
                    else if (finalS.Key >= S1)
                    {
                        huntdata.SelectionColor = Color.LimeGreen;
                        huntdata.SelectionFont = new Font("Myriad Pro", 10, FontStyle.Regular);
                        huntdata.AppendText(finalS.Value);
                    }
                    else
                    {
                        huntdata.SelectionColor = Color.MediumSpringGreen;
                        huntdata.SelectionFont = new Font("Myriad Pro", 10, FontStyle.Regular);
                        huntdata.AppendText(finalS.Value);
                    }
                }

                huntdata.Select(0, 0);
            }

            huntdata.Resume();
        }

        private void TextBoxGotFocus(object sender, EventArgs args)
        {
            HideCaret(huntdata.Handle);
        }

        private void huntdata_MouseDown(object sender, MouseEventArgs e)
        {
            HideCaret(huntdata.Handle);
            if (e.Button == MouseButtons.Left)
            {
                dragging = true;
                pointClicked = new Point(e.X, e.Y);
            }
            else
            {
                dragging = false;
            }
        }

        private void huntdata_MouseMove(object sender, MouseEventArgs e)
        {
            HideCaret(huntdata.Handle);
            if (dragging)
            {
                Point pointMoveTo;
                pointMoveTo = this.PointToScreen(new Point(e.X, e.Y));

                pointMoveTo.Offset(-pointClicked.X, -pointClicked.Y);

                this.Location = pointMoveTo;
            }
        }

        private void huntdata_MouseUp(object sender, MouseEventArgs e)
        {
            HideCaret(huntdata.Handle);
            dragging = false;
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void btn_settings_Click(object sender, EventArgs e)
        {
            settings settings = new settings();
            settings.FormClosed += Form1_FormClosed;
            settings.ShowDialog();
        }

        void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (FFXIV_Safari.Properties.Settings.Default.checktransparent == true)
            {
                huntdata.BackColor = System.Drawing.Color.Maroon;
            }
            else
            {
                huntdata.BackColor = System.Drawing.ColorTranslator.FromHtml(FFXIV_Safari.Properties.Settings.Default.cd_backcol);
            }

            interval = Decimal.ToInt32(FFXIV_Safari.Properties.Settings.Default.nm_interval * 1000);
            refresh.Interval = interval;
            Page_Load();
        }

        private void huntdata_TextChanged(object sender, EventArgs e)
        {
            Size size = TextRenderer.MeasureText(huntdata.Text, huntdata.Font);
            huntdata.Height = size.Height;
        }
    }
}

namespace System.Windows.Forms
{
    public static class ControlExtensions
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool LockWindowUpdate(IntPtr hWndLock);

        public static void Suspend(this Control control)
        {
            LockWindowUpdate(control.Handle);
        }

        public static void Resume(this Control control)
        {
            LockWindowUpdate(IntPtr.Zero);
        }

    }
}