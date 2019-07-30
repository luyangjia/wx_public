
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace EMP2016.API.VO.Common
{

    public class MyConsole
    {
        /// <summary>
        /// 获取Web.Config中AppSetting的值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetAppString(string key)
        {
            if (ConfigurationManager.AppSettings[key] == null)
            {
                throw new Exception("config缺少" + key);
            }
            return ConfigurationManager.AppSettings[key].ToString();
        }

        public static void log(string msg, string type = "")
        {
            try
            {
                string path = GetAppString("log");

                if (!Directory.Exists(path + "console/")) Directory.CreateDirectory(path + "console/");
                string file = DateTime.Now.ToString("yyyyMMdd") + ".log";
                FileStream fs = new FileStream(path + "console/" + file, FileMode.Append);
                StreamWriter sw = new StreamWriter(fs);
                string desc = DateTime.Now.ToString("HH:mm:ss") + "\t" + type;
                sw.WriteLine(desc);
                sw.WriteLine(msg);
                sw.Close();
                fs.Close();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static void log(Exception e, string type = "")
        {
            log(e.Message + "\n" + e.StackTrace, type);
        }

    }


    [Flags]
    public enum SystemLayoutType
    {
        FixNavigation = 0x0001,
        OffCanvasNavigation = 0x0002,
        AsideIn = 0x0004,
        AsideLeft = 0x0008,
        AsideRight = 0x0010,
        AsideOut = 0x0020
    }
}