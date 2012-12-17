using System;
using DevComponents.DotNetBar;
using MichaelFramework.Utils;
using System.ComponentModel;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MichaelFramework.WinForm.UI.Foundation
{
    public partial class FormBase : Office2007Form
    {
        # region 外部接口：属性

        public bool HideOnClose { get; set; }

        # endregion

        # region 外部接口：方法

        # endregion

        # region 初始化

        public FormBase()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            LoadResources();
            RefreshPermissions();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (HideOnClose)
            {
                e.Cancel = true;
                this.Hide();
            }
            else base.OnClosing(e);
        }

        # endregion

        # region 内部变量和方法

        /// <summary>
        /// 装载控件资源，包括图标等
        /// </summary>
        protected virtual void LoadResources()
        {
            this.Icon = Images.GetIcon("Solution");
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

        # endregion

        # region 事件方法

        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateUI();
        }

        # endregion 

        private static Dictionary<Type, Form> dic = new Dictionary<Type, Form>();
        public static Form Instance( Type type)
        {
            if (!dic.ContainsKey(type))
            {
                Form temp = Activator.CreateInstance(type) as Form;
                if (temp == null) return null;
                dic.Add(type, temp);
                if (temp is FormBase)
                    (dic[type] as FormBase).HideOnClose = true;
            }
            return dic[type];
        }
    }
}
