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
    /// ����
    /// </summary>
    public partial class BalanceData
    { 
        public long Id { get; set; }

        /// <summary>
        /// ����ID
        /// </summary>
        public int TargetId { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        public int TargetCategory { get; set; }

        /// <summary>
        /// �ܺ����ͣ� ˮ���һ��ֵΪ ������
        /// </summary>
        public int EnergyCategory { get; set; }

        /// <summary>
        /// ���ۣ� �����ֶ�
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// �ܺģ� �����ֶ�
        /// </summary>
        public decimal EnergyConsumption { get; set; }

        /// <summary>
        /// ����ʣ��
        /// </summary>
        public decimal Overplus { get; set; }

        /// <summary>
        /// Ԥ�ɷ�
        /// </summary>
        public decimal Prepay { get; set; }

        /// <summary>
        /// ���²���
        /// </summary>
        public decimal Subsidy { get; set; }

        /// <summary>
        /// ���³�ֵ
        /// </summary>
        public decimal Recharge { get; set; }

        /// <summary>
        /// �ֽ��ֵ
        /// </summary>
        public decimal CashCharge { get; set; }

        /// <summary>
        /// �ֽ����
        /// </summary>
        public decimal CashCorrect { get; set; }

        /// <summary>
        /// ����ʹ��
        /// </summary>
        public decimal Usage { get; set; }

        /// <summary>
        /// �����˻�
        /// </summary>
        public decimal Refund { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        public decimal BadDebt { get; set; }

        /// <summary>
        /// ���½���
        /// </summary>
        public decimal Total { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        public DateTime AuditDate { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        public DateTime CreateDate { get; set; }
        public decimal? TotalSubsidy { get; set; }
        public decimal? TotalRecharge { get; set; }
        public decimal? TotalCashCharge { get; set; }
        
        /// <summary>
        /// ����Ա
        /// </summary>
        public string OperatorId { get; set; }

        /// <summary>
        /// ��ֵ��Ա���漰�ǲ�����Ա����Ա
        /// </summary>
        public virtual UserData User { get; set; }

        /// <summary>
        /// ������Ա
        /// </summary>
        public virtual UserData Operator { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        public virtual dynamic Target { get; set; }

        /// <summary>
        /// ������ϸ
        /// </summary>
        public virtual ICollection<BalanceDetailData> BalanceDetails { get; set; }

    }

    /// <summary>
    /// �ܺı�������
    /// </summary>
    public class EnergyReportData
    {
        /// <summary>
        /// ������
        /// </summary>
        public string BuildingName { get; set; }
        /// <summary>
        /// ¥����
        /// </summary>
        public string FloorName { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        public string RoomName { get; set; }
        /// <summary>
        /// ����Id
        /// </summary>
        public int RoomId { get; set; }
        /// <summary>
        /// �õ���
        /// </summary>
        public decimal? PowerEnergyValue { get; set; }
        /// <summary>
        /// ��ˮ��
        /// </summary>
        public decimal? WaterEnergyValue { get; set; }
    }

    /// <summary>
    /// �ܺı�������
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class IEnergyReportData<T>
    {
        /// <summary>
        /// �ܺı�������
        /// </summary>
        public ICollection<T> EnergyReportDatas { get; set; }

        /// <summary>
        /// ��ҳ����
        /// </summary>
        public Pagination Pagination { get; set; }

        /// <summary>
        /// �õ�����
        /// </summary>
        public decimal? TotalPowerEnergyValue { get; set; }

        /// <summary>
        /// ��ˮ����
        /// </summary>
        public decimal? TotalWaterEnergyValue { get; set; }
    }

    /// <summary>
    /// Ƿ����Ϣ
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
    /// Ƿ����Ϣ��
    /// </summary>
    public class CreditZero 
    {
        /// <summary>
        /// Ƿ���
        /// </summary>
        public List<CreditZeroData> CreditZeroList { get; set; }
        /// <summary>
        /// ��Ƿ��
        /// </summary>
        public decimal CreditZeroTotal { get; set; }
    }

}
