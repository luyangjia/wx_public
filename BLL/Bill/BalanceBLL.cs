using WxPay2017.API.DAL.EmpModels;
using WxPay2017.API.VO.Common;
using WxPay2017.API.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using WxPay2017.API.VO.Param;

namespace WxPay2017.API.BLL
{
    public class BalanceBLL : Repository<Balance>
    {
        Encrypt encrypt = new Encrypt();

        private DictionaryBLL dictionaryBLL = new DictionaryBLL();
        private BuildingBLL buildingBLL = new BuildingBLL();
        private MeterBLL meterBLL = new MeterBLL();
        private OrganizationBLL organizationBLL = new OrganizationBLL();
        private MonitoringConfigBLL monitoringConfigBLL = new MonitoringConfigBLL();
        public UserBLL userBLL = new UserBLL();
        private UserAccountBLL userAccountBLL = new UserAccountBLL();
        private HistoryBillBLL historyBillBLL = new HistoryBillBLL();
        private BalanceDetailBLL balanceDetailBLL = new BalanceDetailBLL();
        private MessageBLL messageBLL = new MessageBLL();
        private MetersActionBLL metersActionBLL = new MetersActionBLL();

        public BalanceBLL(EmpContext context = null)
            : base(context)
        {
            dictionaryBLL = new DictionaryBLL(this.db);
            buildingBLL = new BuildingBLL(this.db);
            organizationBLL = new OrganizationBLL(this.db);
            monitoringConfigBLL = new MonitoringConfigBLL(this.db);
            meterBLL = new MeterBLL(this.db);
            balanceDetailBLL = new BalanceDetailBLL(this.db);
            historyBillBLL = new HistoryBillBLL(this.db);
            balanceDetailBLL = new BalanceDetailBLL(this.db);
            userBLL = new UserBLL(this.db);
            messageBLL = new MessageBLL(this.db);
            metersActionBLL = new MetersActionBLL(this.db);
        }

        public void InitTargetBalance(CategoryDictionary targetCategory, int targetId, string OperatorId,bool isSaveChange=true)
        {
            if (isSaveChange != true)
                balanceDetailBLL.shareContext = true;
            Balance balance = new Balance();
            balance.TargetCategory = (int)targetCategory;
            balance.TargetId = targetId;
            balance.OperatorId = OperatorId;
            balance.EnergyCategory = DictionaryCache.AllCategory.Id;
            balance.Price = 0;
            balance.EnergyConsumption = 0;
            balance.Overplus = 0;
            balance.Prepay = 0;
            balance.Subsidy = 0;
            balance.Recharge = 0;
            balance.CashCharge = 0;
            balance.CashCorrect = 0;
            balance.Usage = 0;
            balance.Refund = 0;
            balance.BadDebt = 0;
            balance.Total = 0;
            balance.AuditDate = Convert.ToDateTime(DateTime.Now .ToShortDateString());
            balance.CreateDate = DateTime.Now.AddYears(-10);
            balance=this.Create(balance);

            Building buiding = null;
            //获取该对象定价!!!!!!!!!!!!!!!!!!!!!!!!!对机构的配置未考虑
            decimal? price = 0;
            if (CategoryDictionary.Building==targetCategory){
                 price = monitoringConfigBLL.GetBuildingPriceConfigdetail(targetId, DictionaryCache.PowerCateogry.Id, DateTime.Now, ref buiding);
                 if (price == null)
                     price = -1;
                BalanceDetail detail1 = new BalanceDetail();
                detail1.Id = balance.Id;
                detail1.Balance = balance;
                detail1.EnergyCategory = DictionaryCache.PowerCateogry.Id;
                detail1.Consumption = 0;
                detail1.Amount = 0;
                detail1.AmountTotal = 0;
                detail1.ConsumptionTotal = 0;
                detail1.Price = (decimal)price;
                detail1 = balanceDetailBLL.Create(detail1);

                //price = monitoringConfigBLL.GetBuildingPriceConfigdetail(targetId, DictionaryCache.WaterCategory.Id, DateTime.Now, ref buiding);
                //if (price == null)
                //    price = -1;
                detail1 = new BalanceDetail(); 
                detail1.Balance = balance;
                detail1.Id = balance.Id;
                detail1.EnergyCategory = DictionaryCache.WaterCategory.Id;
                detail1.Consumption = 0;
                detail1.Amount = 0;
                detail1.AmountTotal = 0;
                detail1.ConsumptionTotal = 0;
                detail1.Price = (decimal)price;
                detail1 = balanceDetailBLL.Create(detail1);
            }
        }


