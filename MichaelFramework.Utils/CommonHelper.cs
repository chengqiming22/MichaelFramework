using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Reflection;

namespace MichaelFramework.Utils
{
    public static class CommonHelper
    {
        public static string Encript(string str, string encryptKey)
        {
            if (string.IsNullOrEmpty(str)) return "";
            DESCryptoServiceProvider descsp = new DESCryptoServiceProvider();
            byte[] key = Encoding.Unicode.GetBytes(encryptKey);
            byte[] data = Encoding.Unicode.GetBytes(str);
            System.IO.MemoryStream MStream = new System.IO.MemoryStream();
            CryptoStream CStream = new CryptoStream(MStream, descsp.CreateEncryptor(key, key), CryptoStreamMode.Write);
            CStream.Write(data, 0, data.Length);
            CStream.FlushFinalBlock();
            return Convert.ToBase64String(MStream.ToArray());
        }

        public static string Decript(string str, string encryptKey)
        {
            if (string.IsNullOrEmpty(str)) return "";
            DESCryptoServiceProvider descsp = new DESCryptoServiceProvider();
            byte[] key = Encoding.Unicode.GetBytes(encryptKey);
            byte[] data = Convert.FromBase64String(str);
            System.IO.MemoryStream MStream = new System.IO.MemoryStream();
            CryptoStream CStream = new CryptoStream(MStream, descsp.CreateDecryptor(key, key), CryptoStreamMode.Write);
            CStream.Write(data, 0, data.Length);
            CStream.FlushFinalBlock();
            return Encoding.Unicode.GetString(MStream.ToArray());

        }

        public static string MD5Encript(string str)
        {
            if (string.IsNullOrEmpty(str)) return "";
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.Default.GetBytes(str);//将字符编码为一个字节序列 
            byte[] md5data = md5.ComputeHash(data);//计算data字节数组的哈希值 
            md5.Clear();
            string result = "";
            for (int i = 0; i < md5data.Length - 1; i++)
            {
                result += md5data[i].ToString("x").PadLeft(2, '0');
            }
            return result;
        }

        public static Type GetType(string typeFullName)
        {
            foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type t = a.GetType(typeFullName);
                if (t != null) return t;
            }
            return Type.GetType(typeFullName);
        }
    }
}
