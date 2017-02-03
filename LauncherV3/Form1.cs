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

namespace LauncherV3
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            progressBar1.Visible = false;
            lblMain.Visible = false;
            lblWarning.Visible = false;
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
                    lblWoWversion.Text = "Update WoW.";
                    updateWoW();
                }
            }
            else
            {
                updateWoW();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            wowVersion();
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
        //    Process[] gameProcess = Process.GetProcessesByName("World of Warcraft");

        //    System.Timers.Timer checkTimer = new System.Timers.Timer(2000);
        //    checkTimer.AutoReset = true;
        //    checkTimer.Enabled = true;

        //    if (gameProcess == null)
        //    {
        //        // Show that the game is not running
        //        this.btnPlay.BackgroundImage = global::LauncherV3.Properties.Resources.Play_No_Hover;
        //        this.lblGameStatus.Visible = false;
        //    }
        //    else
        //    {
        //        // Show that the game is running
        //        this.btnPlay.BackgroundImage = global::LauncherV3.Properties.Resources.PlayButtonDisabled;
        //        this.lblGameStatus.Visible = true;
        //        this.lblGameStatus.Text = "Game is Running";
        //    }
        //}

        // Launcher Update
        // Open in a new thread
    }
}
