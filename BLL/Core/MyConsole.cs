using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Hosting;

namespace WxPay2017.API.BLL
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

        public static string ReadFile(string path)
        {
            var file = HostingEnvironment.MapPath(path);
            if (!File.Exists(file)) throw new HttpException(404, "文件读取失败");
            var result = "";
            using (var sr = new StreamReader(file))
            {
                try
                {
                    result = sr.ReadToEnd();
                }
                catch (Exception ex)
                {
                    Log(ex);
                    throw ex;
                }

            }
            return result;
        }

        public static void Log(string msg, string type = "")
        {
            try
            {
                string path = HostingEnvironment.MapPath(GetAppString("log"));

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

            }
        }

        public static void Log(Exception e, string type = "")
        {
            Log(e.Message + "\n" + e.StackTrace, type);
        }

        private static Guid? _AppID;
        public static Guid ApplicationID
        {
            get
            {
                if (!_AppID.HasValue)
                {
                    _AppID = Guid.Parse(GetAppString("ApplicationID"));
                }
                return _AppID.Value;
            }
        }

    }

}