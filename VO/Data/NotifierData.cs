using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMP2016.API.VO.Data
{
    public class NotifierData
    {
        public string SignName = "新开普";
        public string TemplateCode = "SMS_8890094";
        public MessageData Msg { get; set; }
        //您好，${source}因${reason}产生此￥{alert}通知，请及时关注并进行处理。
        //public string Source = "";//xxx设备
        //public string Reason { get; set; }//失压
        //public string Alert = "";//紧急告警
       
    }
}
