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
using System.Data.Entity.SqlServer;

namespace WxPay2017.API.BLL
{
    public class MeterResultBLL : Repository<IMeterResult>
    {

        public MeterResultBLL(EmpContext context = null)
            : base(context)
        { 
        }

        /// <summary>
        /// 统计能耗
        /// </summary>
        /// <param name="transfer">统计对象</param>
        /// <param name="timeUnit">时间单位</param>
        /// <param name="start">起始时间</param>
        /// <param name="finish">完成时间</param>
        /// <param name="parameterTypeId">统计参数</param>
        /// <param name="way">统计方式</param>
        /// <returns></returns>
        public async Task<List<StatisticalData>> Statistics(IList<StatisticalTransfer> transfer, TimeUnits timeUnit, DateTime start, DateTime finish, List<int> parameterTypeId, StatisticalWay way)
        {
            var modeInfo = this.GetStatisticalWayInfo(way);
            var parameterType = SystemInfo.AllDictStatus.Where(x => parameterTypeId.Contains(x.Id));

            // 根据统计方式生成统计函数
            Func<StatisticalTransfer, TimeUnits, DateTime, DateTime, int, Task<List<EnergyData>>> StatisticalFunc = null;
            switch (way)
            {
                case StatisticalWay.Total:
                    StatisticalFunc = this.SumFunc;
                    break;
                case StatisticalWay.Avg:
                    StatisticalFunc = this.AvgFunc;
                    break;
                case StatisticalWay.PerCapita:
                case StatisticalWay.PerUnitArea:
                    StatisticalFunc = this.PerFunc;
                    break;
                default:
                    goto case StatisticalWay.Total;
            }

            // 循环统计对象集合生成统计数据
            var result = new List<StatisticalData>();
            foreach (var item in transfer)
            {
                foreach (var pt in parameterType)
                {
                    var transferResult = await StatisticalFunc(item, timeUnit, start, finish, pt.Id);
                    if (transferResult != null && transferResult.Count > 0)
                    {
                        result.Add(new StatisticalData
                        {
                            StatisticalId = item.StatisticalId,
                            StatisticalParentId = item.StatisticalParentId,
                            StatisticalTreeId = item.StatisticalTreeId,
                            StatisticalName = item.StatisticalName,
                            StatisticalWay = item.StatisticalWay,
                            EnergyCategoryId = item.EnergyCategoryId,
                            StandardCoalCoefficient = DictionaryCache.Get()[item.EnergyCategoryId].EquValue,
                            EnergyCategoryName = item.EnergyCategoryName,
                            FormulaParam1 = item.FormulaParam1,
                            WayName = modeInfo.Key,
                            Formula = modeInfo.Value,
                            ParameterTypeId = pt.Id,
                            ParameterTypeName = pt.ChineseName,
                            Unit = pt.EquText,
                            TimeUnit = timeUnit,
                            Result = transferResult
                        });
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 合计统计函数
        /// </summary>
        /// <param name="transfer">统计对象</param>
        /// <param name="timeUnit">时间单位</param>
        /// <param name="start">起始时间</param>
        /// <param name="finish">完成时间</param>
        /// <param name="parameter">统计参数</param>
        /// <returns></returns>
        private async Task<List<EnergyData>> SumFunc(StatisticalTransfer transfer, TimeUnits timeUnit, DateTime start, DateTime finish, int parameterTypeId)
        {
            var result = from mr in this.GetMeterResult(transfer, timeUnit, start, finish, parameterTypeId)
                         group mr by new
                         {
                             mr.StartTime
                         } into g
                         orderby g.Key.StartTime
                         select new EnergyData
                         {
                             StartTime = g.Key.StartTime,
                             FinishTime = g.Max(r => r.FinishTime),
                             Value = g.Sum(r => r.Total * r.Meter.Rate)
                         };
            return await result.ToListAsync();
        }

        /// <summary>
        /// 平均统计函数
        /// </summary>
        /// <param name="transfer">统计对象</param>
        /// <param name="timeUnit">时间单位</param>
        /// <param name="start">起始时间</param>
        /// <param name="finish">完成时间</param>
        /// <param name="parameter">统计参数</param>
        /// <returns></returns>
        private async Task<List<EnergyData>> AvgFunc(StatisticalTransfer transfer, TimeUnits timeUnit, DateTime start, DateTime finish, int parameterTypeId)
        {
            var result = from mr in this.GetMeterResult(transfer, timeUnit, start, finish, parameterTypeId)
                         group mr by new
                         {
                             mr.StartTime
                         } into g
                         orderby g.Key.StartTime
                         select new EnergyData
                         {
                             StartTime = g.Key.StartTime,
                             FinishTime = g.Max(r => r.FinishTime),
                             Value = g.Sum(r => r.Total * r.Meter.Rate) / g.Count()
                         };
            return await result.ToListAsync();
        }

        /// <summary>
        /// 平均统计函数
        /// </summary>
        /// <param name="transfer">统计对象</param>
        /// <param name="timeUnit">时间单位</param>
        /// <param name="start">起始时间</param>
        /// <param name="finish">完成时间</param>
        /// <param name="parameter">统计参数</param>
        /// <returns></returns>
        private async Task<List<EnergyData>> PerFunc(StatisticalTransfer transfer, TimeUnits timeUnit, DateTime start, DateTime finish, int parameterTypeId)
        {
            var result = from mr in this.GetMeterResult(transfer, timeUnit, start, finish, parameterTypeId)
                         group mr by new
                         {
                             mr.StartTime
                         } into g
                         orderby g.Key.StartTime
                         select new EnergyData
                         {
                             StartTime = g.Key.StartTime,
                             FinishTime = g.Max(r => r.FinishTime),
                             Value = g.Sum(r => r.Total * r.Meter.Rate) / (transfer.FormulaParam1.HasValue && transfer.FormulaParam1.Value == 0 ? 1 : transfer.FormulaParam1)
                         };
            return await result.ToListAsync();
        }


        /// <summary>
        /// 获取统计对象关联的设备能耗
        /// </summary>
        /// <param name="transfer">统计对象</param>
        /// <param name="timeUnit">时间单位</param>
        /// <param name="start">起始时间</param>
        /// <param name="finish">完成时间</param>
        /// <param name="parameter">统计参数</param>
        /// <returns></returns>
        private IQueryable<IMeterResult> GetMeterResult(StatisticalTransfer transfer, TimeUnits timeUnit, DateTime start, DateTime finish, int parameterTypeId)
        {
            IQueryable<IMeterResult> meterResult = null;
            var ersid = SystemInfo.EffectiveResultStatusId.ToList();
            // 获取参数对象
            var parameters = db.Parameters.Where(p => p.TypeId == parameterTypeId).Select(p => p.Id);
            var meterId = transfer.Meters.Select(m => m.Id);
            var ids = meterId.ToList();
            // 根据时间单位获取设备能耗
            switch (timeUnit)
            {
                case TimeUnits.Hourly:
                case TimeUnits.H24:
                case TimeUnits.H48:
                case TimeUnits.H72:
                    meterResult = db.MeterHourlyResults.Where(mr => meterId.Contains(mr.MeterId) );
                    break;
                case TimeUnits.Daily:
                    meterResult = db.MeterDailyResults.Where(mr => meterId.Contains(mr.MeterId) );
                    break;
                case TimeUnits.Monthly:
                case TimeUnits.Quarterly:
                    meterResult = db.MeterMonthlyResults.Where(mr => meterId.Contains(mr.MeterId) );
                    break;
                case TimeUnits.Yearly:
                    meterResult = from mr in db.MeterMonthlyResults
                                  where meterId.Contains(mr.MeterId)&&! DictionaryCache.EnergyStatusIdNotUsed.Contains(mr.Status)
                                  select new MeterResult
                                  {
                                      Id = mr.Id,
                                      Meter = mr.Meter,
                                      MeterId = mr.MeterId,
                                      Parameter = mr.Parameter,
                                      ParameterId = mr.ParameterId,
                                      Status = mr.Status,
                                      StatusDict = mr.StatusDict,
                                      Unit = mr.Unit,
                                      StartTime = SqlFunctions.DateAdd("ms",
                                          -(
                                              (SqlFunctions.DatePart("hh", mr.StartTime) * 3600000) +
                                              (SqlFunctions.DatePart("n", mr.StartTime) * 60000) +
                                              (SqlFunctions.DatePart("s", mr.StartTime) * 1000) +
                                              SqlFunctions.DatePart("ms", mr.StartTime)
                                          ),
                                          SqlFunctions.DateAdd("dy", -(SqlFunctions.DatePart("dy", mr.StartTime) - 1), mr.StartTime).Value
                                      ).Value,
                                      FinishTime = mr.FinishTime,
                                      Total = mr.Total
                                  };
                    break;
                case TimeUnits.Daytime:
                    meterResult = from mr in db.MeterHourlyResults
                                  where meterId.Contains(mr.MeterId) 
                                  && (SqlFunctions.DatePart("hh", mr.StartTime) >= 7&& SqlFunctions.DatePart("hh", mr.StartTime) <= 19)
                                  select new MeterResult
                                  {
                                      Id = mr.Id,
                                      Meter = mr.Meter,
                                      MeterId = mr.MeterId,
                                      Parameter = mr.Parameter,
                                      ParameterId = mr.ParameterId,
                                      Status = mr.Status,
                                      StatusDict = mr.StatusDict,
                                      Unit = mr.Unit,
                                      StartTime = SqlFunctions.DateAdd("ms",
                                          -(
                                              (SqlFunctions.DatePart("hh", mr.StartTime) * 3600000) +
                                              (SqlFunctions.DatePart("n", mr.StartTime) * 60000) +
                                              (SqlFunctions.DatePart("s", mr.StartTime) * 1000) +
                                              SqlFunctions.DatePart("ms", mr.StartTime)
                                          ),
                                          mr.StartTime
                                      ).Value,
                                      FinishTime = mr.FinishTime,
                                      Total = mr.Total
                                  };
                    break;
                case TimeUnits.Nighttime:
                    meterResult = from mr in db.MeterHourlyResults
                                  where meterId.Contains(mr.MeterId)
                                  && (SqlFunctions.DatePart("hh", mr.StartTime) <= 7 && SqlFunctions.DatePart("hh", mr.StartTime) >= 19)
                                  select new MeterResult
                                  {
                                      Id = mr.Id,
                                      Meter = mr.Meter,
                                      MeterId = mr.MeterId,
                                      Parameter = mr.Parameter,
                                      ParameterId = mr.ParameterId,
                                      Status = mr.Status,
                                      StatusDict = mr.StatusDict,
                                      Unit = mr.Unit,
                                      StartTime = SqlFunctions.DateAdd("ms",
                                          -(
                                              (SqlFunctions.DatePart("hh", mr.StartTime) * 3600000) +
                                              (SqlFunctions.DatePart("n", mr.StartTime) * 60000) +
                                              (SqlFunctions.DatePart("s", mr.StartTime) * 1000) +
                                              SqlFunctions.DatePart("ms", mr.StartTime)
                                          ),
                                          mr.StartTime
                                      ).Value,
                                      FinishTime = mr.FinishTime,
                                      Total = mr.Total
                                  };
                    break;
                case TimeUnits.Midnight:
                    meterResult = from mr in db.MeterHourlyResults
                                  where meterId.Contains(mr.MeterId)
                                  && SqlFunctions.DatePart("hh", mr.StartTime) <= 6
                                  select new MeterResult
                                  {
                                      Id = mr.Id,
                                      Meter = mr.Meter,
                                      MeterId = mr.MeterId,
                                      Parameter = mr.Parameter,
                                      ParameterId = mr.ParameterId,
                                      Status = mr.Status,
                                      StatusDict = mr.StatusDict,
                                      Unit = mr.Unit,
                                      StartTime = SqlFunctions.DateAdd("ms",
                                          -(
                                              (SqlFunctions.DatePart("hh", mr.StartTime) * 3600000) +
                                              (SqlFunctions.DatePart("n", mr.StartTime) * 60000) +
                                              (SqlFunctions.DatePart("s", mr.StartTime) * 1000) +
                                              SqlFunctions.DatePart("ms", mr.StartTime)
                                          ),
                                          mr.StartTime
                                      ).Value,
                                      FinishTime = mr.FinishTime,
                                      Total = mr.Total
                                  };
                    break;
                default:
                    throw new System.IndexOutOfRangeException("时间单位（TimeUnit）不在指定范围内！");
            }

            // 根据统计条件过滤设备能耗
            var result = from r in meterResult
                         where r.StartTime >= start
                         && r.FinishTime <= finish
                         && ersid.Contains(r.Status)
                         //&& parameters.Contains(r.ParameterId)！！！！！！！！！！！！！！只会有一个值
                         select r;
            return result;
        }

        /// <summary>
        /// 获取统计方式的说明信息
        /// </summary>
        /// <param name="way">统计方式</param>
        /// <returns></returns>
        private KeyValuePair<string, string> GetStatisticalWayInfo(StatisticalWay way)
        {
            KeyValuePair<string, string> result = new KeyValuePair<string, string>();
            switch (way)
            {
                case StatisticalWay.Total:
                    result = new KeyValuePair<string, string>("总量合计", "∑(设备累计能耗×设备变比)");
                    break;
                case StatisticalWay.PerCapita:
                    result = new KeyValuePair<string, string>("人均", "∑((设备累计能耗×设备变比)÷机构或建筑的总人数)");
                    break;
                case StatisticalWay.PerUnitArea:
                    result = new KeyValuePair<string, string>("单位面积", "∑((设备累计能耗×设备变比)÷建筑或机构关联建筑的总面积)");
                    break;
                default:
                    goto case StatisticalWay.Total;
            }
            return result;
        }

    }
}
