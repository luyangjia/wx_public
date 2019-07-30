using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WxPay2017.API.DAL.EmpModels;

namespace WxPay2017.API.VO
{
    public class NotifyInfo
    {
        public MessageData msg{get;set;}
        public string userId{get;set;}
        public List<Subscribe> subs { get; set; }
    }
}
