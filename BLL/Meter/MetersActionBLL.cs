using WxPay2017.API.DAL.EmpModels;
using WxPay2017.API.VO.Common;
using WxPay2017.API.VO;
using WxPay2017.API.VO.Param;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using WxPay2017.API.BLL.Core;
using System.Transactions;
using System.Xml;
using System.Xml.Linq;


namespace WxPay2017.API.BLL
{

    public class MetersActionBLL : Repository<MetersAction>
    {
      
        UserBLL userBLL;
        HistoryBillBLL historyBillBLL;
        UserAccountBLL userAccountBLL;
        MeterBLL meterBLL;
        BrandBLL brandBLL;
        public MetersActionBLL(EmpContext context = null)
            : base(context)
        {
          
            userBLL = new UserBLL(this.db);
            historyBillBLL = new HistoryBillBLL(this.db);
            userAccountBLL = new UserAccountBLL(this.db);
            meterBLL = new MeterBLL(this.db);
            brandBLL = new BrandBLL(this.db);
        }

        /// <summary>
        /// 初始化设备，并下发设置到终端
        /// </summary>
        /// <param name="meter"></param>
        public void InitMeterWithConfig(Meter meter)
        {

            //余额清0
            try
            {
                decimal value = 0;
                //!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                //CreateByMeterIds(new List<int> { meter.Id }, DictionaryCache.ActionSetControlModel.Id, DateTime.Now, 3 + "", "", false, false, 9, null, null, true);
                CreateByMeterIds(new List<int> { meter.Id }, DictionaryCache.ActionChangeMeter.Id, DateTime.Now, value + "", "", false, false, 6, null, null, true);
            }
            catch { }
            if (meter.BuildingId != null)
            {
                meter.EnergyCategoryDict=DictionaryCache.Get()[meter.EnergyCategoryId];
                meter.Building = this.db.Buildings.FirstOrDefault(o => o.Id == meter.BuildingId);
                meter.Brand = this.db.Brands.FirstOrDefault(o => o.Id == meter.BrandId);
                if (meter.BuildingId != null && meter.Brand.IsControllable && meter.Brand.IsFJNewcapSystem)
                {
                    var b = meter.Building;
                    //获取设备楼宇配置
                    var building = db.Buildings.FirstOrDefault(o =>
                      b.TreeId.StartsWith(o.TreeId + "-")
                      && o.Type == DictionaryCache.BuildingTypeBuilding.Id);
                    if (building == null)
                        return;
                    //下发节假日保电配置
                    var meterIds = new List<int> { meter.Id };
                    var oldEnable = db.MonitoringConfig.Where(o => o.Name == "节假日保电配置").FirstOrDefault().Enabled;
                    if (DictionaryCache.Get()[meter.EnergyCategoryId].FirstValue==1)
                        CreateByMeterIds(meterIds, DictionaryCache.ActionGuaranteedElectricity.Id, DateTime.Now, (oldEnable == true ? 1 : 0) + "", "", false, false, 3, null, null, true);

                    //查找配置信息
                    var node = db.ConfigDetail.Where(o =>
                         o.Enabled
                        && meter.Building.TreeId.StartsWith(o.Building.TreeId + "-")
                        && (meter.EnergyCategoryId == o.EnergyCategoryId)
                        ).OrderBy(o => o.Building.TreeId.Length)
                        .FirstOrDefault();
                    
                    decimal? price = 0;
                    if (node != null)
                    {
                        var priceModel = db.MonitoringConfig.Where(
                        o => o.Enabled
                        && o.TemplateId == node.TemplateId
                        ).FirstOrDefault();
                        if (priceModel != null)
                            price = priceModel.Value;
                    }
                    if (node == null)
                    {
                        //默认远控，关闭,价格为0
                        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                        CreateByMeterIds(meterIds, DictionaryCache.ActionSetControlModel.Id, DateTime.Now, 3 + "", "", false, false, 9, null, null, true);
                        if (meter.EnergyCategoryId == DictionaryCache.PowerCateogry.Id)
                            CreateByMeterIds(meterIds, DictionaryCache.ActionSetONOFF.Id, DateTime.Now, 0 + "", "", false, false, 7, null, null, true);
                        //恶性负载配置
                        if (DictionaryCache.Get()[meter.EnergyCategoryId].FirstValue == 1)
                        {
                            var configEX1 = db.MonitoringConfig.Where(o => o.Name == "默认的通用恶性负载配置" && o.Description == "默认的通用配置,系统生成，请勿重名配置！").FirstOrDefault();
                            var configEX2 = db.RatedParameters.Where(o => o.RatedParameterTypeId == DictionaryCache.RatedParameterTypeMalignantLoad.Id).FirstOrDefault();
                            if (configEX1 != null)
                            {
                                if (configEX1.Enabled == false || configEX2 == null || configEX2.MinValue == 0)
                                    CreateByMeterIds(meterIds, DictionaryCache.ActionMalignantLoadOff.Id, DateTime.Now, "", "", false, false, 5, null, null, true);
                                else
                                    CreateByMeterIds(meterIds, DictionaryCache.ActionMalignantLoadSet.Id, DateTime.Now, configEX2.MinValue + "", "", false, false, 6, null, null, true);
                            }
                            //超负荷配置
                            configEX1 = db.MonitoringConfig.Where(o => o.Name == "默认的通用超负荷配置" && o.Description == "默认的通用配置,系统生成，请勿重名配置！").FirstOrDefault();
                            configEX2 = db.RatedParameters.Where(o => o.RatedParameterTypeId == DictionaryCache.RatedParameterTypeOverLoad.Id).FirstOrDefault();
                            if (configEX1 != null)
                            {
                                if (configEX1.Enabled == false || configEX2 == null || configEX2.MinValue == 0)
                                    CreateByMeterIds(meterIds, DictionaryCache.ActionOverLoadOff.Id, DateTime.Now, 0 + "", "", false, false, 5, null, null, true);
                                else
                                    CreateByMeterIds(meterIds, DictionaryCache.ActionOverLoadOff.Id, DateTime.Now, configEX2.MinValue + "", "", false, false, 6, null, null, true);
                            }
                        }
                        return;
                    }
                    //有配置，按配置下载

                    //控制模式
                    else
                    {
                        int controlModel = 3;
                        if (node.IsControlByAccount == true && node.IsControlByTime == true)
                        {
                            controlModel = 4;
                        }
                        else if (node.IsControlByAccount == true)
                        {
                            controlModel = 1;
                        }
                        else if (node.IsControlByTime == true)
                        {
                            controlModel = 2;
                        }
                        if (controlModel != 3)
                        {
                            using (var meterBLL = new MeterBLL())
                            {
                                var m = meterBLL.db.Meters.FirstOrDefault(o => o.Id == meter.Id);
                                m.EffectiveModel = controlModel;
                                m.EffectiveBasePrice = price;
                                meterBLL.db.SaveChanges();
                            }
                        }
                        CreateByMeterIds(meterIds, DictionaryCache.ActionSetControlModel.Id, DateTime.Now, controlModel + "", "", false, false, 9, null, null, true);


                        if (node.IsOpenMalignantLoadAlert == false || node.EnergyCategoryId != DictionaryCache.PowerCateogry.Id)
                        {
                            //恶性负载配置
                            if (DictionaryCache.Get()[meter.EnergyCategoryId].FirstValue == 1)
                                CreateByMeterIds(meterIds, DictionaryCache.ActionMalignantLoadOff.Id, DateTime.Now, "", "", false, false, 5, null, null, true);
                        }
                        else
                            if (node.MinThresholdForMalignantLoad != null)
                                CreateByMeterIds(meterIds, DictionaryCache.ActionMalignantLoadSet.Id, DateTime.Now, node.MinThresholdForMalignantLoad + "", "", false, false, 5, null, null, true);


                        if (node.IsOpenOverLoadAlert == false)
                        {
                            //超负荷配置
                            if (DictionaryCache.Get()[meter.EnergyCategoryId].FirstValue == 1)
                                CreateByMeterIds(meterIds, DictionaryCache.ActionOverLoadOff.Id, DateTime.Now, "", "", false, false, 4, null, null, true);
                        }
                        else
                            if (node.MinThresholdForOverLoad != null)
                                if (DictionaryCache.Get()[meter.EnergyCategoryId].FirstValue == 1)
                                 CreateByMeterIds(meterIds, DictionaryCache.ActionOverLoadSet.Id, DateTime.Now, node.MinThresholdForOverLoad + "", "", false, false, 4, null, null, true);


                        if (node.VacationTimeControlTemplateId != null)
                        {
                            node.VacationTimeControlTemplate = db.MonitoringConfig.FirstOrDefault(o => o.Id == node.VacationTimeControlTemplateId);
                            if (node.VacationTimeControlTemplate != null)
                            {
                                //假期时套
                                CreateByMeterIds(meterIds, DictionaryCache.ActionSetVacationTimeControlTemplate.Id, DateTime.Now, node.Id + "", "", false, false, 11, null, null, true);
                            }
                        }
                        else
                            CreateByMeterIds(meterIds, DictionaryCache.ActionSetVacationTimeControlTemplate.Id, DateTime.Now, -1 + "", "", false, false, 11, null, null, true);


                        if (node.HolidayTimeControlTemplateId != null)
                        {
                            node.HolidayTimeControlTemplate = db.MonitoringConfig.FirstOrDefault(o => o.Id == node.HolidayTimeControlTemplateId);
                            if (node.HolidayTimeControlTemplate != null)
                            {
                                //假日时套
                                CreateByMeterIds(meterIds, DictionaryCache.ActionSetHolidayTimeControlTemplate.Id, DateTime.Now, node.Id + "", "", false, false, 12, null, null, true);
                            }
                        }
                        else
                            CreateByMeterIds(meterIds, DictionaryCache.ActionSetHolidayTimeControlTemplate.Id, DateTime.Now, -1 + "", "", false, false, 12, null, null, true);
                        if (node.WeekEndTimeControlTemplateId != null)
                        {
                            node.WeekEndTimeControlTemplate = db.MonitoringConfig.FirstOrDefault(o => o.Id == node.WeekEndTimeControlTemplateId);

                            if (node.WeekEndTimeControlTemplate != null)
                            {
                                //周末时套
                                CreateByMeterIds(meterIds, DictionaryCache.ActionSetWeekEndTimeControlTemplate.Id, DateTime.Now, node.Id + "", "", false, false, 13, null, null, true);
                            }
                        }
                        else
                            CreateByMeterIds(meterIds, DictionaryCache.ActionSetWeekEndTimeControlTemplate.Id, DateTime.Now, -1 + "", "", false, false, 13, null, null, true);
                        if (node.PeacetimeTimeControlTemplateId != null)
                        {
                            node.PeacetimeTimeControlTemplate = db.MonitoringConfig.FirstOrDefault(o => o.Id == node.PeacetimeTimeControlTemplateId);
                            if (node.PeacetimeTimeControlTemplate != null)
                            {
                                //平时时套
                                CreateByMeterIds(meterIds, DictionaryCache.ActionSetPeacetimeTimeControlTemplate.Id, DateTime.Now, node.Id + "", "", false, false, 14, null, null, true);
                            }
                        }
                        //else
                        //    CreateByMeterIds(meterIds, DictionaryCache.ActionSetPeacetimeTimeControlTemplate.Id, DateTime.Now, -1 + "", "", false, false, 13, null, null, true);
                    }

                    
                    //电价
                    try
                    {
                        if (meter.EnergyCategoryId == DictionaryCache.PowerCateogry.Id)
                            CreateByMeterIds(meterIds, DictionaryCache.ActionSetPrice1.Id, DateTime.Now, price + "", "", false, false, 8, null, null, true);
                    }
                    catch { }

                    //开关状态
                    var ONOFF = 1;
                    if (meter.IsTurnOn == false)
                        ONOFF = 0;
                    if (meter.EnergyCategoryId == DictionaryCache.PowerCateogry.Id)
                        CreateByMeterIds(meterIds, DictionaryCache.ActionSetONOFF.Id, DateTime.Now, ONOFF + "", "", false, false, 7, null, null, true);


                }
            }
        }

