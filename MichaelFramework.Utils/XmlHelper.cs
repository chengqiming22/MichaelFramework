using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

namespace MichaelFramework.Utils
{
    public sealed class XmlHelper
    {
        /// <summary>
        /// 装载Xml文档，若不存在则创建
        /// </summary>
        /// <param name="path"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static XmlDocument Load(string filepath)
        {
            string path = Path.GetDirectoryName(filepath);
            string filename = Path.GetFileName(filepath);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            
            if (!File.Exists(filepath))
            {
                XmlTextWriter writer = null;

                writer = new XmlTextWriter(filepath, Encoding.UTF8);

                //Write the XML delcaration. 
                writer.WriteStartDocument();

                //Write the ProcessingInstruction node.
                String PItext = "type='text/xsl'";
                writer.WriteProcessingInstruction("xml-stylesheet", PItext);

                //Write a Comment node.
                writer.WriteComment("user XML");

                writer.WriteStartElement(filename.Substring(0, filename.LastIndexOf('.')));
                writer.WriteAttributeString("UpdateDate", DateTime.Now.ToString());

                writer.WriteEndElement();

                writer.WriteEndDocument();

                //Write the XML to file and close the writer.
                writer.Flush();
                writer.Close();
            }
            XmlDocument doc = new XmlDocument();
            while (true)
            {
                try
                {
                    doc.Load(filepath);
                    return doc;
                }
                catch { }
            }
        }
    }
}
