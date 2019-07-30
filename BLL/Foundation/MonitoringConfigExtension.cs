using WxPay2017.API.DAL.EmpModels;
using WxPay2017.API.VO.Common;
using WxPay2017.API.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WxPay2017.API.BLL
{
    public static class MonitoringConfigExtension
    {


        public static MonitoringConfigData ToViewData(this MonitoringConfig node, CategoryDictionary suffix = CategoryDictionary.None)
        {
            MonitoringConfigBLL MonitoringConfigBLL = new MonitoringConfigBLL();
            if (node == null)
                return null;
            var model = new MonitoringConfigData()
            {
                Id = node.Id,
                Name = node.Name,
                Description = node.Description,
                TargetTypeId = node.TargetTypeId,
                TargetId = node.TargetId,
                EnergyCategoryId = node.EnergyCategoryId,
                ConfigTypeId = node.ConfigTypeId,
                WayId = node.WayId,
                UnitValue = node.UnitValue,
                LowerLimit = node.LowerLimit,
                UpperLimit = node.UpperLimit,
                Value = node.Value,
                ValidStartTime = node.ValidStartTime,
                ValidEndTime = node.ValidEndTime,
                Priority = node.Priority,
                Enabled = node.Enabled,
                StartTime = node.StartTime,
                EndTime = node.EndTime,
                RegeneratorId = node.RegeneratorId,
                RegeneratorName = node.RegeneratorName,
                UpdatingTime = node.UpdatingTime,
                ParameterId = node.ParameterId,
                TemplateId = node.TemplateId,
                OverTimeDate = node.OverTimeDate,
                CycleTime = node.CycleTime,
                CycleTypeId = node.CycleTypeId,
                AlarmLevelId = node.AlarmLevelId,
                IsvalidNextCycle = node.IsvalidNextCycle,
                ValidTypeId = node.ValidTypeId,
                ValidType = node.ValidTypeId == null ? null : node.ValidType == null ? DictionaryCache.Get()[(int)node.ValidTypeId].ToViewData() : node.ValidType.ToViewData(),
                AlarmLevel = node.AlarmLevelId == null ? null : node.AlarmLevel == null ? DictionaryCache.Get()[(int)node.AlarmLevelId].ToViewData() : node.AlarmLevel.ToViewData(),
                ConfigType = node.ConfigType == null ? DictionaryCache.Get()[(int)node.ConfigTypeId].ToViewData() : node.ConfigType.ToViewData(),
                CycleType = node.CycleTypeId == null ? null : node.CycleType == null ? DictionaryCache.Get()[(int)node.CycleTypeId].ToViewData() : node.CycleType.ToViewData(),
                EnergyCategory = node.EnergyCategoryId == null ? null : node.EnergyCategory == null ? DictionaryCache.Get()[(int)node.EnergyCategoryId].ToViewData() : node.EnergyCategory.ToViewData(),
                TargetType = node.TargetType == null ? DictionaryCache.Get()[(int)node.TargetTypeId].ToViewData() : node.TargetType.ToViewData(),
                Way = node.WayId == null ? null : node.Way == null ? DictionaryCache.Get()[(int)node.WayId].ToViewData() : node.Way.ToViewData(),
                TemplateName = node.TemplateId.HasValue ? null : MonitoringConfigBLL.Filter(o => o.TemplateId == node.TemplateId).Take(1).Select(o => o.Name).ToList()[0]
            };
            //模板联动动作
            if (node.Value.HasValue)
            {
                try
                {
                    int actionId = Convert.ToInt32(node.Value);
                    model.ActionName = DictionaryCache.Get()[actionId].ChineseName;
                }
                catch { }
            }

            if (node.ConfigCycleSettings != null)
            {
                ConfigCycleSettingBLL ConfigCycleSettingBLL = new ConfigCycleSettingBLL();
                int id = model.Id;
                model.ConfigCycleSettings = ConfigCycleSettingBLL.Filter(o => o.ConfigId == id).ToViewList();
            }


            if (node.CycleTypeId != null)
            {
                if (model.CycleType.ChineseName == "每年")
                {
                    //model.CycleTimeName = "按年";
                    //model.CycleTimeDesc = Convert.ToDateTime(node.CycleTime).Year + "年";
                    model.CycleTimeName = model.CycleType.ChineseName;
                    model.CycleTimeDesc = Convert.ToDateTime(node.CycleTime).Month + "月" + Convert.ToDateTime(node.CycleTime).Day + "日" + Convert.ToDateTime(node.CycleTime).Hour + "时";
                }
                else if (model.CycleType.ChineseName == "每月")
                {
                    model.CycleTimeName = model.CycleType.ChineseName;
                    model.CycleTimeDesc = Convert.ToDateTime(node.CycleTime).Day + "日" + Convert.ToDateTime(node.CycleTime).Hour + "时";
                    //model.CycleTimeName = model.CycleType.ChineseName;
                    //model.CycleTimeDesc = Convert.ToDateTime(node.CycleTime).Month + "月"+Convert.ToDateTime(node.CycleTime).Day + "日"+Convert.ToDateTime(node.CycleTime).Hour + "时";
                }
                else if (model.CycleType.ChineseName == "每周")
                {
                    model.CycleTimeName = model.CycleType.ChineseName;
                    model.CycleTimeDesc = "第" + Convert.ToDateTime(node.CycleTime).DayOfWeek + "日" + Convert.ToDateTime(node.CycleTime).Hour + "时";
                    //model.CycleTimeName = "每周";
                    //model.CycleTimeDesc = "第" + Convert.ToDateTime(node.CycleTime).DayOfWeek + "日";
                }
                else if (model.CycleType.ChineseName == "每日")
                {
                    model.CycleTimeName = model.CycleType.ChineseName;
                    model.CycleTimeDesc = Convert.ToDateTime(node.CycleTime).Hour + "时";
                    //model.CycleTimeName = "每月";
                    //model.CycleTimeDesc = "第" + Convert.ToDateTime(node.CycleTime).Day + "日";
                }
                else if (model.CycleType.ChineseName == "每小时")
                {
                    model.CycleTimeName = model.CycleType.ChineseName;
                    //model.CycleTimeName = "每天";
                    //model.CycleTimeDesc = "第" + Convert.ToDateTime(node.CycleTime).Year + "时";
                }
            }
            if (node.ValidTypeId != null)
            {
                if (model.ValidType.ChineseName == "每年")
                {
                    model.ValidTypeDesc = "检测";
                    model.ValidTypeDesc = model.ValidTypeDesc + Convert.ToDateTime(model.ValidStartTime).Year;
                    if (Convert.ToDateTime(model.ValidStartTime).Year != Convert.ToDateTime(model.ValidEndTime).Year)
                        model.ValidTypeDesc = model.ValidTypeDesc + "至" + Convert.ToDateTime(model.ValidEndTime).Year;
                    model.ValidTypeDesc = model.ValidTypeDesc + "年数据";
                }
                else if (model.ValidType.ChineseName == "每月")
                {
                    model.ValidTypeDesc = "每年检测";
                    model.ValidTypeDesc = model.ValidTypeDesc + Convert.ToDateTime(model.ValidStartTime).Month;
                    if (Convert.ToDateTime(model.ValidStartTime).Month != Convert.ToDateTime(model.ValidEndTime).Month)
                        model.ValidTypeDesc = model.ValidTypeDesc + "至" + Convert.ToDateTime(model.ValidEndTime).Month;
                    model.ValidTypeDesc = model.ValidTypeDesc + "月数据";
                }
                else if (model.ValidType.ChineseName == "每周")
                {
                    model.ValidTypeDesc = "每周检测周";
                    model.ValidTypeDesc = model.ValidTypeDesc + Convert.ToDateTime(model.ValidStartTime).DayOfWeek;
                    if (Convert.ToDateTime(model.ValidStartTime).DayOfWeek != Convert.ToDateTime(model.ValidEndTime).DayOfWeek)
                        model.ValidTypeDesc = model.ValidTypeDesc + "至" + Convert.ToDateTime(model.ValidEndTime).DayOfWeek;
                    model.ValidTypeDesc = model.ValidTypeDesc + "数据";
                }
                else if (model.ValidType.ChineseName == "每日")
                {
                    model.ValidTypeDesc = "每月检测第";
                    model.ValidTypeDesc = model.ValidTypeDesc + Convert.ToDateTime(model.ValidStartTime).Day;
                    if (Convert.ToDateTime(model.ValidStartTime).Day != Convert.ToDateTime(model.ValidEndTime).Day)
                        model.ValidTypeDesc = model.ValidTypeDesc + "至" + Convert.ToDateTime(model.ValidEndTime).Day;
                    model.ValidTypeDesc = model.ValidTypeDesc + "日数据";
                }
                else if (model.ValidType.ChineseName == "每小时")
                {
                    model.ValidTypeDesc = "每天检测第";
                    model.ValidTypeDesc = model.ValidTypeDesc + Convert.ToDateTime(model.ValidStartTime).Hour;
                    if (Convert.ToDateTime(model.ValidStartTime).Hour != Convert.ToDateTime(model.ValidEndTime).Hour)
                        model.ValidTypeDesc = model.ValidTypeDesc + "至" + Convert.ToDateTime(model.ValidEndTime).Hour;
                    model.ValidTypeDesc = model.ValidTypeDesc + "时数据";
                }
            }
            if (node.ParameterId != null && (suffix & CategoryDictionary.Parameter) == CategoryDictionary.Parameter)
            {
                if (node.Parameter != null)
                    model.Parameter = node.Parameter.ToViewData();
                else
                {
                    ParameterBLL parameterBLL = new ParameterBLL();
                    model.Parameter = parameterBLL.Find(node.ParameterId).ToViewData();
                }
            }

            //string TargetTypeStr = node.TargetType == null ? DictionaryCache.Get()[node.TargetTypeId].Description : node.TargetType.Description;
            //if (TargetTypeStr != null)
            //{
            //    ViewMeterFullInfoBLL meterBLL = new ViewMeterFullInfoBLL();
            //    BuildingBLL buildingBLL = new BuildingBLL();
            //    OrganizationBLL organizationBLL = new OrganizationBLL();
            //    BrandBLL brandBLL = new BrandBLL();
            //    MessageBLL messageBLL = new MessageBLL();
            //    int id = -1;
            //    if (model.TargetId != null)
            //    {
            //        id=model.TargetId;
            //        if (TargetTypeStr.ToLower() == "meter")
            //            model.Target = meterBLL.Find(id).ToViewData();
            //        else if (TargetTypeStr.ToLower() == "building")
            //            model.Target = buildingBLL.Find(id).ToViewData();
            //        else if (TargetTypeStr.ToLower() == "organization")
            //            model.Target = organizationBLL.Find(id).ToViewData();
            //        else if (TargetTypeStr.ToLower() == "brand")
            //            model.Target = brandBLL.Find(id).ToViewData();
            //        else if (TargetTypeStr.ToLower() == "message")
            //            model.Target = messageBLL.Find(id).ToViewData();
            //    }
            //}
            return model;
        }

        public static IList<MonitoringConfigData> ToViewList(this IQueryable<MonitoringConfig> nodes, CategoryDictionary suffix = CategoryDictionary.None)
        {
            MonitoringConfigBLL MonitoringConfigBLL = new BLL.MonitoringConfigBLL();
            Dictionary<int, MessageData> dicMessages = new Dictionary<int, MessageData>();
            var nodeList = nodes.ToList();
            var results = nodeList.Select(node => new MonitoringConfigData()
            {
                Id = node.Id,
                Name = node.Name,
                Description = node.Description,
                TargetTypeId = node.TargetTypeId,
                TargetId = node.TargetId,
                EnergyCategoryId = node.EnergyCategoryId,
                ConfigTypeId = node.ConfigTypeId,
                WayId = node.WayId,
                UnitValue = node.UnitValue,
                TemplateId = node.TemplateId,
                OverTimeDate = node.OverTimeDate,
                LowerLimit = node.LowerLimit,
                UpperLimit = node.UpperLimit,
                Value = node.Value,
                ValidStartTime = node.ValidStartTime,
                ValidEndTime = node.ValidEndTime,
                Priority = node.Priority,
                Enabled = node.Enabled,
                StartTime = node.StartTime,
                EndTime = node.EndTime,
                RegeneratorId = node.RegeneratorId,
                RegeneratorName = node.RegeneratorName,
                UpdatingTime = node.UpdatingTime,
                ParameterId = node.ParameterId,
                CycleTime = node.CycleTime,
                CycleTypeId = node.CycleTypeId,
                AlarmLevelId = node.AlarmLevelId,
                IsvalidNextCycle = node.IsvalidNextCycle,
                ValidTypeId = node.ValidTypeId,
                ValidType = node.ValidTypeId == null ? null : node.ValidType == null ? DictionaryCache.Get()[(int)node.ValidTypeId].ToViewData() : node.ValidType.ToViewData(),
                AlarmLevel = node.AlarmLevelId == null ? null : node.AlarmLevel == null ? DictionaryCache.Get()[(int)node.AlarmLevelId].ToViewData() : node.AlarmLevel.ToViewData(),
                ConfigType = node.ConfigType == null ? DictionaryCache.Get()[(int)node.ConfigTypeId].ToViewData() : node.ConfigType.ToViewData(),
                CycleType = node.CycleTypeId == null ? null : node.CycleType == null ? DictionaryCache.Get()[(int)node.CycleTypeId].ToViewData() : node.CycleType.ToViewData(),
                EnergyCategory = node.EnergyCategoryId == null ? null : node.EnergyCategory == null ? DictionaryCache.Get()[(int)node.EnergyCategoryId].ToViewData() : node.EnergyCategory.ToViewData(),
                TargetType =  node.TargetType == null ? DictionaryCache.Get()[(int)node.TargetTypeId].ToViewData() : node.TargetType.ToViewData(),
                Way = node.WayId == null ? null : node.Way == null ? DictionaryCache.Get()[(int)node.WayId].ToViewData() : node.Way.ToViewData(),
                TemplateName = node.TemplateId.HasValue ? null : MonitoringConfigBLL.Filter(o => o.TemplateId == node.TemplateId).Take(1).Select(o => o.Name).ToList()[0]
            }).ToList();
            for (int i = 0; i < nodeList.Count(); i++)
            {
                ConfigCycleSettingBLL ConfigCycleSettingBLL = new ConfigCycleSettingBLL();
                int id = results[i].Id;
                results[i].ConfigCycleSettings = ConfigCycleSettingBLL.Filter(o => o.ConfigId == id).ToViewList();

                //模板联动动作
                if (nodeList[i].Value.HasValue)
                {
                    try
                    {
                        int actionId = Convert.ToInt32(nodeList[i].Value);
                        results[i].ActionName = DictionaryCache.Get()[actionId].ChineseName;
                    }
                    catch { }
                }
                if (nodeList[i].CycleTypeId != null)
                {
                    if (results[i].CycleType.ChineseName == "每年")
                    {
                        results[i].CycleTimeName = "按年";
                        results[i].CycleTimeDesc = "第" + Convert.ToDateTime(nodeList[i].CycleTime).Day + "月" + "第" + Convert.ToDateTime(nodeList[i].CycleTime).Day + "日" + "第" + Convert.ToDateTime(nodeList[i].CycleTime).Hour + "时";
                    }
                    else if (results[i].CycleType.ChineseName == "每月")
                    {
                        results[i].CycleTimeName = "每月";
                        results[i].CycleTimeDesc = "第" + Convert.ToDateTime(nodeList[i].CycleTime).Day + "日" + "第" + Convert.ToDateTime(nodeList[i].CycleTime).Hour + "时";
                    }
                    else if (results[i].CycleType.ChineseName == "每周")
                    {
                        results[i].CycleTimeName = "每周";
                        results[i].CycleTimeDesc = "第" + Convert.ToDateTime(nodeList[i].CycleTime).DayOfWeek + "日" + "第" + Convert.ToDateTime(nodeList[i].CycleTime).Hour + "时";
                    }
                    else if (results[i].CycleType.ChineseName == "每日")
                    {
                        results[i].CycleTimeName = "每日";
                        results[i].CycleTimeDesc = "第" + Convert.ToDateTime(nodeList[i].CycleTime).Hour + "时";
                    }
                    else if (results[i].CycleType.ChineseName == "每小时")
                    {
                        results[i].CycleTimeName = "每小时";
                        results[i].CycleTimeDesc = "第" + Convert.ToDateTime(nodeList[i].CycleTime).Hour + "时";
                    }
                }
                if (results[i].ValidTypeId != null)
                {
                    if (results[i].ValidType.ChineseName == "每年")
                    {
                        results[i].ValidTypeDesc = "检测";
                        results[i].ValidTypeDesc = results[i].ValidTypeDesc + Convert.ToDateTime(results[i].ValidStartTime).Year;
                        if (Convert.ToDateTime(results[i].ValidStartTime).Year != Convert.ToDateTime(results[i].ValidEndTime).Year)
                            results[i].ValidTypeDesc = results[i].ValidTypeDesc + "至" + Convert.ToDateTime(results[i].ValidEndTime).Year;
                        results[i].ValidTypeDesc = results[i].ValidTypeDesc + "年数据";
                    }
                    else if (results[i].ValidType.ChineseName == "每月")
                    {
                        results[i].ValidTypeDesc = "每年检测";
                        results[i].ValidTypeDesc = results[i].ValidTypeDesc + Convert.ToDateTime(results[i].ValidStartTime).Month;
                        if (Convert.ToDateTime(results[i].ValidStartTime).Month != Convert.ToDateTime(results[i].ValidEndTime).Month)
                            results[i].ValidTypeDesc = results[i].ValidTypeDesc + "至" + Convert.ToDateTime(results[i].ValidEndTime).Month;
                        results[i].ValidTypeDesc = results[i].ValidTypeDesc + "月数据";
                    }
                    else if (results[i].ValidType.ChineseName == "每周")
                    {
                        results[i].ValidTypeDesc = "每周检测周";
                        results[i].ValidTypeDesc = results[i].ValidTypeDesc + Convert.ToDateTime(results[i].ValidStartTime).DayOfWeek;
                        if (Convert.ToDateTime(results[i].ValidStartTime).DayOfWeek != Convert.ToDateTime(results[i].ValidEndTime).DayOfWeek)
                            results[i].ValidTypeDesc = results[i].ValidTypeDesc + "至" + Convert.ToDateTime(results[i].ValidEndTime).DayOfWeek;
                        results[i].ValidTypeDesc = results[i].ValidTypeDesc + "数据";
                    }
                    else if (results[i].ValidType.ChineseName == "每日")
                    {
                        results[i].ValidTypeDesc = "每月检测第";
                        results[i].ValidTypeDesc = results[i].ValidTypeDesc + Convert.ToDateTime(results[i].ValidStartTime).Day;
                        if (Convert.ToDateTime(results[i].ValidStartTime).Day != Convert.ToDateTime(results[i].ValidEndTime).Day)
                            results[i].ValidTypeDesc = results[i].ValidTypeDesc + "至" + Convert.ToDateTime(results[i].ValidEndTime).Day;
                        results[i].ValidTypeDesc = results[i].ValidTypeDesc + "日数据";
                    }
                    else if (results[i].ValidType.ChineseName == "每小时")
                    {
                        results[i].ValidTypeDesc = "每天检测第";
                        results[i].ValidTypeDesc = results[i].ValidTypeDesc + Convert.ToDateTime(results[i].ValidStartTime).Hour;
                        if (Convert.ToDateTime(results[i].ValidStartTime).Hour != Convert.ToDateTime(results[i].ValidEndTime).Hour)
                            results[i].ValidTypeDesc = results[i].ValidTypeDesc + "至" + Convert.ToDateTime(results[i].ValidEndTime).Hour;
                        results[i].ValidTypeDesc = results[i].ValidTypeDesc + "时数据";
                    }
                }
                if (nodeList[i].ParameterId != null)
                {
                    if (nodeList[i].Parameter != null)
                        results[i].Parameter = nodeList[i].Parameter.ToViewData();
                    else
                    {
                        ParameterBLL parameterBLL = new ParameterBLL();
                        results[i].Parameter = parameterBLL.Find(nodeList[i].ParameterId).ToViewData();
                    }
                }

                //    string TargetTypeStr = nodeList[i].TargetType == null ? DictionaryCache.Get()[nodeList[i].TargetTypeId].Description : nodeList[i].TargetType.Description;
                //    if (TargetTypeStr != null)
                //    {
                //        ViewMeterFullInfoBLL meterBLL = new ViewMeterFullInfoBLL();
                //        BuildingBLL buildingBLL = new BuildingBLL();
                //        OrganizationBLL organizationBLL = new OrganizationBLL();
                //        BrandBLL brandBLL = new BrandBLL();
                //        MessageBLL messageBLL = new MessageBLL();

                //        if (results[i].TargetId != null)
                //        {
                //            int id = results[i].TargetId;
                //            if (TargetTypeStr.ToLower() == "meter")
                //                results[i].Target = meterBLL.Find(id).ToViewData();
                //            else if (TargetTypeStr.ToLower() == "building")
                //                results[i].Target = buildingBLL.Find(id).ToViewData();
                //            else if (TargetTypeStr.ToLower() == "organization")
                //                results[i].Target = organizationBLL.Find(id).ToViewData();
                //            else if (TargetTypeStr.ToLower() == "brand")
                //                results[i].Target = brandBLL.Find(id).ToViewData();
                //            else if (TargetTypeStr.ToLower() == "message")
                //                results[i].Target = messageBLL.Find(id).ToViewData();
                //        }
                //    }
            }
            return results;

        }
        public static IList<MonitoringConfigData> ToShortViewList(this IQueryable<MonitoringConfig> nodes, CategoryDictionary suffix = CategoryDictionary.None)
        {
            MonitoringConfigBLL MonitoringConfigBLL = new BLL.MonitoringConfigBLL();
            Dictionary<int, MessageData> dicMessages = new Dictionary<int, MessageData>();
            var nodeList = nodes.ToList();
            var results = nodeList.Select(node => new MonitoringConfigData()
            {
                Id = node.Id,
                Name = node.Name,
                Description = node.Description,
                TargetTypeId = node.TargetTypeId,
                TargetId = node.TargetId,
                EnergyCategoryId = node.EnergyCategoryId,
                ConfigTypeId = node.ConfigTypeId,
                WayId = node.WayId,
                UnitValue = node.UnitValue,
                TemplateId = node.TemplateId,
                OverTimeDate = node.OverTimeDate,
                LowerLimit = node.LowerLimit,
                UpperLimit = node.UpperLimit,
                Value = node.Value,
                ValidStartTime = node.ValidStartTime,
                ValidEndTime = node.ValidEndTime,
                Priority = node.Priority,
                Enabled = node.Enabled,
                StartTime = node.StartTime,
                EndTime = node.EndTime,
                RegeneratorId = node.RegeneratorId,
                RegeneratorName = node.RegeneratorName,
                UpdatingTime = node.UpdatingTime,
                ParameterId = node.ParameterId,
                CycleTime = node.CycleTime,
                CycleTypeId = node.CycleTypeId,
                AlarmLevelId = node.AlarmLevelId,
                IsvalidNextCycle = node.IsvalidNextCycle,
                ValidTypeId = node.ValidTypeId,
            }).ToList();
            return results;

        }

        public static MonitoringConfig ToModel(this MonitoringConfigData node)
        {
            return new MonitoringConfig()
            {
                Id = node.Id,
                Name = node.Name,
                Description = node.Description,
                TargetTypeId = node.TargetTypeId,
                TargetId = node.TargetId,
                EnergyCategoryId = node.EnergyCategoryId,
                ConfigTypeId = node.ConfigTypeId,
                WayId = node.WayId,
                UnitValue = node.UnitValue,
                LowerLimit = node.LowerLimit,
                UpperLimit = node.UpperLimit,
                Value = node.Value,
                ValidStartTime = node.ValidStartTime,
                ValidEndTime = node.ValidEndTime,
                Priority = node.Priority,
                Enabled = node.Enabled,
                TemplateId = node.TemplateId,
                OverTimeDate = node.OverTimeDate,
                StartTime = node.StartTime,
                EndTime = node.EndTime,
                RegeneratorId = node.RegeneratorId,
                RegeneratorName = node.RegeneratorName,
                UpdatingTime = node.UpdatingTime,
                ParameterId = node.ParameterId,
                CycleTime = node.CycleTime,
                CycleTypeId = node.CycleTypeId,
                AlarmLevelId = node.AlarmLevelId,
                IsvalidNextCycle = node.IsvalidNextCycle,
            };
        }
    }
}
