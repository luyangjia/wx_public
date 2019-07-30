using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMP2016.API.VO.Data;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
namespace EMP2016.API.VO.Common
{
    public class WebNotifier:INotifier
    {

        public string Send(NotifierData msg, string userId)
        {
            try
            {
                var notifier = GlobalHost.ConnectionManager.GetHubContext<MyHub>();

                var isonline = MyHub.IsOnline(userId);
                if (!isonline) return null;

                var cids = MyHub.CurrClients.Where(x => x.Value.ClientName == userId).Select(x => x.Key).ToList();
                foreach (var item in cids)
                {
                    notifier.Clients.Client(item).broadcast(msg.Msg);
                }
              
                return userId;
            }
            catch (Exception ex)
            {
                MyConsole.log(ex, "Web消息发送异常");
            }
            return null;
        }

        public List<string> Send(NotifierData msg, List<string> userIds)
        {
            List<string> successes = new List<string>();
            foreach (var userId in userIds)
            {
                try
                {
                    var notifier = GlobalHost.ConnectionManager.GetHubContext<MyHub>();

                    var isonline = MyHub.IsOnline(userId);
                    if (!isonline) return null;

                    var cids = MyHub.CurrClients.Where(x => x.Value.ClientName == userId).Select(x => x.Key).ToList();
                    foreach (var item in cids)
                    {
                        notifier.Clients.Client(item).broadcast(msg.Msg);
                    }
                    successes.Add(userId);
                    
                }
                catch (Exception ex)
                {
                    MyConsole.log(ex, "Web消息发送异常");
                }
            }
            return successes;
        }
    }
}
