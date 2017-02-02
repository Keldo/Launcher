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

namespace LauncherV3
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            progressBar1.Visible = false;
            lblMain.Visible = false;
        }
        private void wowVersion()
        {
            FileVersionInfo lversion = FileVersionInfo.GetVersionInfo("Wow.exe");
            string localVersion = lversion.FileVersion;

            // We will need a web connection to read the remote version file
            WebClient client = new WebClient();
            string remoteVersionFile = client.DownloadString("http://www.trinitywow.org/game/version/wow");

            if(localVersion == remoteVersionFile)
            {
                lblWoWversion.Text = "Version " + localVersion + ".";
            }
            else
            {
                lblWoWversion.Text = "Update WoW.";
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
            System.Diagnostics.Process.Start("http://us.battle.net/wow/en/game/patch-notes/");
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
            string installURL = "http://www.trinitywow.org/game/install/legion/";

            new WebClient().DownloadFile(installURL + "connection_patcher.exe", "connection_patcher.exe");
            new WebClient().DownloadFile(installURL + "common.dll", "common.dll");
            new WebClient().DownloadFile(installURL + "libeay32.dll", "libeay32.dll");
            new WebClient().DownloadFile(installURL + "libmysql.dll", "libmysql.dll");
            new WebClient().DownloadFile(installURL + "libssl32.dll", "libssl32.dll");
            new WebClient().DownloadFile(installURL + "ssleay32.dll", "ssleay32.dll");
            new WebClient().DownloadFile(installURL + "Wow.exe", "Wow.exe");
            new WebClient().DownloadFile(installURL + "Launcher.exe", "Launcher.exe");
            new WebClient().DownloadFile(installURL + "WTF/Config.wtf", @"WTF\Config.wtf");
        }

        private void worker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.lblMain.Visible = true;
            this.progressBar1.Visible = true;
            this.progressBar1.Value = e.ProgressPercentage;
            this.lblMain.Text = "Downloaded " + e.ProgressPercentage.ToString() + "%";
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
        }
    }
}