        /// <summary>
        /// 结算特定对象，此时间前未结算日数据均会生成
        /// </summary>
        /// <param name="time">结算时间，取年月日</param>
        /// <param name="category">分类building,orgarnization</param>
        /// <param name="targetId">对象id</param>
        /// <param name="operatorId">操作者id</param>
        /// <returns></returns>
        public async Task<bool> Settle(DateTime time, CategoryDictionary category, int targetId, string operatorId, User user = null)
        {
         
            bool isOk = false;
            time = Convert.ToDateTime(time.ToShortDateString());
            var yestoday = time.AddDays(-1);
            ////如果是今天，则删除今天的上一条统计数据重新统计
            //if (time.Year == DateTime.Now.Year && time.Month == DateTime.Now.Month && time.Day == DateTime.Now.Day)
            {
                //删除该日数据
                var balanceToday = this.Filter(o => o.TargetId == targetId && o.TargetCategory == (int)category && o.AuditDate == time).FirstOrDefault();
                if (balanceToday != null)
                {
                    balanceDetailBLL.db.BalanceDetials.RemoveRange(balanceToday.BalanceDetails);
                    balanceDetailBLL.db.SaveChanges();
                    this.Delete(balanceToday);
                }
            }
          
           
            {
                try
                {
                    if (user == null)
                         user = userBLL.GetAccountUser(category, targetId,false);
                   
                    if (user == null)
                        return false;
                   
                    //生成该账户数据VVVVVVVVVVVVVV
                    Balance balance = new Balance();
                    balance.TargetId = targetId;
                    balance.TargetCategory = (int)category;

                   
                    #region 昨日剩余
                    //如果没有昨日结算，则向前递归计算昨日的结算数据
                    //一直结算到该对象没有历史账单为止

                    var yesterdayBalance = this.Filter(o => o.AuditDate == yestoday && o.TargetId == targetId && o.TargetCategory == (int)category).ToList();
                    if (yesterdayBalance.Count() == 0)
                    {
                        if (historyBillBLL.Count(o => o.ReceiverId == user.Id && o.PayMentTime < time) > 0)
                        {
                            //统计昨天数据
                            await Settle(yestoday, category, targetId, operatorId, user);
                            //获得昨天数值
                            balance.Overplus = this.Filter(o => o.AuditDate == yestoday && o.TargetId == targetId && o.TargetCategory == (int)category).FirstOrDefault().Total;
                        }
                        else
                            balance.Overplus = 0;
                    }
                    else
                        balance.Overplus = yesterdayBalance[0].Total;
                    #endregion
                   
                    var endTime = time.AddDays(1);

                
                    IList<HistoryBill> list = historyBillBLL.Filter(o => o.ReceiverId == user.Id &&
                        o.PayMentTime >= time && o.PayMentTime < endTime &&
                        o.IsPay == true).ToList();
                    #region 本日补贴
                    int BillTypeSubsidyOverTime = DictionaryCache.BillTypeSubsidyOverTime.Id;
                    int BillTypeSubsidy = DictionaryCache.BillTypeSubsidy.Id;
                    balance.Subsidy = list.Where(o => o.BillTypeId == BillTypeSubsidyOverTime || o.BillTypeId == BillTypeSubsidy).Sum(o => o.Value);

                    #endregion

                    #region 本日预交费
                    balance.Prepay = list.Where(o =>
                        (o.BillTypeId == DictionaryCache.BillTypePay.Id || o.BillTypeId == DictionaryCache.BillTypePrePay.Id) &&
                        o.PayMethodId == DictionaryCache.PayMethodPrePay.Id).Sum(o => o.Value);
                    #endregion
                    #region 本日 纠错
                    balance.CashCorrect = list.Where(o =>
                        (o.BillTypeId == DictionaryCache.BillTypePay.Id || o.BillTypeId == DictionaryCache.BillTypePrePay.Id) &&
                        o.PayMethodId == DictionaryCache.PayMethodCashCorrect.Id).Sum(o => o.Value);
                    #endregion

                    #region 本日非现金在线充值
                    balance.Recharge = list.Where(o =>
                        (o.BillTypeId == DictionaryCache.BillTypePay.Id || o.BillTypeId == DictionaryCache.BillTypePrePay.Id) &&
                        o.PayMethodId != DictionaryCache.PayMethodCash.Id 
                        && o.PayMethodId != DictionaryCache.PayMethodCashCorrect.Id 
                        && o.PayMethodId != DictionaryCache.PayMethodBadBalance.Id 
                        && o.PayMethodId != DictionaryCache.PayMethodSubsidyOverTime.Id 
                        && o.PayMethodId != DictionaryCache.PayMethodPrePay.Id).SumZero(o => o.Value);
                  
                    #endregion

                    #region 本日现金充值
                    balance.CashCharge = list.Where(o =>
                        o.BillTypeId == DictionaryCache.BillTypePay.Id &&
                        o.PayMethodId == DictionaryCache.PayMethodCash.Id ).SumZero(o => o.Value);
                    #endregion

                    #region 本日能耗
                    balance.Usage = 0;// balance.BalanceDetails.Sum(o => o.Consumption);
                    #endregion

                    #region 本日退还
                    balance.Refund = list.Where(o =>
                        o.BillTypeId == DictionaryCache.BillTypePay.Id &&
                        o.PayMethodId == DictionaryCache.PayMethodRefund.Id).SumZero(o => o.Value);
                    #endregion

                    #region 本日坏账
                    balance.BadDebt = list.Where(o =>
                        o.BillTypeId == DictionaryCache.BillTypePay.Id &&
                        o.PayMethodId == DictionaryCache.PayMethodBadBalance.Id).SumZero(o => o.Value);
                    #endregion

                    #region 本日结余
                    balance.Total = balance.Overplus + balance.Subsidy + balance.Recharge + balance.CashCharge + balance.Usage + balance.Refund + balance.BadDebt + balance.CashCorrect + balance.Prepay;
                    //本日结余=上日结余+本日（自身消费+充值+补贴  +  下级所有子机构、子建筑的充值+补贴）
                    //建筑：获取下级所有缴费子建筑userid
                    List<int> childrenBuildingIds = new List<int>();
                    List<int> childrenOrgIds = new List<int>();
                    if (category == CategoryDictionary.Building)
                    {
                        var building = user.Buildings.First();
                        childrenBuildingIds = buildingBLL.Filter(o => o.Enable && o.Users.Any(c => c.Roles.Any(k => k.Id == RoleCache.PayMentRoleId)) && o.TreeId.StartsWith(building.TreeId + "-")).Select(o => o.Id).ToList();
                    }
                    //机构：获取下级所有缴费子机构、子建筑userid
                    else
                    {
                        var org = user.Organizations.First();
                        //获得所有收费的子机构
                        var orgsTreeIdshHasGetPay = organizationBLL.Filter(o => o.Enable && o.TreeId.StartsWith(org.TreeId + "-") && o.Users.Any(c => c.Roles.Any(k => k.Id == RoleCache.PayMentRoleId) && c.SecurityStamp != null && c.SecurityStamp.EndsWith(".pem"))).Select(o => o.TreeId).Distinct().ToList();

                        //缴费子机构，且其上级机构不在orgsTreeIdshHasGetPay集合中
                        childrenOrgIds = organizationBLL.Filter(o => o.Enable && o.Users.Any(c => c.Roles.Any(k => k.Id == RoleCache.PayMentRoleId)) && o.TreeId.StartsWith(org.TreeId + "-") && !orgsTreeIdshHasGetPay.Any(c => o.TreeId.StartsWith(c + "-"))).Select(o => o.Id).ToList();
                        //缴费子建筑：建筑的所属机构是其子机构或自身，且其上级机构不在orgsTreeIdshHasGetPay集合中
                        childrenBuildingIds = buildingBLL.Filter(o => o.Enable && o.Users.Any(c => c.Roles.Any(k => k.Id == RoleCache.PayMentRoleId)) && o.Organization.TreeId.StartsWith(org.TreeId + "-") && !orgsTreeIdshHasGetPay.Any(c => o.Organization.TreeId.StartsWith(c + "-"))).Select(o => o.Id).ToList();
                    }

                    foreach (var cid in childrenBuildingIds)
                    {
                        //如果该子账号未完成本日结算，先进行本日结算
                        var childBalance = this.db.Balances.Where(o => o.AuditDate == time && o.TargetId == cid && o.TargetCategory == (int)CategoryDictionary.Building).FirstOrDefault();
                        if (childBalance == null)
                        {
                            //统计子对象数据
                            await Settle(time, CategoryDictionary.Building, cid, operatorId);
                            //获得昨天数值
                            childBalance = this.db.Balances.Where(o => o.AuditDate == time && o.TargetId == cid && o.TargetCategory == (int)CategoryDictionary.Building).FirstOrDefault();
                        }
                        balance.Total = balance.Total + childBalance.CashCharge + childBalance.Subsidy + childBalance.Recharge + childBalance.Refund + childBalance.BadDebt;
                    }
                    foreach (var cid in childrenOrgIds)
                    {
                        //如果该子账号未完成本日结算，先进行本日结算
                        var childBalance = this.db.Balances.Where(o => o.AuditDate == time && o.TargetId == cid && o.TargetCategory == (int)CategoryDictionary.Organization).FirstOrDefault();
                        if (childBalance == null)
                        {
                            //统计子对象数据
                            await Settle(time, CategoryDictionary.Organization, cid, operatorId);
                            //获得昨天数值
                            childBalance = this.db.Balances.Where(o => o.AuditDate == time && o.TargetId == cid && o.TargetCategory == (int)CategoryDictionary.Organization).FirstOrDefault();
                        }
                        balance.Total = balance.Total + childBalance.CashCharge + childBalance.Subsidy + childBalance.Recharge + childBalance.Refund + childBalance.BadDebt;
                    }
                    #endregion

                    #region 结算日期
                    balance.AuditDate = time;
                    #endregion
                    balance.OperatorId = operatorId;
                    balance.CreateDate = DateTime.Now;
                    balance.EnergyCategory = 90000;// DictionaryCache.PayTypeAll.Id;

                    //balance = Create(balance);

                    #region 能耗数据
                    var historys = list.Where(o =>
                        (o.BillTypeId == DictionaryCache.BillTypeAuto.Id || o.BillTypeId == DictionaryCache.BillTypeManual.Id));

                    var payTypes = dictionaryBLL.Filter(o => o.Enable && o.Code == "PayType" && o.EquText != null).ToList();
                    foreach (var payType in payTypes)
                    {
                        BalanceDetail detail = new BalanceDetail();
                        detail.EnergyCategory = DictionaryCache.Get().Values.FirstOrDefault(o => o.TreeId == payType.EquText).Id;
                        //能耗消费
                        detail.Consumption = historys.Where(o => o.PayTypeId == payType.Id).SumZero(o => o.Value);
                        //能耗总量
                        detail.Amount = historys.Where(o => o.PayTypeId == payType.Id).SumZero(o => (decimal)o.UsedEnergyValue);

                        detail.Price = -1;//单价暂不保存，查询通知信息
                        detail.Balance = balance;
                        detail.BalanceId = balance.Id;
                        balance.BalanceDetails.Add(detail);

                    }
                    balance.Usage = balance.BalanceDetails.Sum(o => o.Consumption);
                    balance.Total = balance.Total + balance.Usage;
                    Create(balance);         
                    #endregion
                    
                    isOk = true;
                }
                catch (Exception ex)
                {
                    MyConsole.Log(ex, "结算出错");
                    throw ex;
                }
            }

           
            return isOk;
        }

