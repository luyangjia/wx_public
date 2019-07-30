using EMP2016.API.DAL.EmpModels;
using EMP2016.API.VO.Data;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EMP2016.API.VO.Common
{
    [HubName("MyHub")]
    public class MyHub : Hub
    {
        public static Dictionary<string, ClientInfo> CurrClients = new Dictionary<string, ClientInfo>();

        protected async Task Broadcast(dynamic msg) { await Clients.All.broadcast(msg); }
        protected async Task Broadcast(string usr, dynamic msg) { await Clients.Client(usr).broadcast(msg); }
        //連線關閉時觸發
        public Task Disconnect()
        {
            string cid = Context.ConnectionId;
            lock (CurrClients)
            {
                if (CurrClients.ContainsKey(cid))
                {
                    CurrClients.Remove(cid);
                    UpdateConsoleStats();
                }
            }
            return null;
        }
        //連線建立時觸發
        public Task Connect()
        {
            string cid = Context.ConnectionId;

            return null;
        }
        //重新連線時觸發
        public Task Reconnect(IEnumerable<string> groups)
        {
            return null;
        }

        //ClientName為Console時，可用來接收控制資訊
        static string consoleCid = null;
        private void UpdateConsoleStats()
        {
            if (string.IsNullOrEmpty(consoleCid))
                return;
            Clients.Client(consoleCid).RefreshStats(CurrClients);
        }

        //註冊Client識別名稱
        public void Register(string uid)
        {
            string cid = Context.ConnectionId;
            lock (CurrClients)
            {
                if (CurrClients.Count(x => x.Value.ClientName == uid) > 0)
                {
                    var pid = CurrClients.FirstOrDefault(x => x.Value.ClientName == uid).Key;
                    CurrClients.Remove(pid);
                }
                //命名為Console時，作為接收控制資料之用
                if (uid == "Console")
                {
                    consoleCid = cid;
                } //其餘連線加入Dictionary中
                else if (!CurrClients.ContainsKey(cid))
                {
                    CurrClients.Add(cid, new ClientInfo()
                    {
                        ConnId = cid,
                        ClientName = uid
                    });
                }
                //將目前連線的Client資料傳送給Console Client
                UpdateConsoleStats();
            }
        }

        public static bool IsOnline(string uid)
        {
            return CurrClients.Count(x => x.Value.ClientName == uid) > 0;
        }

        public static string GetConnectId(string uid)
        {
            return CurrClients.FirstOrDefault(x => x.Value.ClientName == uid).Key;
        }

        //保存Client識別資料的物件
        public class ClientInfo
        {
            public string ConnId { get; set; }
            public string ClientName { get; set; }
        }

        #region Meter Monitor

        public void AddWatch(string uid, int meterid, CategoryDictionary category, IList<string> param, bool infinite = true)
        {
            HubMonitor.AddWatch(uid, meterid, category, param.ToList(), infinite);
        }
        public void RemoveWatch(string uid, int meterid, CategoryDictionary category)
        {
            HubMonitor.RemoveWatch(uid, meterid, category);
        }
        #endregion

    }
}
