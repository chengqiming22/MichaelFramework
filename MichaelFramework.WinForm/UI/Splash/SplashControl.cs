using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace MichaelFramework.UI.Splash
{
    public partial class SplashControl : UserControl
    {
        private Exception innerException = null;

        private bool showProgress = false;
        public bool ShowProgress
        {
            get
            {
                return showProgress;
            }
            set
            {
                showProgress = progressBarX1.Visible = value;
                AdjustSize();

            }
        }

        private bool supportCancellation = false;
        public bool SupportCancellation
        {
            get
            {
                return supportCancellation;
            }
            set
            {
                supportCancellation = btnCancel.Visible = value;
                AdjustSize();
            }
        }

        private void AdjustSize()
        {
            if (!showProgress && !supportCancellation)
            {
                loadingCircle1.Height = labelX1.Height = this.Height;
            }
            else if (showProgress && !supportCancellation)
            {
                loadingCircle1.Height = labelX1.Height = this.Height - progressBarX1.Height - 10;
                progressBarX1.Width = this.Width - 3;
            }
            else
            {
                loadingCircle1.Height = labelX1.Height = this.Height - progressBarX1.Height - 10;
                progressBarX1.Width = this.Width - btnCancel.Width - 8;
            }
            labelX1.Height -= 5;
            ribbonClientPanel1.Invalidate();
        }

        [Browsable(true)]
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                labelX1.Text = base.Text = value;
            }
        }

        public SplashControl()
        {
            InitializeComponent();
            AdjustSize();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            innerException = null;
            try
            {
                Action a = (Action)e.Argument;
                if (a != null) a.Invoke();
            }
            catch (Exception ex)
            {
                innerException = ex;
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBarX1.Value = e.ProgressPercentage;
            if (!string.IsNullOrEmpty((e.UserState as ReportProgressData).Tip))
                labelX1.Text = (e.UserState as ReportProgressData).Tip;
            int min = progressBarX1.Minimum;
            int max = progressBarX1.Maximum;
            if (max > min)
                progressBarX1.Text = string.Format("{0}%", (e.ProgressPercentage - min) * 100 / (max - min));
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Form f = this.FindForm();
            if (f.InvokeRequired)
                f.Invoke(new Action(HideForm));
            else
                this.FindForm().Hide();
            progressBarX1.Text = "";
        }
        private void HideForm()
        {
            this.FindForm().Hide();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();
            labelX1.Text = "正在取消……";
        }

        # region 实例化

        class SplashForm : Form
        {
            protected override bool ShowWithoutActivation
            {
                get
                {
                    return true;
                }
            }
            public SplashForm()
            {
                StartPosition = FormStartPosition.CenterScreen;
                FormBorderStyle = FormBorderStyle.None;
                ShowInTaskbar = false;
            }
        }

        private static SplashForm f = null;
        private static SplashControl sp = null;
        public static void StartSplash(bool showProgress, bool supportCancellation, string text, Action action)
        {
            if (f == null)
            {
                sp = new SplashControl();
                sp.backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(SplashControl_ProgressChanged);
                sp.Dock = DockStyle.Fill;
                f = new SplashForm();
                f.ClientSize = sp.Size;
                f.Controls.Add(sp);
            }
            sp.progressBarX1.Value = 0;
            sp.ShowProgress = showProgress;
            sp.SupportCancellation = supportCancellation;
            sp.Text = text;
            sp.backgroundWorker1.RunWorkerAsync(action);
            f.ShowDialog();
            if (sp.innerException != null) throw sp.innerException;
        }

        public static event ProgressChangedEventHandler ProgressChanged;
        static void SplashControl_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (ProgressChanged != null) ProgressChanged(sp, e);
        }

        public static void StartSplash(Action action)
        {
            StartSplash(false, false, "加载中，请稍后……", action);
        }

        public static void StartSplash(string text, Action action)
        {
            StartSplash(false, false, text, action);
        }

        public static void SetProgressParams(int min, int max,bool showProgressBarPercent)
        {
            if (f == null)
            {
                sp = new SplashControl();
                sp.Dock = DockStyle.Fill;
                f = new SplashForm();
                f.ClientSize = sp.Size;
                f.Controls.Add(sp);
            }
            sp.progressBarX1.Minimum = min;
            sp.progressBarX1.Maximum = max;
            sp.progressBarX1.TextVisible = showProgressBarPercent;
        }

        public static void ReportProgress(int progress, ReportProgressData data)
        {
            if (sp != null)
            {
                sp.backgroundWorker1.ReportProgress(progress, data);
            }
        }

        public static bool CancellationPending
        {
            get { return sp == null ? true : sp.backgroundWorker1.CancellationPending; }
        }

        public static void Wait(int waitTime)
        {
            Thread.Sleep(waitTime);
        }

        # endregion
    }

    public class ReportProgressData
    {
        public string Tip { get; set; }

        private Dictionary<string, object> userData = new Dictionary<string, object>();
        public Dictionary<string, object> UserData { get { return userData; } }
    }
}
