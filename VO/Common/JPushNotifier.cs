using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cn.jpush.api;
using cn.jpush.api.common;
using cn.jpush.api.common.resp;
using cn.jpush.api.device;
using cn.jpush.api.push;
using cn.jpush.api.push.mode;
using cn.jpush.api.report;
using cn.jpush.api.util;
using Newtonsoft.Json;
using EMP2016.API.VO.Data;

namespace EMP2016.API.VO.Common
{
    public class JPushNotifier : INotifier
    {
        string app_key = MyConsole.GetAppString("JpushAppkey");
        string master_secret = MyConsole.GetAppString("JpushSecret");

        public string Send(NotifierData msg, string userId)
        {
            DAL.EmpModels.EmpContext emp = new DAL.EmpModels.EmpContext();
            JPushClient client = new JPushClient(app_key, master_secret);
            PushPayload pushPayload = new PushPayload();

            pushPayload.platform = Platform.android();
            pushPayload.audience = Audience.s_tag(userId);
            pushPayload.notification = Notification.android(msg.Msg.Body, msg.Msg.Subject);
            try
            {
                var result = client.SendPush(pushPayload);
                return userId;
            }
            catch {
                return null;
            }
            return null;

        }

        public List<string> Send(NotifierData msg, List<string> userIds)
        {
            DAL.EmpModels.EmpContext emp = new DAL.EmpModels.EmpContext();
            List<string> successes = new List<string>();
            JPushClient client = new JPushClient(app_key, master_secret);
            foreach (var userId in userIds)
            {
                PushPayload pushPayload = new PushPayload();
                pushPayload.platform = Platform.android();
                pushPayload.audience = Audience.s_tag(userId);
                pushPayload.notification = Notification.android(msg.Msg.Body, msg.Msg.Subject);
                try
                {
                    var result = client.SendPush(pushPayload);
                    successes.Add(userId);
                }
                catch
                {
                }               
            }
            return successes;
        }
    }
}