        /// <summary>
        /// 批量结算,此时间前未结算日数据均会生成
        /// </summary>
        public async Task SettleBatch(DateTime time, string operatorId)
        {
          
            TransactionOptions option = new TransactionOptions();
            option.Timeout = new TimeSpan(0, 60, 0);
            var endTime = Convert.ToDateTime(time.ToShortDateString()).AddDays(1);
            int total =0;
            int num = 0;
            {
                try
                {
                    var childrenOrgIds = organizationBLL.Filter(o => o.Enable && o.Users.Any(c => c.Roles.Any(k => k.Id == RoleCache.PayMentRoleId))).OrderByDescending(o => o.TreeId).Select(o => o.Id).ToList();
                    var childrenBuildingIds = buildingBLL.Filter(o => o.Enable && o.Users.Any(c => c.Roles.Any(k => k.Id == RoleCache.PayMentRoleId))).OrderByDescending(o => o.TreeId).Select(o => o.Id).ToList();
                    var users = userBLL.Filter(o => (o.Buildings.Any(c => childrenBuildingIds.Contains(c.Id)) || o.Organizations.Any(c => childrenOrgIds.Contains(c.Id))) && o.Roles.Any(c => c.Id == RoleCache.PayMentRoleId)).ToList();
                    total = childrenBuildingIds.Count();

                    //var lastBalances = Filter(o => o.TargetCategory == (int)CategoryDictionary.Building && childrenBuildingIds.Contains(o.TargetId)).OrderByDescending(o => o.AuditDate).GroupBy(o => o.TargetId).Select(o => o.FirstOrDefault()).ToList();

                    foreach (var id in childrenBuildingIds)
                    {
                    
                        num++;
                        System.Web.HttpContext.Current.Application["nowBalanceLevel"] = "正处理" + total + "个建筑结算中的第" + num + "个";
                        //最后一条balance数据，统计从其时间开始到endtime
                        //如果没有balance，则查找history的第一条,作为开始时间
                        //如果都没有,则直接统计endtime这天
                        var beginTime = endTime.AddDays(-1);
                        var balance = Filter(o => o.TargetCategory == (int)CategoryDictionary.Building && o.TargetId == id).OrderByDescending(o => o.AuditDate).Take(1).FirstOrDefault();
                        //if (lastBalances.Where(o => o.TargetCategory == (int)CategoryDictionary.Building && o.TargetId == id).Count() > 1)
                        //{
                        //    int k = 0;
                        //}
                        //var balance = lastBalances.Where(o => o.TargetCategory == (int)CategoryDictionary.Building && o.TargetId == id).FirstOrDefault();
                        if (balance != null)
                            beginTime = balance.AuditDate;
                        else
                        {
                            //var user = userBLL.GetAccountUser(CategoryDictionary.Building, id);
                            var user = users.Where(o=>o.Buildings.Any(c=>c.Id==id)).FirstOrDefault();
                            if (user == null)
                                continue;
                            var history = historyBillBLL.Filter(o => o.ReceiverId == user.Id).OrderBy(o => o.CreateTime).Take(1).FirstOrDefault();
                            if (history != null)
                                beginTime = Convert.ToDateTime(((DateTime)history.CreateTime).ToShortDateString());
                        }     
                        for (var t = beginTime; t < endTime; t = t.AddDays(1))
                        {
                            for (int i = 0; i < 10; i++)
                            {

                                using (var scope = new TransactionScope(TransactionScopeOption.Required, option))
                                {
                                    try
                                    {
                                        await SettleToObejct(operatorId, t, CategoryDictionary.Building, id);
                                        scope.Complete();
                                       
                                        break;
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine("org:" + id + "," + i);
                                        MyConsole.Log(ex);
                                        System.Threading.Thread.Sleep(1000);
                                        //出现事务互操作操作异常，延时1秒重新进行，10次均失败抛出异常
                                        if (i >= 9)
                                            throw ex;
                                    }
                                }

                            }
                        }
                      
                    }

                    num = 0;
                    total = childrenOrgIds.Count();
                    foreach (var id in childrenOrgIds)
                    {
                        num++;
                        System.Web.HttpContext.Current.Application["nowBalanceLevel"] = "正处理" + total + "个机构结算中的第" + num + "个";
                        //最后一条balance数据，统计从其时间开始到endtime
                        //如果没有balance，则查找history的第一条,作为开始时间
                        //如果都没有,则直接统计endtime这天
                        var beginTime = endTime.AddDays(-1);
                        var balance = Filter(o => o.TargetCategory == (int)CategoryDictionary.Organization && o.TargetId == id).OrderByDescending(o => o.AuditDate).Take(1).FirstOrDefault();
                        if (balance != null)
                            beginTime = balance.AuditDate;
                        else
                        {
                            //var user = userBLL.GetAccountUser(CategoryDictionary.Organization, id);
                            var user = users.Where(o => o.Buildings.Any(c => c.Id == id)).FirstOrDefault();
                            if (user == null)
                                continue;
                            var history = historyBillBLL.Filter(o => o.ReceiverId == user.Id).OrderByDescending(o => o.CreateTime).Take(1).FirstOrDefault();
                            if (history != null)
                                beginTime = Convert.ToDateTime(((DateTime)history.CreateTime).ToShortDateString()).AddDays(1);
                        }
                        for (var t = beginTime; t < endTime; t = t.AddDays(1))
                        {
                            for (int i = 0; i < 10; i++)
                            {
                                using (var scope = new TransactionScope(TransactionScopeOption.Required, option))
                                {
                                    try
                                    {
                                        await SettleToObejct(operatorId, t, CategoryDictionary.Organization, id);
                                        scope.Complete();
                                        break;
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine("org:" + id + "," + i);
                                        MyConsole.Log(ex);
                                        System.Threading.Thread.Sleep(1000);
                                        //出现事务互操作操作异常，延时1秒重新进行，10次均失败抛出异常
                                        if (i >= 9)
                                            throw ex;
                                    }
                                }
                            }

                        }
                    }
                   
                }


                catch (Exception ex)
                {
                    MyConsole.Log(ex, "结算出错");
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 结算特定对象，生成过去的历史结算数据
        /// </summary>
        /// <param name="operatorId"></param>
        /// <param name="endTime"></param>
        /// <param name="category"></param>
        /// <param name="id"></param>
        public async Task SettleToObejct(string operatorId, DateTime endTime, CategoryDictionary category, int id)
        {
            //触发器自动统计
            if (MyConsole.GetAppString("IsUseTigerToSettle") == "true")
                return;
            //获取该对象的第一笔未结算数据
            var balance = this.Filter(o => o.TargetCategory == (int)category && o.TargetId == id).OrderByDescending(o => o.AuditDate).FirstOrDefault();
            DateTime beginTime = endTime;
            if (balance != null)
            {
                //如果结算日期和最后一条数据日期相同，这天重新统计
                if (balance.AuditDate.ToShortDateString() == endTime.ToShortDateString())
                    beginTime = balance.AuditDate;
                else
                    beginTime = balance.AuditDate.AddDays(1);
            }
            //以此时间向后推，每日结算，直到今天
            for (var i = beginTime; i <= endTime; i = i.AddDays(1))
                await Settle(i, category, id, operatorId);
        }
        /// <summary>
        /// 结算特定对象，生成过去的历史结算数据
        /// </summary>
        /// <param name="operatorId"></param>
        /// <param name="endTime"></param>
        /// <param name="category"></param>
        /// <param name="id"></param>
        public async Task SettleToObejct(string operatorId, DateTime endTime, string userId)
        {
            //触发器自动统计
            if (MyConsole.GetAppString("IsUseTigerToSettle") == "true")
                return;
            var targetInfo = userBLL.GetTargetInfo(userId);
            //获取该对象的第一笔未结算数据
            var balance = this.Filter(o => o.TargetCategory == (int)targetInfo.Category && o.TargetId == targetInfo.TargetId).OrderByDescending(o => o.AuditDate).FirstOrDefault();
            DateTime beginTime = endTime;
            if (balance != null)
            {
                //如果结算日期和最后一条数据日期相同，这天重新统计
                if (balance.AuditDate.ToShortDateString() == endTime.ToShortDateString())
                    beginTime = balance.AuditDate;
                else
                    beginTime = balance.AuditDate.AddDays(1);
            }
            //以此时间向后推，每日结算，直到今天
            for (var i = beginTime; i <= endTime; i = i.AddDays(1))
                await Settle(i, targetInfo.Category, targetInfo.TargetId, operatorId);
        }


        /// <summary>
        /// 缴费充值
        /// </summary>
        /// <param name="receiverId">收款账号</param>
        /// <param name="payerId">付款账户</param>
        /// <param name="paymentAmount">支付金额</param>
        /// <param name="isPay">是否支付成功（支付宝等需要等待服务器确认才支付成功）</param>
        /// <param name="payTypeId">缴费类型（水，电，通用）</param>
        /// <param name="payMethodId">支付方式（支付宝，现金缴费，水电控终端缴费等）</param>
        /// <param name="isDeductPrePay">是否抵扣预交费（默认true）</param>
        /// <param name="originalId">纠错，保存修改对象的id或能耗类型id</param>
        /// <param name="desc">描述信息补充，加载头部</param>
        /// <param name="usedValue">能耗，不写为空</param>
        /// <param name="isSync">是否已经同步，默认false</param>
        /// <returns></returns>
        public HistoryBill Charge(string receiverId, string payerId, decimal paymentAmount, bool isPay, int billTypeId, int payTypeId, int? payMethodId, string operatorId, bool isDeductPrePay = true, int? originalId = null, string desc = "", decimal? UsedEnergyValue = null, bool isSync = false,DateTime? createTime=null)
        {
            if (createTime == null)
                createTime = DateTime.Now;
            if (paymentAmount == 0&&billTypeId!=DictionaryCache.BillTypeAuto.Id)
                return null;
            try
            {
                //生成账单
                HistoryBill order = new HistoryBill();
                order.BeginTime = DateTime.Now;
                order.EndTime = DateTime.Now;
                order.ReceiverId = receiverId;
                order.Value = paymentAmount;
                order.OperatorId = operatorId;
                //缴费
                order.BillTypeId = billTypeId;
                //缴费类型
                order.PayTypeId = payTypeId;
                order.PayerId = payerId;
                order.IsPay = isPay;
                string PayMethordName = "";
                string payTypeName = "";
                if (payMethodId != null)
                    PayMethordName = DictionaryCache.Get()[(int)payMethodId].ChineseName;

                payTypeName = DictionaryCache.Get()[(int)payTypeId].ChineseName;

                order.subject = PayMethordName;
                
                if (payerId != null)
                    order.Body = userBLL.Find(payerId).FullName + PayMethordName + "金额" + string.Format("{0:0.00}", paymentAmount) + "元。";
                else if (payTypeName != "")
                {
                    if (order.BillTypeId == DictionaryCache.BillTypePrePay.Id && order.Value < 0)
                    {
                        order.subject = "预付费回收";
                        order.UsedValue = order.Value;
                        order.Body = "回收" + desc + PayMethordName + "(" + payTypeName + ")金额" + string.Format("{0:0.00}", -paymentAmount) + "元。";
                    }else  if (order.BillTypeId == DictionaryCache.BillTypePrePay.Id && order.Value > 0)
                    {
                        order.UsedValue = 0;
                        order.Body = desc + PayMethordName + "(" + payTypeName + ")金额" + string.Format("{0:0.00}", paymentAmount) + "元。";
                    }
                    else
                        order.Body = desc + PayMethordName + "(" + payTypeName + ")金额" + string.Format("{0:0.00}", paymentAmount) + "元。";
                }
                else
                    order.Body = desc + PayMethordName + "金额" + string.Format("{0:0.00}", paymentAmount) + "元。";
                if (billTypeId == DictionaryCache.BillTypeUnifiedAccount.Id && payMethodId == DictionaryCache.PayMethodWaterElectCtrl.Id && paymentAmount < 0)
                {
                    order.subject = desc+"基础用电统一缴费";
                    order.Body = desc + "基础用电统一缴费，代扣金额" + string.Format("{0:0.00}", paymentAmount) + "元。";
                }
                order.CreateTime = createTime;
                order.IsZero = false;
                order.IsSynchro = isSync;
                //退费补贴，则金额自动标记为消耗完毕
                if (order.PayMethodId == DictionaryCache.PayMethodSubsidyOverTime.Id)
                    order.UsedValue = order.Value;
                else if (UsedEnergyValue != null)
                    order.UsedEnergyValue = UsedEnergyValue;
                else
                    order.UsedEnergyValue = 0;
                //支付方式
                order.PayMethodId = payMethodId;
                order.TransNumber = null;
                if (isPay)
                {
                    order.PayMentTime = createTime;
                }
                if (originalId != null)
                    order.TransNumber = originalId+"";
                order = historyBillBLL.Create(order);

                bool isOnlyOneAccount = BLL.MyConsole.GetAppString("IsOnlyOneAccount") == "true";
                //如果支付完成，则修改账户
                if (isPay)
                {
                    //抵扣预交费
                    if (isDeductPrePay && order.PayMethodId != DictionaryCache.PayMethodPrePay.Id && order.Value > 0)
                    {
                        var value = DeductPrePay(order, operatorId);
                        //更新用户现金账户
                        if (value!=0)
                        if (billTypeId==DictionaryCache.BillTypeSubsidy.Id||billTypeId==DictionaryCache.BillTypeSubsidyOverTime.Id)
                            UpdateCashAccount(order.ReceiverId, value, DictionaryCache.BalancePowerSubsidy.Id);
                        else
                            UpdateCashAccount(order.ReceiverId, value, DictionaryCache.BalancePowerMoney.Id);
                    }
                    else
                    {
                        if (order.Value != 0)
                            if (billTypeId == DictionaryCache.BillTypeSubsidy.Id || billTypeId == DictionaryCache.BillTypeSubsidyOverTime.Id)
                            {
                                if (isOnlyOneAccount||(payTypeId == DictionaryCache.PayTypePower.Id || payTypeId == DictionaryCache.PayTypeAll.Id))
                                {
                                    UpdateCashAccount(order.ReceiverId, order.Value, DictionaryCache.BalancePowerSubsidy.Id);
                                }else
                                    UpdateCashAccount(order.ReceiverId, order.Value, DictionaryCache.BalanceWaterSubsidy.Id);
                            }

                            else
                            {
                                if (isOnlyOneAccount||(payTypeId == DictionaryCache.PayTypePower.Id || payTypeId == DictionaryCache.PayTypeAll.Id))
                                    UpdateCashAccount(order.ReceiverId, order.Value, DictionaryCache.BalancePowerMoney.Id);
                                else
                                    UpdateCashAccount(order.ReceiverId, order.Value, DictionaryCache.BalanceWaterMoney.Id);
                            }
                    }
                }
                return order;
            }
            catch (Exception ex)
            {
                MyConsole.Log(ex, "现金充值出错");
                throw ex;
            }

        }

        /// <summary>
        ///更新用户现金账户
        /// </summary>
        /// <param name="receiverId">用户id</param>
        /// <param name="value">金额</param>
        public void UpdateCashAccount(string receiverId, decimal value,int typeId)
        {
            if (MyConsole.GetAppString("IsUseTigerToSettle") == "true")
                return;
            try
            {
                //修改用户账户
                var userAccount = userAccountBLL.Filter(o => o.UserId == receiverId && o.BalanceTypeId == typeId).ToList()[0];
                double money = Convert.ToDouble(encrypt.Decrypto(userAccount.Balance)) + (double)value;
                userAccount.Balance = encrypt.Encrypto(money.ToString());
                userAccountBLL.Update(userAccount);
            }
            catch (Exception ex)
            {
                MyConsole.Log(ex);
                throw ex;
            }

        }

        /// <summary>
        /// 抵扣预交费账单
        /// </summary>
        /// <param name="order">生成的充值账单</param>
        /// <returns>实际充值金额</returns>
        public decimal DeductPrePay(HistoryBill order, string operatorId)
        {
            var value = order.Value;
            try
            {
                //检查是否有预交费未抵扣数据
                int prePayId = DictionaryCache.BillTypePrePay.Id;
                List<HistoryBill> prepayBills = null;
                //!!!!!!!!!!!!!!!!!!with nolock查询
                var transactionOptions = new System.Transactions.TransactionOptions();
                transactionOptions.IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted;
                using (var transactionScope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Required, transactionOptions))
                {
                    prepayBills = historyBillBLL.Filter(o => o.BillTypeId == DictionaryCache.BillTypePrePay.Id && o.ReceiverId == order.ReceiverId && o.Value > o.UsedValue).ToList();
                    transactionScope.Complete();
                }

                foreach (var bill in prepayBills)
                {
                    var remainValue = bill.Value - bill.UsedValue;
                    decimal payValue = 0;
                    if (value <= remainValue)//如果预交费金额大于本次充值，本次充值全部抵扣
                    {
                        bill.UsedValue = bill.UsedValue + value;
                        payValue = 0 - value;
                        value = 0;
                    }
                    else
                    {
                        //否则只抵扣本笔预交费，
                        bill.UsedValue = bill.Value;
                        value = value - (decimal)remainValue;
                        payValue = 0 - (decimal)remainValue;
                    }
                    using (var scope = new TransactionScope())
                    {
                        try
                        {
                            historyBillBLL.Update(bill);
                            HistoryBill bill2 = new HistoryBill();
                            bill2.ReceiverId = bill.ReceiverId;
                            bill2.PayerId = bill.PayerId;
                            bill2.BeginTime = DateTime.Now;
                            bill2.EndTime = DateTime.Now;
                            bill2.Value = payValue;
                            bill2.OperatorId = operatorId;
                            bill2.PayTypeId = bill.PayTypeId;
                            bill2.UsedValue = 0;
                            bill2.PayMethodId = DictionaryCache.PayMethodDeductionPrePay.Id;
                            bill2.BillTypeId = bill.BillTypeId;
                            bill2.IsPay = true;
                            bill2.subject = "预交费抵扣";
                            bill2.Body = "抵扣" + bill.CreateTime + "时的预交费" + string.Format("{0:0.00}", -payValue) + "元";
                            bill2.CreateTime = DateTime.Now;
                            bill2.PayMentTime = DateTime.Now;
                            bill2.IsZero = false;
                            bill2.IsSynchro = false;
                            bill2 = historyBillBLL.Create(bill2);
                            scope.Complete();

                        }
                        catch (Exception ex)
                        {
                            MyConsole.Log(ex);
                            throw ex;
                        }
                    }
                    if (value == 0)
                        break;
                }

            }
            catch (Exception ex)
            {
                MyConsole.Log(ex);
                throw ex;
            }
            //UpdateCashAccount(order.ReceiverId, value-order.Value, DictionaryCache.BalanceMoney.Id);
            return value;

        }

        /// <summary>
        /// 退费
        /// </summary>
        /// <param name="receiverId">退费建筑或机构账号id</param>
        /// <param name="payerId">收款人的id</param>
        /// <param name="paymentAmount">退费金额，此处为正，写入数据库为负值，标示从机构账户扣费</param>
        /// <returns></returns>
        public async Task<List<NotifyInfo>> Refund(string receiverId, string payerId, decimal paymentAmount, string operatorId)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    Charge(receiverId, payerId, paymentAmount, true, DictionaryCache.BillTypePay.Id, DictionaryCache.PayTypeAll.Id, DictionaryCache.PayMethodRefund.Id, operatorId);
                    await SettleToObejct(operatorId, DateTime.Now, receiverId);
                    scope.Complete();

                }
                catch (Exception ex)
                {
                    MyConsole.Log(ex);
                    throw ex;
                }
            }
            //处理设备欠费相关告警联动操作
            var msgList = await historyBillBLL.CheckIsArrearage(receiverId);
            return messageBLL.SendBuildingMessageToNotify(true, msgList);//目前web层才能发送消息通知，后期调整消息通知类
        }

