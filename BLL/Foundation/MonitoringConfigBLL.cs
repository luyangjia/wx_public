using WxPay2017.API.DAL.EmpModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WxPay2017.API.VO;
using WxPay2017.API.VO.Common;
using WxPay2017.API.VO.Param;
using System.Threading;

namespace WxPay2017.API.BLL
{
    public class MonitoringConfigBLL : Repository<MonitoringConfig>
    {
        MeterBLL meterBLL = new MeterBLL();
        MetersActionBLL meterActionBLL = new MetersActionBLL();
        BuildingBLL buildingBLL = new BuildingBLL();

        public MonitoringConfigBLL(EmpContext context = null)
            : base(context)
        {
            meterActionBLL = new MetersActionBLL(meterBLL.db);
            buildingBLL = new BuildingBLL(meterBLL.db);
        }

        /// <summary>
        /// 获得建筑对象的定额或指标或补贴数据
        /// </summary>
        /// <param name="bid"></param>
        /// <param name="configTypeId"></param>
        /// <returns></returns>
        public MonitoringConfigData GetBuildingMonitoringConfig(int targetTypeId, int targetId, int configTypeId)
        {
            var configs = db.MonitoringConfig.Where(o => o.ConfigTypeId == configTypeId && o.TargetTypeId == targetTypeId && o.TargetId == targetId && o.Enabled == true && o.StartTime <= DateTime.Now && o.EndTime > DateTime.Now).OrderByDescending(o => o.Priority).Take(1).ToList();
            if (configs.Count() == 0)
                return null;
            else
                return configs[0].ToViewData();
        }

        /// <summary>
        /// 根据配置文件获得指定检测循环周期的开始时间
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public DateTime GetBeginTime(MonitoringConfigData config, DateTime thisTime)
        {
            DateTime beginTime = (DateTime)config.CycleTime;
            if (DictionaryCache.PeriodYear.Id == config.CycleTypeId)
            {
                beginTime = new DateTime(thisTime.Year, beginTime.Month, beginTime.Day, beginTime.Hour, beginTime.Minute, beginTime.Second);
                if (beginTime > thisTime)
                    beginTime = beginTime.AddYears(-1);
            }
            else if (DictionaryCache.PeriodMonth.Id == config.CycleTypeId)
            {
                beginTime = new DateTime(thisTime.Year, thisTime.Month, beginTime.Day, beginTime.Hour, beginTime.Minute, beginTime.Second);
                if (beginTime > thisTime)
                    beginTime = beginTime.AddMonths(-1);
            }
            else if (DictionaryCache.PeriodWeek.Id == config.CycleTypeId)
            {
                TimeSpan ts = thisTime - beginTime;
                var totalDays = ts.TotalDays;
                beginTime = thisTime.AddDays(0 - (totalDays % 7));
            }
            else if (DictionaryCache.PeriodDay.Id == config.CycleTypeId)
            {
                beginTime = new DateTime(thisTime.Year, thisTime.Month, thisTime.Day, beginTime.Hour, beginTime.Minute, beginTime.Second);
                if (beginTime > thisTime)
                    beginTime = beginTime.AddDays(-1);
            }
            else if (DictionaryCache.PeriodHour.Id == config.CycleTypeId)
            {
                beginTime = new DateTime(thisTime.Year, thisTime.Month, thisTime.Day, thisTime.Hour, beginTime.Minute, beginTime.Second);
                if (beginTime > thisTime)
                    beginTime = beginTime.AddHours(-1);
            }
            return beginTime;
        }

        /// <summary>
        /// 根据配置文件获得指定检测循环周期的结束时间
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public DateTime GetEndTime(MonitoringConfigData config, DateTime thisTime)
        {
            DateTime endTime = (DateTime)config.CycleTime;
            if (DictionaryCache.PeriodYear.Id == config.CycleTypeId)
            {
                endTime = new DateTime(thisTime.Year, endTime.Month, endTime.Day, endTime.Hour, endTime.Minute, endTime.Second);
                if (endTime <= thisTime)
                    endTime = endTime.AddYears(1);

            }
            else if (DictionaryCache.PeriodMonth.Id == config.CycleTypeId)
            {
                endTime = new DateTime(thisTime.Year, thisTime.Month, endTime.Day, endTime.Hour, endTime.Minute, endTime.Second);
                if (endTime <= thisTime)
                    endTime = endTime.AddMonths(1);
            }
            else if (DictionaryCache.PeriodWeek.Id == config.CycleTypeId)
            {
                TimeSpan ts = thisTime - endTime;
                var totalDays = ts.TotalDays;
                endTime = thisTime.AddDays(0 - (totalDays % 7)).AddDays(7);
            }
            else if (DictionaryCache.PeriodDay.Id == config.CycleTypeId)
            {
                endTime = new DateTime(thisTime.Year, thisTime.Month, thisTime.Day, endTime.Hour, endTime.Minute, endTime.Second);
                if (endTime <= thisTime)
                    endTime = endTime.AddDays(1);
            }
            else if (DictionaryCache.PeriodHour.Id == config.CycleTypeId)
            {
                endTime = new DateTime(thisTime.Year, thisTime.Month, thisTime.Day, thisTime.Hour, endTime.Minute, endTime.Second);
                if (endTime <= thisTime)
                    endTime = endTime.AddHours(1);
            }
            else if (DictionaryCache.PeriodTCustomer != null && DictionaryCache.PeriodTCustomer.Id == config.CycleTypeId)
            {
                endTime = (DateTime)config.CycleTime;
            }
            return endTime;
        }

