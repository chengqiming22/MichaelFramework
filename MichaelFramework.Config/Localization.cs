using System.Xml;
using MichaelFramework.Utils;
using System.Windows.Forms;
using System.IO;

namespace MichaelFramework.Config
{
    public class Localization
    {
        private static XmlDocument doc = null;
        private static string path = "";
        private static string dataDirectory = "";
        public static string DataDirectory
        {
            get { return dataDirectory; }
            set
            {
                dataDirectory = value;
                path = Path.Combine(Path.Combine(Application.StartupPath, dataDirectory), "localization.xml");
                Refresh();
            }
        }

        public static void Save()
        {
            doc.Save(path);
        }

        public static void Refresh()
        {
            doc = XmlHelper.Load(path);
        }

        public static string GetValue(string key)
        {
            XmlNode node = doc.DocumentElement.SelectSingleNode(key);
            return node != null ? node.InnerText : null;
        }

        public static string GetValue(string key, string defaultValue)
        {
            XmlNode node = doc.DocumentElement.SelectSingleNode(key);
            if (node==null)
            {
                node = doc.CreateElement(key);
                node.InnerText = defaultValue;
                doc.DocumentElement.AppendChild(node);
                Save();
            }
            return node.InnerText;
        }

        public static void SetValue(string key, string value)
        {
            XmlNode node = doc.DocumentElement.SelectSingleNode( key);
            if (node == null)
            {
                node = doc.CreateElement(key);
                doc.DocumentElement.AppendChild(node);
            }
            node.InnerText = value;
            Save();
        }
    }
}
