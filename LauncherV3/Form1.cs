using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Timers;

namespace LauncherV3
{
    public partial class frmMain : Form
    {
        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        public frmMain()
        {
            InitializeComponent();
            progressBar1.Visible = false;
            lblMain.Visible = false;
            lblWarning.Visible = false;
            lblGameStatus.Visible = false;

            timer.Tick += new EventHandler(timer_Tick); // Every time timer ticks, timer_Tick will be called
            timer.Interval = (10) * (1000);             // Timer will tick every 10 seconds
            timer.Enabled = true;                       // Enable the timer
            timer.Start();                              // Start the timer
        }

        void timer_Tick(object sender, EventArgs e)
        {
            // Check updates every 10 seconds via the timer
            wowVersion();
            launcherVersion();

            // Check game process ( 0 or 1) via the timer
            //CheckGameProcess();
        }

        private void wowVersion()
        {
            if (File.Exists("Wow.exe"))
            {
                // Game specific Variables from Class1
                var WoWversion = new Class1().versionURL;
                var gameBuild = new Class1().wowBuild;

                // Local Game Version
                FileVersionInfo lversion = FileVersionInfo.GetVersionInfo("Wow.exe");
                string localVersion = lversion.FileVersion;

                // We will need a web connection to read the remote version file
                WebClient client = new WebClient();
                string remoteVersionFile = client.DownloadString(WoWversion + gameBuild);

                if (localVersion == remoteVersionFile)
                {
                    lblWoWversion.Text = "Version " + localVersion + ".";
                }
                else
                {
                    lblWoWversion.Text = "Wow Update.";
                    updateWoW();
                }
            }
            else
            {
                lblWoWversion.Visible = false;
                lblWarning.Text = "Downlading World of Warcraft for the Trinity Servers";
                updateWoW();
            }
        }

        private void launcherVersion()
        {
                // Game specific Variables from Class1
                var launcher_version = new Class1().versionURL;
                var launcher_build = new Class1().launcherBuild;

                // Local Game Version
                FileVersionInfo lversion = FileVersionInfo.GetVersionInfo("Launcher.exe");
                string localLVersion = lversion.FileVersion;

                // We will need a web connection to read the remote version file
                WebClient connect = new WebClient();
                string remoteLVersionFile = connect.DownloadString(launcher_version + launcher_build);

                if (localLVersion == remoteLVersionFile)
                {
                   
                }
                else
                {
                    lblWarning.Visible = true;
                    lblWarning.Text = "A Launcher Update will be downloaded and installed.";
                    updateLauncher();
                }
        }

        private void updateLauncher()
        {
            BackgroundWorker backgroundWorker = new BackgroundWorker();
            backgroundWorker.WorkerReportsProgress = true;
            DoWorkEventHandler workEventHandler = new DoWorkEventHandler(this.worker2_DoWork);
            backgroundWorker.DoWork += workEventHandler;
            ProgressChangedEventHandler changedEventHandler = new ProgressChangedEventHandler(this.worker1_ProgressChanged);
            backgroundWorker.ProgressChanged += changedEventHandler;
            RunWorkerCompletedEventHandler completedEventHandler = new RunWorkerCompletedEventHandler(this.worker2_Complete);
            backgroundWorker.RunWorkerCompleted += completedEventHandler;
            backgroundWorker.RunWorkerAsync();
            this.btnPlay.BackgroundImage = global::LauncherV3.Properties.Resources.PlayButtonDisabled;
        }

        private void worker2_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i < 100; i++)
            {
                (sender as BackgroundWorker).ReportProgress(i);
                Thread.Sleep(200);
            }

            // variables pooled from Class1
            var toolsURL = new Class1().toolsURL;
            var f1 = new Class1().launcherUpdater;

