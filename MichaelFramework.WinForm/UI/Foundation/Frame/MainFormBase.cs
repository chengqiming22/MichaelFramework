using System;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using MichaelFramework.Utils;
using System.Collections.Generic;
using System.Reflection;

namespace MichaelFramework.WinForm.UI.Foundation.Frame
{
    public partial class MainFormBase : Office2007RibbonForm
    {
        # region 外部接口：属性

        private AppStatus appStatus = null;
        public AppStatus AppStatus
        {
            get
            {
                if (appStatus == null)
                    appStatus = new AppStatus(labelItem1, labelItem2, progressBarItem1);
                return appStatus;
            }
        }

        public bool LeftPaneVisible
        {
            get { return leftSplitter.Visible; }
            set { leftPane.Visible = leftSplitter.Visible = value; }
        }

        public bool RightPaneVisible
        {
            get { return rightSplitter.Visible; }
            set { rightPane.Visible = rightSplitter.Visible = value; }
        }

        public bool BottomPaneVisible
        {
            get { return bottomSplitter.Visible; }
            set { bottomPane.Visible = bottomSplitter.Visible = value; }
        }

        public RibbonLocalization SystemText { get { return this.ribbonControl1.SystemText; } }

        # endregion

        # region 外部接口：方法

        # endregion

        # region 初始化

        public MainFormBase()
        {
            InitializeComponent();
            MessageBoxEx.UseSystemLocalizedString = true;
            try
            {
                ApplyConfig();
            }
            catch { }
        }

        protected override void OnLoad(EventArgs e)
        {
            try
            {
                ManageItems();
            }
            catch { }
            try
            {
                RefreshPermissions();
            }
            catch { }
            try
            {
                LoadResources();
            }
            catch { }
            base.OnLoad(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            try
            {
                if (this.WindowState == FormWindowState.Normal)
                {
                    AppContext.CustomSettings.Application.MainForm.Bounds = this.Bounds;
                }
                AppContext.CustomSettings.Application.MainForm.Style = styleManager1.ManagerStyle;
                AppContext.CustomSettings.Application.MainForm.ColorTint = styleManager1.ManagerColorTint;
                AppContext.CustomSettings.Application.MainForm.WindowState = this.WindowState;

                AppContext.CustomSettings.Application.MainForm.LeftPaneState =
                    new PaneState() { Visible = LeftPaneVisible, Expanded = leftSplitter.Expanded, WidthOrHeight = leftPane.Width };
                AppContext.CustomSettings.Application.MainForm.RightPaneState =
                    new PaneState() { Visible = RightPaneVisible, Expanded = rightSplitter.Expanded, WidthOrHeight = rightPane.Width };
                AppContext.CustomSettings.Application.MainForm.BottomPaneState =
                    new PaneState() { Visible = BottomPaneVisible, Expanded = bottomSplitter.Expanded, WidthOrHeight = bottomPane.Height };

                AppContext.CustomSettings.Application.MainForm.SelectedRibbonTabIndex = ribbonControl1.Items.IndexOf(ribbonControl1.SelectedRibbonTabItem);

                AppContext.CustomSettings.Application.MainForm.RibbonControlExpanded = ribbonControl1.Expanded;

                AppContext.SaveConfig();
            }
            catch { }
        }

        # endregion

        # region 内部变量和方法

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (editorContainer1.CloseAll())
                base.OnFormClosing(e);
            else e.Cancel = true;
        }

        private List<BaseItem> getItems(BaseItem item)
        {
            List<BaseItem> list = new List<BaseItem>();
            if (item != null)
            {
                list.Add(item);
                foreach (BaseItem i in item.SubItems)
                    list.AddRange(getItems(i));
            }
            return list;
        }
        private void ManageItems()
        {
            List<BaseItem> items = new List<BaseItem>();
            foreach (BaseItem item in ribbonControl1.QuickToolbarItems)
                items.AddRange(getItems(item));
            foreach (Control c in ribbonControl1.Controls)
            {
                if (c is RibbonPanel)
                    foreach (RibbonBar bar in c.Controls)
                        foreach (BaseItem i in bar.Items)
                            items.AddRange(getItems(i));
            }
            foreach(BaseItem item in items)
                item.Click += new EventHandler(Item_Click);
        }

        # region command execution

