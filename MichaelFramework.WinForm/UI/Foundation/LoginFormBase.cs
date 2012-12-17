using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DevComponents.DotNetBar.Validator;
using MichaelFramework.Utils;
using DevComponents.DotNetBar;
using MichaelFramework.UI.Splash;

namespace MichaelFramework.WinForm.UI.Foundation
{
    public partial class LoginFormBase : FormBase
    {
        # region 公共属性

        # endregion

        # region 初始化

        public LoginFormBase()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            try
            {
                this.Text = string.IsNullOrEmpty(AppContext.CustomSettings.Application.LoginForm.Title) ?
                    "登录" : AppContext.CustomSettings.Application.LoginForm.Title;
            }
            catch { }
        }

        # endregion


    }
}
