using WxPay2017.API.DAL.EmpModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WxPay2017.API.VO.Common;
using WxPay2017.API.VO;
using WxPay2017.API.VO.Param;

namespace WxPay2017.API.BLL
{
    public class HistoryBillBLL : Repository<HistoryBill>
    {
        public DictionaryBLL dictionaryBLL = new DictionaryBLL();
        public BuildingBLL buildingBLL = new BuildingBLL();
        public MeterBLL meterBLL = new MeterBLL();
        public OrganizationBLL organizationBLL = new OrganizationBLL();
        //public MonitoringConfigBLL monitoringConfigBLL = new MonitoringConfigBLL();
        public UserBLL userBLL = new UserBLL();
        public MessageBLL messageBLL = new MessageBLL();
         

        public HistoryBillBLL(EmpContext context = null)
            : base(context)
        { 
            dictionaryBLL = new DictionaryBLL(this.db);
            buildingBLL = new BuildingBLL(this.db);
            organizationBLL = new OrganizationBLL(this.db);
            //monitoringConfigBLL = new MonitoringConfigBLL(this.db);
            meterBLL = new MeterBLL(this.db);
            userBLL = new UserBLL(this.db);
            messageBLL = new MessageBLL(this.db);
        }
        /// <summary>
        /// 获得当前用户的账号信息
        /// </summary>
        /// <param name="category">查询对象分类</param>
        /// <param name="targetId">查询对象</param>
        /// <param name="nowBill">账户信息</param>
        /// <param name="user">查询用户</param>
        public NowBill GetNowMustPay(CategoryDictionary category, int targetId, NowBill nowBill)
        {
            var billPayTypes = DictionaryCache.Get().Values.Where(o => o.Code == "PayType" && o.Enable == true && o.ChineseName != "通用").ToList();
            User user = userBLL.GetAccountUser(category, targetId, true);
            if (user == null)
                return nowBill;

            var lastBalance = db.Balances.Where(o => o.TargetCategory == (int)category && o.TargetId == targetId).OrderByDescending(o => o.Id).Take(1).FirstOrDefault();
            if (lastBalance == null)
                return nowBill;
            nowBill = new NowBill();
            nowBill.NowCost = lastBalance.Total;
            return nowBill;
        }



        /// <summary>
        /// 检测对象是否有和费用相关的告警触发或解除，并进行设备联动
        /// 返回为需要付出告警的消息信息
        /// </summary>
        /// <param name="userId"></param>
        public async Task<List<MessageData>> CheckIsArrearage(string userId1,int? buildingId=null,Balance balance=null,List<MonitoringConfig> monitoringConfigs=null)
        {
            TargetData targetInfo=new TargetData();
            if (buildingId == null)
                targetInfo = userBLL.GetTargetInfo(userId1);
            else
            {
                targetInfo.Category = CategoryDictionary.Building;
                targetInfo.TargetId = (int)buildingId;
            }

            List<MessageData> results = new List<MessageData>();
            //if (user != null)
            {
                var nowBill = new NowBill();
                //当前账单
                if (balance== null)
                    nowBill = GetNowMustPay(targetInfo.Category, targetInfo.TargetId, nowBill);
                else
                {
                    nowBill.NowCost = (decimal)balance.Total;
                }
                if (monitoringConfigs == null)
                {
                    if (BLL.MyConsole.GetAppString("IsNeedSync") != "true")
                        monitoringConfigs = this.db.MonitoringConfig.Where(o => o.Enabled == true && o.ConfigTypeId == DictionaryCache.MonitoringConfigTypeWarning.Id && DictionaryCache.MessagesAboutMoney.Contains((int)o.WayId) && o.StartTime <= DateTime.Now && o.EndTime >= DateTime.Now && o.TargetTypeId == (targetInfo.Category == CategoryDictionary.Building ? DictionaryCache.ConfigToBuilding.Id : DictionaryCache.ConfigToOrg.Id) && o.TargetId == targetInfo.TargetId).ToList();
                    else
                        monitoringConfigs = this.db.MonitoringConfig.Where(o => o.Enabled == true && (o.ConfigTypeId == DictionaryCache.MonitoringConfigTypeWarning.Id + 5) && DictionaryCache.MessagesAboutMoney.Contains((int)o.WayId) && o.StartTime <= DateTime.Now && o.EndTime >= DateTime.Now && o.TargetTypeId == (targetInfo.Category == CategoryDictionary.Building ? DictionaryCache.ConfigToBuilding.Id : DictionaryCache.ConfigToOrg.Id)).ToList();
                }
                if (monitoringConfigs.Count > 0)
                {
                    //有此类型告警需求
                    foreach (var config in monitoringConfigs)
                    {
                        var messages = messageBLL.Filter(o => o.EndDate == null && o.MessageSourceTypeId == (targetInfo.Category == CategoryDictionary.Building ? DictionaryCache.MessageSourceTypeBuilding.Id : DictionaryCache.MessageSourceTypeOrg.Id) && o.SrcId == targetInfo.TargetId + "" && o.MessageTypeId == config.WayId).ToList();
                        if (nowBill.NowCost <= config.UnitValue)
                        {
                            //需要告警
                            //是否已经发出过此类告警，未结束  
                            if (messages.Count() == 0)
                            {
                                //未发出此类告警，则触发告警联动，此方法用于缴费退费等操作，告警消息不从此发出，
                                if (config.Value != null && BLL.MyConsole.GetAppString("IsNeedSync") != "true")
                                {
                                    var action = DictionaryCache.Get()[Convert.ToInt32(config.Value)];
                                    await meterBLL.LinkageControl(targetInfo.Category, new List<int> { targetInfo.TargetId }, action, 0);
                                }
                                 string name = "";
                                if (targetInfo.Category == CategoryDictionary.Building)
                                    name = buildingBLL.Find(targetInfo.TargetId).Name;
                                else
                                    name = organizationBLL.Find(targetInfo.TargetId).Name;
                                var subject = name + config.Name+ ",截止" + DateTime.Now.ToString();
                                subject = subject + ",您的可用金额为" + string.Format("{0:0.00}", nowBill.NowCost) + "元";
                                var message = messageBLL.CreateMessageData((int)config.WayId, DictionaryCache.MessageSourceTypeBuilding.Id, targetInfo.TargetId,subject,subject);
                                results.Add(message);
                            }
                        }
                        else
                        {
                            //解除此告警
                            if (messages.Count() > 0)
                            {
                                foreach (var item in messages)
                                {
                                    item.EndDate = DateTime.Now;
                                    messageBLL.Update(item);
                                }

                                if (config.Value != null&&BLL.MyConsole.GetAppString("IsNeedSync") != "true")
                                {
                                    var action = DictionaryCache.Get()[Convert.ToInt32(config.Value)];
                                     await meterBLL.LinkageControl(CategoryDictionary.Building, new List<int> { targetInfo.TargetId}, action, 0, false);
                                }
                            }
                           
                        }
                    }
                }
            }
            return results;
        }