        protected virtual bool ExecuteCommand(object sender, string cmd)
        {
            CommandParser p = new CommandParser() { CmommandString = cmd };
            if (p.IsValid)
            {
                Type t = null;
                switch (p.Command)
                {
                    case "editor":
                        t = CommonHelper.GetType((string)p["type"]);
                        if (t != null)
                            editorContainer1.NavigateTo(t, null, p["id"].ToString(),null);
                        return true;
                    case "dialog":
                        t = CommonHelper.GetType((string)p["type"]);
                        if (t != null)
                        {
                            Form f = FormBase.Instance(t);
                            if (f == null)
                                f = Activator.CreateInstance(t) as Form;
                            if (f != null && !f.Visible)
                            {
                                if (p.IsOn("showdialog"))
                                    f.ShowDialog();
                                else
                                    f.Show(this);
                            }
                        }
                        return true;
                }
            }
            return false;
        }

        # endregion

        /// <summary>
        /// 装载控件资源，包括图标等
        /// </summary>
        protected virtual void LoadResources()
        {
            this.Icon = Images.GetIcon("Solution");
            office2007StartButton1.Image = Images.ScaleIcon(Images.GetIcon("Solution"), 20);
            this.Text = AppContext.CustomSettings.Application.Name;

            this.WindowState = AppContext.CustomSettings.Application.MainForm.WindowState;
            if (this.WindowState == FormWindowState.Minimized)
                this.WindowState = FormWindowState.Normal;

            PaneState state = AppContext.CustomSettings.Application.MainForm.LeftPaneState;
            LeftPaneVisible = state.Visible;
            leftSplitter.Expanded = state.Expanded;
            leftPane.Width = state.WidthOrHeight;

            state = AppContext.CustomSettings.Application.MainForm.RightPaneState;
            RightPaneVisible = state.Visible;
            rightSplitter.Expanded = state.Expanded;
            rightPane.Width = state.WidthOrHeight;

            state = AppContext.CustomSettings.Application.MainForm.BottomPaneState;
            BottomPaneVisible = state.Visible;
            bottomSplitter.Expanded = state.Expanded;
            bottomPane.Height = state.WidthOrHeight;

            int i = AppContext.CustomSettings.Application.MainForm.SelectedRibbonTabIndex;
            ribbonControl1.SelectedRibbonTabItem = (RibbonTabItem)ribbonControl1.Items[i >= 0 ? i : 0];

            ribbonControl1.Expanded = AppContext.CustomSettings.Application.MainForm.RibbonControlExpanded;
        }

        /// <summary>
        /// 刷新权限设置
        /// </summary>
        protected virtual void RefreshPermissions()
        {

        }

        protected virtual void UpdateUI()
        {

        }

        protected virtual void ApplyConfig()
        {
            this.Bounds = AppContext.CustomSettings.Application.MainForm.Bounds;
            this.styleManager1.ManagerStyle = AppContext.CustomSettings.Application.MainForm.Style;
            this.styleManager1.ManagerColorTint = AppContext.CustomSettings.Application.MainForm.ColorTint;
        }

        # endregion

        # region 事件方法

        void Item_Click(object sender, EventArgs e)
        {
            string s = (string)(sender as BaseItem).Tag;
            if (string.IsNullOrEmpty(s)) return;
            ExecuteCommand(sender, s);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateUI();
        }

        # endregion
    }

    public class AppStatus
    {
        private LabelItem label1 = null;
        private LabelItem label2 = null;
        private ProgressBarItem progressBar = null;
        public AppStatus(LabelItem statusLabel1, LabelItem statusLabel2,
            ProgressBarItem pbItem)
        {
            label1 = statusLabel1;
            label2 = statusLabel2;
            progressBar = pbItem;
        }

        public string Status1
        {
            get { return label1.Text; }
            set
            {
                label1.Text = value;
                label1.Parent.RecalcSize();
                label1.Refresh();
            }
        }

        public string Status2
        {
            get { return label2.Text; }
            set 
            { 
                label2.Text = value; 
                label2.Parent.RecalcSize();
                label2.Refresh();
            }
        }

        public int ProgressMaxValue
        {
            get { return progressBar.Maximum; }
            set { progressBar.Maximum = value; }
        }

        public int Progress
        {
            get { return progressBar.Value; }
            set { progressBar.Value = value; }
        }

        public string ProgressText
        {
            get { return progressBar.Text; }
            set { progressBar.Text = value; }
        }

        public bool ProgressVisible
        {
            get { return progressBar.Visible; }
            set { progressBar.Visible = value; }
        }
    }
}