        /// <summary>
        /// 获得配置对象在指定周期内的定额数据
        /// </summary>
        /// <param name="needConfigs"></param>
        /// <param name="startTime"></param>
        /// <param name="finishTime"></param>
        /// <param name="meterIds"></param>
        private decimal GetQuota(MonitoringConfigData quota, DateTime startTime, DateTime finishTime, List<int> meterIds)
        {

            //获得当前定额周期用量
            var beginTime1 = GetBeginTime(quota, startTime);
            var finishTime1 = GetEndTime(quota, startTime);
            var beginTime2 = GetBeginTime(quota, finishTime.AddMilliseconds(-1));
            var finishTime2 = GetEndTime(quota, finishTime.AddMilliseconds(-1));
            if (beginTime1 != beginTime2 || finishTime1 != finishTime2 || beginTime1 > startTime || finishTime1 < finishTime)
            {
                throw new Exception("当前定额周期应包含定价周期");
            }
            decimal total = 0;
            //先统计月表数据
            var beginMonthTime = new DateTime(beginTime1.AddMonths(1).Year, beginTime1.AddMonths(1).Month, 1, 0, 0, 0);
            var finishMonthTime = new DateTime(finishTime1.Year, finishTime1.Month, 1, 0, 0, 0);
            if (finishMonthTime > beginMonthTime)
                try
                {
                    total = total + db.MeterMonthlyResults.Where(o => meterIds.Contains(o.MeterId) && DictionaryCache.EnergyStatusActiveIds.Contains(o.Status) && o.StartTime >= beginMonthTime && o.FinishTime <= finishMonthTime).Sum(o => o.Total * o.Meter.Rate);
                }
                catch { }
            //再统计日表数据
            //在同一个月
            if (beginTime1.Year + " " + beginTime1.Month == finishTime1.Year + " " + finishTime1.Month)
            {
                var beginDayTime = new DateTime(beginTime1.AddDays(1).Year, beginTime1.AddDays(1).Month, beginTime1.AddDays(1).Day, 0, 0, 0);
                var finishDayTime = new DateTime(finishTime1.Year, finishTime1.Month, finishTime1.Day, 0, 0, 0);
                try
                {
                    total = total + db.MeterDailyResults.Where(o => meterIds.Contains(o.MeterId) && DictionaryCache.EnergyStatusActiveIds.Contains(o.Status) && o.StartTime >= beginDayTime && o.FinishTime <= finishDayTime).Sum(o => o.Total * o.Meter.Rate);
                }
                catch { }
            }
            else
            {
                //不在同一个月
                var beginDayTime = new DateTime(beginTime1.AddDays(1).Year, beginTime1.AddDays(1).Month, beginTime1.AddDays(1).Day, 0, 0, 0);
                var finishDayTime = new DateTime(beginTime1.AddMonths(1).Year, beginTime1.AddMonths(1).Month, 1, 0, 0, 0);
                try
                {
                    total = total + db.MeterDailyResults.Where(o => meterIds.Contains(o.MeterId) && DictionaryCache.EnergyStatusActiveIds.Contains(o.Status) && o.StartTime >= beginDayTime && o.FinishTime <= finishDayTime).Sum(o => o.Total * o.Meter.Rate);
                }
                catch { }
                beginDayTime = new DateTime(finishTime1.Year, finishTime1.Month, 1, 0, 0, 0);
                finishDayTime = new DateTime(finishTime1.Year, finishTime1.Month, finishTime1.Day, 0, 0, 0);
                try
                {
                    total = total + db.MeterDailyResults.Where(o => meterIds.Contains(o.MeterId) && DictionaryCache.EnergyStatusActiveIds.Contains(o.Status) && o.StartTime >= beginDayTime && o.FinishTime <= finishDayTime).Sum(o => o.Total * o.Meter.Rate);
                }
                catch { }
            }
            //再统计头尾两天小时表数据
            var endDayTime2 = new DateTime(beginTime1.AddDays(1).Year, beginTime1.AddDays(1).Month, beginTime1.AddDays(1).Day, 0, 0, 0);
            try
            {
                total = total + db.MeterDailyResults.Where(o => meterIds.Contains(o.MeterId) && DictionaryCache.EnergyStatusActiveIds.Contains(o.Status) && o.StartTime >= beginTime1 && o.FinishTime <= endDayTime2).Sum(o => o.Total * o.Meter.Rate);
            }
            catch { }
            var beginDayTime2 = new DateTime(finishTime1.Year, finishTime1.Month, 1, 0, 0, 0);
            try
            {
                total = total + db.MeterDailyResults.Where(o => meterIds.Contains(o.MeterId) && DictionaryCache.EnergyStatusActiveIds.Contains(o.Status) && o.StartTime >= beginDayTime2 && o.FinishTime <= finishTime1).Sum(o => o.Total * o.Meter.Rate);
            }
            catch { }

            return total;
        }