        /// <summary>
        /// 批量设置要操作的数据
        /// </summary>
        /// <param name="meterIds">设备id列表</param>
        /// <param name="actionId">动作</param>
        /// <param name="time">时间</param>
        ///  <param name="value">发送的值</param>
        /// <param name="IsPowerOffByMoney">是否因欠费引起的关机，默认false</param>
        /// <param name="IsPowerOffByTime">是否因时间策略引起的关机，默认false</param>
        /// <param name="priority">优先度，默认1</param>
        /// <param name="groupId">分组，默认null</param>
        /// <param name="pid">父级id，默认null</param>
        ///  <param name="isNow">是否立即执行</param>
        public void CreateByMeterIds(List<int> meterIds, int actionId, DateTime actionTime, string value,string desc ,bool IsPowerOffByMoney=false, bool IsPowerOffByTime=false,int priority=1,int? groupId=null,int? pid=null,bool? isNow=false)
        {
            //目前仅支持配置下载!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            //int[] inWork = new int[] { DictionaryCache.ActionSetPrice1.Id, DictionaryCache.ActionGetRemainingAccount.Id, DictionaryCache.ActionSetControlModel.Id, DictionaryCache.ActionSetONOFF.Id, DictionaryCache.ActionBalanceChange.Id, DictionaryCache..Id, DictionaryCache.ActionMeterCheckLine.Id, DictionaryCache.ActionGatewayCheckLine.Id, DictionaryCache.ActionMeterCheckConfig.Id ,DictionaryCache.ActionOverLoadSet.Id,DictionaryCache.ActionMalignantLoadSet.Id};
            //if (!inWork.Contains(actionId))
            //    return;
            if ((isNow==false)&&(actionId != DictionaryCache.ActionBalanceChange.Id && actionId != DictionaryCache.ActionSetONOFF.Id))
                actionTime = DateTime.Now.AddYears(100);
            string ids = "";
            //需要更新为存储过程
            //恶性负载不允许配置其他用电的
            if (actionId == DictionaryCache.ActionMalignantLoadSet.Id || actionId == DictionaryCache.ActionMalignantLoadOn.Id || actionId == DictionaryCache.ActionMalignantLoadOff.Id)
                meterIds = meterBLL.Filter(o =>o.Brand.MeterType==350003&& meterIds.Contains(o.Id) && o.EnergyCategoryId == DictionaryCache.PowerCateogry.Id).Select(o => o.Id).ToList();

            foreach (var id in meterIds)
            {
                ids = ids + id + ",";
            }

            var GroupId = groupId != null ? "'"+groupId.ToString()+"'" : "null";
            var Pid = pid != null ? pid.ToString() : "null";
            var Desc = desc != "" ? "'" + desc + "'" : "null";

           
            //时套下发
            if (actionId == DictionaryCache.ActionSetVacationTimeControlTemplate.Id)
            {

                //假期时套
                var id = Convert.ToInt32(value);
                if (id == -1)
                {
                    value = string.Format("<holiday><onoff>{0}</onoff></holiday>", 0);
                }
                else
                {
                    var config = db.MonitoringConfig.FirstOrDefault(o => o.ConfigDetailVacationTimeControlTemplates.Any(c => c.Id == id));
                    if (config != null)
                    {
                        value = "";
                        int num = 0;
                        //先设置时套
                        foreach (var time in config.ConfigCycleSettings)
                        {
                            value = value + string.Format("<houritem><hour>{0:00}{1:00}</hour><act>{2}</act></houritem>", time.BeginTime.Hour, time.BeginTime.Minute, (time.Description == "送电" ? 0: 1));
                            num++;
                        }
                        if (num < 2)
                        {
                            value = value + string.Format("<houritem><hour>{0}{1}</hour><act>{2}</act></houritem>", -1, -1, -1);
                        }
                        value = string.Format("<hoursid>2</hoursid><hours>{0}</hours>", value);
                        string value1 = value;
                        value = "";
                        //设置假期时间
                        num = 0;
                        var baseConfig = db.MonitoringConfig.FirstOrDefault(o => o.Name == "节假日保电配置");
                        var configTimes = baseConfig.ConfigCycleSettings.Where(o => o.Description != "周末" && o.BeginTime.Year != 2007 && o.BeginTime != o.EndTime).ToList();
                        foreach (var item in configTimes)
                        {
                            value = value + string.Format("<item><starttime>{0:00}{1:00}{2:00}</starttime><endtime>{3:00}{4:00}{5:00}</endtime></item>", item.BeginTime.Year % 100, item.BeginTime.Month, item.BeginTime.Day, item.EndTime.Year % 100, item.EndTime.Month, item.EndTime.Day);
                            num++;
                        }
                        if (num < 2)
                        {
                            value = value + string.Format("<item><starttime>{0}</starttime><endtime>{1}</endtime></item>", -1, -1);
                        }
                        value = string.Format("<items>{0}</items>", value);
                        value = string.Format("<holiday><onoff>1</onoff>{0}</holiday>", value + value1);
                    }
                }
            }
            if (actionId == DictionaryCache.ActionGuaranteedElectricity.Id)
            {
                //节假日保电
                if (value == "1")
                {
                    //开启，则下发假日策略
                    //设置假期时间
                    //假日时套
                    var id = Convert.ToInt32(value);
                    value = "";
                    int num = 0;
                    //先设置时套
                    value = value + string.Format("<houritem><hour>0000</hour><act>0</act></houritem>");
                    value = value + string.Format("<houritem><hour>0000</hour><act>0</act></houritem>");
                    value = string.Format("<hoursid>3</hoursid><hours>{0}</hours>", value);
                    string value1 = value;
                    value = "";
                    //设置假期时间
                    num = 0;
                    var baseConfig = db.MonitoringConfig.FirstOrDefault(o => o.Name == "节假日保电配置");
                    var configTimes = baseConfig.ConfigCycleSettings.Where(o => o.Description != "周末" && o.BeginTime.Year != 2007 && o.BeginTime == o.EndTime).ToList();
                    foreach (var item in configTimes)
                    {
                        value = value + string.Format("<item><starttime>{0:00}{1:00}{2:00}</starttime></item>", item.BeginTime.Year % 100, item.BeginTime.Month, item.BeginTime.Day);
                        num++;
                    }
                    if (num < 2)
                    {
                        value = value + string.Format("<item><starttime>{0}</starttime></item>", -1);
                    }
                    value = string.Format("<items>{0}</items>", value);
                    value = string.Format("<holiday><onoff>1</onoff>{0}</holiday>", value + value1);
                  
                }
            }
            

            if (actionId == DictionaryCache.ActionSetHolidayTimeControlTemplate.Id)
            {
                //假日时套
                var id = Convert.ToInt32(value);
                if (id == -1)
                {
                    value = string.Format("<holiday><onoff>{0}</onoff></holiday>", 0);
                }
                else
                {
                    var config = db.MonitoringConfig.FirstOrDefault(o => o.ConfigDetailHolidayTimeControlTemplates.Any(c => c.Id == id));
                    if (config != null)
                    {
                        value = "";
                        int num = 0;
                        //先设置时套
                        foreach (var time in config.ConfigCycleSettings)
                        {
                            value = value + string.Format("<houritem><hour>{0:00}{1:00}</hour><act>{2}</act></houritem>", time.BeginTime.Hour, time.BeginTime.Minute, (time.Description == "送电" ? 0: 1));
                            num++;
                        }
                        if (num < 2)
                        {
                            value = value + string.Format("<houritem><hour>{0}{1}</hour><act>{2}</act></houritem>", -1, -1, -1);
                        }
                        value = string.Format("<hoursid>3</hoursid><hours>{0}</hours>", value);
                        string value1 = value;
                        value = "";
                        //设置假期时间
                        num = 0;
                        var baseConfig = db.MonitoringConfig.FirstOrDefault(o => o.Name == "节假日保电配置");
                        var configTimes = baseConfig.ConfigCycleSettings.Where(o => o.Description != "周末" && o.BeginTime.Year != 2007 && o.BeginTime == o.EndTime).ToList();
                        foreach (var item in configTimes)
                        {
                            value = value + string.Format("<item><starttime>{0:00}{1:00}{2:00}</starttime></item>", item.BeginTime.Year % 100, item.BeginTime.Month, item.BeginTime.Day);
                            num++;
                        }
                        if (num < 2)
                        {
                            value = value + string.Format("<item><starttime>{0}</starttime></item>", -1);
                        }
                        value = string.Format("<items>{0}</items>", value);
                        value = string.Format("<holiday><onoff>1</onoff>{0}</holiday>", value + value1);
                    }
                }
            }

          

            if (actionId == DictionaryCache.ActionSetWeekEndTimeControlTemplate.Id)
            {
                //周末时套
                var id = Convert.ToInt32(value);
                if (id == -1)
                {
                    value = string.Format("<holiday><onoff>{0}</onoff></holiday>", 0);
                }
                else
                {
                    var config = db.MonitoringConfig.FirstOrDefault(o => o.ConfigDetailWeekEndTimeControlTemplates.Any(c => c.Id == id));
                    if (config != null)
                    {
                        value = "";
                        int num = 0;
                        //先设置时套
                        foreach (var time in config.ConfigCycleSettings)
                        {
                            value = value + string.Format("<houritem><hour>{0:00}{1:00}</hour><act>{2}</act></houritem>", time.BeginTime.Hour, time.BeginTime.Minute, (time.Description == "送电" ? 0: 1));
                            num++;
                        }
                        if (num < 2)
                        {
                            value = value + string.Format("<houritem><hour>{0}{1}</hour><act>{2}</act></houritem>", -1, -1, -1);
                        }
                        value = string.Format("<hoursid>4</hoursid><hours>{0}</hours>", value);
                        string value1 = value;
                        value = "";
                        //设置周末
                        num = 127;
                        var baseConfig = db.MonitoringConfig.FirstOrDefault(o => o.Name == "节假日保电配置");
                        var configTimes = baseConfig.ConfigCycleSettings.Where(o => o.Description == "周末" && o.BeginTime.Year == 2007).ToList();
                        foreach (var item in configTimes)
                        {
                            switch (item.BeginTime.Day)
                            {
                                case 1:
                                    num = num - 2;
                                    break;
                                case 2:
                                    num = num - 4;
                                    break;
                                case 3:
                                    num = num - 8;
                                    break;
                                case 4:
                                    num = num - 16;
                                    break;
                                case 5:
                                    num = num - 32;
                                    break;
                                case 6:
                                    num = num - 64;
                                    break;
                                case 7:
                                    num = num - 1;
                                    break;

                                default:
                                    break;
                            }
                        }

                        value = value + string.Format("<items><item><starttime>{0}</starttime></item><item><starttime>-1</starttime></item></items>", num);
                        value = string.Format("<holiday><onoff>1</onoff>{0}</holiday>", value + value1);
                    }
                }
            }

            if (actionId == DictionaryCache.ActionSetPeacetimeTimeControlTemplate.Id)
            {
                //平时时套
                var id = Convert.ToInt32(value);
                if (id == -1)
                {
                    value = string.Format("<holiday><onoff>{0}</onoff></holiday>", 0);
                }else
                {

                    var config = db.MonitoringConfig.FirstOrDefault(o => o.ConfigDetailPeacetimeTimeControlTemplates.Any(c => c.Id == id));
                    if (config != null)
                    {
                        value = "";
                        int num = 0;
                        //先设置时套
                        foreach (var time in config.ConfigCycleSettings)
                        {
                            value = value + string.Format("<houritem><hour>{0:00}{1:00}</hour><act>{2}</act></houritem>", time.BeginTime.Hour, time.BeginTime.Minute, (time.Description == "送电" ? 0: 1));
                            num++;
                        }
                        if (num < 2)
                        {
                            value = value + string.Format("<houritem><hour>{0}{1}</hour><act>{2}</act></houritem>", -1, -1, -1);
                        }
                        value = string.Format("<hoursid>1</hoursid><hours>{0}</hours>", value);
                        value = string.Format("<holiday><onoff>1</onoff>{0}</holiday>", value);
                    }
                }
               
            }

            //恶性负载和负荷监控处理下传值
            if (actionId == DictionaryCache.ActionMalignantLoadOff.Id)
            {

                actionId = DictionaryCache.ActionMalignantLoadSet.Id;
                value = string.Format("<MinimumThreshold>{0}</MinimumThreshold>", 0);
               
            }
            if (actionId == DictionaryCache.ActionMalignantLoadSet.Id)
            {
                
                var config = db.RatedParameters.Where(o => o.RatedParameterTypeId == DictionaryCache.RatedParameterTypeMalignantLoad.Id).FirstOrDefault();
                if (config==null)
                    return ;

                value = string.Format("<MinimumThreshold>{0}</MinimumThreshold>",value);
                //value = string.Format("<MinimumThreshold>{0}</MinimumThreshold><MaxAlarmNumber>{1}</MaxAlarmNumber><PPF>{2}</PPF><RPF>{3}</RPF>", value, config.MaxValue, config.PPF, config.RPF);
                //var whiteConfigs = db.RatedParameters.Where(o => o.RatedParameterTypeId == DictionaryCache.RatedParameterTypeMalignantLoadWhite.Id).ToList();
                //if (whiteConfigs.Count() != 0)
                //{
                //    value = value + string.Format("<whiteConfig>");
                //    int num = 0;
                //    foreach (var whiteConfig in whiteConfigs)
                //    {
                //        value = value + string.Format("<item><MinPower>{0}</MinPower><MaxPower>{1}</MaxPower><PPFMax>{2}</PPFMax><RPFMax>{3}</RPFMax><PPFMin>{4}</PPFMin><RPFMin>{5}</RPFMin></item>", whiteConfig.MinValue, whiteConfig.MaxValue, whiteConfig.PPFMax, whiteConfig.RPFMax, whiteConfig.PPFMin, whiteConfig.RPFMin);
                //        num++;
                //    }
                //    if (num < 2)
                //    {
                //        value = value + string.Format("<item><MinPower>{0}</MinPower><MaxPower>{1}</MaxPower><PPFMax>{2}</PPFMax><RPFMax>{3}</RPFMax><PPFMin>{4}</PPFMin><RPFMin>{5}</RPFMin></item>", -1, -1, -1, -1, -1, -1);
                //    }
                //    value = value + string.Format("</whiteConfig>");
                //}

                //var blackConfigs = db.RatedParameters.Where(o => o.RatedParameterTypeId == DictionaryCache.RatedParameterTypeMalignantLoadBlack.Id).ToList();
                //if (blackConfigs.Count() != 0)
                //{
                //    int num = 0;
                //    value = value + string.Format("<blackConfig>");
                //    foreach (var blackConfig in blackConfigs)
                //    {
                //        value = value + string.Format("<item><MinPower>{0}</MinPower><MaxPower>{1}</MaxPower><PPFMax>{2}</PPFMax><RPFMax>{3}</RPFMax><PPFMin>{4}</PPFMin><RPFMin>{5}</RPFMin></item>", blackConfig.MinValue, blackConfig.MaxValue, blackConfig.PPFMax, blackConfig.RPFMax, blackConfig.PPFMin, blackConfig.RPFMin);
                //        num++;
                //    }
                //    if (num < 2)
                //    {
                //        value = value + string.Format("<item><MinPower>{0}</MinPower><MaxPower>{1}</MaxPower><PPFMax>{2}</PPFMax><RPFMax>{3}</RPFMax><PPFMin>{4}</PPFMin><RPFMin>{5}</RPFMin></item>", -1, -1, -1, -1, -1, -1);
                //    }
                //    value = value + string.Format("</blackConfig>");
                //}
            }

            if (actionId == DictionaryCache.ActionOverLoadSet.Id)
            {
                if (value == null)
                    value = 0 + "";
                value = string.Format("<MinPower>{0}</MinPower>", value);
            }
            if (actionId == DictionaryCache.ActionOverLoadOff.Id)
            {
                //阀值设置0为关闭
                actionId = DictionaryCache.ActionOverLoadSet.Id;
                value = 0 + "";
                value = string.Format("<MinPower>{0}</MinPower>", value);
            }
            var sql = string.Format(" CreateMeterAction '{0}',{1},'{2}',{3},{4},{5},{6},{7},{8} ,{9},{10}", ids, actionId, value, "'" + (actionTime).ToString("yyyy-MM-dd HH:mm:ss") + "'", IsPowerOffByMoney ? 1 : 0, IsPowerOffByTime ? 1 : 0, "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'", priority, GroupId, Pid, Desc);            

            //var sql = string.Format(" CreateMeterAction '{0}',{1},'{2}','{3}',{4},{5},'{6}',{7},{8} ,{9},'{10}'", ids, actionId, value, actionTime.ToString().Replace("/", "-"), IsPowerOffByMoney ? 1 : 0, IsPowerOffByTime ? 1 : 0, DateTime.Now.ToString().Replace("/", "-"), priority, (groupId==null?"NULL":""+groupId), (pid==null?"NULL":""), desc);
            var reuslt = this.db.Database.ExecuteSqlCommand(sql);
        }
        public override int Delete(MetersAction metersAction)
        {
            var sql = string.Format(" DelMeterAction {0}", metersAction.Id);
             return this.db.Database.ExecuteSqlCommand(sql);

        }