        /// <summary>
        /// 发补贴
        /// </summary>
        /// <param name="buildingInfos">建筑对象</param>
        /// <param name="value">每个人补贴金额</param>
        /// <param name="value">过期时间</param>
        public void GiveSubsidies(List<SubsidiesForBuildingInfo> buildingInfos,DateTime? startTime,DateTime? overTime,decimal value,int energyCategoryId)
        {
            string str = "";
            foreach (var buildingInfo in buildingInfos)
            {
                str = str + buildingInfo.BuildingId +"*"+buildingInfo.ReceiverId+">"+buildingInfo.BeforeAccount+"#"+buildingInfo.EndAccount+ ",";
                foreach(var user in buildingInfo.UserInfos)
                    str = str + user.UserFullName +"<"+user.UserId + "|";
                str = str + "@";
            }
            var sql = string.Format("GiveSubsidiesForBuilding '{0}',{1},{2},{3},{4}", str, value, (startTime == null ? "NULL" : "'" + startTime.ToString() + "'"), (overTime == null ? "NULL" : "'" + overTime.ToString() + "'"), energyCategoryId);

            //var sql = string.Format(" CreateMeterAction '{0}',{1},'{2}','{3}',{4},{5},'{6}',{7},{8} ,{9},'{10}'", ids, actionId, value, actionTime.ToString().Replace("/", "-"), IsPowerOffByMoney ? 1 : 0, IsPowerOffByTime ? 1 : 0, DateTime.Now.ToString().Replace("/", "-"), priority, (groupId==null?"NULL":""+groupId), (pid==null?"NULL":""), desc);
            var reuslt = this.db.Database.ExecuteSqlCommand(sql);

        }

        /// <summary>
        /// 删除过期的historybill
        /// </summary>
        public void DelOverTimeBill()
        {
            try
            {
                int days = Convert.ToInt32(MyConsole.GetAppString("HistoryBillMaxDays"));
                var beginday = DateTime.Now.Date.AddDays(-days);
                var yestoryday = DateTime.Now.Date.AddDays(-1);
               
                //string str = "delete from bill.historybill where  PayMentTime<'" + String.Format("{0:0000}-{1:00}-{2:00}", beginday.Year, beginday.Month, beginday.Day) + "' and billtypeid=120001 and paytypeid=380001 and ispay=1 and issynchro=1";
                //备份昨日前每日采集扣费数据
                string str = "[backupHistoryBill] " + String.Format("'{0:0000}-{1:00}-{2:00}',120001", yestoryday.Year, yestoryday.Month, yestoryday.Day);
                this.db.Database.ExecuteSqlCommand(str);
                //备份设定日期，默认100天前每日统一账户扣费数据
                str = "[backupHistoryBill] " + String.Format("'{0:0000}-{1:00}-{2:00}',120007", beginday.Year, beginday.Month, beginday.Day);
                this.db.Database.ExecuteSqlCommand(str);
                beginday = DateTime.Now.Date.AddDays(-30);
                str = "[backupAction] " + String.Format("'{0:0000}-{1:00}-{2:00}',120007", beginday.Year, beginday.Month, beginday.Day);
                this.db.Database.ExecuteSqlCommand(str);
            }
            catch { }

        }

    }
}
