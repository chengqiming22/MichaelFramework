namespace MichaelFramework.WinForm.UI.Foundation.Frame
{
    partial class EditorContainer
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new DevComponents.DotNetBar.TabControl();
            this.contextMenuBar1 = new DevComponents.DotNetBar.ContextMenuBar();
            this.menu1 = new DevComponents.DotNetBar.ButtonItem();
            this.btnSaveTab = new DevComponents.DotNetBar.ButtonItem();
            this.btnCloseTab = new DevComponents.DotNetBar.ButtonItem();
            this.btnCloseAllTabsExceptThis = new DevComponents.DotNetBar.ButtonItem();
            ((System.ComponentModel.ISupportInitialize)(this.tabControl1)).BeginInit();
            this.tabControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.contextMenuBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.BackColor = System.Drawing.Color.Transparent;
            this.tabControl1.CanReorderTabs = true;
            this.tabControl1.Controls.Add(this.contextMenuBar1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedTabFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.tabControl1.SelectedTabIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(428, 351);
            this.tabControl1.Style = DevComponents.DotNetBar.eTabStripStyle.Office2007Document;
            this.tabControl1.TabIndex = 0;
            this.tabControl1.TabLayoutType = DevComponents.DotNetBar.eTabLayoutType.FixedWithNavigationBox;
            this.tabControl1.Text = "tabControl1";
            this.tabControl1.SelectedTabChanged += new DevComponents.DotNetBar.TabStrip.SelectedTabChangedEventHandler(this.tabControl1_SelectedTabChanged);
            this.tabControl1.SelectedTabChanging += new DevComponents.DotNetBar.TabStrip.SelectedTabChangingEventHandler(this.tabControl1_SelectedTabChanging);
            // 
            // contextMenuBar1
            // 
            this.contextMenuBar1.DockSide = DevComponents.DotNetBar.eDockSide.Document;
            this.contextMenuBar1.Font = new System.Drawing.Font("宋体", 9F);
            this.contextMenuBar1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.menu1});
            this.contextMenuBar1.Location = new System.Drawing.Point(52, 127);
            this.contextMenuBar1.Name = "contextMenuBar1";
            this.contextMenuBar1.Size = new System.Drawing.Size(113, 25);
            this.contextMenuBar1.Stretch = true;
            this.contextMenuBar1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.contextMenuBar1.TabIndex = 1;
            this.contextMenuBar1.TabStop = false;
            this.contextMenuBar1.Text = "contextMenuBar1";
            // 
            // menu1
            // 
            this.menu1.AutoExpandOnClick = true;
            this.menu1.Name = "menu1";
            this.menu1.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.btnSaveTab,
            this.btnCloseTab,
            this.btnCloseAllTabsExceptThis});
            this.menu1.Text = "buttonItem1";
            this.menu1.PopupOpen += new DevComponents.DotNetBar.DotNetBarManager.PopupOpenEventHandler(this.menu1_PopupOpen);
            this.menu1.PopupClose += new System.EventHandler(this.menu1_PopupClose);
            // 
            // btnSaveTab
            // 
            this.btnSaveTab.Name = "btnSaveTab";
            this.btnSaveTab.Text = "保存";
            this.btnSaveTab.Click += new System.EventHandler(this.btnSaveTab_Click);
            // 
            // btnCloseTab
            // 
            this.btnCloseTab.Name = "btnCloseTab";
            this.btnCloseTab.Text = "关闭";
            this.btnCloseTab.Click += new System.EventHandler(this.btnCloseTab_Click);
            // 
            // btnCloseAllTabsExceptThis
            // 
            this.btnCloseAllTabsExceptThis.Name = "btnCloseAllTabsExceptThis";
            this.btnCloseAllTabsExceptThis.Text = "除此之外全部关闭";
            this.btnCloseAllTabsExceptThis.Click += new System.EventHandler(this.btnCloseAllTabsExceptThis_Click);
            // 
            // EditorContainer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "EditorContainer";
            this.Size = new System.Drawing.Size(428, 351);
            ((System.ComponentModel.ISupportInitialize)(this.tabControl1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.contextMenuBar1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.TabControl tabControl1;
        private DevComponents.DotNetBar.ContextMenuBar contextMenuBar1;
        private DevComponents.DotNetBar.ButtonItem menu1;
        private DevComponents.DotNetBar.ButtonItem btnSaveTab;
        private DevComponents.DotNetBar.ButtonItem btnCloseTab;
        private DevComponents.DotNetBar.ButtonItem btnCloseAllTabsExceptThis;
    }
}