        public void SyncToMeter(string mode, Meter meter)//,bool isSyncToMeter
        {
            switch (mode)
            {
                case "PUT":
                    {
                //        //子设备
                //        if (meter.GbCode.Contains('-'))
                //        {
                //            //子设备所属网关id
                //            int gatewayMeterId = Int32.Parse(meter.GbCode.Substring(0, 4));
                //            var gatewayMeter = meterBLL.Find(gatewayMeterId);//(o => o.Id == gatewayMeterId && o.Type == DictionaryCache.MeterTypeGateway.Id);
                //            //存在metersAction记录
                //            if (this.Count(o => o.MeterId == gatewayMeterId) > 0)
                //            {
                //                //该子设备网关metersAction记录
                //                var gatewayMeterAction = this.Find(o => o.MeterId == gatewayMeterId);
                //                //读取xml[MetersAction表中SettingValue字段的值]
                //                var actionValue = gatewayMeterAction.SettingValue;
                //                XmlDocument xdoc = new XmlDocument();
                //                xdoc.LoadXml(actionValue);
                //                //所有设备子节点<meter>
                //                XmlNodeList metersChileren = xdoc.ChildNodes[0].ChildNodes[0].ChildNodes;
                //                foreach (XmlNode child in metersChileren)
                //                {
                //                    XmlElement xe = (XmlElement)child;
                //                    if (xe.GetAttribute("id") == meter.Id.ToString())
                //                    {
                //                        //xe.SetAttribute("remark","");
                //                        //xe.SetAttribute("item","");
                //                        //xe.SetAttribute("collectaddr","");
                //                        //xe.SetAttribute("mt","");
                //                        //xe.SetAttribute("ctid","");
                //                        xe.SetAttribute("rate", meter.Rate.ToString());
                //                        //xe.SetAttribute("coding","");
                //                        //xe.SetAttribute("protocol","");
                //                        xe.SetAttribute("port", meter.PortNumber == null ? "" : meter.PortNumber.ToString());
                //                        xe.SetAttribute("addr", meter.Rs485Address);
                //                    }
                //                }
                //                //保存修改后的xml到MetersAction记录
                //                gatewayMeterAction.SettingValue = xdoc.InnerXml;
                //                this.Update(gatewayMeterAction);
                //            }
                //            else
                //            {
                //                string message = string.Format("该网关Id_{0}的MetersAction记录不存在，请新增该网关设备的MetersAction记录", gatewayMeterId);
                //                //return BadRequest(message);
                //            }
                //        }   
                        var brand = brandBLL.Find(meter.BrandId);
                        if (brand.IsControllable == true && brand.IsFJNewcapSystem == true)
                            InitMeterWithConfig(meter);
                   }
                    break;
                case "POST":
                    //{
                    //    //更新GbCode
                    //    meter.GbCode = meter.GbCode == null ? meter.ParentId == null ? meter.Id.ToString().PadLeft(4, '0') : meter.ParentId.ToString().PadLeft(4, '0') + '-' + meter.Id.ToString().PadLeft(4, '0') : meter.GbCode;
                    //    meterBLL.Update(meter);
                    //    //网关设备
                    //    if (meter.Type == DictionaryCache.MeterTypeGateway.Id)
                    //    {
                    //        //获取设备基本信息
                    //        List<int> ids = new List<int>();
                    //        ids.Add(meter.Id);

                    //        //封装XML
                    //        XDocument xdoc = new XDocument
                    //        (
                    //        new XDeclaration("1.0", "utf-8", "yes"),
                    //            new XElement("root",
                    //                new XElement("meters", ""
                    //                )
                    //            )
                    //        );
                    //        //清理已存在metersAction的记录
                    //        if (this.Count(o => o.MeterId == meter.Id) > 0)
                    //        {
                    //            this.Delete(o => o.MeterId == meter.Id);
                    //        }
                            
                    //    }
                    //    //网关子设备
                    //    else if (meter.GbCode.Contains('-'))
                    //    {
                    //        //获取网关id
                    //        int gatewayMeterId = Int32.Parse(meter.GbCode.Substring(0, 4));
                    //        //判断该网关metersAction记录是否存在
                    //        if (this.Count(o => o.MeterId == gatewayMeterId) <= 0)
                    //        {
                    //            string message = string.Format("该设备的网关没有操作记录(MeterAction),请检查gbcode中的网关id_{0}、MeterAction的记录，为该网关{0}进行初始化", gatewayMeterId);
                    //            //return BadRequest(message);
                    //        }
                    //        //查找该网关设备
                    //        var gatewayMeter = meterBLL.Find(gatewayMeterId);//(o => o.Id == gatewayMeterId && o.Type == DictionaryCache.MeterTypeGateway.Id);
                    //        //查询该网关MetersAction记录
                    //        var gatewayMeterAction = this.Find(o => o.MeterId == gatewayMeterId);
                    //        //读取xml[MetersAction表中SettingValue字段的值]
                    //        var actionValue = gatewayMeterAction.SettingValue;
                    //        XmlDocument xdoc = new XmlDocument();
                    //        xdoc.LoadXml(actionValue);
                    //        //新增子节点对象
                    //        XmlElement element = xdoc.CreateElement("meter");
                    //        //新增属性对象
                    //        XmlAttribute remark = xdoc.CreateAttribute("remark");
                    //        XmlAttribute item = xdoc.CreateAttribute("item");
                    //        XmlAttribute collectaddr = xdoc.CreateAttribute("collectaddr");
                    //        XmlAttribute mt = xdoc.CreateAttribute("mt");
                    //        XmlAttribute ctid = xdoc.CreateAttribute("ct-id");
                    //        XmlAttribute rate = xdoc.CreateAttribute("rate");
                    //        rate.InnerText = meter.Rate.ToString();
                    //        XmlAttribute coding = xdoc.CreateAttribute("coding");
                    //        XmlAttribute protocol = xdoc.CreateAttribute("protocol");
                    //        protocol.InnerText = "1";   //protocol写死
                    //        XmlAttribute port = xdoc.CreateAttribute("port");
                    //        port.InnerText = meter.PortNumber == null ? "" : meter.PortNumber.ToString();
                    //        XmlAttribute addr = xdoc.CreateAttribute("addr");
                    //        addr.InnerText = meter.Rs485Address;
                    //        XmlAttribute id = xdoc.CreateAttribute("id");
                    //        id.InnerText = meter.Id.ToString();
                    //        //新增子节点属性
                    //        element.SetAttributeNode(remark);
                    //        element.SetAttributeNode(item);
                    //        element.SetAttributeNode(collectaddr);
                    //        element.SetAttributeNode(mt);
                    //        element.SetAttributeNode(ctid);
                    //        element.SetAttributeNode(rate);
                    //        element.SetAttributeNode(coding);
                    //        element.SetAttributeNode(protocol);
                    //        element.SetAttributeNode(port);
                    //        element.SetAttributeNode(addr);
                    //        element.SetAttributeNode(id);
                    //        //新增子节点
                    //        xdoc.ChildNodes[0].ChildNodes[0].AppendChild(element);
                    //        gatewayMeterAction.SettingValue = xdoc.InnerXml;
                    //        //更新网关MetersAction记录信息
                    //        this.Update(gatewayMeterAction);
                    //        //返回该网关MetersAction记录
                    //        //return Ok(gatewayMeterAction.ToViewData());
                    var brand1 = brandBLL.Find(meter.BrandId);
                    if ( brand1.IsControllable==true&&brand1.IsFJNewcapSystem==true)
                        InitMeterWithConfig(meter);
                    //    }                    
                    //}
                    break;
                case "DELETE":
                    //{
                        //网关设备
                    //    if (meter.Type == DictionaryCache.MeterTypeGateway.Id)
                    //    {
                    //        if (this.Count(o => o.MeterId == meter.Id) > 0)
                    //        {
                    //            var metersAction = this.Find(o => o.MeterId == meter.Id);
                    //            var actionValue = metersAction.SettingValue;
                    //            XmlDocument xdoc = new XmlDocument();
                    //            xdoc.LoadXml(actionValue);
                    //            if (xdoc.ChildNodes[0].ChildNodes[0].HasChildNodes)
                    //                //return BadRequest("请删除该网关设备所有子设备后，再删除该网关设备");
                    //            //没有子设备节点，删除该网关MetersAction记录
                    //            this.Delete(metersAction);
                    //        }
                    //    }
                    //    //子设备
                    //    else if (meter.GbCode.Contains('-'))
                    //    {
                    //        MetersAction gatewayMeterAction = new MetersAction();
                    //        //获取网关id
                    //        int gatewayMeterId = Int32.Parse(meter.GbCode.Substring(0, 4));
                    //        int gatewayMeterActionCount = this.Count(o => o.MeterId == gatewayMeterId);
                    //        if (gatewayMeterActionCount > 0)
                    //        {
                    //            gatewayMeterAction = this.Find(o => o.MeterId == gatewayMeterId);
                    //            XmlDocument xdoc = new XmlDocument();
                    //            xdoc.LoadXml(gatewayMeterAction.SettingValue);
                    //            if (xdoc.ChildNodes[0].ChildNodes[0].HasChildNodes)
                    //            {
                    //                //获得所有Meter子节点
                    //                XmlNodeList xl = xdoc.ChildNodes[0].ChildNodes[0].ChildNodes;
                    //                //遍历
                    //                foreach (XmlNode x in xl)
                    //                {
                    //                    //XmlNode node = x.FirstChild;

                    //                    //查找属性id为设备id的子节点
                    //                    if (x.Attributes["id"].Value == meter.Id.ToString())
                    //                    {
                    //                        //删除子节点
                    //                        x.ParentNode.RemoveChild(x);
                    //                        gatewayMeterAction.SettingValue = xdoc.InnerXml;
                    //                        this.Update(gatewayMeterAction);
                    //                    }
                    //                }
                    //            }
                    //        }
                    //    }                    
                    //}
                    break;
                default:
                    break;
            }
            
        }