        /// <summary>
        /// 现金缴费充值
        /// </summary>
        /// <param name="receiverId">收款账号</param>
        /// <param name="payerId">付款账户</param>
        /// <param name="paymentAmount">支付金额</param>
        ///  <param name="syncToMeterId">需要同步到哪个设备上进行扣费</param>
        ///     <param name="isAutoSetMessage">是否自动发送消息</param>
        public async Task<object> CashCharge(string receiverId, string payerId, decimal paymentAmount, string operatorId,int? syncToMeterId=null,bool isAutoSetMessage=true,bool? isDeductPrePay=true)
        {
            HistoryBill order = null;
            using (var scope = new TransactionScope())
            {
                try
                {
                    if (syncToMeterId != null)
                    {
                        order = Charge(receiverId, payerId, paymentAmount, true, DictionaryCache.BillTypePay.Id, DictionaryCache.PayTypeAll.Id, DictionaryCache.PayMethodCash.Id, operatorId, (bool)isDeductPrePay, null, "", null, false);
                        //触发器实现同步 
                        //metersActionBLL.CreateByMeterIds(new List<int> { (int)syncToMeterId }, DictionaryCache.ActionBalanceChange.Id, DateTime.Now, paymentAmount + "", "");
                    }else
                         order = Charge(receiverId, payerId, paymentAmount, true, DictionaryCache.BillTypePay.Id, DictionaryCache.PayTypeAll.Id, DictionaryCache.PayMethodCash.Id, operatorId,true,null,"",null,true);
                    await SettleToObejct(operatorId, DateTime.Now, receiverId);
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    MyConsole.Log(ex);
                    throw ex;
                }
            }
            //处理设备欠费相关告警联动操作
            if (isAutoSetMessage)
            {
                var msgList = await historyBillBLL.CheckIsArrearage(receiverId);
                return messageBLL.SendBuildingMessageToNotify(false, msgList);//目前web层才能发送消息通知，后期调整消息通知类
            }
            return order;
        }


