using WxPay2017.API.DAL.EmpModels;
using WxPay2017.API.VO.Common;
using WxPay2017.API.VO;
using WxPay2017.API.VO.Param;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WxPay2017.API.BLL
{
    public static class BuildingExtension
    {
        public static BuildingData ToViewData(this Building node, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (node == null) return null;
            var model = new BuildingData()
            {
                Id = node.Id,
                ParentId = node.ParentId,
                Parent = (suffix & CategoryDictionary.Parent) == CategoryDictionary.Parent && node.ParentId.HasValue ? node.Parent.ToViewData() : null,
                HasChildren = node.Children.Count,
                OrganizationId = node.OrganizationId,
                BuildingCategoryId = node.BuildingCategoryId,
                CoordinateMapId = node.CoordinateMapId,
                Coordinate2dId = node.Coordinate2dId,
                TreeId = node.TreeId,
                Coordinate3dId = node.Coordinate3dId,
                GbCode = node.GbCode,
                Name = node.Name,
                AliasName = node.AliasName,
                Initial = node.Initial,
                ProvinceId = node.ProvinceId,
                Province = (suffix & CategoryDictionary.Province) == CategoryDictionary.Province && node.ProvinceId.HasValue ? node.Province.ToViewData() : null,
                CityId = node.CityId,
                City = (suffix & CategoryDictionary.City) == CategoryDictionary.City && node.CityId.HasValue ? node.City.ToViewData() : null,
                DistrictId = node.DistrictId,
                District = (suffix & CategoryDictionary.District) == CategoryDictionary.District && node.DistrictId.HasValue ? node.District.ToViewData() : null,
                Type = node.Type,
                ManagerCount = node.ManagerCount,
                CustomerCount = node.CustomerCount,
                TotalArea = node.TotalArea,
                WorkingArea = node.WorkingArea,
                LivingArea = node.LivingArea,
                ReceptionArea = node.ReceptionArea,
                Enable = node.Enable,
                Description = node.Description,
                UpFloor = node.UpFloor,
                Year = node.Year,
                Sort = node.Sort,
                MaxCustomerCount = node.MaxCustomerCount,
                Purpose = node.Purpose
            };
            if ((suffix & CategoryDictionary.Meter) == CategoryDictionary.Meter)
            {
                model.Meters = node.Meters.ToList().Select(x => x.ToViewData()).ToList();
            }
            if ((suffix & CategoryDictionary.Organization) == CategoryDictionary.Organization)
            {
                model.Organization = node.Organization.ToViewData();
            }

            if ((suffix & CategoryDictionary.Dictionary) == CategoryDictionary.Dictionary || (suffix & CategoryDictionary.BuildingType) == CategoryDictionary.BuildingType)
            {
                model.TypeDict = node.TypeDict.ToViewData();
            }

            if ((suffix & CategoryDictionary.BuidlingCategory) == CategoryDictionary.BuidlingCategory)
            {
                model.BuildingCategory = node.BuildingCategoryDict.ToViewData();
            }

            if ((suffix & CategoryDictionary.Children) == CategoryDictionary.Children)
            {
                model.Children = node.Children.Select(x => x.ToViewData()).ToList();
            }
            if ((suffix & CategoryDictionary.Descendant) == CategoryDictionary.Descendant)
            {
                model.Descendants = node.Descendants(false).Select(x => x.ToViewData()).ToList();
            }
            if ((suffix & CategoryDictionary.Ancestor) == CategoryDictionary.Ancestor)
            {
                model.Ancestors = node.Ancestors().Select(x => x.ToViewData()).ToList();
            }

            if ((suffix & CategoryDictionary.Coordinate) == CategoryDictionary.Coordinate)
            {
                if (node.Coordinate2d != null) model.Coordinate2d = node.Coordinate2d.ToViewData();
                if (node.Coordinate3d != null) model.Coordinate3d = node.Coordinate3d.ToViewData();
                if (node.CoordinateMap != null) model.CoordinateMap = node.CoordinateMap.ToViewData();
            }
            if ((suffix & CategoryDictionary.ExtensionField) == CategoryDictionary.ExtensionField)
            {
                var ctx = new EmpContext();
                model.ExtensionFields = ctx.ExtensionFields.Where(x => x.Table == "Building" && x.JoinId == node.Id).ToViewList();
            }
            return model;
        }
        public static BuildingData ToViewData(this BuildingFullInfo node, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (node != null)
            {
                var model = new BuildingData()
                {
                    Id = node.Id,
                    ParentId = node.ParentId,
                    Parent = (suffix & CategoryDictionary.Parent) == CategoryDictionary.Parent && node.ParentId.HasValue ? new BuildingData
                    {
                        Id = (int)node.ParentId,
                        OrganizationId = node.ParentOrganizationId,
                        BuildingCategoryId = node.BuildingCategoryId,
                        GbCode = node.ParentGbCode,
                        Name = node.ParentName,
                        AliasName = node.ParentAliasName,
                        Type = node.ParentType == null ? -1 : (int)node.ParentType,
                    } : null,
                    HasChildren = node.HasChildren == null ? 0 : (int)node.HasChildren,
                    OrganizationId = node.OrganizationId,
                    BuildingCategoryId = node.BuildingCategoryId,
                    CoordinateMapId = node.CoordinateMapId,
                    Coordinate2dId = node.Coordinate2dId,
                    TreeId = node.TreeId,
                    Coordinate3dId = node.Coordinate3dId,
                    GbCode = node.GbCode,
                    Name = node.Name,
                    AliasName = node.AliasName,
                    Initial = node.Initial,
                    Type = node.Type,
                    ManagerCount = node.ManagerCount,
                    CustomerCount = node.CustomerCount,
                    TotalArea = node.TotalArea,
                    WorkingArea = node.WorkingArea,
                    LivingArea = node.LivingArea,
                    ReceptionArea = node.ReceptionArea,
                    Enable = node.BuildingEnable,
                    Description = node.Description,
                    UpFloor = node.UpFloor,
                    Year = node.Year,
                    Sort = node.Sort,
                    MaxCustomerCount = node.MaxCustomerCount,
                    Purpose = node.Purpose
                };
                if ((suffix & CategoryDictionary.ExtensionField) == CategoryDictionary.ExtensionField)
                {
                    var ctx = new EmpContext();
                    model.ExtensionFields = ctx.ExtensionFields.Where(x => x.Table == "Building" && x.JoinId == node.Id).ToViewList();
                }
                return model;
            }
            else
            {
                return null;
            }
        }

        public static BuildingShortData ToShortViewData(this Building node, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (node == null) return null;
            var model = new BuildingShortData()
            {
                Id = node.Id,
                ParentId = node.ParentId,

                OrganizationId = node.OrganizationId,
                BuildingCategoryId = node.BuildingCategoryId,
                CoordinateMapId = node.CoordinateMapId,
                Coordinate2dId = node.Coordinate2dId,
                TreeId = node.TreeId,
                Coordinate3dId = node.Coordinate3dId,
                GbCode = node.GbCode,
                Name = node.Name,
                AliasName = node.AliasName,
                Initial = node.Initial,
                ProvinceId = node.ProvinceId,
                CityId = node.CityId,
                DistrictId = node.DistrictId,
                Type = node.Type,
                ManagerCount = node.ManagerCount,
                CustomerCount = node.CustomerCount,
                TotalArea = node.TotalArea,
                WorkingArea = node.WorkingArea,
                LivingArea = node.LivingArea,
                ReceptionArea = node.ReceptionArea,
                Enable = node.Enable,
                Description = node.Description,
                UpFloor = node.UpFloor,
                Year = node.Year,
                Sort = node.Sort,
                MaxCustomerCount = node.MaxCustomerCount
            };

            return model;
        }
        
        public static async Task<GisBuildingData> ToGisData(this Building node, bool includeStat = false, CategoryDictionary suffix = CategoryDictionary.None)
        {
            var start_month = DateTime.Parse(DateTime.Today.ToString("yyyy-MM-01"));
            var start_year = DateTime.Parse(DateTime.Today.ToString("yyyy-01-01"));
            var descents = node.Descendants(true);
            var ids = descents.Select(x => x.Id).ToList();
            var model = new GisBuildingData()
            {
                Id = node.Id,
                Name = node.Name,
                OrganizationId = node.OrganizationId,
                BuildingCategoryName = node.BuildingCategoryDict.ChineseName,
                Type = node.Type,
                //Year = node.BuildingInfo.Year,
                //UpFloor = node.BuildingInfo.UpFloor,
                ManagerCount = node.ManagerCount,
                CustomerCount = node.CustomerCount,
                TotalArea = node.TotalArea,
                LivingArea = node.LivingArea,
                WorkingArea = node.WorkingArea,
                ReceptionArea = node.ReceptionArea,
                Year = node.Year,
                UpFloor = node.UpFloor,
                Sort = node.Sort,
                Coordinate2d = node.Coordinate2d == null ? null : node.Coordinate2d.ToViewData(),
                Coordinate3d = node.Coordinate3d == null ? null : node.Coordinate3d.ToViewData(),
                CoordinateMap = node.CoordinateMap == null ? null : node.CoordinateMap.ToViewData(),
                //Electric = GetTotal(new List<int>() { node.Id }, 90001, 60019, TimeUnits.Monthly, start_month, DateTime.Today).SelectMany(x => x.Result).Sum(x => x.Value),
                //Water = GetTotal(new List<int>() { node.Id }, 90031, 60023, TimeUnits.Monthly, start_month, DateTime.Today).SelectMany(x => x.Result).Sum(x => x.Value),
                //ElectricYear = GetTotal(new List<int>() { node.Id }, 90001, 60019, TimeUnits.Yearly, start_year, DateTime.Today).SelectMany(x => x.Result).Sum(x => x.Value),
                //WaterYear = GetTotal(new List<int>() { node.Id }, 90031, 60023, TimeUnits.Yearly, start_year, DateTime.Today).SelectMany(x => x.Result).Sum(x => x.Value),
                Icon = "",
                Children = ((suffix & CategoryDictionary.Children) == CategoryDictionary.Children) ? node.Children.Select(c => new GisBuildingData()
                {
                    Id = c.Id,
                    Name = c.Name,
                    BuildingCategoryName = node.BuildingCategoryDict.ChineseName,
                    ManagerCount = node.ManagerCount,
                    CustomerCount = node.CustomerCount,
                    TotalArea = node.TotalArea,
                    LivingArea = node.LivingArea,
                    WorkingArea = node.WorkingArea,
                    ReceptionArea = node.ReceptionArea,
                    Year = node.Year,
                    UpFloor = node.UpFloor,
                    Sort = node.Sort,
                    //Electric = GetTotal(new List<int>() { node.Id }, 90001, 60019, TimeUnits.Monthly, start_month, DateTime.Today).SelectMany(x => x.Result).Sum(x => x.Value),
                    //Water = GetTotal(new List<int>() { node.Id }, 90031, 60023, TimeUnits.Monthly, start_month, DateTime.Today).SelectMany(x => x.Result).Sum(x => x.Value),
                    //ElectricYear = GetTotal(new List<int>() { node.Id }, 90001, 60019, TimeUnits.Yearly, start_year, DateTime.Today).SelectMany(x => x.Result).Sum(x => x.Value),
                    //WaterYear = GetTotal(new List<int>() { node.Id }, 90031, 60023, TimeUnits.Yearly, start_year, DateTime.Today).SelectMany(x => x.Result).Sum(x => x.Value),
                    Icon = ""
                }).ToList() : null,
                Meters = ((suffix & CategoryDictionary.Meter) == CategoryDictionary.Meter) ? descents.SelectMany(d => d.Meters).ToList().Select(m => m.ToViewData(CategoryDictionary.Children | CategoryDictionary.Momentary)).ToList() : null
            };
            try
            {
                if (includeStat)
                {

                    var water_today = await GetTotal(ids, 90031, 60023, TimeUnits.Daily, DateTime.Now.Date, DateTime.Now);
                    var elec_today = await GetTotal(ids, 90001, 60019, TimeUnits.Daily, DateTime.Now.Date, DateTime.Now);
                    var water_yesterday = await GetTotal(ids, 90031, 60023, TimeUnits.Daily, DateTime.Now.AddDays(-1).Date, DateTime.Now);
                    var elec_yesterday = await GetTotal(ids, 90001, 60019, TimeUnits.Daily, DateTime.Now.AddDays(-1).Date, DateTime.Now);

                    var water_month = await GetTotal(ids, 90031, 60023, TimeUnits.Monthly, start_month, DateTime.Now);
                    var elec_month = await GetTotal(ids, 90001, 60019, TimeUnits.Monthly, start_month, DateTime.Now);
                    var water_year = await GetTotal(ids, 90031, 60023, TimeUnits.Yearly, start_year, DateTime.Now);
                    var elec_year = await GetTotal(ids, 90001, 60019, TimeUnits.Yearly, start_year, DateTime.Now);
                    foreach (var item in model.Children)
                    {
                        var elec_m = elec_month.FirstOrDefault(x => x.StatisticalId == item.Id);
                        if (elec_m != null) item.Electric = elec_m.Result.Sum(x => x.Value);
                        var water_m = water_month.FirstOrDefault(x => x.StatisticalId == item.Id);
                        if (water_m != null) item.Water = water_m.Result.Sum(x => x.Value);
                        var elec_y = elec_year.FirstOrDefault(x => x.StatisticalId == item.Id);
                        if (elec_y != null) item.ElectricYear = elec_y.Result.Sum(x => x.Value);
                        var water_y = water_year.FirstOrDefault(x => x.StatisticalId == item.Id);
                        if (water_y != null) item.WaterYear = water_y.Result.Sum(x => x.Value);

                        var elec_to = elec_today.FirstOrDefault(x => x.StatisticalId == item.Id);
                        if (elec_to != null) item.ElectricToday = elec_to.Result.Sum(x => x.Value);
                        var water_to = water_today.FirstOrDefault(x => x.StatisticalId == item.Id);
                        if (water_to != null) item.WaterToday = water_to.Result.Sum(x => x.Value);


                        var elec_ye = elec_yesterday.FirstOrDefault(x => x.StatisticalId == item.Id);
                        if (elec_ye != null) item.ElectricYesterday = elec_ye.Result.Sum(x => x.Value);
                        var water_ye = water_yesterday.FirstOrDefault(x => x.StatisticalId == item.Id);
                        if (water_ye != null) item.WaterYesterday = water_ye.Result.Sum(x => x.Value);
                    }
                    var elec_sm = elec_month.FirstOrDefault(x => x.StatisticalId == node.Id);
                    if (elec_sm != null) model.Electric = elec_sm.Result.Sum(x => x.Value);
                    var water_sm = water_month.FirstOrDefault(x => x.StatisticalId == node.Id);
                    if (water_sm != null) model.Water = water_sm.Result.Sum(x => x.Value);
                    var elec_sy = elec_year.FirstOrDefault(x => x.StatisticalId == model.Id);
                    if (elec_sy != null) model.ElectricYear = elec_sy.Result.Sum(x => x.Value);
                    var water_sy = water_year.FirstOrDefault(x => x.StatisticalId == model.Id);
                    if (water_sy != null) model.WaterYear = water_sy.Result.Sum(x => x.Value);

                    //昨日
                    var elec_sye = elec_yesterday.FirstOrDefault(x => x.StatisticalId == model.Id);
                    if (elec_sye != null) model.ElectricYesterday = elec_sye.Result.Sum(x => x.Value);
                    var water_sye = water_yesterday.FirstOrDefault(x => x.StatisticalId == model.Id);
                    if (water_sye != null) model.WaterYesterday = water_sye.Result.Sum(x => x.Value);

                    //今日
                    var elec_st = elec_today.FirstOrDefault(x => x.StatisticalId == node.Id);
                    if (elec_st != null) model.ElectricToday = elec_st.Result.Sum(x => x.Value);
                    var water_st = water_today.FirstOrDefault(x => x.StatisticalId == node.Id);
                    if (water_st != null) model.WaterToday = water_st.Result.Sum(x => x.Value);
                }
            }
            catch (Exception ex)
            {
                string info = ex.Message;
                throw ex;
            }
            //var elec_sy = elec_year.FirstOrDefault(x => x.StatisticalId == node.Id);
            //if (elec_sy != null) model.ElectricYear = elec_sy.Result.Sum(x => x.Value);
            //var water_sy = water_year.FirstOrDefault(x => x.StatisticalId == node.Id);
            //if (water_sy != null) model.WaterYear = water_sy.Result.Sum(x => x.Value);

            return model;

        }


        //public static IEnumerable<GisBuildingData> ToGisList(IQueryable<Building> nodes)
        //{
        //    var start_month = DateTime.Parse(DateTime.Today.ToString("yyyy-MM-01"));
        //    var start_year = DateTime.Parse(DateTime.Today.ToString("yyyy-01-01"));
        //    var ids = nodes.Select(x => x.Id).ToList();
        //    var cids = nodes.SelectMany(x => x.Children).Select(x => x.Id).ToList();
        //    var arr = ids.Concat(cids).Distinct().ToList();
        //    var list = nodes.ToList().Select(node => new GisBuildingData()
        //    {
        //        Id = node.Id,
        //        Name = node.Name,
        //        BuildingCategoryName = node.BuildingCategoryDict.ChineseName,
        //        //Year = node.BuildingInfo.Year,
        //        //UpFloor = node.BuildingInfo.UpFloor,
        //        ManagerCount = node.ManagerCount,
        //        CustomerCount = node.CustomerCount,
        //        TotalArea = node.TotalArea,
        //        LivingArea = node.LivingArea,
        //        WorkingArea = node.WorkingArea,
        //        ReceptionArea = node.ReceptionArea,
        //        Year = node.Year,
        //        UpFloor = node.UpFloor,
        //        //Electric = GetTotal(new List<int>() { node.Id }, 90001, 60019, TimeUnits.Monthly, start_month, DateTime.Today).SelectMany(x => x.Result).Sum(x => x.Value),
        //        //Water = GetTotal(new List<int>() { node.Id }, 90031, 60023, TimeUnits.Monthly, start_month, DateTime.Today).SelectMany(x => x.Result).Sum(x => x.Value),
        //        //ElectricYear = GetTotal(new List<int>() { node.Id }, 90001, 60019, TimeUnits.Yearly, start_year, DateTime.Today).SelectMany(x => x.Result).Sum(x => x.Value),
        //        //WaterYear = GetTotal(new List<int>() { node.Id }, 90031, 60023, TimeUnits.Yearly, start_year, DateTime.Today).SelectMany(x => x.Result).Sum(x => x.Value),
        //        Icon = "",
        //        Children = node.Children.Select(c => new GisBuildingData()
        //        {
        //            Id = c.Id,
        //            Name = c.Name,
        //            BuildingCategoryName = node.BuildingCategoryDict.ChineseName,
        //            ManagerCount = node.ManagerCount,
        //            CustomerCount = node.CustomerCount,
        //            TotalArea = node.TotalArea,
        //            LivingArea = node.LivingArea,
        //            WorkingArea = node.WorkingArea,
        //            ReceptionArea = node.ReceptionArea,
        //            Year = node.Year,
        //            UpFloor = node.UpFloor,
        //            //Electric = GetTotal(new List<int>() { node.Id }, 90001, 60019, TimeUnits.Monthly, start_month, DateTime.Today).SelectMany(x => x.Result).Sum(x => x.Value),
        //            //Water = GetTotal(new List<int>() { node.Id }, 90031, 60023, TimeUnits.Monthly, start_month, DateTime.Today).SelectMany(x => x.Result).Sum(x => x.Value),
        //            //ElectricYear = GetTotal(new List<int>() { node.Id }, 90001, 60019, TimeUnits.Yearly, start_year, DateTime.Today).SelectMany(x => x.Result).Sum(x => x.Value),
        //            //WaterYear = GetTotal(new List<int>() { node.Id }, 90031, 60023, TimeUnits.Yearly, start_year, DateTime.Today).SelectMany(x => x.Result).Sum(x => x.Value),
        //            Icon = ""
        //        }).ToList(),
        //        Meters = node.Meters.Select(m => m.ToGisData(1)).ToList()
        //    });
        //    var water_month = GetTotal(arr, 90031, 60023, TimeUnits.Monthly, start_month, DateTime.Now);
        //    var elec_month = GetTotal(arr, 90031, 60023, TimeUnits.Monthly, start_month, DateTime.Now);
        //    var water_year = GetTotal(arr, 90031, 60023, TimeUnits.Yearly, start_year, DateTime.Now);
        //    var elec_year = GetTotal(arr, 90031, 60023, TimeUnits.Yearly, start_year, DateTime.Now);
        //    foreach (var item in list)
        //    {
        //        var elec_m = elec_month.FirstOrDefault(x => x.StatisticalId == item.Id);
        //        if (elec_m != null) item.Electric = elec_m.Result.Sum(x => x.Value);
        //        var water_m = water_month.FirstOrDefault(x => x.StatisticalId == item.Id);
        //        if (water_m != null) item.Water = water_m.Result.Sum(x => x.Value);
        //        var elec_y = elec_year.FirstOrDefault(x => x.StatisticalId == item.Id);
        //        if (elec_y != null) item.ElectricYear = elec_y.Result.Sum(x => x.Value);
        //        var water_y = water_year.FirstOrDefault(x => x.StatisticalId == item.Id);
        //        if (water_y != null) item.WaterYear = water_y.Result.Sum(x => x.Value);
        //    }
        //    return list;
        //}

        public static async Task<IEnumerable<StatisticalData>> GetTotal(List<int> targets, int energy, int paramter, TimeUnits unit, DateTime start, DateTime finish)
        {
            var node = new StatisticalNode(targets, energy, new List<int> { paramter }, unit, start, finish, StatisticalModes.Building, StatisticalWay.Total, null, null);

            var dicBLL = new DictionaryBLL();
            var meterBLL = new MeterBLL();
            var statBLL = new MeterResultBLL();
            // 获取参数对象
            var parameter = dicBLL.Get(node.ParameterTypeId).Select(d => d.Id).ToList();

            // 获取关联的一级统计设备
            IList<StatisticalTransfer> meters = await meterBLL.GetStatisticalObj(node);
            // 统计能耗结果
            var result = await statBLL.Statistics(meters, node.TimeUnit.Value, node.StartTime.Value, node.FinishTime.Value, parameter, node.StatWay.Value);

            return result;
        }

        public static async Task<bool> HasMonthly(List<int> targets, int energy, int paramter, TimeUnits unit, DateTime start, DateTime finish)
        {
            var node = new StatisticalNode(targets, energy, new List<int> { paramter }, unit, start, finish, StatisticalModes.Building, StatisticalWay.Total, null, null);

            var dicBLL = new DictionaryBLL();
            var meterBLL = new MeterBLL();
            var parameter = dicBLL.Get(node.ParameterTypeId).Select(d => d.Id).ToList();

            // 获取关联的一级统计设备
            IEnumerable<int> meters = (await meterBLL.GetStatisticalObj(node)).SelectMany(x => x.Meters).Select(x => x.Id);
            // 统计能耗结果
            return meterBLL.Count(x => meters.Contains(x.Id) && x.MeterMonthlyResults.Any(m => m.StartTime >= start)) > 0;
        }

        public static IEnumerable<BuildingData> ToViewList(this IQueryable<Building> nodes, CategoryDictionary suffix = CategoryDictionary.None)
        {
            return nodes.ToList().Select(n => n.ToViewData(suffix));
        }
        public static IEnumerable<BuildingData> ToViewList(this IEnumerable<BuildingFullInfo> nodes, CategoryDictionary suffix = CategoryDictionary.None)
        {
            var nodeList = nodes.ToList();
            var results = nodes.Select(node => new BuildingData()
            {
                Id = node.Id,
                ParentId = node.ParentId,
                Parent = (suffix & CategoryDictionary.Parent) == CategoryDictionary.Parent && node.ParentId.HasValue ? new BuildingData
                {
                    Id = (int)node.ParentId,
                    OrganizationId = node.ParentOrganizationId,
                    BuildingCategoryId = node.BuildingCategoryId,
                    GbCode = node.ParentGbCode,
                    Name = node.ParentName,
                    AliasName = node.ParentAliasName,
                    Type = node.ParentType == null ? -1 : (int)node.ParentType,
                } : null,
                HasChildren = node.HasChildren == null ? 0 : (int)node.HasChildren,
                OrganizationId = node.OrganizationId,
                BuildingCategoryId = node.BuildingCategoryId,
                CoordinateMapId = node.CoordinateMapId,
                Coordinate2dId = node.Coordinate2dId,
                Coordinate3dId = node.Coordinate3dId,
                TreeId = node.TreeId,
                GbCode = node.GbCode,
                Name = node.Name,
                AliasName = node.AliasName,
                Initial = node.Initial,
                Type = node.Type,
                ManagerCount = node.ManagerCount,
                CustomerCount = node.CustomerCount,
                TotalArea = node.TotalArea,
                WorkingArea = node.WorkingArea,
                LivingArea = node.LivingArea,
                ReceptionArea = node.ReceptionArea,
                Enable = node.BuildingEnable,
                Description = node.Description,
                UpFloor = node.UpFloor,
                Year = node.Year,
                Sort = node.Sort,
                MaxCustomerCount = node.MaxCustomerCount,
                Purpose = node.Purpose

            });
            if ((suffix & CategoryDictionary.ExtensionField) == CategoryDictionary.ExtensionField)
            {
                var ctx = new EmpContext();
                results.Join(ctx.ExtensionFields.Where(x => x.Table == "Building"), o => o.Id, f => f.JoinId, (o, f) =>
                {
                    if (o.ExtensionFields == null) o.ExtensionFields = new List<ExtensionFieldData>();
                    o.ExtensionFields.Add(f.ToViewData());
                    return o;
                }).ToList();

            }
            return results;
        }

        public static IEnumerable<BuildingData> ToShortViewList(this IEnumerable<Building> nodes, CategoryDictionary suffix = CategoryDictionary.None)
        {
            var nodeList = nodes.ToList();
            var results = nodes.Select(node => new BuildingData()
            {
                Id = node.Id,
                ParentId = node.ParentId,
                OrganizationId = node.OrganizationId,
                BuildingCategoryId = node.BuildingCategoryId,
                TreeId = node.TreeId,
                GbCode = node.GbCode,
                Name = node.Name,
                Type = node.Type,
                Sort = node.Sort,
                MaxCustomerCount = node.MaxCustomerCount
            });

            return results;
        }
        public static Building ToModel(this BuildingData node)
        {
            var model = new Building()
            {
                Id = node.Id,
                ParentId = node.ParentId,
                OrganizationId = node.OrganizationId,
                BuildingCategoryId = node.BuildingCategoryId,
                CoordinateMapId = node.CoordinateMapId,
                Coordinate2dId = node.Coordinate2dId,
                Coordinate3dId = node.Coordinate3dId,
                GbCode = node.GbCode,
                Name = node.Name,
                AliasName = node.AliasName,
                Initial = node.Initial,
                ProvinceId = node.ProvinceId,
                CityId = node.CityId,
                DistrictId = node.DistrictId,
                Type = node.Type,
                ManagerCount = node.ManagerCount,
                CustomerCount = node.CustomerCount,
                TotalArea = node.TotalArea,
                WorkingArea = node.WorkingArea,
                LivingArea = node.LivingArea,
                ReceptionArea = node.ReceptionArea,
                Enable = node.Enable,
                Description = node.Description,
                UpFloor = node.UpFloor,
                Year = node.Year,
                Sort = node.Sort,
                MaxCustomerCount = node.MaxCustomerCount,
                Purpose = node.Purpose

            };

            return model;
        }


        public static BuildingDiagram ToViewDiagram(this Building node, int layer = 0, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (node.Parent != null)
                node.Parent.Parent = null;
            var model = new BuildingDiagram()
            {
                Id = node.Id,
                ParentId = node.ParentId,
                OrganizationId = node.OrganizationId,
                BuildingCategoryId = node.BuildingCategoryId,
                CoordinateMapId = node.CoordinateMapId,
                Coordinate2dId = node.Coordinate2dId,
                Coordinate3dId = node.Coordinate3dId,
                GbCode = node.GbCode,
                Name = node.Name,
                AliasName = node.AliasName,
                Initial = node.Initial,
                Type = node.Type,
                ManagerCount = node.ManagerCount,
                CustomerCount = node.CustomerCount,
                TotalArea = node.TotalArea,
                WorkingArea = node.WorkingArea,
                LivingArea = node.LivingArea,
                ReceptionArea = node.ReceptionArea,
                Enable = node.Enable,
                Description = node.Description,
                UpFloor = node.UpFloor,
                Year = node.Year,
                Sort = node.Sort,
                Parent = (suffix & CategoryDictionary.Parent) == CategoryDictionary.Parent ? (node.Parent == null ? null : node.Parent.ToViewDiagram()) : null,
                Organization = (suffix & CategoryDictionary.Organization) == CategoryDictionary.Organization ? (node.Organization == null ? null : node.Organization.ToViewDiagram()) : null
            };

            if (layer > 0 && node.Children != null && node.Children.Count() != 0 && (suffix & CategoryDictionary.Children) == CategoryDictionary.Children)
            {
                model.Children = node.Children.Select(o => o.ToViewDiagram(--layer));
            }
            else
            {
                model.Children = null;
            }
            if ((suffix & CategoryDictionary.ExtensionField) == CategoryDictionary.ExtensionField)
            {
                var ctx = new EmpContext();
                model.ExtensionFields = ctx.ExtensionFields.Where(x => x.Table == "Building" && x.JoinId == node.Id).ToViewList();
            }
            return model;
        }
        public static BuildingDiagram ToViewDiagram(this BuildingFullInfo node, int layer = 0)
        {
            var model = new BuildingDiagram()
            {
                Id = node.Id,
                ParentId = node.ParentId,
                OrganizationId = node.OrganizationId,
                BuildingCategoryId = node.BuildingCategoryId,
                CoordinateMapId = node.CoordinateMapId,
                Coordinate2dId = node.Coordinate2dId,
                Coordinate3dId = node.Coordinate3dId,
                GbCode = node.GbCode,
                Name = node.Name,
                AliasName = node.AliasName,
                Initial = node.Initial,
                Type = node.Type,
                ManagerCount = node.ManagerCount,
                CustomerCount = node.CustomerCount,
                TotalArea = node.TotalArea,
                WorkingArea = node.WorkingArea,
                LivingArea = node.LivingArea,
                ReceptionArea = node.ReceptionArea,
                Enable = node.BuildingEnable,
                Description = node.Description,
                UpFloor = node.UpFloor,
                Year = node.Year,
                Sort = node.Sort,
                Parent = (node.ParentId.HasValue && layer == 0) ? new BuildingDiagram
                {
                    Id = (int)node.ParentId,
                    OrganizationId = node.ParentOrganizationId,
                    BuildingCategoryId = node.BuildingCategoryId,
                    GbCode = node.ParentGbCode,
                    Name = node.ParentName,
                    AliasName = node.ParentAliasName,
                    Type = node.ParentType == null ? -1 : (int)node.ParentType,
                } : null,
                Organization = node.OrganizationId == null ? null : new OrganizationDiagram
                {
                    Id = (int)node.OrganizationId,
                    TreeId = node.TreeId,
                    Rank = node.Rank,
                    ParentId = node.OrganizationParentId,
                    Name = node.OrganizationName,
                    AliasName = node.OrganizationAliasName,
                    Enable = node.OrganizationEnable == null ? true : (bool)node.OrganizationEnable

                },
            };

            if (layer > 0 && node.HasChildren != null && node.HasChildren != 0)
            {
                ViewBuildingFullInfoBLL buildingBLL = new ViewBuildingFullInfoBLL();
                var l = layer - 1;
                model.Children = buildingBLL.Filter(o => o.ParentId == node.Id).Select(o => o.ToViewDiagram(l));
            }
            else
            {
                model.Children = null;
            }


            var ctx = new EmpContext();
            model.ExtensionFields = ctx.ExtensionFields.Where(x => x.Table == "Building" && x.JoinId == node.Id).ToViewList();

            return model;
        }


        public static IQueryable<Building> Peers(this Building node, bool includeSelf = false)
        {
            var ctx = new BuildingBLL();
            var list = ctx.Filter(x => x.Rank == node.Rank || (includeSelf && x.Id == node.Id)).AsQueryable();
            return list;
        }


        #region Descendant  Ancestor
        /// <summary>
        /// 获取所有后代对象列表
        /// </summary>
        /// <param name="node"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IEnumerable<Building> Descents(this Building node, Expression<Func<Building, bool>> predicate)
        {
            return node.Descendants().Where(predicate.Compile());
        }

        /// <summary>
        /// 获取所有前代对象
        /// </summary>
        /// <param name="node"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IEnumerable<Building> Ancestors(this Building node, Expression<Func<Building, bool>> predicate)
        {
            return node.Ancestors().Where(predicate.Compile());
        }

        /// <summary>
        /// 所有后代
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static IEnumerable<Building> Descendants(this Building node, bool includeSelf = false)
        {
            var ctx = new EmpContext();
            return ctx.Buildings.Where(DescendantsFunc(node, includeSelf)).AsEnumerable();
        }

        private static Func<Building, bool> DescendantsFunc(Building node, bool includeSelf)
        {
            //return x => x.TreeId.StartsWith(node.TreeId + (includeSelf ? string.Empty : "-"));
            return x => x.TreeId.StartsWith(node.TreeId + "-") || (includeSelf && x.TreeId == node.TreeId);
            //Func<Dictionary, bool> func = x => (includeSelf || node.Id != x.Id) && node.FirstValue == x.FirstValue
            //    && (!node.SecondValue.HasValue || (x.SecondValue == node.SecondValue
            //    && (!node.ThirdValue.HasValue || (x.ThirdValue == node.ThirdValue
            //    && (!node.FourthValue.HasValue || (x.FourthValue == node.FourthValue
            //    && (!node.FifthValue.HasValue || x.FifthValue == node.FifthValue)))))));
            //return func;
        }


        /// <summary>
        /// 所有前代
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static IEnumerable<Building> Ancestors(this Building node)
        {
            var ctx = new EmpContext();
            return ctx.Buildings.Where(AncestorsFunc(node));
        }

        private static Func<Building, bool> AncestorsFunc(Building node)
        {
            var segs = node.TreeId.Split('-');
            Expression<Func<Building, bool>> exp = x => false;
            var reducer = "";
            if (segs.Length > 0)
            {
                for (int i = 0; i < segs.Length - 1; i++)
                {
                    reducer += i == 0 ? segs[i] : ("-" + segs[i]);
                    Regex regx = new Regex("^" + reducer + "$");
                    Expression<Func<Building, bool>> lambda = x => regx.IsMatch(x.TreeId);
                    exp = exp.Or(lambda);
                }
            }
            return exp.Compile();
            //Func<Dictionary, bool> func = x => node.Id != x.Id && node.FirstValue == x.FirstValue &&
            //            (!node.SecondValue.HasValue && !x.FirstValue.HasValue ||
            //            (!node.ThirdValue.HasValue && !x.SecondValue.HasValue || (!x.SecondValue.HasValue || node.SecondValue == x.SecondValue && (
            //            (!node.FourthValue.HasValue && !x.ThirdValue.HasValue || (!x.ThirdValue.HasValue || node.ThirdValue == x.ThirdValue && (
            //            (!node.FifthValue.HasValue && !x.FourthValue.HasValue || (!x.FourthValue.HasValue || node.FourthValue == x.FourthValue)))))))));
            //return func;
        }
        #endregion

    }
}