        /// <summary>
        /// 根据能耗获得每个周期的产生费用，实时计算
        /// </summary>
        /// <param name="config">定价配置</param>
        /// <param name="power">能耗值</param>
        ///  <param name="startTime">周期开始时间</param>
        /// <param name="finishTime">周期结束时间</param>
        /// <param name="meterIds">当前对象的一级设备ids</param>
        /// <returns></returns>
        public decimal GetBill(List<MonitoringConfigData> needConfigs, List<EnergyData> hourlyResults, DateTime startTime, DateTime finishTime, List<int> meterIds)
        {
            decimal cost = 0;
            if (needConfigs[0].WayId == DictionaryCache.PriceWayNormal.Id)
            {
                //每度定价
                cost = (decimal)hourlyResults.Sum(o => o.Value) * (decimal)needConfigs[0].Value;
            }
            else if (needConfigs[0].WayId == DictionaryCache.PriceWayByTime.Id)
            {
                //分时定价
                //遍历定价分时，查询匹配的定价数据
                foreach (var activeConfig in needConfigs)
                {
                    int beginHour = ((DateTime)activeConfig.ValidStartTime).Hour;
                    int endHour = ((DateTime)activeConfig.ValidEndTime).Hour;
                    decimal sum = 0;
                    if (beginHour < endHour)
                        sum = (decimal)hourlyResults.Where(o => o.StartTime.Hour >= beginHour && o.FinishTime.Hour <= endHour).Sum(o => o.Value);
                    else
                        sum = (decimal)hourlyResults.Where(o => o.StartTime.Hour >= beginHour || o.FinishTime.Hour >= endHour).Sum(o => o.Value);
                    cost = cost + (decimal)sum * (decimal)activeConfig.Value;
                }
            }
            else if (needConfigs[0].WayId == DictionaryCache.PriceWayGradientByQuota.Id)
            {
                //阶梯价（超定量,计费方式根据阶梯价格上下限获得）
                //总量
                var usedValue = (decimal)hourlyResults.Sum(o => o.Value);
                var notCostValue = usedValue;
                needConfigs = needConfigs.OrderBy(o => o.LowerLimit).ToList();
                foreach (var activeConfig in needConfigs)
                {
                    var notCostValue2 = notCostValue - ((decimal)activeConfig.UpperLimit - (decimal)activeConfig.LowerLimit);
                    if (notCostValue2 <= 0)
                    {
                        cost = cost + notCostValue * (decimal)activeConfig.Value;
                        break;
                    }
                    else
                    {
                        notCostValue = notCostValue2;
                        cost = cost + ((decimal)activeConfig.UpperLimit - (decimal)activeConfig.LowerLimit) * (decimal)activeConfig.Value;
                    }
                }
            }
            else if (needConfigs[0].WayId == DictionaryCache.PriceWayGradientByPercentage.Id)
            {
                //阶梯价（超定量,计费方式根据基价以及阶梯价格上下百分比获得）
                //总量
                var usedValue = (decimal)hourlyResults.Sum(o => o.Value);
                var notCostValue = usedValue;
                //基价
                needConfigs = needConfigs.OrderBy(o => o.LowerLimit).ToList();
                foreach (var activeConfig in needConfigs)
                {
                    var baseValue = (decimal)activeConfig.UnitValue;
                    var notCostValue2 = notCostValue - baseValue * ((decimal)activeConfig.UpperLimit - (decimal)activeConfig.LowerLimit);
                    if (notCostValue2 <= 0)
                    {
                        cost = cost + notCostValue * (decimal)activeConfig.Value;
                        break;
                    }
                    else
                    {
                        notCostValue = notCostValue2;
                        cost = cost + baseValue * ((decimal)activeConfig.UpperLimit - (decimal)activeConfig.LowerLimit) * (decimal)activeConfig.Value;
                    }
                }
            }
            else
            {
                //超定额定价，必须有定额，定额周期要符合要求，大于定价周期
                //获得当前建筑的定额
                int cid = (int)needConfigs[0].CycleTypeId;
                int tid = needConfigs[0].TargetId;
                int typeid = needConfigs[0].TargetTypeId;
                var quotas = db.MonitoringConfig.Where(o => o.CycleTypeId <= cid && o.TargetTypeId == typeid && o.TargetId == tid && o.ConfigTypeId == DictionaryCache.MonitoringConfigTypeQuota.Id && o.Enabled == true && o.StartTime <= startTime && o.EndTime >= finishTime).OrderByDescending(o => o.Priority).Take(1).ToViewList();
                if (quotas.Count() == 0)
                {
                    throw new Exception("定额未配置、或周期小于定价");
                }
                decimal total = 0;
                try
                {
                    total = GetQuota(quotas[0], startTime, finishTime, meterIds);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                //获取此计费周期前定额已经使用量
                var beginTimeBefore = GetBeginTime(quotas[0], startTime);
                decimal totalBefore = 0;
                if (startTime > beginTimeBefore)
                    try
                    {
                        totalBefore = GetQuota(quotas[0], beginTimeBefore, startTime, meterIds);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                //当前定额值
                var quota = quotas[0];
                decimal quotaValue = 0;
                //获得配置的总定额数据
                quotaValue = GetQuotaValue(quota, quotaValue);

                if (needConfigs[0].WayId == DictionaryCache.PriceWayOverRationByQuota.Id)
                {
                    //超定额，按定量计价
                    //获得本周期开始扣除已经使用定额后余下定额
                    var nowQuota = quotaValue - totalBefore;
                    //var usedValue =total;// 本周期能耗
                    var usedValue = (decimal)hourlyResults.Sum(o => o.Value);
                    var notCostValue = usedValue;//本周期未计费能耗
                    needConfigs = needConfigs.OrderBy(o => o.UpperLimit).ToList();
                    for (int i = 0; i < needConfigs.Count; i++)
                    {
                        if (needConfigs[i].LowerLimit == 0 && needConfigs[i].UpperLimit == 0)
                        {
                            if (nowQuota <= 0)
                            {
                                needConfigs.RemoveAt(i);
                                i--;
                            }
                            else
                                needConfigs[i].UpperLimit = nowQuota;
                        }
                        else
                        {
                            needConfigs[i].LowerLimit = needConfigs[i].LowerLimit + nowQuota;
                            needConfigs[i].UpperLimit = needConfigs[i].UpperLimit + nowQuota;
                            if (needConfigs[i].UpperLimit <= 0)
                            {
                                needConfigs.RemoveAt(i);
                                i--;
                            }
                            else if (needConfigs[i].LowerLimit < 0)
                                needConfigs[i].LowerLimit = 0;
                        }
                    }

                    foreach (var activeConfig in needConfigs)
                    {
                        var notCostValue2 = notCostValue - ((decimal)activeConfig.UpperLimit - (decimal)activeConfig.LowerLimit);
                        if (notCostValue2 <= 0)
                        {
                            cost = cost + notCostValue * (decimal)activeConfig.Value;
                            break;
                        }
                        else
                        {
                            notCostValue = notCostValue2;
                            cost = cost + ((decimal)activeConfig.UpperLimit - (decimal)activeConfig.LowerLimit) * (decimal)activeConfig.Value;
                        }
                    }
                }
                else if (needConfigs[0].WayId == DictionaryCache.PriceWayOverRationByPercentage.Id)
                {
                    //超定额，按百分比计价
                    //var usedValue =total;// 本周期能耗
                    var usedValue = (decimal)hourlyResults.Sum(o => o.Value);
                    var notCostValue = usedValue;//本周期未计费能耗
                    needConfigs = needConfigs.OrderBy(o => o.UpperLimit).ToList();
                    for (int i = 0; i < needConfigs.Count; i++)
                    {
                        needConfigs[i].LowerLimit = needConfigs[i].LowerLimit * quotaValue - totalBefore;
                        needConfigs[i].UpperLimit = needConfigs[i].UpperLimit * quotaValue - totalBefore;
                        if (needConfigs[i].UpperLimit <= 0)
                        {
                            needConfigs.RemoveAt(i);
                            i--;
                        }
                        else if (needConfigs[i].LowerLimit < 0)
                            needConfigs[i].LowerLimit = 0;
                    }
                    foreach (var activeConfig in needConfigs)
                    {
                        var notCostValue2 = notCostValue - ((decimal)activeConfig.UpperLimit - (decimal)activeConfig.LowerLimit);
                        if (notCostValue2 <= 0)
                        {
                            cost = cost + notCostValue * (decimal)activeConfig.Value;
                            break;
                        }
                        else
                        {
                            notCostValue = notCostValue2;
                            cost = cost + ((decimal)activeConfig.UpperLimit - (decimal)activeConfig.LowerLimit) * (decimal)activeConfig.Value;
                        }
                    }
                }
            }
            return cost;
        }

        /// <summary>
        /// 获得配置的总定额数据
        /// </summary>
        /// <param name="quota"></param>
        /// <param name="quotaValue"></param>
        /// <returns></returns>
        private decimal GetQuotaValue(MonitoringConfigData quota, decimal quotaValue)
        {
            if (quota.WayId == DictionaryCache.QuotaWayPerCapita.Id)
            {
                if (quota.TargetTypeId == DictionaryCache.ConfigToOrg.Id)
                {
                    var org = db.Organizations.FirstOrDefault(o => o.Id == quota.TargetId);
                    if (org.CustomerCount == null)
                        throw new Exception("组织机构没有设置人数，无法计算人均定额数据");
                    quotaValue = (int)org.CustomerCount * (decimal)quota.UnitValue;
                }
                else
                {
                    var building = db.Buildings.FirstOrDefault(o => o.Id == quota.TargetId);
                    if (building.CustomerCount == null)
                        throw new Exception("建筑没有设置人数，无法计算人均定额数据");
                    quotaValue = (int)building.CustomerCount * (decimal)quota.UnitValue;
                }
            }
            else if (quota.WayId == DictionaryCache.QuotaWayPerUnitArea.Id)
            {
                if (quota.TargetTypeId == DictionaryCache.ConfigToOrg.Id)
                {
                    var org = db.Organizations.FirstOrDefault(o => o.Id == quota.TargetId);
                    decimal area = (decimal)org.Buildings.Sum(o => o.TotalArea);
                    if (area == 0)
                        throw new Exception("组织机构下属建筑均没有设置总面积，无法计算单位面积定额数据");
                    quotaValue = area * (decimal)quota.UnitValue;
                }
                else
                {
                    var building = db.Buildings.FirstOrDefault(o => o.Id == quota.TargetId);
                    if (building.TotalArea == null)
                        throw new Exception("建筑没有设置总面积，无法计算单位面积定额数据");
                    quotaValue = (int)building.TotalArea * (decimal)quota.UnitValue;
                }
            }
            else
            {
                quotaValue = (decimal)quota.Value;
            }
            return quotaValue;
        }



        /// <summary>
        /// 获得所有的有效定额或指标或补贴数据
        /// </summary>
        /// <param name="configTypeId"></param>
        ///  <param name="isHour">是否只取按小时 周期配置的数据</param>
        /// <returns></returns>
        public List<MonitoringConfigData> GetBuildingMonitoringConfig(int configTypeId, bool isHour)
        {
            List<MonitoringConfigData> results = new List<MonitoringConfigData>();
            var configs = db.MonitoringConfig.Where(o => o.ConfigTypeId == configTypeId && o.Enabled == true && o.StartTime <= DateTime.Now && o.EndTime > DateTime.Now && (isHour ? o.CycleTypeId == DictionaryCache.PeriodHour.Id : true)).OrderByDescending(o => o.Priority).GroupBy(o => o.TargetTypeId + "_" + o.TargetId);
            foreach (var item in configs)
            {
                foreach (var c in item)
                {
                    results.Add(c.ToViewData());
                    break;
                }
            }
            return results;
        }
        /// <summary>
        /// 用于postsearch中，转换旧版本搜索格式支持新版本格式
        /// </summary>
        /// <param name="me"></param>
        /// <param name="targetType"></param>
        /// <param name="targetId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="templateIds"></param>
        /// <returns></returns>
        public IList<MonitoringConfigData> GetConfigsByTargetAndTime(DAL.EmpModels.User me, int targetType, int targetId, DateTime? startTime, DateTime? endTime, List<int?> templateIds)
        {
            IQueryable<ConfigDetail> details;
            if (targetId != -1)
            {
                details = db.ConfigDetail.Where(o => templateIds.Contains(o.Id)
                    && ((targetType == DictionaryCache.ConfigToBuilding.Id && o.BuildingId == targetId)
                    || (targetType == DictionaryCache.ConfigToMeter.Id && o.MeterId == targetId)
                    || (targetType == DictionaryCache.ConfigToOrg.Id && o.OrganizationId == targetId))
                    );
            }
            else
            {
                details = this.db.ConfigDetail.Where(o => templateIds.Contains(o.Id)
                    );
            }
            if (startTime != null)
                details = details.Where(o => o.Template.ConfigCycleSettings.Any(c => c.BeginTime <= startTime));
            if (endTime != null)
                details = details.Where(o => o.Template.ConfigCycleSettings.Any(c => c.EndTime >= endTime));
            var detailsList = details.ToList();
            IList<MonitoringConfigData> results = new List<MonitoringConfigData>();
            List<string> hasRightUserIds = new List<string>();
            List<string> notHasRightUserIds = new List<string>();
            var tempIDs = detailsList.Select(o => o.TemplateId).ToList();
            var monitoringConfigs = this.Filter(o => tempIDs.Contains((int)o.TemplateId)).ToList();
            foreach (var detail in detailsList)
            {
                bool isHasRight = true;
                if (notHasRightUserIds.Contains(detail.OperatorId))
                    isHasRight = false;
                else if (hasRightUserIds.Contains(detail.OperatorId))
                    isHasRight = true;
                else
                {
                    //修改是否允许配置参数
                    var editUser = this.db.Users.FirstOrDefault(o=>o.Id==detail.OperatorId);

                    //如果配置者有权限的机构存在任意一是当前用户有权限机构的上级机构，则不允许修改
                    var orgsForEditUser = editUser.Organizations;
                    var orgsForMe = me.Organizations;
                    var count = orgsForEditUser.Where(o => orgsForMe.Any(c => c.TreeId.StartsWith(o.TreeId + "-"))).Count();
                    if (count > 0)
                    {
                        isHasRight = false;
                        notHasRightUserIds.Add(editUser.Id);
                    }
                    else
                        hasRightUserIds.Add(editUser.Id);
                }
                var templates = monitoringConfigs.Where(o => o.TemplateId == detail.TemplateId).ToList();
                foreach (var template in templates)
                    foreach (var cycleSetting in template.ConfigCycleSettings)
                    {
                        MonitoringConfigData config = new MonitoringConfigData();
                        EntityTools.EntityCopy(template.ToViewData(), config, "");
                        if (config.TargetTypeId == DictionaryCache.ConfigToBuilding.Id)
                            detail.BuildingId = config.TargetId;
                        else if (config.TargetTypeId == DictionaryCache.ConfigToOrg.Id)
                            detail.OrganizationId = config.TargetId;
                        else if (config.TargetTypeId == DictionaryCache.ConfigToMeter.Id)
                            detail.MeterId = config.TargetId;
                        config.StartTime = cycleSetting.BeginTime;
                        config.EndTime = cycleSetting.EndTime;
                        config.isAlowEdit = isHasRight;
                        results.Add(config);
                    }
            }
            return results;
        }
        /// <summary>
        /// 增加默认的恶性负载和负荷监控告警配置
        /// </summary>
        public void CreateBaseAlertSettiing()
        {
            var setting = this.Filter(o => o.Description == "默认的通用配置,系统生成，请勿重名配置！" && o.ConfigTypeId == (5 + DictionaryCache.MonitoringConfigTypeWarning.Id) && o.WayId == DictionaryCache.MessageTypeMalignantLoad.Id && o.Name == "默认的通用恶性负载配置").FirstOrDefault();
            if (setting == null)
            {
                MonitoringConfig config = new MonitoringConfig();
                config.Name = "默认的通用恶性负载配置";
                config.EnergyCategoryId = DictionaryCache.PowerCateogry.Id;
                config.Description = "默认的通用配置,系统生成，请勿重名配置！";
                config.TargetTypeId = DictionaryCache.ConfigToBuilding.Id;
                config.TargetId = -1;
                config.ConfigTypeId = (5 + DictionaryCache.MonitoringConfigTypeWarning.Id);
                config.WayId = DictionaryCache.MessageTypeMalignantLoad.Id;
                config.Enabled = false;
                config.StartTime = Convert.ToDateTime(DateTime.Now.AddDays(-1));
                config.EndTime = Convert.ToDateTime(DateTime.Now.AddYears(100));
                setting = this.Create(config);
            }
           
            setting = this.Filter(o =>o.Description == "默认的通用配置,系统生成，请勿重名配置！"&& o.ConfigTypeId == (5 + DictionaryCache.MonitoringConfigTypeWarning.Id) && o.WayId == DictionaryCache.MessageTypeOverLoad.Id && o.Name == "默认的通用超负荷配置").FirstOrDefault();
            if (setting == null)
            {
                MonitoringConfig config = new MonitoringConfig();
                config.EnergyCategoryId = DictionaryCache.PowerCateogry.Id;
                config.Name = "默认的通用超负荷配置";
                config.Description = "默认的通用配置,系统生成，请勿重名配置！";
                config.TargetTypeId = DictionaryCache.ConfigToBuilding.Id;
                config.TargetId = -1;
                config.ConfigTypeId = (5 + DictionaryCache.MonitoringConfigTypeWarning.Id);
                config.WayId = DictionaryCache.MessageTypeOverLoad.Id;
                config.Enabled = false;
                config.StartTime = Convert.ToDateTime(DateTime.Now.AddDays(-1));
                config.EndTime = Convert.ToDateTime(DateTime.Now.AddYears(100));
                setting = this.Create(config);
            }
            //
            setting = this.Filter(o => 
                o.Description == "默认的通用配置,系统生成，请勿重名配置！" 
                && o.ConfigTypeId == (5 + DictionaryCache.MonitoringConfigTypeWarning.Id) 
                && o.WayId == DictionaryCache.MessageTypeEnergyCreditZero.Id
                && o.EnergyCategoryId == DictionaryCache.PowerCateogry.Id
                && o.Name == "默认的水电双控以电控水欠费断水电配置").FirstOrDefault();
            if (setting == null)
            {
                MonitoringConfig config = new MonitoringConfig();
                config.EnergyCategoryId = DictionaryCache.PowerCateogry.Id;
                config.Name = "默认的水电双控以电控水欠费断水电配置";
                config.Description = "默认的通用配置,系统生成，请勿重名配置！";
                config.TargetTypeId = DictionaryCache.ConfigToBuilding.Id;
                config.TargetId = -1;
                config.ConfigTypeId = (5 + DictionaryCache.MonitoringConfigTypeWarning.Id);
                config.WayId = DictionaryCache.MessageTypeEnergyCreditZero.Id;
                config.UnitValue = 0;
                config.AlarmLevelId = DictionaryCache.AlarmLevelWarning.Id;
                config.Value = DictionaryCache.ActionOffALL.Id;
                config.Enabled = false;
                config.StartTime = Convert.ToDateTime(DateTime.Now.AddDays(-1));
                config.EndTime = Convert.ToDateTime(DateTime.Now.AddYears(100));
                setting = this.Create(config);
            }
            setting = this.Filter(o =>
              o.Description == "默认的通用配置,系统生成，请勿重名配置！"
              && o.ConfigTypeId == (5 + DictionaryCache.MonitoringConfigTypeWarning.Id)
              && o.WayId == DictionaryCache.MessageTypeEnergyCreditZero.Id
              && o.EnergyCategoryId == DictionaryCache.PowerCateogry.Id
              && o.Name == "电费欠费告警").FirstOrDefault();
            if (setting == null)
            {
                MonitoringConfig config = new MonitoringConfig();
                config.EnergyCategoryId = DictionaryCache.PowerCateogry.Id;
                config.Name = "电费欠费告警";
                config.Description = "默认的通用配置,系统生成，请勿重名配置！";
                config.TargetTypeId = DictionaryCache.ConfigToBuilding.Id;
                config.TargetId = -1;
                config.ConfigTypeId = (5 + DictionaryCache.MonitoringConfigTypeWarning.Id);
                config.WayId = DictionaryCache.MessageTypeEnergyCreditZero.Id;
                config.UnitValue = 0;
                config.AlarmLevelId = DictionaryCache.AlarmLevelWarning.Id;
                config.Value = DictionaryCache.ActionOffPower.Id;
                config.Enabled = true;
                config.StartTime = Convert.ToDateTime(DateTime.Now.AddDays(-1));
                config.EndTime = Convert.ToDateTime(DateTime.Now.AddYears(100));
                setting = this.Create(config);
            }
            setting = this.Filter(o => 
                o.Description == "默认的通用配置,系统生成，请勿重名配置！" 
                && o.ConfigTypeId == (5 + DictionaryCache.MonitoringConfigTypeWarning.Id)
                && o.EnergyCategoryId == DictionaryCache.PowerCateogry.Id
                && o.WayId == DictionaryCache.MessageTypeEnergyCreditLow.Id
                && o.Name == "电费低余额预警").FirstOrDefault();
            if (setting == null)
            {
                MonitoringConfig config = new MonitoringConfig();
                config.EnergyCategoryId = DictionaryCache.PowerCateogry.Id;
                config.Name = "电费低余额预警";
                config.Description = "默认的通用配置,系统生成，请勿重名配置！";
                config.TargetTypeId = DictionaryCache.ConfigToBuilding.Id;
                config.TargetId = -1;
                config.ConfigTypeId = (5 + DictionaryCache.MonitoringConfigTypeWarning.Id);
                config.WayId = DictionaryCache.MessageTypeEnergyCreditLow.Id;
                config.UnitValue = 5;
                config.AlarmLevelId = DictionaryCache.AlarmLevelEarlyWarning.Id;
                config.Enabled = true;
                config.StartTime = Convert.ToDateTime(DateTime.Now.AddDays(-1));
                config.EndTime = Convert.ToDateTime(DateTime.Now.AddYears(100));
                setting = this.Create(config);
            }

             setting = this.Filter(o => 
                o.Description == "默认的通用配置,系统生成，请勿重名配置！" 
                && o.ConfigTypeId == (5 + DictionaryCache.MonitoringConfigTypeWarning.Id)
                && o.EnergyCategoryId == DictionaryCache.WaterCategory.Id
                && o.WayId == DictionaryCache.MessageTypeEnergyCreditZero.Id 
                && o.Name == "默认的水费欠费断电配置").FirstOrDefault();
            if (setting == null)
            {
                MonitoringConfig config = new MonitoringConfig();
                config.EnergyCategoryId = DictionaryCache.WaterCategory.Id;
                config.Name = "默认的水费欠费断电配置";
                config.Description = "默认的通用配置,系统生成，请勿重名配置！";
                config.TargetTypeId = DictionaryCache.ConfigToBuilding.Id;
                config.TargetId = -1;
                config.ConfigTypeId = (5 + DictionaryCache.MonitoringConfigTypeWarning.Id);
                config.WayId = DictionaryCache.MessageTypeEnergyCreditZero.Id;
                config.UnitValue = 0;
                config.AlarmLevelId = DictionaryCache.AlarmLevelWarning.Id;
                config.Value = DictionaryCache.ActionOffPower.Id;
                config.Enabled = false;
                config.StartTime = Convert.ToDateTime(DateTime.Now.AddDays(-1));
                config.EndTime = Convert.ToDateTime(DateTime.Now.AddYears(100));
                setting = this.Create(config);
            }
            setting = this.Filter(o => 
                o.Description == "默认的通用配置,系统生成，请勿重名配置！" 
                && o.ConfigTypeId == (5 + DictionaryCache.MonitoringConfigTypeWarning.Id)
                 && o.EnergyCategoryId == DictionaryCache.WaterCategory.Id
                && o.WayId == DictionaryCache.MessageTypeEnergyCreditLow.Id
                && o.Name == "默认的水费低余额预警配置").FirstOrDefault();
            if (setting == null)
            {
                MonitoringConfig config = new MonitoringConfig();
                config.EnergyCategoryId = DictionaryCache.WaterCategory.Id;
                config.Name = "默认的水费低余额预警配置";
                config.Description = "默认的通用配置,系统生成，请勿重名配置！";
                config.TargetTypeId = DictionaryCache.ConfigToBuilding.Id;
                config.TargetId = -1;
                config.ConfigTypeId = (5 + DictionaryCache.MonitoringConfigTypeWarning.Id);
                config.WayId = DictionaryCache.MessageTypeEnergyCreditLow.Id;
                config.UnitValue = 5;
                config.AlarmLevelId = DictionaryCache.AlarmLevelEarlyWarning.Id;
                config.Enabled = false;
                config.StartTime = Convert.ToDateTime(DateTime.Now.AddDays(-1));
                config.EndTime = Convert.ToDateTime(DateTime.Now.AddYears(100));
                setting = this.Create(config);
            }
        
        }

        /// <summary>
        /// 获得指定建筑的相关定价列表
        /// </summary>
        /// <param name="buildingId">建筑id，可以为空，为空时传第4个参数建筑id</param>
        ///  <param name="energyCategoryId">能耗类型</param>
        ///   <param name="time">有效时间</param>
        ///    <param name="building">建筑，和第一个参数取其1，为空时查询建筑id获得建筑</param>
        /// <returns></returns>
        public IQueryable<ConfigDetail> GetBuildingPriceConfigdetails(int? buildingId, int energyCategoryId, DateTime time, Building building=null )
        {
           var energyCategory=DictionaryCache.Get()[energyCategoryId];
           if (building==null)
               building = buildingBLL.Find(buildingId);
            var configdetails = this.db.ConfigDetail.Where(o =>
                o.Enabled
                && (o.BuildingId == building.Id || building.TreeId.StartsWith(o.Building.TreeId + "-"))
                && (o.BuildingCategoryId == building.BuildingCategoryId || building.BuildingCategoryDict.TreeId.StartsWith(o.BuildingCategory.TreeId + "-"))
                && (o.EnergyCategoryId == energyCategory.Id || energyCategory.TreeId.StartsWith(o.EnergyCategory.TreeId + "-"))
                &&o.Template.ConfigTypeId==(DictionaryCache.MonitoringConfigTypePrice.Id+5)
                &&o.Template.WayId==DictionaryCache.PriceWayNormal.Id
                &&o.Template.Enabled
                //&&o.Template.ConfigCycleSettings.Any(c=>c.BeginTime<=time&&c.EndTime>time)
                && this.db.MonitoringConfig.Count(c => c.TemplateId == o.Template.Id && c.ConfigCycleSettings.Any(d => d.BeginTime <= time && d.EndTime > time))>0
                );
           
            return configdetails;
         
        }

        /// <summary>
        /// 获得指定建筑的定价
        /// </summary>
        /// <param name="buildingId">建筑id，可以为空，为空时传第4个参数建筑id</param>
        ///  <param name="energyCategoryId">能耗类型</param>
        ///   <param name="time">有效时间</param>
        ///    <param name="building">建筑，和第一个参数取其1，为空时查询建筑id获得建筑,返回对应配置设置的建筑（可能是上级建筑）</param>
        /// <returns></returns>
        public decimal? GetBuildingPriceConfigdetail(int? buildingId, int energyCategoryId, DateTime time, ref Building building)
        {
            //此查询结果可能包括上级建筑或上级能耗类型的定价
            var configs = GetBuildingPriceConfigdetails(buildingId, energyCategoryId, time, building).ToList();
            if (configs == null || configs.Count()==0)
                return null;
            var configdetail = configs.OrderByDescending(o => o.Building.TreeId).OrderByDescending(o => o.EnergyCategory.TreeId).Take(1).Select(o=>new {
                Value = db.MonitoringConfig.FirstOrDefault(c => c.TemplateId == o.TemplateId && c.ConfigCycleSettings.Any(d => d.BeginTime <= time && d.EndTime > time)).Value,
                Building=o.Building
            }).FirstOrDefault();
            building = configdetail.Building;
            return configdetail.Value;

        }

        ///// <summary>
        ///// 获得指定建筑的基础策略配置
        ///// </summary>
        ///// <param name="buildingId">建筑id，可以为空，为空时传第4个参数建筑id</param>
        /////  <param name="energyCategoryId">能耗类型</param>
        /////   <param name="time">有效时间</param>
        /////    <param name="building">建筑，和第一个参数取其1，为空时查询建筑id获得建筑,返回对应配置设置的建筑（可能是上级建筑）</param>
        ///// <returns></returns>
        //public ConfigDetailData GetBuildingPriceConfigdetail(int? buildingId,DateTime time, ref Building building)
        //{
        //    //此查询结果可能包括上级建筑或上级能耗类型的定价
        //    var configs = GetBuildingPriceConfigdetails(buildingId, DictionaryCache.PowerCateogry.Id, time, building).ToList();
        //    if (configs == null || configs.Count() == 0)
        //        return null;
        //    var configdetail = configs.OrderByDescending(o => o.Building.TreeId).OrderByDescending(o => o.EnergyCategory.TreeId).Take(1).Select(o =>o.ToViewData()).FirstOrDefault();
        //    if ()
        //    return configdetail;

        //}
        public void CreateGuaranteePowerSettiing()
        {
            var setting = this.Filter(o => o.ConfigTypeId == DictionaryCache.MonitoringConfigTypeGuaranteePower.Id).FirstOrDefault();
            if (setting == null)
            {
                MonitoringConfig config = new MonitoringConfig();
                config.Name = "节假日保电配置";
                config.TargetTypeId = DictionaryCache.MonitoringConfigTypeGuaranteePower.Id;
                config.TargetId = -1;
                config.ConfigTypeId = DictionaryCache.MonitoringConfigTypeGuaranteePower.Id;
                config.Enabled = false;
                config.StartTime = Convert.ToDateTime(DateTime.Now.AddDays(-1));
                config.EndTime = Convert.ToDateTime(DateTime.Now.AddYears(100));
                setting = this.Create(config);
            }
        }
        /// <summary>
        /// 更新此配置相关硬件设备价格
        /// </summary>
        /// <param name="config">如果是需要配置全部设备的操作，如对所有设备下发恶性负载参数，只取config的能耗类型确定下发的设备类型</param>
        /// <param name="actionid">执行动作</param>
        ///  <param name="isSetDefalutSetting">0 只配置configdetail相关的设备
        ///                                     -1通用配置(针对符合监控等告警批量配置)，只对没有在configdetail中的配置进行配置
        ///                                     1配置全部设备，比如恶性负载需要全部下载，是否启用另外一回事，此时动作参数为config的id</param>
        ///  <param name="isUserId">使用configid作为执行参数的id，如果否，则使用config的value</param>                                   
        public void updateMetersActionByConfig(MonitoringConfig config,int actionid,int isSetDefalutSetting=0,bool isUserId=false)
        {
            var value = config.Value;
            if (isSetDefalutSetting != 1)
            {
                
                if (config.TemplateId != null)
                    config = this.Find(config.TemplateId);
                List<ConfigDetail> details = config.ConfigDetailTemplates.ToList();
                List<int> results = new List<int>();
                foreach (var configDetail in details)
                {
                    var building = configDetail.Building;
                    var bTreeId = DictionaryCache.Get()[(int)configDetail.BuildingCategoryId].TreeId;
                    var eTreeId = DictionaryCache.Get()[(int)configDetail.EnergyCategoryId].TreeId;
                    var meterIds = meterBLL.Filter(o =>
                            o.Enable
                        //是其建筑下设备
                            && (configDetail.BuildingId == o.Building.Id
                            || o.Building.TreeId.StartsWith(building.TreeId + "-"))
                               //分类正确，默认使用顶级分类
                            && (configDetail.BuildingCategoryId == o.Building.BuildingCategoryId ||
                            o.Building.BuildingCategoryDict.TreeId.StartsWith(bTreeId + "-"))
                            && (configDetail.EnergyCategoryId == o.EnergyCategoryId ||
                            o.EnergyCategoryDict.TreeId.StartsWith(eTreeId + "-")
                            && o.Brand.IsFJNewcapSystem && o.Brand.IsControllable//是公司自己的可控系统
                        )
                        ).Select(o => o.Id).ToList();
                    results.AddRange(meterIds);
                }
                results = results.Distinct().ToList();
                if (isSetDefalutSetting == -1)
                {
                    string treeId="";
                    if (config.EnergyCategoryId != null)
                        treeId = DictionaryCache.Get()[(int)config.EnergyCategoryId].TreeId;
                    else
                        treeId = DictionaryCache.PowerCateogry.TreeId;
                    if (treeId.Length >= 18)
                        treeId = treeId.Substring(0, 17);
                    var buildingIds = this.db.ConfigDetail.Where(o =>o.BuildingId!=null&&o.BuildingCategoryId==20000&& o.Enabled && o.Template.Enabled).Select(o=>o.BuildingId).ToList();
                    var treeids = this.db.Buildings.Where(o => o.Enable && buildingIds.Contains(o.Id)).Select(o => o.TreeId).ToList();
                    results = meterBLL.Filter(o =>
                         o.Enable
                        //是公司自己的可控系统
                         && o.Brand.IsFJNewcapSystem
                         && o.Brand.IsControllable
                         && (o.EnergyCategoryDict.TreeId.StartsWith(treeId + "-") || o.EnergyCategoryDict.TreeId == treeId)
                         &&o.Building.Type==DictionaryCache.BuildingTypeRoom.Id
                         && !treeids.Any(c=>o.Building.TreeId.StartsWith(c+"-"))
                         //&& !results.Contains(o.Id)
                         ).Select(o => o.Id).ToList();
                }
                meterActionBLL.CreateByMeterIds(results, actionid, DateTime.Now, isUserId ? config.Id + "" : value + "", "");
            }
            else
            {
                var treeId = DictionaryCache.Get()[(int)config.EnergyCategoryId].TreeId;
                if (treeId.Length >= 18)
                    treeId = treeId.Substring(0, 17);
                var results = meterBLL.Filter(o =>
                     o.Enable
                     //是公司自己的可控系统
                     && o.Brand.IsFJNewcapSystem
                     && o.Brand.IsControllable
                     && (o.EnergyCategoryDict.TreeId.StartsWith(treeId + "-") || o.EnergyCategoryDict.TreeId == treeId)
                     ).Select(o => o.Id).ToList();
                meterActionBLL.CreateByMeterIds(results, actionid, DateTime.Now, isUserId ? config.Id + "" : value + "", "");
            }
        }
    }
}
