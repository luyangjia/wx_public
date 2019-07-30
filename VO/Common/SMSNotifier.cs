using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Top.Api;
using Top.Tmc;
using Top.Api.Request;
using Top.Api.Response;
using EMP2016.API.VO.Data;

namespace EMP2016.API.VO.Common
{
    public  class SMSNotifier:INotifier
    {
         string appkey = MyConsole.GetAppString("SMSAppkey");
         string secret = MyConsole.GetAppString("SMSSecret");
        
        public  string  Send(NotifierData msg, string userId)
        {
            DAL.EmpModels.EmpContext emp = new DAL.EmpModels.EmpContext();
            ITopClient client = new DefaultTopClient("http://gw.api.taobao.com/router/rest", appkey, secret);
            try
            {
                var u = emp.Users.FirstOrDefault(x => x.Id == userId);
                if (u == null) return null;
                AlibabaAliqinFcSmsNumSendRequest req = new AlibabaAliqinFcSmsNumSendRequest();
                req.SmsType = "normal";
                req.SmsFreeSignName = msg.SignName;
                req.SmsParam = "{\"source\":\"\",\"reason\":\"" + msg.Msg.Subject + "\",\"alert\":\"\"}";
                req.RecNum = "" + u.PhoneNumber;
                req.SmsTemplateCode = msg.TemplateCode;
                AlibabaAliqinFcSmsNumSendResponse rsp = client.Execute(req);
                return rsp.Body;
            }
            catch (Exception ex)
            {
                MyConsole.log(ex, "短信发送异常");
            }
            return null;

        }

        public  List<string> Send(NotifierData msg, List<string> userIds)
        {
            DAL.EmpModels.EmpContext emp = new DAL.EmpModels.EmpContext();
            List<string> successes = new List<string>();

            ITopClient client = new DefaultTopClient("http://gw.api.taobao.com/router/rest", appkey, secret);
            var users = emp.Users.Where(x => userIds.Contains(x.Id)).ToList();
            if (users.Count() == 0) return successes;

            foreach (var u in users)
            {
                try
                {
                    AlibabaAliqinFcSmsNumSendRequest req = new AlibabaAliqinFcSmsNumSendRequest();
                    req.SmsType = "normal";
                    req.SmsFreeSignName = msg.SignName;
                    req.SmsParam = "{\"source\":\"\",\"reason\":\"" + msg.Msg.Subject + "\",\"alert\":\"\"}";
                    req.RecNum = "" + u.PhoneNumber;
                    req.SmsTemplateCode = msg.TemplateCode;
                    AlibabaAliqinFcSmsNumSendResponse rsp = client.Execute(req);
                    successes.Add(u.Id);
                }
                catch (Exception ex)
                {
                    MyConsole.log(ex, "短信发送异常");
                }
            }
            return successes;
        }
    }
}
