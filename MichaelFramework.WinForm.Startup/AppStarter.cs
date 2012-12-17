using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using MichaelFramework.WinForm;
using System.IO;

namespace MichaelFramework.WinFormStartup
{
    class AppStarter
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            AppContext.StartApplication();
        }

        private static void test()
        {
            FolderBrowserDialog f = new FolderBrowserDialog();
            while (f.ShowDialog() == DialogResult.OK)
            {
                int c = 0;
                countLine(f.SelectedPath, ref c);
                MessageBox.Show(string.Format("{0}行代码。", c));
            }
        }

        private static void countLine(string path, ref int lines)
        {
            foreach (string s in Directory.GetFiles(path,"*.cs"))
            {
                if (s.EndsWith(".Designer.cs")) continue;
                using (StreamReader sr = new StreamReader(s))
                {
                    while (!sr.EndOfStream)
                    {
                        string str = sr.ReadLine();
                        if (!string.IsNullOrEmpty(str) && !str.Trim().StartsWith("//") && !str.Trim().StartsWith("/*"))
                            lines++;
                    }
                }
            }
            foreach (string s in Directory.GetDirectories(path))
                countLine(s, ref lines);
        }
    }
}
