namespace WxPay2017.API.VO
{
    using WxPay2017.API.VO.Param;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BalanceShortData
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public decimal Total { get; set; }
        public int TargetId { get; set; }
        public int TargetCategory { get; set; }
    }
    /// <summary>
    /// 结算
    /// </summary>
    public partial class BalanceData
    { 
        public long Id { get; set; }

        /// <summary>
        /// 对象ID
        /// </summary>
        public int TargetId { get; set; }

        /// <summary>
        /// 对象类型
        /// </summary>
        public int TargetCategory { get; set; }

        /// <summary>
        /// 能耗类型， 水电合一则值为 无类型
        /// </summary>
        public int EnergyCategory { get; set; }

        /// <summary>
        /// 单价， 保留字段
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 能耗， 保留字段
        /// </summary>
        public decimal EnergyConsumption { get; set; }

        /// <summary>
        /// 上月剩余
        /// </summary>
        public decimal Overplus { get; set; }

        /// <summary>
        /// 预缴费
        /// </summary>
        public decimal Prepay { get; set; }

        /// <summary>
        /// 本月补贴
        /// </summary>
        public decimal Subsidy { get; set; }

        /// <summary>
        /// 本月充值
        /// </summary>
        public decimal Recharge { get; set; }

        /// <summary>
        /// 现金充值
        /// </summary>
        public decimal CashCharge { get; set; }

        /// <summary>
        /// 现金纠错
        /// </summary>
        public decimal CashCorrect { get; set; }

        /// <summary>
        /// 本月使用
        /// </summary>
        public decimal Usage { get; set; }

        /// <summary>
        /// 本月退还
        /// </summary>
        public decimal Refund { get; set; }

        /// <summary>
        /// 坏账
        /// </summary>
        public decimal BadDebt { get; set; }

        /// <summary>
        /// 本月结余
        /// </summary>
        public decimal Total { get; set; }

        /// <summary>
        /// 结算日期
        /// </summary>
        public DateTime AuditDate { get; set; }

        /// <summary>
        /// 建单日期
        /// </summary>
        public DateTime CreateDate { get; set; }
        public decimal? TotalSubsidy { get; set; }
        public decimal? TotalRecharge { get; set; }
        public decimal? TotalCashCharge { get; set; }
        
        /// <summary>
        /// 操作员
        /// </summary>
        public string OperatorId { get; set; }

        /// <summary>
        /// 充值人员，涉及非操作人员的人员
        /// </summary>
        public virtual UserData User { get; set; }

        /// <summary>
        /// 操作人员
        /// </summary>
        public virtual UserData Operator { get; set; }

        /// <summary>
        /// 对象
        /// </summary>
        public virtual dynamic Target { get; set; }

        /// <summary>
        /// 结算明细
        /// </summary>
        public virtual ICollection<BalanceDetailData> BalanceDetails { get; set; }

    }

    /// <summary>
    /// 能耗报表数据
    /// </summary>
    public class EnergyReportData
    {
        /// <summary>
        /// 建筑名
        /// </summary>
        public string BuildingName { get; set; }
        /// <summary>
        /// 楼层名
        /// </summary>
        public string FloorName { get; set; }
        /// <summary>
        /// 房间名
        /// </summary>
        public string RoomName { get; set; }
        /// <summary>
        /// 房间Id
        /// </summary>
        public int RoomId { get; set; }
        /// <summary>
        /// 用电量
        /// </summary>
        public decimal? PowerEnergyValue { get; set; }
        /// <summary>
        /// 用水量
        /// </summary>
        public decimal? WaterEnergyValue { get; set; }
    }

    /// <summary>
    /// 能耗报表结果集
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class IEnergyReportData<T>
    {
        /// <summary>
        /// 能耗报告数据
        /// </summary>
        public ICollection<T> EnergyReportDatas { get; set; }

        /// <summary>
        /// 分页参数
        /// </summary>
        public Pagination Pagination { get; set; }

        /// <summary>
        /// 用电总量
        /// </summary>
        public decimal? TotalPowerEnergyValue { get; set; }

        /// <summary>
        /// 用水总量
        /// </summary>
        public decimal? TotalWaterEnergyValue { get; set; }
    }

    /// <summary>
    /// 欠款信息
    /// </summary>
    public class CreditZeroData 
    {
        public string RoomName { get; set; }
        public int RoomId { get; set; }
        public string LevelName { get; set; }
        public string BuildingName { get; set; }
        public decimal Total { get; set; }
        public DateTime Time { get; set; }
    }

    /// <summary>
    /// 欠款信息集
    /// </summary>
    public class CreditZero 
    {
        /// <summary>
        /// 欠款集合
        /// </summary>
        public List<CreditZeroData> CreditZeroList { get; set; }
        /// <summary>
        /// 总欠款
        /// </summary>
        public decimal CreditZeroTotal { get; set; }
    }

}
