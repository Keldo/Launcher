using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Launcher.Properties;
using System.Threading;

namespace Launcher
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // Check Patch
            checkPatch();

            // Check Launcher Version
            FileVersionInfo launcherVersion = FileVersionInfo.GetVersionInfo("Launcher.exe");
            string url = Settings1.Default.version;
            WebClient wc = new WebClient();
            string rVersion = wc.DownloadString(url);
            string remoteLVersion = rVersion;
            string localVersion = launcherVersion.FileVersion;
            if (localVersion == remoteLVersion)
            {
            label4.Text = "Launcher Version: " + launcherVersion.FileVersion;
            }
            else
            {
                label4.Text = "Launcher Update Required";
            }
            // End Check Launcher Version

            // Check Server Status
            bool status = false;
            try
            {
                TcpClient client = new TcpClient();

                client.Connect(Settings1.Default.baseURL, Settings1.Default.worldport);

                status = true;
            }
            catch (Exception)
            {
                status = false;
            }

            if (status == false)
            {
                label5.ForeColor = Color.Red;
                label5.Text = "Offline";
            }
            else
            {
                label5.ForeColor = Color.Green;
                label5.Text = "Online";
            }

            // End Check Server Status
        }

        // Close the Launcher
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Launch the WoW Client
        private void button2_Click(object sender, EventArgs e)
        {
            Process.Start("Wow_Patched.exe");

            this.Close();
        }

        // Check Patch Function
        private void checkPatch()
        {
            // Check for Wow Patch
            string patchexe = @"Wow_Patched.exe";
            if (!File.Exists(patchexe))
            {
                label1.Text = "WOW PATCH IS NOT INSTALLED";
                //button2.Image = Properties.Resources.PlayButtonDisabled;
                progressBar1.Visible = false;
                button4.Visible = true;
                getPatch();
            }
            else
            {
                label1.Text = "World of Warcraft is up to date";
                progressBar1.Visible = false;
                button2.Image = Properties.Resources.Play_No_Hover;
                button4.Visible = false;
            }
        }

        // Start Downloads
        // Should all be referenced in an array
        // then downloaded with one webClient Asynch Download
        private void getPatch()
        {
            var t = Task.Run(() => {
                label1.Text = "Downloading Connection Patcher";
                button2.Image = Properties.Resources.PlayButtonDisabled;
                //button4.Visible = true;
                progressBar1.Visible = true;
                progressBar1.BackColor = Color.Black;
                progressBar1.ForeColor = Color.DarkOrange;
            
                var webClient = new WebClient();
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                webClient.DownloadFileAsync(new Uri(Settings1.Default.connection_patcher), "connection_patcher.exe");
            });
            t.Wait();
            GetM1();
        }

        private void GetM1()
        {
            var t = Task.Run(() => {
                button2.Image = Properties.Resources.PlayButtonDisabled;
                //button4.Visible = true;
                label1.Text = "Downloading Readme PDF";
                var webClient = new WebClient();
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                webClient.DownloadFileAsync(new Uri(Settings1.Default.readme_patcher), "PATCHER_README.pdf");
            });
            t.Wait();
            GetM2();

        }

        private void GetM2()
        {
            var t = Task.Run(() => {
                button2.Image = Properties.Resources.PlayButtonDisabled;
                //button4.Visible = true;
                label1.Text = "Downloading";
                var webClient = new WebClient();
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                webClient.DownloadFileAsync(new Uri(Settings1.Default.libeay), "libeay32.dll");
            });
            t.Wait();
            GetM3();
        }

        private void GetM3()
        {
            var t = Task.Run(() => {
                button2.Image = Properties.Resources.PlayButtonDisabled;
                //button4.Visible = true;
                label1.Text = "Downloading";
                var webClient = new WebClient();
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                webClient.DownloadFileAsync(new Uri(Settings1.Default.libmysql), "libmysql.dll");
            });
            t.Wait();
            GetM4();
        }

        private void GetM4()
        {
            var t = Task.Run(() => {
                button2.Image = Properties.Resources.PlayButtonDisabled;
               // button4.Visible = true;
                label1.Text = "Downloading";
                var webClient = new WebClient();
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                webClient.DownloadFileAsync(new Uri(Settings1.Default.libssl), "libssl32.dll");
            });
            t.Wait();
            GetM5();
        }

        private void GetM5()
        {
            var t = Task.Run(() => {
                button2.Image = Properties.Resources.PlayButtonDisabled;
            //button4.Visible = true;
                label1.Text = "Downloading";
                var webClient = new WebClient();
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                webClient.DownloadFileAsync(new Uri(Settings1.Default.ssleay), "ssley32.dll");
                webClient.DownloadStringCompleted += OnGetDownloadedStringCompleted;
            });
            t.Wait();
            GetM6();
        }

        private void GetM6()
        {
            var t = Task.Run(() => {
                Directory.CreateDirectory("WTF");
                 button2.Image = Properties.Resources.PlayButtonDisabled;
            //button4.Visible = true;
                label1.Text = "Downloading Config";
                var webClient = new WebClient();
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                webClient.DownloadFileAsync(new Uri(Settings1.Default.config), "WTF\\Config.wtf");
                webClient.DownloadStringCompleted += OnGetDownloadedStringCompleted;
            });
            t.Wait();
            DownloadComplete();
        }

        // Download Complete
        // Does not show when downloads are finished
        private void DownloadComplete()
        {
            var t = Task.Run(() => {
                label1.Text = "Downloads Complete.";
                progressBar1.Visible = false;
                button4.Visible = true;
            });
            t.Wait();
            goPatch();
        }

        // Offer the Patch Button
        private void goPatch()
        {
            label1.Text = "Ready to Patch.";
            progressBar1.Visible = false;
            button4.Visible = true;
        }

        // Referenced but does not show on Download Completion
        private void OnGetDownloadedStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            label1.Text = "Download complete!";
            button4.Visible = true;
            progressBar1.Visible = false;
        }

        // Progress Bar
        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar1.Visible = true;
            progressBar1.Value = e.ProgressPercentage;
            progressBar1.BackColor = Color.Black;
            progressBar1.ForeColor = Color.DarkOrange;

            button2.Image = Properties.Resources.PlayButtonDisabled;
        }

        // Minimize
        private void button3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        // Patch the Client
        private void button4_Click(object sender, EventArgs e)
        {
            // This function needs the Task & Wait in order to run the patch sequence
            var t = Task.Run(() => {
               Process.Start(@"connection_patcher.exe", @"Wow.exe"); 
            });
            t.Wait();

            // Reload the GUI
            button4.Visible = false;
            progressBar1.Visible = false;
            button2.Image = Properties.Resources.Play_No_Hover;
            label1.Text = "World of Warcraft is up to date";
        }
    }
}
