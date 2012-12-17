using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MichaelFramework.Model.Logging
{
    public class Logger
    {
        //public static List<User> GetAccessedUsers()
        //{
        //    List<User> list = new List<User>();
        //    try
        //    {
        //        string accounts = AppContext.CustomSettings.Global.Application.LoginForm.AccessedUsers;
        //        accounts = CommonHelper.Decript(accounts, "acco");
        //        if (!string.IsNullOrEmpty(accounts))
        //            foreach (string userString in accounts.Split(new char[] { ';' },
        //                StringSplitOptions.RemoveEmptyEntries))
        //            {
        //                if (string.IsNullOrEmpty(userString)) continue;
        //                string[] temp = userString.Split(':');
        //                User user = new User();
        //                user.Name = temp[0];
        //                user.PlainPassword = temp[1];
        //                list.Add(user);
        //            }
        //    }
        //    catch { }
        //    return list;
        //}

        //public static void AddAccessedUser(User user, bool rememberPwd)
        //{
        //    List<User> list = GetAccessedUsers();
        //    foreach (User temp in list)
        //    {
        //        if (user.Name == temp.Name)
        //        {
        //            list.Remove(temp);
        //            break;
        //        }
        //    }
        //    list.Insert(0, user);
        //    string str = "";
        //    foreach (User temp in list)
        //    {
        //        if (temp.Name == user.Name)
        //            str += string.Format("{0}:{1};", temp.Name, rememberPwd ? temp.PlainPassword : "");
        //        else
        //            str += string.Format("{0}:{1};", temp.Name, temp.PlainPassword);
        //    }

        //    AppContext.CustomSettings.Global.Application.LoginForm.AccessedUsers = CommonHelper.Encript(str, "acco");
        //    AppContext.SaveConfig();
        //}
    }
}
