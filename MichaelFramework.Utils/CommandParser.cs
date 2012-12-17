using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MichaelFramework.Utils
{
    public class CommandParser
    {
        private Dictionary<string, object> paras = new Dictionary<string, object>();
        private string commandString = "";
        public string CmommandString
        {
            get { return commandString; }
            set
            {
                commandString = value;
                paras.Clear();
                Command = null;
                string[] s = commandString.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                if (s.Length > 0)
                {
                    string[] temp = s[0].Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                    if (temp.Length == 1) Command = temp[0].Trim().ToLower();
                    else if (temp.Length >= 2 && (temp[0].Trim().ToLower() == "cmd" || temp[0].Trim().ToLower() == "command"))
                        Command = temp[1].Trim().ToLower();
                }
                for (int i = 1; i < s.Length; i++)
                {
                    string[] temp = s[i].Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                    string para = null;
                    object v = null;
                    if (temp.Length == 1)
                    {
                        para = temp[0];
                        if (s[i].Contains('='))
                            v = "";
                        else
                            v = true;
                    }
                    else if (temp.Length >= 2)
                    {
                        para = temp[0];
                        v = temp[1].Trim();
                    }
                    if (!string.IsNullOrEmpty(para) && !paras.ContainsKey(para))
                        paras.Add(para.Trim().ToLower(), v);
                }
            }
        }

        public string Command { get; private set; }

        public bool IsValid { get { return !string.IsNullOrEmpty(Command); } }

        public object this[string paraName]
        {
            get
            {
                if (paras.ContainsKey(paraName.Trim().ToLower())) return paras[paraName.Trim().ToLower()];
                return null;
            }
        }

        public bool IsOn(string paraName)
        {
            object obj = this[paraName];
            if (obj is bool) return (bool)obj;
            return false;
        }
    }
}