        /// <summary>
        /// 现金缴费纠错
        /// </summary>
        /// <param name="receiverId">收款账号</param>
        /// <param name="payerId">付款账户</param>
        /// <param name="paymentAmount">支付金额</param>
        public async Task CashCorrect(string receiverId, string payerId, decimal paymentAmount, string operatorId, int? originalId)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    Charge(receiverId, payerId, paymentAmount, true, DictionaryCache.BillTypePay.Id, DictionaryCache.PayTypeAll.Id, DictionaryCache.PayMethodCashCorrect.Id, operatorId,true, originalId);
                    await SettleToObejct(operatorId, DateTime.Now, receiverId);
                    scope.Complete();

                }
                catch (Exception ex)
                {
                    MyConsole.Log(ex);
                    throw ex;
                }
            }
            //处理设备欠费相关告警联动操作
            var msgList = await historyBillBLL.CheckIsArrearage(receiverId);
            messageBLL.SendBuildingMessageToNotify(false, msgList);
        }

        /// <summary>
        /// 预交费
        /// </summary>
        /// <param name="receiverId">缴费建筑或机构账号id</param>
        /// <param name="paymentAmount">金额</param>
        /// <returns></returns>
        public async Task PrePay(string receiverId, decimal paymentAmount, string operatorId)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    Charge(receiverId, null, paymentAmount, true, DictionaryCache.BillTypePrePay.Id, DictionaryCache.PayTypeAll.Id, DictionaryCache.PayMethodPrePay.Id, operatorId);
                    await SettleToObejct(operatorId, DateTime.Now, receiverId);
                    scope.Complete();

                }
                catch (Exception ex)
                {
                    MyConsole.Log(ex);
                    throw ex;
                }
            }
            //处理设备欠费相关告警联动操作
            if (paymentAmount > 0)
            {
                var msgList = await historyBillBLL.CheckIsArrearage(receiverId);
                messageBLL.SendBuildingMessageToNotify(false, msgList);//目前web层才能发送消息通知，后期调整消息通知类
            }
        }
        public async Task PrePay(string receiverId,Setting setting, string operatorId)
        {
            using (var scope = new TransactionScope())
            {
                
                try
                {
                    if (setting.ElectricityPrePay!=null&&setting.ElectricityPrePay != 0)
                    {
                        Charge(receiverId, null, (decimal)setting.ElectricityPrePay, true, DictionaryCache.BillTypePrePay.Id, DictionaryCache.PayTypePower.Id, DictionaryCache.PayMethodPrePay.Id, operatorId,false,null,"",null,false,null);
                    }
                    if (setting.WaterPrePay!=null&&setting.WaterPrePay != 0)
                        Charge(receiverId, null, (decimal)setting.WaterPrePay, true, DictionaryCache.BillTypePrePay.Id, DictionaryCache.PayTypeWater.Id, DictionaryCache.PayMethodPrePay.Id, operatorId, false, null, "", null, false, null);
                    await SettleToObejct(operatorId, DateTime.Now, receiverId);
                    scope.Complete();   
                    
                }
                catch (Exception ex)
                {
                    MyConsole.Log(ex);
                    throw ex;
                }

              
            }
            //处理设备欠费相关告警联动操作
            var msgList = await historyBillBLL.CheckIsArrearage(receiverId);
            messageBLL.SendBuildingMessageToNotify(false, msgList);//目前web层才能发送消息通知，后期调整消息通知类
        }

        /// <summary>
        /// 清零操作,针对宿舍，对象为建筑，无下级付费子对象,生成坏账
        /// </summary>
        /// <param name="ids">宿舍建筑id集合</param>
        public async Task SetZero(List<int> ids, string operatorId)
        {
            //获取对象列表
            List<string> userIds = new List<string>();
            userIds = userBLL.Filter(o => o.Roles.Any(c => c.Id == RoleCache.PayMentRoleId) && o.Buildings.Any(c => ids.Contains(c.Id))).Select(o => o.Id).ToList();

            using (var scope = new TransactionScope())
            {
                try
                {

                    foreach (var userId in userIds)
                    {
                        //清除对象现金余额
                        var balance = userBLL.Find(userId).UserAccount.Where(o => o.BalanceTypeId == DictionaryCache.BalancePowerMoney.Id).ToList()[0];
                        decimal money = Convert.ToDecimal(encrypt.Decrypto(balance.Balance));
                        if (money != 0)
                            Charge(userId, null, 0 - money, true, DictionaryCache.BillTypePay.Id, DictionaryCache.PayTypeAll.Id, DictionaryCache.PayMethodBadBalance.Id, operatorId,false);
                        //清除对象补贴余额,让所有补贴过期清零
                        var balanceSubsidy = userBLL.Find(userId).UserAccount.Where(o => o.BalanceTypeId == DictionaryCache.BalancePowerSubsidy.Id).ToList()[0];
                        money = Convert.ToDecimal(encrypt.Decrypto(balanceSubsidy.Balance));
                        if (money != 0)
                            Charge(userId, null, 0 - money, true, DictionaryCache.BillTypeSubsidy.Id, DictionaryCache.PayTypeAll.Id, DictionaryCache.PayMethodSubsidyOverTime.Id, operatorId, false);
                        var bills = historyBillBLL.Filter(o => o.ReceiverId == userId && (o.Value - o.UsedValue) > 0 && (o.BillTypeId == DictionaryCache.BillTypeSubsidy.Id || o.BillTypeId == DictionaryCache.BillTypePrePay.Id)).ToList();
                        foreach (var bill in bills)
                        {
                            bill.UsedValue = bill.Value;
                            bill.IsZero = false;
                            historyBillBLL.Update(bill);
                        }
                        await SettleToObejct(operatorId, DateTime.Now, userId);

                    }

                    scope.Complete();
                }
                catch (Exception ex)
                {
                    MyConsole.Log(ex);
                    throw ex;
                }
            }
            foreach (var userId in userIds)
            {
                //清零则开启所有设备
                var targetInfo = userBLL.GetTargetInfo(userId);
                List<int> id = new List<int> { targetInfo.TargetId };
                await meterBLL.LinkageControl(targetInfo.Category, id, DictionaryCache.ActionOnALL, 0, true);
            }


        }

        /// <summary>
        /// 获得统计数据
        /// </summary>
        /// <param name="category">building，organization</param>
        /// <param name="targetId"></param>
        /// <param name="isTotalInfo">是否需要计算总值</param>
        ///   <param name="isHasPreData">是否需要计算上个周期数据</param>
        /// <returns></returns>
        public NowBill GetNowBills(CategoryDictionary category, int targetId, bool isTotalInfo, bool isHasPreData)
        {

            NowBill nowBill = null;

            var billPayTypes = DictionaryCache.Get().Values.Where(o => o.Code == "PayType" && o.Enable == true && o.ChineseName != "通用").ToList();
            User user = userBLL.GetAccountUser(category, targetId);
            if (user == null)
                return nowBill;
            var lastBalance = db.Balances.Where(o => o.TargetCategory == (int)category && o.TargetId == targetId).OrderByDescending(o => o.AuditDate).Take(1).FirstOrDefault();
            if (lastBalance == null)
                return nowBill;
            nowBill = new NowBill();
            nowBill.EnergyBills = new Dictionary<int, EnergyBill>();
            nowBill.TargetType = lastBalance.TargetCategory;
            nowBill.UserId = user.Id;
            nowBill.Id = targetId;
            if (category == CategoryDictionary.Building)
            {
                var b = buildingBLL.Find(targetId);
                nowBill.Name = b.Name;
            }
            else
            {
                var b = organizationBLL.Find(targetId);
                nowBill.Name = b.Name;
            }
            foreach (var billPayType in billPayTypes)
            {
                EnergyBill energyBill = new EnergyBill();
                int energyCategoryId = -1;
                try
                {
                     energyCategoryId = DictionaryCache.Get().Values.First(o => o.TreeId == billPayType.EquText).Id;
                }
                catch {
                    continue;
                }
                //获得此对象此能耗类型的能耗详单
                var thisTypeBalanceDetails = balanceDetailBLL.Filter(o => 
                    o.EnergyCategory == energyCategoryId
                    && o.Balance.TargetCategory == (int)category 
                    && o.Balance.TargetId == targetId);
                if (isTotalInfo)
                {
                    try
                    {
                        energyBill.TotalValue = thisTypeBalanceDetails.Sum(o => o.Amount);
                        energyBill.TotalCost = thisTypeBalanceDetails.Sum(o => o.Consumption);
                    }
                    catch { }
                }
                var lastbalance = thisTypeBalanceDetails.OrderByDescending(o => o.Balance.AuditDate).Take(1).FirstOrDefault();
                if (lastbalance != null)
                    energyBill.FinishTime = lastbalance.Balance.CreateDate;
                if (energyBill.FinishTime != null)
                {
                    nowBill.DeadlineTime = energyBill.FinishTime;
                    var time = Convert.ToDateTime(((DateTime)energyBill.FinishTime).ToShortDateString()).AddDays(1).AddMilliseconds(-1);
                    //获得本类型定价周期开始
                    //查询所有当前有效机构计费信息
                    var billConfigBaseInfos = monitoringConfigBLL.Filter(o => o.EnergyCategoryId == energyCategoryId && o.ConfigTypeId == DictionaryCache.MonitoringConfigTypePrice.Id && o.Enabled == true && o.StartTime <= time && o.EndTime >= energyBill.FinishTime && o.TargetTypeId == (category == CategoryDictionary.Organization ? DictionaryCache.ConfigToOrg.Id : DictionaryCache.ConfigToBuilding.Id) && o.TargetId == targetId);
                    if (billConfigBaseInfos.Count() > 0)
                    {
                        var maxPriority = billConfigBaseInfos.Select(o => o.Priority).Distinct().OrderByDescending(o => o).Take(1).ToList()[0];
                        var billConfigs = billConfigBaseInfos.Where(o => o.Priority == maxPriority).ToViewList();
                        //本周期开始时间
                        energyBill.ThisPeriodBeginTime = monitoringConfigBLL.GetBeginTime(billConfigs[0], (DateTime)energyBill.FinishTime);
                        //本周期能耗

                        energyBill.ThisPeriodValue = thisTypeBalanceDetails.Where(o => o.Balance.AuditDate >= energyBill.ThisPeriodBeginTime && o.Balance.CreateDate <= time).Sum(o => o.Amount);
                        energyBill.ThisPeriodCost = thisTypeBalanceDetails.Where(o => o.Balance.AuditDate >= energyBill.ThisPeriodBeginTime && o.Balance.CreateDate <= time).Sum(o => o.Consumption);

                        if (isHasPreData)
                        {
                            //上周期开始时间
                            var timePre = monitoringConfigBLL.GetBeginTime(billConfigs[0], ((DateTime)energyBill.ThisPeriodBeginTime).AddDays(-1));
                            //上周期能耗
                            energyBill.PrePeriodBeginTime = timePre;
                            try
                            {
                                energyBill.PrePeriodValue = thisTypeBalanceDetails.Where(o => o.Balance.AuditDate >= timePre && o.Balance.CreateDate <= energyBill.ThisPeriodBeginTime).Sum(o => o.Amount);
                            }
                            catch {
                                energyBill.PrePeriodValue = 0;
                            }
                            try
                            {
                                energyBill.PrePeriodCost = thisTypeBalanceDetails.Where(o => o.Balance.AuditDate >= timePre && o.Balance.CreateDate <= energyBill.ThisPeriodBeginTime).Sum(o => o.Consumption);
                            }
                            catch
                            {
                                energyBill.PrePeriodCost = 0;
                            }
                        }
                    }
                }
                nowBill.EnergyBills.Add(energyCategoryId, energyBill);
            }
            //Encrypt encrypt = new Encrypt();
            //var accountCash = user.UserAccount.Where(o => o.BalanceTypeId == DictionaryCache.BalancePowerMoney.Id).ToList()[0];
            //现金账户
            //nowBill.CashAccount = Convert.ToDecimal(encrypt.Decrypto(accountCash.Balance));
            //var subsidyAccount = user.UserAccount.Where(o => o.BalanceTypeId == DictionaryCache.BalancePowerSubsidy.Id).ToList()[0];
            //补贴账户
            //nowBill.SubsidyAccount = Convert.ToDecimal(encrypt.Decrypto(subsidyAccount.Balance));
            //nowBill.ChildAccount = lastBalance.Total - nowBill.CashAccount - nowBill.SubsidyAccount;
            //nowBill.NowCost = lastBalance.Total;
            ////上周期应缴费用
            //nowBill.PreCost = 0;
            //foreach (var bill in nowBill.EnergyBills)
            //{
            //    if (bill.Value.PrePeriodCost!=null)
            //        nowBill.PreCost += (decimal)bill.Value.PrePeriodCost;
            //}
            //if (nowBill.NowCost == null)
            //    nowBill.NowCost = 0;
            //if (nowBill.CashAccount == null)
            //    nowBill.CashAccount = 0;
            return nowBill;
        }

        /// <summary>
        /// 获取某时间开始到现在触发某告警的统计数据
        /// </summary>
        /// <param name="alertDicId">告警类型id</param>
        /// <param name="date">统计开始时间</param>
        /// <param name="num">触发此告警的建筑人数</param>
        /// <returns></returns>
        public List<BalanceShortData> GetAlertBalances(int alertDicId, DateTime beginDate)
        {
            //此告警的配置信息
            var buidingConfigZeros = monitoringConfigBLL.Filter(o => o.Enabled && o.ConfigTypeId == DictionaryCache.MonitoringConfigTypeWarning.Id && o.TargetTypeId == DictionaryCache.ConfigToBuilding.Id && o.StartTime <= beginDate && o.EndTime >= DateTime.Now && o.WayId == alertDicId).GroupBy(o => o.TargetId).Select(o => o.OrderByDescending(c => c.UnitValue).FirstOrDefault());
            var orgConfigZeros = monitoringConfigBLL.Filter(o => o.Enabled && o.ConfigTypeId == DictionaryCache.MonitoringConfigTypeWarning.Id && o.TargetTypeId == DictionaryCache.ConfigToOrg.Id && o.StartTime <= beginDate && o.EndTime >= DateTime.Now && o.WayId == alertDicId).GroupBy(o => o.TargetId).Select(o => o.OrderByDescending(c => c.UnitValue).FirstOrDefault());

            var buildingIds = buidingConfigZeros.Select(o => o.TargetId).ToList();
            var orgIds = orgConfigZeros.Select(o => o.TargetId).ToList();

            //符合此告警的统计信息
            var buildBalanceList = this.Filter(x => x.AuditDate >= beginDate && buildingIds.Contains(x.TargetId) && x.TargetCategory == (int)CategoryDictionary.Building).GroupBy(x => x.TargetId)
             .Select(g => g.OrderByDescending(e => e.AuditDate).FirstOrDefault()).ToShortViewList();
            var orgBalanceList = this.Filter(x => x.AuditDate >= beginDate && orgIds.Contains(x.TargetId) && x.TargetCategory == (int)CategoryDictionary.Organization).GroupBy(x => x.TargetId)
             .Select(g => g.OrderByDescending(e => e.AuditDate).FirstOrDefault()).ToShortViewList();

            var arrearageListBuiding = (from x in buildBalanceList
                                        join y in buidingConfigZeros
                                        on x.TargetId equals y.TargetId
                                        where x.Total < y.UnitValue
                                        select x).ToList();
            buildingIds = arrearageListBuiding.Select(o => o.TargetId).ToList();
            //num = buildingBLL.Filter(x => buildingIds.Contains(x.Id)).SumZero(x => x.CustomerCount ?? 0);

            var arrearageListOrg = (from x in orgBalanceList
                                    join y in orgConfigZeros
                                    on x.TargetId equals y.TargetId
                                    where x.Total < y.UnitValue
                                    select x).ToList();
            arrearageListBuiding.AddRange(arrearageListOrg);
            arrearageListBuiding.OrderBy(o => o.Total);
            return arrearageListBuiding;
        }
       
        /// <summary>
        /// 根据设备的最后一次采集数据和此后充值数据修改设备状态
        /// </summary>
        /// <param name="meter"></param>
        public void SetMeterStatusForCredit(Meter meter)
        {
            if (meter.RelayElecState == DictionaryCache.RelayElecStateCreditLow.Id || meter.RelayElecState == DictionaryCache.RelayElecStateCreditZero.Id)
            {
                //该设备最后一次采集金额+此后成功充值或扣费的总金额，就是当前余额
                var total =db.MomentaryValues.FirstOrDefault(o => o.MeterId == meter.Id && o.Parameter.TypeId == DictionaryCache.ParameterCredit.Id);
                if (total != null)
                {
                    var credit = total.Value;
                    var actionsAfterTime = db.MetersActions.Where(o => o.MeterId == meter.Id && o.ActionTime > total.Time && o.ActionId == DictionaryCache.ActionBalanceChange.Id && o.CommandStatus == DictionaryCache.CommandStatusAlreadySucess.Id).Select(o => o.SettingValue);
                    foreach (var item in actionsAfterTime)
                    {
                        credit = credit + Convert.ToDecimal(item);
    }
                    if (credit > 0 && credit < 3)
                    {
                        meter.RelayElecState = DictionaryCache.RelayElecStateCreditLow.Id;
                        db.SaveChanges();
                    }
                    else
                        if (credit > 3)
                        {
                            meter.RelayElecState = DictionaryCache.RelayElecStateNormal.Id;
                            db.SaveChanges();
                        }
                }

            }
        }







        #region 微信




        /// <summary>
        /// 生成缴费数据
        /// </summary>
        /// <param name="value">费用</param>
        /// <param name="receiverId">收款账户</param>
        /// <param name="payTypeId">支付类别</param>
        /// <param name="category">缴费对象分类（building或Organization）</param>
        /// <param name="targetId">对象id</param>
        /// <returns>历史账单数据</returns>

        public HistoryBillData GetPayOrder(decimal value, string openid)
        {
            string last = "";
            string payerid = "";
            int buildingid = 0 ;
            User user = null;
            User userBuild = null;
            string receiverld = "";
            HistoryBill order = null;
            
            Repository<User> userbll = new Repository<User>();
           // user = userbll.Filter(o => o.WeChat == openid && o.Roles.Any(r => (r.Name == "缴费账户" || r.Name == "能耗用户"))).FirstOrDefault();
            //先获得这个用户的房间
            user = userbll.Filter(o => o.WeChat == openid && o.Roles.Any(c => c.Id == RoleCache.PowerUserRoleId)).FirstOrDefault(); 
             if (user != null)
            { 
                payerid = user.Id;
                buildingid = user.Buildings.FirstOrDefault().Id;
            }
           // 在获得这个房间的账户
             //userBuild = this.db.Users.FirstOrDefault(o => o.Buildings.Any(c => c.Id == buildingid) && o.Roles.Any(c => c.Id == RoleCache.PayMentRoleId));
            // userBuild = this.db.Users.FirstOrDefault(o => o.Buildings.Any(c => c.Id == buildingid) && o.Roles.Any(c => c.Id == RoleCache.PayMentRoleId));
             user = userbll.Filter(o => o.Buildings.Any(c => c.Id == buildingid) && o.Roles.Any(c => c.Id == RoleCache.PayMentRoleId)).FirstOrDefault(); 
             if (userBuild != null)
             {
                 receiverld = userBuild.Id;
             }

             order = this.Charge(receiverld, payerid, value, false, 120003, 380003, 370003, payerid);
            return order.ToViewData();

        }


        /// <summary>
        /// 缴费充值
        /// </summary>
        /// <param name="receiverId">收款账号</param>
        /// <param name="payerId">付款账户</param>
        /// <param name="paymentAmount">支付金额</param>
        /// <param name="isPay">是否支付成功（支付宝等需要等待服务器确认才支付成功）</param>
        /// <param name="payTypeId">缴费类型（水，电，通用）</param>
        /// <param name="payMethodId">支付方式（支付宝，现金缴费，水电控终端缴费等）</param>
        /// <param name="isDeductPrePay">是否抵扣预交费（默认true）</param>
        /// <param name="originalId">纠错，保存修改对象的id</param>
        /// <param name="desc">描述信息补充，加载头部</param>
        /// <param name="usedValue">能耗，不写为空</param>
        /// <param name="isSync">是否已经同步，默认false</param>
        /// <returns></returns> 
       
       
        /// <summary>
        /// 支付完成
        /// </summary>
        /// <param name="strid">订单号</param>
        public async Task<string> PayOk(string strid)
        {

            string oldbody = "";
       Mylog.Debug("PayOk", "开始" + strid);

            string last = "";
            string receiverId = "";
           // int oid = Convert.ToInt32(strid);
            int oid = Convert.ToInt32(strid.Substring(25));
           // int oid = 80609;
             Mylog.Debug("PayOk", "oid:" + oid.ToString());
            var order = historyBillBLL.Find(oid);
            
            if (order.IsPay != true)
            {
                 Mylog.Debug("PayOk", "11111");
                Repository<MeterStatus> meterStatusBLL = new Repository<MeterStatus>();

                TargetData target = userBLL.GetTargetInfo(order.ReceiverId);
                Meter meter = null;
                var meters = meterBLL.GetMetersInBuilding(buildingBLL.Find(target.TargetId), DictionaryCache.AllCategory).ToList();
                if (meters.Count() == 1)
                    meter = meters.FirstOrDefault();
                else if (meters.Count() > 0)
                    meter = meters.FirstOrDefault(o => o.EnergyCategoryId == DictionaryCache.PowerCateogry.Id);//能耗为电，不是空调用电等

                   Mylog.Debug("PayOk", "22222");
                receiverId = order.ReceiverId;
                string msg = "";
                var maxBill = historyBillBLL.Filter(o => o.ReceiverId == receiverId && o.BillTypeId == order.BillTypeId).OrderByDescending(o => o.CreateTime).Take(1).FirstOrDefault();
                List<HistoryBill> newBills = new List<HistoryBill>();

              Mylog.Debug("PayOk", "1111111111111111");
                try
                {
                    order.IsPay = true;
                    //order.IsSynchro = true;
                    oldbody = order.Body;

                    order.PayMentTime = DateTime.Now;
                    order.Body = oldbody + "支付完成，设备充值中...";
                    historyBillBLL.Update(order);
                  
                  
                    var time = DateTime.Now;
                    //等待充值完成
                    System.Threading.Thread.Sleep(5000);
                    string answerValue = null;
                    //using (var meterActionBLLTemp2 = new MetersActionBLL())
                    //{
                    //    answerValue = meterActionBLLTemp2.Filter(o => o.MeterId == meter.Id && o.ActionId == DictionaryCache.ActionBalanceChange.Id && o.ActionTime <= time && o.SettingValue.StartsWith(order.Value + "")).OrderByDescending(o => o.Id).FirstOrDefault().AnswerValue;
                    //}
                    if (answerValue == null)
                    {
                        await Task.Run(() =>
                        {
                            for (int i = 0; i < 120; i++)
                            {

                                using (var meterActionBLLTemp = new MetersActionBLL())
                                {
                                    answerValue = meterActionBLLTemp.Filter(o => o.MeterId == meter.Id && o.ActionId == DictionaryCache.ActionBalanceChange.Id && o.ActionTime <= time && o.SettingValue.StartsWith(order.Value + "")).OrderByDescending(o => o.Id).FirstOrDefault().AnswerValue;
                                    answerValue = "成功，成功，成功，";
                                }
                                if (answerValue != null)
                                {
                                    break;
                                }
                                else
                                    System.Threading.Thread.Sleep(500);
                            }
                        });

                    }
            


               
                    Mylog.Debug("PayOk", "answerValue:" + answerValue);
                    if (answerValue != null && answerValue.Contains("成功，"))
                    {

                       order.Body = order.Body.Substring(0, order.Body.IndexOf("。")) + "支付完成，设备充值成功。";
                       // order.Body = oldbody + "支付完成，设备充值成功。";
                       order.IsSynchro = true;
                        historyBillBLL.Update(order);
                  

                        var value = this.DeductPrePay(order, null);
                        this.UpdateCashAccount(order.ReceiverId, value, DictionaryCache.BalancePowerMoney.Id);
                     //  Mylog.Debug("PayOk", "333333333");
                        //触发器自动统计
                        if (MyConsole.GetAppString("IsUseTigerToSettle") != "true")
                        {
                            await this.Settle(DateTime.Now, target.Category, target.TargetId, order.PayerId);
                        }
                        msg = "您已经成功为" + target.Name + "充值" + string.Format("{0:0.00}", order.Value) + "元。";

                        var meterStatus = meterStatusBLL.Filter(o => o.MeterId == meter.Id && o.MeterMessageTypeId == DictionaryCache.MessageTypeMobilePayFail.Id).FirstOrDefault();
                        if (meterStatus != null && meterStatus.Enabled == true)
                        {
                            meterStatus.Enabled = false;
                            meterStatusBLL.Update(meterStatus);
                        }


                        newBills = historyBillBLL.Filter(o => o.ReceiverId == receiverId && o.PayMethodId == DictionaryCache.PayMethodDeductionPrePay.Id && o.BillTypeId == order.BillTypeId && o.CreateTime > maxBill.CreateTime).ToList();
                        if (newBills.Count() > 0)
                            msg = msg + "所充值金额,";
                        foreach (var bill in newBills)
                        {
                            msg = msg + bill.Body;
                        }

                         Mylog.Debug("PayOk", "4444444444");
                        //检查是否有失败的扣费记录，如果有，则重新扣费
                        var actions = metersActionBLL.Filter(o => o.MeterId == meter.Id && o.ActionId == DictionaryCache.ActionBalanceChange.Id && o.CommandStatus == DictionaryCache.CommandOverLimit.Id && o.SettingValue.StartsWith("-")).ToList();
                        foreach (var action in actions)
                        {
                            foreach (var commandQueue in action.CommandQueue)
                            {
                                commandQueue.SendCount = 0;
                                commandQueue.IsReply = false;
                                commandQueue.ReplyTime = null;
                                commandQueue.ReplyValue = null;
                            }
                        }
                        metersActionBLL.db.SaveChanges();

                        //处理设备欠费相关告警联动操作
                        var msgList = await historyBillBLL.CheckIsArrearage(receiverId);



                       //   Mylog.Debug("PayOk", "success");

                        last = "success";

                    }
                    else
                    {

                     //    Mylog.Debug("PayOk", "充值失败");
                        //充值失败
                        var model = metersActionBLL.Filter(o => o.MeterId == meter.Id && o.ActionId == DictionaryCache.ActionBalanceChange.Id && o.ActionTime <= time && o.SettingValue.StartsWith(order.Value + "")).OrderByDescending(o => o.Id).FirstOrDefault();
                        order.UsedValue = model.Id;
                        order.Body = order.Body.Substring(0, order.Body.IndexOf("。")) + "支付完成，设备连接超时，稍后系统自动重试。您可以在充值记录查询支付情况。";
                        historyBillBLL.Update(order);

                        var name = meter.Building.Name;
                        if (meter.Building.Parent != null)
                        {
                            name = meter.Building.Parent.Name + "," + name;
                            if (meter.Building.Parent.Parent != null)
                            {
                                name = meter.Building.Parent.Parent.Name + "," + name;

                            }
                        }
                        var info = string.Format("位于{0}的用于{1}设备(名称:{2})充值失败，请及时处理！", name, meter.EnergyCategoryDict.EquText + "的" + meter.TypeDict.ChineseName, meter.Name);
                        var info2 = string.Format("位于{0}的用于{1}设备(名称:{2})充值失败（{3}）,联系人:{4},电话:{5}", name, meter.EnergyCategoryDict.EquText + "的" + meter.TypeDict.ChineseName, meter.Name, (answerValue == null ? "超时" : answerValue), order.Payer.FullName, order.Payer.PhoneNumber);

                        var meterStatus = meterStatusBLL.Filter(o => o.MeterId == meter.Id && o.MeterMessageTypeId == DictionaryCache.MessageTypeMobilePayFail.Id).FirstOrDefault();
                        if (meterStatus == null)
                        {
                            meterStatus = new MeterStatus();
                            meterStatus.MeterId = meter.Id;
                            meterStatus.MeterMessageTypeId = DictionaryCache.MessageTypeMobilePayFail.Id;
                            meterStatus.Enabled = true;
                            meterStatus.UpdateTime = DateTime.Now;
                            meterStatus.Value = -1;
                            meterStatus.IsFluctuationData = null;
                            meterStatus.Description = info2;
                            meterStatusBLL.Create(meterStatus);
                        }
                        else if (meterStatus.Enabled == false)
                        {
                            meterStatus.Enabled = true;
                            meterStatus.Description = info2;
                            meterStatusBLL.Update(meterStatus);
                        }





                      //  Mylog.Debug("PayOk", "充值失败结束");




                        last = "fail";

                    }






                }// try
                catch (Exception ex)
                {
                    last = "ex:" + ex.Message;
                     Mylog.Debug("PayOk Exception:", last);
                }










                Mylog.Debug("PayOk", "last：" + last);


            }  //order.IsPay != true


            return last;

        }


 

        #endregion







    }


        
}