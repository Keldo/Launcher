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
        Stopwatch sw = new Stopwatch();
        public Form1()
        {
            InitializeComponent();
            button5.Visible = false;
            button4.Visible = false;

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

                client.Connect(Settings1.Default.realmlist, Settings1.Default.port);

                status = true;
            }
            catch (Exception)
            {
                status = false;
            }

            if (status)
            {
                label5.ForeColor = Color.Green;
                label5.Text = "Online";
            }
            else
            {
                label5.ForeColor = Color.Red;
                label5.Text = "Offline";
            }

            // End Check Server Status
            // System.Threading.Thread.Sleep(5000);
            checkWoW();
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

        // Check Wow Client Function
        private void checkWoW()
        {
            string wowexe = @"Wow.exe";
            if (!File.Exists(wowexe))
            {
                label1.Text = "Cannot find Wow.exe. Is WoW Installed?";
                button2.Image = Properties.Resources.PlayButtonDisabled;
                button4.Visible = false;
                progressBar1.Visible = true;
                button5.Visible = true;
                resultLabel.Visible = false;

            }
            else
            {
                checkPatch();
            }
        }

        private void installWoW()
        {
            Process.Start("World-of-Warcraft-Setup-enUS.exe");
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
                button2.Image = Properties.Resources.PlayButtonDisabled;
                progressBar1.Visible = true;
                getPatch();
            }
            else
            {
                label1.Text = "World of Warcraft is up to date";
                progressBar1.Visible = false;
                button2.Image = Properties.Resources.Play_No_Hover;
                button4.Visible = false;
                resultLabel.Visible = false;
                button6.Visible = false;
            }
        }

        // Start Downloads
        private void getPatch()
        {
            sw.Start();
            string patchlist = Settings1.Default.patchlist; 
            label1.Text = "Downloading";
            button2.Image = Properties.Resources.PlayButtonDisabled;
            button4.Visible = false;
            progressBar1.Visible = true;
            Directory.CreateDirectory("WTF");

            string[] files = new string[5];
            files[0] = "libeay32.dll";
            files[1] = "libmysql.dll";
            files[2] = "libssl32.dll";
            files[3] = "ssleay32.dll";
            files[4] = "connection_patcher.exe";
            files[5] = "WTF\\Config.wtf";

            // Loop 
            foreach (var file in files)
            {

                var webClient = new WebClient();
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                webClient.DownloadStringCompleted += OnGetDownloadedStringCompleted;
                webClient.DownloadFileAsync(new Uri(Settings1.Default.baseURL + file), file);
            }
            DownloadComplete();
        }

        // Download Complete
        private void DownloadComplete()
        {
            sw.Reset();
            label1.Text = "Downloads Complete.";
            progressBar1.Visible = true;
            string installer = "World-of-Warcraft-Setup-enUS.exe";
            if (File.Exists(installer))
            {
                button6.Visible = true;
            }

            string patcher = "Wow_patched.exe";
            if (File.Exists(patcher))
            {
                button4.Visible = true;
            }
            
            /*
            string patchedexe = @"Wow_Patched.exe";
            string game = @"Wow.exe";
            if (!File.Exists(patc11hedexe))
            {
                label4.Text = "Ready to Patch";
            }
            else
            {
                if (!File.Exists(game))
                {
                    label1.Text = "Wow is NOT installed!";
                    button5.Visible = true;
                }
                else
                {
                    label1.Text = "Start Wow to finish the install";
                    button2.Image = Properties.Resources.Play_No_Hover;
                }
                button2.Image = Properties.Resources.PlayButtonDisabled;
                label1.Text = "Check your installation, do you have everything?";
            }
            if(File.Exists(patchedexe))
            {
                button2.Image = Properties.Resources.Play_No_Hover;
            }
            else
            {
                Form1();
            }
            if (File.Exists(game)){
                button2.Image = Properties.Resources.Play_No_Hover;
            }
            else
            {
                button5.Visible = true;
            }
            
            */    
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
            DownloadComplete();
        }

        // Progress Bar
        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            this.progressBar1.Value = e.ProgressPercentage;
            resultLabel.Text = e.ProgressPercentage.ToString() + "%";
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

        private void button5_Click(object sender, EventArgs e)
        {
            // Download WoW
            var webClient = new WebClient();
            string installer = "World-of-Warcraft-Setup-enUS.exe";
            webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
            webClient.DownloadStringCompleted += OnGetDownloadedStringCompleted;
            webClient.DownloadFileAsync(new Uri(Settings1.Default.install + installer), installer);
            progressBar1.Visible = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            installWoW();
        }
    }
}