            try
            {
                new WebClient().DownloadFile(toolsURL + f1, f1);
            }
            catch
            {
                lblWarning.Visible = true;
                lblWarning.Text = "Unable to Download files, please check you internet connection.";
                progressBar1.Visible = false;
            }
        }

        private void worker2_Complete(object sender, EventArgs e)
        {
            this.lblMain.Text = "Install Complete, Patching";
            this.progressBar1.Visible = false;
            patchLauncher();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            wowVersion();
            cleanUp();
        }

        private void lnkLbl1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Navigate to a URL.
            Process.Start("http://us.battle.net/wow/en/game/patch-notes/");
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            Process.Start("Wow_Patched.exe");
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        // WOW UPDATE
        private void updateWoW()
        {
            BackgroundWorker backgroundWorker = new BackgroundWorker();
            backgroundWorker.WorkerReportsProgress = true;
            DoWorkEventHandler workEventHandler = new DoWorkEventHandler(this.worker1_DoWork);
            backgroundWorker.DoWork += workEventHandler;
            ProgressChangedEventHandler changedEventHandler = new ProgressChangedEventHandler(this.worker1_ProgressChanged);
            backgroundWorker.ProgressChanged += changedEventHandler;
            RunWorkerCompletedEventHandler completedEventHandler = new RunWorkerCompletedEventHandler(this.worker1_Complete);
            backgroundWorker.RunWorkerCompleted += completedEventHandler;
            backgroundWorker.RunWorkerAsync();
            this.btnPlay.BackgroundImage = global::LauncherV3.Properties.Resources.PlayButtonDisabled;
        }

        private void worker1_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i < 100; i++)
            {
                (sender as BackgroundWorker).ReportProgress(i);
                Thread.Sleep(200);
            }

            // variables pooled from Class1
            var instURL = new Class1().installURL;
            var f1 = new Class1().file1;
            var f2 = new Class1().file2;
            var f3 = new Class1().file3;
            var f4 = new Class1().file4;
            var f5 = new Class1().file5;
            var f6 = new Class1().file6;
            var f7 = new Class1().file7;

            try
            {
                new WebClient().DownloadFile(instURL + f1, f1);
                new WebClient().DownloadFile(instURL + f2, f2);
                new WebClient().DownloadFile(instURL + f3, f3);
                new WebClient().DownloadFile(instURL + f4, f4);
                new WebClient().DownloadFile(instURL + f5, f5);
                new WebClient().DownloadFile(instURL + f6, f6);
                new WebClient().DownloadFile(instURL + f7, f7);
                Directory.CreateDirectory("WTF");
                new WebClient().DownloadFile(instURL + "WTF/Config.wtf", @"WTF\Config.wtf");
            }
            catch
            {
                lblWarning.Visible = true;
                lblWarning.Text = "Unable to Download files, please check you internet connection.";
                progressBar1.Visible = false;
            }

            
        }

        private void worker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.lblMain.Visible = true;
            this.progressBar1.Visible = true;
            this.progressBar1.Value = e.ProgressPercentage;
            this.lblMain.Text = "Downloaded " + e.ProgressPercentage.ToString() + "%";
            if(e.ProgressPercentage < 25)
            {
                this.progressBar1.ForeColor = System.Drawing.Color.Red;
            }

            if (e.ProgressPercentage > 50)
            {
                this.progressBar1.ForeColor = System.Drawing.Color.Yellow;
            }
            if (e.ProgressPercentage > 75)
            {
                this.progressBar1.ForeColor = System.Drawing.Color.Green;
            }
        }

        private void worker1_Complete(object sender, EventArgs e)
        {
            this.lblMain.Text = "Install Complete, Patching";
            this.progressBar1.Visible = false;
            patchWoW();
        }
        private void patchWoW()
        {
            Process.Start("connection_patcher.exe", "Wow.exe");
            this.lblMain.Visible = false;
            this.btnPlay.BackgroundImage = global::LauncherV3.Properties.Resources.Play_No_Hover;
            reload();
        }
        private void reload()
        {
            Application.Restart();
        }

        private void btnWelcome_Click(object sender, EventArgs e)
        {
            frmwelcome frm = new frmwelcome();
            frm.Show();
        }

        // Check if the Game is running (Process.GetProcess() and report)
        //void CheckGameProcess()
        //{
        //    //Process gameProcess = null;

        //    // Needs a bool respsonse
        //    try
        //    {
        //        Process[] gameProcess = Process.GetProcessesByName("World of Warcraft");
        //        Convert.ToBoolean(true);
        //        return (gameProcess = 1);

        //        if (gameProcess == false)
        //        {

        //            // Show that the game is not running
        //            this.btnPlay.BackgroundImage = global::LauncherV3.Properties.Resources.Play_No_Hover;
        //            this.lblGameStatus.Visible = false;
        //        }
        //        else
        //        {
        //            // Show that the game is running
        //            this.btnPlay.BackgroundImage = global::LauncherV3.Properties.Resources.PlayButtonDisabled;
        //            this.lblGameStatus.Visible = true;
        //            this.lblGameStatus.Text = "Game is Running";
        //        }
        //    }
        //    catch
        //    {
        //        MessageBox.Show("ERROR!");
        //    }
            
        //}

        // Launcher Update
        // Open in a new thread
        private void patchLauncher()
        {
            Thread.Sleep(5000);
            Process lUpdate = new Process();
            lUpdate.StartInfo.FileName = "Launcher_Update.exe";
            //lUpdate.StartInfo.Arguments = "/s";
            lUpdate.StartInfo.CreateNoWindow = true;
            lUpdate.StartInfo.UseShellExecute = false;
            lUpdate.Start();

            // Shut the launcher down
            Application.Exit();
        }

        // Cleaning up
        private void cleanUp()
        {
            var patcher1 = new Class1().file6;
            var patcher2 = new Class1().launcherUpdater;
            var mysqldll = new Class1().file3;
            var sslddll = new Class1().file4;
            var ssl2dll = new Class1().file5;
            var eaydll = new Class1().file2;

            if(File.Exists(patcher1))
            {
                File.Delete(patcher1);
            }
            if(File.Exists(patcher2))
            {
                File.Delete(patcher2);
            }
            if (File.Exists(mysqldll))
            {
                File.Delete(mysqldll);
            }
            if (File.Exists(sslddll))
            {
                File.Delete(sslddll);
            }
            if (File.Exists(ssl2dll))
            {
                File.Delete(ssl2dll);
            }
            if (File.Exists(eaydll))
            {
                File.Delete(eaydll);
            }
        }
    }
}