        ///// <summary>
        ///// 对已经完成同步的操作进行后期处理，扣除预付费以及补贴金额
        ///// </summary>
        //public void SetCancelAccountBill(string operatorId)
        //{
        //    Encrypt encrypt = new Encrypt();
        //    var actions = this.Filter(o => 
        //        o.ActionId == DictionaryCache.ActionCancelAccount.Id 
        //        && o.IsOk == true 
        //        && o.AnswerValue != null 
        //        && o.SettingValue == null).GroupBy(o => o.Description.Substring(1, o.Description.IndexOf("/|")-1));
        //    foreach (var actionsInBuilding in actions)          
        //    {
        //        var firstAction = actionsInBuilding.FirstOrDefault();
        //        string buildingTreeIdStr = actionsInBuilding.Key;
        //        var bid = buildingTreeIdStr.Substring(buildingTreeIdStr.LastIndexOf("-")+1);
        //        var buildingUser = userBLL.GetAccountUser(CategoryDictionary.Building, Convert.ToInt32(bid), true);
        //        //查询是否有补贴或预付费余额
        //        var time=firstAction.AddTime.AddYears(-1);
              

        //        using (var scope = new TransactionScope())
        //        {
        //            try
        //            {
        //                var bills = this.db.HistoryBill.Where(o =>
        //                              o.CreateTime > time
        //                              && (((o.BillTypeId == DictionaryCache.BillTypePay.Id || o.BillTypeId == DictionaryCache.BillTypePrePay.Id)
        //                                      && (o.PayMethodId == DictionaryCache.PayMethodPrePay.Id))
        //                                  || o.BillTypeId == DictionaryCache.BillTypeSubsidy.Id)
        //                              && o.ReceiverId == buildingUser.Id
        //                              && o.IsPay == true
        //                              && (o.Value - o.UsedValue > 0)
        //                              ).ToList();
        //                decimal subsidyValue = 0;
        //                decimal prePayValue = 0;

