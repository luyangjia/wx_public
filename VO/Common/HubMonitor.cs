using EMP2016.API.DAL.EmpModels;
using EMP2016.API.VO.Common;
using EMP2016.API.VO.Data;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Web;

namespace EMP2016.API.VO.Common
{
    public class HubMonitor
    {
        public static IHubContext notifier = GlobalHost.ConnectionManager.GetHubContext<MyHub>();
        public static Dictionary<string, List<MonitorMeta>> CurrObservers = new Dictionary<string, List<MonitorMeta>>();
        public static Timer timer;


        /// <summary>
        /// 添加要实时监测的设备
        /// </summary>
        /// <param name="user"></param>
        /// <param name="meter"></param>
        /// <param name="param"></param>
        /// <param name="infinite">单次监测或一直监测</param>
        public static void AddWatch(string user, int target, CategoryDictionary targetType, List<string> param, bool infinite = true)
        {
            try
            {
                if (string.IsNullOrEmpty(user)) return;
                var notifier = GlobalHost.ConnectionManager.GetHubContext<MyHub>();

                //var onlines = MyHub.CurrClients.Keys;
                var isonline = MyHub.IsOnline(user);
                if (!isonline)
                {
                    if (CurrObservers.ContainsKey(user)) CurrObservers.Remove(user);
                    return;
                }
                else
                {
                    var dic = new Dictionary<string, decimal>();
                    param.ForEach(x => dic.Add(x, decimal.Zero));
                    var node = new MonitorMeta()
                    {
                        TargetId = target,
                        TargetType = targetType,
                        //Category = param.Count > 0 ? (StrangerCategory.State | StrangerCategory.Parameter) : StrangerCategory.State,
                        Params = dic,
                        Infinite = infinite
                    };
                    if (CurrObservers.ContainsKey(user))
                    {
                        //CurrObservers[user].Clear();

                        CurrObservers[user] = CurrObservers[user].Where(x => x.TargetId != target).ToList();
                        CurrObservers[user].Add(node);
                    }
                    else
                    {
                        CurrObservers.Add(user, new List<MonitorMeta>() { node });
                    }
                }

                if (timer == null)
                {
                    //如果未启动，则启动监控程序
                    timer = new Timer(2000);
                    timer.Elapsed += new ElapsedEventHandler(DispatchTask);
                    timer.Enabled = true;
                    timer.AutoReset = true;
                }
            }
            catch (Exception ex)
            {
                MyConsole.log(ex, "添加设备监测异常");
            }
        }


        public static void RemoveWatch(string user, int target, CategoryDictionary type)
        {
            try
            {
                if (string.IsNullOrEmpty(user)) return;
                var notifier = GlobalHost.ConnectionManager.GetHubContext<MyHub>();

                //var onlines = MyHub.CurrClients.Keys;
                var isonline = MyHub.IsOnline(user);
                if (!isonline)
                {
                    if (CurrObservers.ContainsKey(user)) CurrObservers.Remove(user);
                    return;
                }
                else
                {
                    if (CurrObservers.ContainsKey(user))
                    {
                        CurrObservers[user] = CurrObservers[user].Where(x => !(x.TargetId == target && x.TargetType == type)).ToList();
                        if (CurrObservers[user].Count == 0) CurrObservers.Remove(user);
                    }
                    //else
                    //{
                    //    CurrObservers.Add(user, new List<string>() { meter });
                    //}
                }

                if (timer != null)
                {
                    //如果需要监测的用户为启动，则启动监控程序
                    if (CurrObservers.Keys.Count == 0)
                    {
                        timer.Dispose();
                    }
                    //timer = new Timer(2000);
                    //timer.Elapsed += new ElapsedEventHandler(DispatchTask);
                    //timer.Enabled = true;
                    //timer.AutoReset = true;
                }
            }
            catch (Exception ex)
            {
                MyConsole.log(ex, "移除设备监测异常");
            }

        }


        public static void DispatchTask(object sender, EventArgs e)
        {
            //EnergyContext ctx = new EnergyContext();
            ////var receivers = msg.Receiver.Where(x => (NoticeMode)x.mode == NoticeMode.Web && onlines.Contains(x.user)).ToList();
            //foreach (var usr in CurrObservers.Keys)
            //{
            //    var meterids = CurrObservers[usr].Where(x => x.Infinite).Select(x => x.Meter).ToList();
            //    var keys = CurrObservers[usr].SelectMany(x => x.Params.Keys).ToList();
            //    var list = ctx.Meters.Where(x => meterids.Contains(x.Id)).Select(x => new MonitorMeta()
            //    {
            //        Meter = x.Id,
            //        State = x.State
            //    }).ToList();
            //    var result = new List<MonitorMeta>();
            //    //foreach (var item in list)
            //    //{
            //    //    result.Add(new MonitorMeta(){
            //    //        Meter = item.Id,
            //    //        Category = 
            //    //    })
            //    //}
            //    //foreach (var item in cids)
            //    //{
            //    //    notifier.Clients.Client(item).momentary(new
            //    //    {

            //    //    });
            //    //}

            //    notifier.Clients.Client(usr).deliver(list);
            //}
        }


        /// <summary>
        /// 转发从
        /// </summary>
        /// <param name="man"></param>
        public static void Transmit(MonitorMeta man)
        {
            //EmpContext ctx = new EmpContext();
            ////var receivers = msg.Receiver.Where(x => (NoticeMode)x.mode == NoticeMode.Web && onlines.Contains(x.user)).ToList();
            //var users = new List<string>();
            //MyConsole.log(string.Format(" Meter: '{0}', State: {1} ", man.Meter, man.State), "水阀状态反馈");
            //foreach (var usr in CurrObservers.Keys)
            //{
            //    var haveToDeliver = CurrObservers[usr].Where(x => x.Meter == man.Meter).Count() > 0;
            //    if (haveToDeliver)
            //    {
            //        var connId = MyHub.GetConnectId(usr);
            //        if (string.IsNullOrEmpty(connId)) continue;
            //        notifier.Clients.Client(connId).deliver(man);
            //        //CurrObservers.Value] = CurrObservers[usr].Where(x => !x.Infinite && x.Meter != man.Meter).ToList();
            //    }
            //}


        }
    }
}