using System;
using DevComponents.DotNetBar;

namespace MichaelFramework.WinForm.UI.Foundation.Frame
{
    public partial class EditorTab : TabItem
    {
        # region 外部接口

        public Editor Editor
        {
            get
            {
                return this.AttachedControl.Controls[0] as Editor;
            }
        }

        # endregion

        # region 初始化

        public EditorTab()
        {
            InitializeComponent();
        }

        public EditorTab(Editor editor)
            : this()
        {
            this.Text = editor.Title;
            editor.EditorTab = this;
            editor.ContentChanged += new EventHandler(Editor_ContentChanged);
            CreateTabControlPanel(editor);
        }

        void Editor_ContentChanged(object sender, EventArgs e)
        {
            if (Editor.CanSave)
                this.Text = (sender as Editor).Title + "*";
        }

        # endregion

        # region 工具方法与属性

        /// <summary>
        /// 创建编辑试题页面Tab
        /// </summary>
        /// <returns></returns>
        private void CreateTabControlPanel(Editor editor)
        {
            // 
            // tabControlPanel1
            // 
            TabControlPanel tabControlPanel1 = new TabControlPanel();
            tabControlPanel1.AutoScroll = true;
            tabControlPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            tabControlPanel1.Padding = new System.Windows.Forms.Padding(1);
            tabControlPanel1.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(179)))), ((int)(((byte)(231)))));
            tabControlPanel1.Style.BackColor2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(237)))), ((int)(((byte)(254)))));
            tabControlPanel1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            tabControlPanel1.Style.BorderColor.Color = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(97)))), ((int)(((byte)(156)))));
            tabControlPanel1.Style.BorderSide = ((DevComponents.DotNetBar.eBorderSide)(((DevComponents.DotNetBar.eBorderSide.Left | DevComponents.DotNetBar.eBorderSide.Right)
                        | DevComponents.DotNetBar.eBorderSide.Bottom)));
            tabControlPanel1.Style.GradientAngle = 90;
            tabControlPanel1.Controls.Add(editor);
            tabControlPanel1.TabItem = this;
            this.AttachedControl = tabControlPanel1;
        }

        # endregion
    }
}
