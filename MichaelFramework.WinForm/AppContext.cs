using System;
using System.Linq;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using MichaelFramework.WinForm.UI.Foundation;
using MichaelFramework.WinForm.UI.Foundation.Frame;
using MichaelFramework.Config;
using MichaelFramework.UI.Splash;
using System.Runtime.InteropServices;
using MichaelFramework.Utils;
using System.IO;

namespace MichaelFramework.WinForm
{
    public static class AppContext
    {
        # region config

        private static Configuration config = null;
        public static CustomSettings CustomSettings { get; private set; }

        private static void InitializeConfig()
        {
            if (config == null)
                config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            CustomSettings = config.Sections["customSettings"] as CustomSettings;
            if (CustomSettings == null)
            {
                CustomSettings = new CustomSettings();
                config.Sections.Add("customSettings", CustomSettings);
                config.Save();
            }
        }

        public static void SaveConfig()
        {
            config.Save();
        }

# if DEBUG

        public static void ProtectConfig()
        {
            if (!initialized)
                InitializeConfig();
            config.ConnectionStrings.SectionInformation.ProtectSection("RsaProtectedConfigurationProvider");
            CustomSettings.Protect();
            SaveConfig();
        }

        public static void UnprotectConfig()
        {
            if (!initialized)
                InitializeConfig();
            config.ConnectionStrings.SectionInformation.UnprotectSection();
            CustomSettings.Unprotect();
            SaveConfig();
        }

# endif

        # endregion

        # region ui

        public static Form LoginForm { get; private set; }

        public static MainFormBase MainForm { get; private set; }

        public static AdvPropertyGrid PropertyGrid { get; set; }

        private static void LoadMainFormLocalization(MainFormBase mainForm)
        {
            RibbonLocalization systemText = mainForm.SystemText;
            foreach (PropertyInfo pi in systemText.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                pi.SetValue(systemText, Localization.GetValue(pi.Name, (string)pi.GetValue(systemText, null)), null);
            }
        }

        static void LocalizationKeys_LocalizeString(object sender, LocalizeEventArgs e)
        {
            e.LocalizedValue = Localization.GetValue(e.Key, e.LocalizedValue);
            e.Handled = true;
        }

        # endregion

        # region app

        private static bool initialized = false;
        public static void InitializeApplication()
        {
            if (initialized) return;

            AppDomain.CurrentDomain.UnhandledException -= new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            Application.ThreadException -= new ThreadExceptionEventHandler(Application_ThreadException);
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);

            CustomSettings.Application.RunCount++;

            SplashControl.StartSplash("正在初始化应用……", delegate
            {
                InitializeConfig();
                foreach (CustomInitItem item in CustomSettings.System.CustomInit)
                {
                    Type t = CommonHelper.GetType(item.Type);
                    string method = string.IsNullOrEmpty(item.Method) ? "Initialize" : item.Method;
                    MethodInfo mi = t.GetMethod(method, BindingFlags.Public | BindingFlags.Static);
                    mi.Invoke(null,null);
                }
            });

            AppCache = new Dictionary<string, object>();

            AppCache.Add("customsettings", CustomSettings);

            Localization.DataDirectory = CustomSettings.System.DataDirectory;

            LocalizationKeys.LocalizeString += new DotNetBarManager.LocalizeStringEventHandler(LocalizationKeys_LocalizeString);

            if (CustomSettings.System.UsePermissionControl)
            {
                LoginForm = Activator.CreateInstance(Type.GetType(CustomSettings.Application.LoginForm.AssemblyQualifiedName)) as Form;
                AppCache.Add("loginform", LoginForm);
            }
            MainForm = Activator.CreateInstance(Type.GetType(CustomSettings.Application.MainForm.AssemblyQualifiedName)) as MainFormBase;
            AppCache.Add("mainform", MainForm);

            if ((CustomSettings.System.UsePermissionControl && LoginForm == null) || MainForm == null)
                throw new Exception("应用初始化失败，请检查配置文件的App节的loginForm和mainForm元素是否正确。");

            LoadMainFormLocalization(MainForm);

            initialized = true;
        }

        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {

        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {

        }

        public static void StartApplication()
        {
            try
            {
                if (!initialized)
                    InitializeConfig();
                bool createdNew;
                Mutex mutex = new Mutex(true, CustomSettings.Application.Name, out createdNew);
                if (!CustomSettings.Application.AllowMultiStartup && !createdNew)
                    return;
                if (!initialized)
                    InitializeApplication();
            }
            catch (Exception ex)
            {
                try
                {
                    File.AppendAllText(Path.Combine(Application.StartupPath, "startupFailed.txt"), string.Format(
                        "启动失败：{0}\r\n{1}\r\n", DateTime.Now, ex));
                }
                catch { }
                MessageBoxEx.Show(ex.Message, "启动失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (CustomSettings.System.UsePermissionControl && LoginForm.ShowDialog() == DialogResult.OK || 
                !CustomSettings.System.UsePermissionControl)
            {
                Application.Run(MainForm);
            }
        }

        public static Dictionary<string, object> AppCache { get; private set; }

        # endregion
    }
}
