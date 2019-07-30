using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WxPay2017.API.VO
{
    public class FileInfoData
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public long Size { get; set; }

        public FileInfoData(string n, string p, long s)
        {
            Name = n;
            Path = p;
            Size = s;
        }
    }
}