        //                //清空每笔补贴
        //                foreach (var billToCancel in bills)
        //                {
        //                    var value = billToCancel.Value - billToCancel.UsedValue;
        //                    if (billToCancel.BillTypeId == DictionaryCache.BillTypeSubsidy.Id)
        //                        subsidyValue = subsidyValue + (decimal)value;
        //                    else
        //                        prePayValue = prePayValue + (decimal)value;
        //                    billToCancel.UsedValue = billToCancel.Value;
        //                    historyBillBLL.Update(billToCancel);
        //                }
        //                HistoryBill bill2 = new HistoryBill();
        //                bill2.ReceiverId = buildingUser.Id;
        //                bill2.PayerId = null;
        //                bill2.BeginTime = DateTime.Now;
        //                bill2.EndTime = DateTime.Now;
        //                bill2.OperatorId = operatorId;
        //                bill2.PayTypeId = DictionaryCache.PayTypeAll.Id;
        //                bill2.UsedValue = 0;
        //                bill2.CreateTime = DateTime.Now;
        //                bill2.PayMentTime = DateTime.Now;
        //                bill2.IsZero = false;
        //                bill2.IsSynchro = false;

        //                if (prePayValue > 0)
        //                {
        //                    bill2.Value = -prePayValue;
        //                    bill2.PayMethodId = DictionaryCache.PayMethodDeductionPrePay.Id;
        //                    bill2.BillTypeId = DictionaryCache.BillTypePrePay.Id;
        //                    bill2.IsPay = true;
        //                    bill2.subject = "销户预交费余额清零";
        //                    bill2.Body = "销户预交费余额清零" + string.Format("{0:0.00}", -prePayValue) + "元";
        //                    bill2 = historyBillBLL.Create(bill2);
        //                }
        //                if (subsidyValue > 0)
        //                {
        //                    bill2.Value = -subsidyValue;
        //                    bill2.PayMethodId = DictionaryCache.PayMethodSubsidyOverTime.Id;
        //                    bill2.BillTypeId = DictionaryCache.BillTypeSubsidyOverTime.Id;
        //                    bill2.IsPay = true;
        //                    bill2.subject = "销户补贴余额清零";
        //                    bill2.Body = "销户补贴余额清零" + string.Format("{0:0.00}", -subsidyValue) + "元";
        //                    bill2 = historyBillBLL.Create(bill2);
        //                }
        //                //用户账户清零
        //                var zeroStr = encrypt.Encrypto("0");
        //                foreach (var userAccount in buildingUser.UserAccount)
        //                {
        //                    userAccount.Balance = zeroStr;
        //                    userAccountBLL.Update(userAccount);
        //                }
        //                foreach (var action in actionsInBuilding)
        //                {
        //                    action.SettingValue=firstAction.AnswerValue;
        //                }
        //                //修改操作上退费金额
        //                firstAction.SettingValue = (Convert.ToDecimal(firstAction.AnswerValue) - subsidyValue - subsidyValue).ToString();
        //                this.Update(firstAction);
        //                scope.Complete();
        //            }
        //            catch (Exception ex)
        //            {
        //                MyConsole.Log(ex);
        //                throw ex;
        //            }
        //        }
        //    }
        //}
    }
}
