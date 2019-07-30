using WxPay2017.API.VO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO.Param
{
    /// <summary>
    /// 能耗结果检索条件
    /// </summary>
    public class StatisticalNode
    {
        private List<int> _targetId;
        private int? _energyCategoryId;
        private List<int> _parameterTypeId;
        private TimeUnits? _timeUnit;
        private DateTime? _startTime;
        private DateTime? _finishTime;
        private StatisticalModes? _mode;
        private StatisticalWay? _way;
        private StatisticalModes? _childrenMode;
        private bool _isTimeInterval = false;

       
        public StatisticalNode(List<int> targetId, int? energyCategoryId, List<int> parameterTypeId, TimeUnits? timeUnit,
            DateTime? startTime, DateTime? finishTime, StatisticalModes? mode, StatisticalWay? way,
            StatisticalModes? childrenMode, int? childrenCategoryId)
        {
            if (startTime.HasValue)
            {
                this._isTimeInterval = true;
            }
            this.TargetId = targetId;
            this.EnergyCategoryId = energyCategoryId;
            this.ParameterTypeId = parameterTypeId;
            this.TimeUnit = timeUnit;
            this.FinishTime = finishTime;
            this.StartTime = startTime;
            this.StatMode = mode;
            this.StatWay = way;
            this.ChildrenMode = childrenMode;
        }

        /// <summary>
        /// 统计对象
        /// </summary>
        public List<int> TargetId
        {
            get
            {
                if (this._targetId == null || this._targetId.Count == 0)
                {
                    //this._targetId = new List<int> { 1 }; // TODO: 临时支持调试，后续根据用户登录信息获取。
                    this._targetId = new List<int> {  }; // TODO: 由前端赋值，后台不作无值处理
                }
                return this._targetId;
            }
            set
            {
                this._targetId = value;
            }
        }

        /// <summary>
        /// 能耗类型
        /// </summary>
        public int? EnergyCategoryId
        {
            get
            {
                if (!this._energyCategoryId.HasValue)
                {
                    throw new ArgumentException("能耗类型编号（EnergyCategoryId）无效！");
                }
                return this._energyCategoryId;
            }
            set
            {
                this._energyCategoryId = value;
            }
        }

        public List<int> ParameterTypeId
        {
            get
            {
                if (this._parameterTypeId == null || this._parameterTypeId.Count == 0)
                {
                    throw new ArgumentException("能耗参数类型编号（ParameterId）无效！");
                }
                return this._parameterTypeId;
            }
            set
            {
                this._parameterTypeId = value;
            }
        }

        /// <summary>
        /// 时间单位
        /// </summary>
        public TimeUnits? TimeUnit
        {
            get
            {
                if (!this._timeUnit.HasValue)
                {
                    this._timeUnit = TimeUnits.H24;
                }
                return this._timeUnit;
            }
            set
            {
                this._timeUnit = value;
            }
        }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime
        {
            get
            {
                if (!this._startTime.HasValue)
                {
                    var isZeroTime = this.FinishTime.Value.TimeOfDay == TimeSpan.Zero; // 是否 0 点
                    var isFirstDay = this.FinishTime.Value.Day == 1; // 是否 1 号
                    var isFirstMonth = this.FinishTime.Value.Month == 1; // 是否 1 月

                    switch (this.TimeUnit)
                    {
                        case TimeUnits.Hourly:
                            this._startTime = isZeroTime ? this.FinishTime.Value.AddDays(-1) : new DateTime(this.FinishTime.Value.Year, this.FinishTime.Value.Month, this.FinishTime.Value.Day);
                            break;
                        case TimeUnits.H24:
                            this._startTime = Convert.ToDateTime(this.FinishTime.Value.AddHours(-24).ToString("yyyy-MM-dd HH:00:00"));
                            break;
                        case TimeUnits.H48:
                            this._startTime = Convert.ToDateTime(this.FinishTime.Value.AddHours(-48).ToString("yyyy-MM-dd HH:00:00"));
                            break;
                        case TimeUnits.H72:
                            this._startTime = Convert.ToDateTime(this.FinishTime.Value.AddHours(-72).ToString("yyyy-MM-dd HH:00:00"));
                            break;
                        case TimeUnits.Daily:
                        case TimeUnits.Daytime:
                        case TimeUnits.Nighttime:
                        case TimeUnits.Midnight:
                            this._startTime = isZeroTime && isFirstDay ? this.FinishTime.Value.AddMonths(-1) : new DateTime(this.FinishTime.Value.Year, this.FinishTime.Value.Month, 1);
                            break;
                        case TimeUnits.Monthly:
                            this._startTime = isZeroTime && isFirstDay && isFirstMonth ? this.FinishTime.Value.AddYears(-1) : new DateTime(this.FinishTime.Value.Year, 1, 1);
                            break;
                        case TimeUnits.Yearly:
                            this._startTime = new DateTime(this.FinishTime.Value.Year - 1, 1, 1);
                            break;
                        case TimeUnits.Quarterly:
                            if (this.FinishTime.Value.Month <= 4)
                            {
                                this._startTime = new DateTime(this.FinishTime.Value.Year, 1, 1);
                            }
                            else if (this.FinishTime.Value.Month <= 7)
                            {
                                this._startTime = new DateTime(this.FinishTime.Value.Year, 4, 1);
                            }
                            else if (this.FinishTime.Value.Month <= 10)
                            {
                                this._startTime = new DateTime(this.FinishTime.Value.Year, 7, 1);
                            }
                            else
                            {
                                this._startTime = new DateTime(this.FinishTime.Value.Year, 10, 1);
                            }
                            break;
                        default:
                            throw new System.IndexOutOfRangeException("时间单位（TimeUnit）不在指定范围内！");
                    }
                }
                else
                {
                    switch (this.TimeUnit)
                    {
                        case TimeUnits.Yearly:
                            return new DateTime(this._startTime.Value.Year, 1, 1);
                    }
                }
                return this._startTime;

            }
            set
            {
                this._startTime = value;
            }
        }

        public bool IsTimeInterval {
            get
            {
                return this._isTimeInterval;
            }
            set
            {
                this._isTimeInterval = value;
            }
        }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? FinishTime
        {
            get
            {
                if (!this._finishTime.HasValue)
                {
                    this._finishTime = DateTime.Now;
                }
                return this._finishTime;
            }
            set
            {
                var finishTime = value;
                if (this._isTimeInterval)
                {
                    this._finishTime = finishTime;
                    return;
                }
                var isZeroTime = finishTime.Value.TimeOfDay == TimeSpan.Zero; // 是否 0 点
                switch (this.TimeUnit)
                {
                    case TimeUnits.Hourly:
                        this._finishTime = isZeroTime ? finishTime : Convert.ToDateTime(finishTime.Value.ToString("yyyy-MM-dd 00:00:00")).AddDays(1);
                        break;
                    case TimeUnits.H24:
                    case TimeUnits.H48:
                    case TimeUnits.H72:
                        this._finishTime = finishTime;
                        break;
                    case TimeUnits.Daily:
                    case TimeUnits.Daytime:
                    case TimeUnits.Nighttime:
                    case TimeUnits.Midnight:
                        this._finishTime = isZeroTime ? finishTime : Convert.ToDateTime(finishTime.Value.ToString("yyyy-MM-dd 00:00:00")).AddDays(1);
                        break;
                    case TimeUnits.Monthly:
                        this._finishTime = Convert.ToDateTime(finishTime.Value.ToString("yyyy-MM-01 00:00:00")).AddMonths(1);
                        break;
                    case TimeUnits.Yearly:
                        this._finishTime = Convert.ToDateTime(finishTime.Value.AddYears(1).ToString("yyyy-01-01 00:00:00"));
                        break;
                    case TimeUnits.Quarterly:
                        if (finishTime.Value.Month < 4)
                        {
                            this._finishTime = Convert.ToDateTime(finishTime.Value.ToString("yyyy-03-01 00:00:00")).AddMonths(1);
                        }
                        else if (finishTime.Value.Month < 7)
                        {
                            this._finishTime = Convert.ToDateTime(finishTime.Value.ToString("yyyy-06-01 00:00:00")).AddMonths(1);
                        }
                        else if (finishTime.Value.Month < 10)
                        {
                            this._finishTime = Convert.ToDateTime(finishTime.Value.ToString("yyyy-09-01 00:00:00")).AddMonths(1);
                        }
                        else
                        {
                            this._finishTime = Convert.ToDateTime(finishTime.Value.ToString("yyyy-12-01 00:00:00")).AddMonths(1);
                        }
                        break;
                    default:
                        throw new System.IndexOutOfRangeException("时间单位（TimeUnit）不在指定范围内！");
                }
            }
        }

        /// <summary>
        /// 统计类型
        /// </summary>
        public StatisticalModes? StatMode
        {
            get
            {
                if (!this._mode.HasValue)
                {
                    this._mode = StatisticalModes.Organization;
                }
                return this._mode;
            }
            set
            {
                this._mode = value;
            }
        }

        public StatisticalWay? StatWay
        {
            get
            {
                if (!this._way.HasValue)
                {
                    this._way = StatisticalWay.Total;
                }
                return this._way;
            }
            set
            {
                this._way = value;
            }
        }

        /// <summary>
        /// 统计类型
        /// </summary>
        public StatisticalModes? ChildrenMode
        {
            get
            {
                return this._childrenMode;
            }
            set
            {
                this._childrenMode = value;
            }
        }
    }


}
