namespace WxPay2017.API.DAL.EmpModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Meter.OriginalData")]
    public partial class OriginalData
    {
        public Guid Id { get; set; }

        public int MeterId { get; set; }

        public DateTime CollectTime { get; set; }

        public DateTime ReceiveTime { get; set; }

        public DateTime? CalcTime { get; set; }

        public int MeterTypeFirstValue { get; set; }

        [Required]
        [StringLength(16)]
        public string Status { get; set; }

        public decimal? Parameter01 { get; set; }

        public decimal? Parameter02 { get; set; }

        public decimal? Parameter03 { get; set; }

        public decimal? Parameter04 { get; set; }

        public decimal? Parameter05 { get; set; }

        public decimal? Parameter06 { get; set; }

        public decimal? Parameter07 { get; set; }

        public decimal? Parameter08 { get; set; }

        public decimal? Parameter09 { get; set; }

        public decimal? Parameter10 { get; set; }

        public decimal? Parameter11 { get; set; }

        public decimal? Parameter12 { get; set; }

        public decimal? Parameter13 { get; set; }

        public decimal? Parameter14 { get; set; }

        public decimal? Parameter15 { get; set; }

        public decimal? Parameter16 { get; set; }

        public decimal? Parameter17 { get; set; }

        public decimal? Parameter18 { get; set; }

        public decimal? Parameter19 { get; set; }

        public decimal? Parameter20 { get; set; }

        public decimal? Parameter21 { get; set; }

        public decimal? Parameter22 { get; set; }

        public decimal? Parameter23 { get; set; }

        public decimal? Parameter24 { get; set; }

        public decimal? Parameter25 { get; set; }

        public decimal? Parameter26 { get; set; }

        public decimal? Parameter27 { get; set; }

        public decimal? Parameter28 { get; set; }

        public decimal? Parameter29 { get; set; }

        public decimal? Parameter30 { get; set; }

        public decimal? Parameter31 { get; set; }

        public decimal? Parameter32 { get; set; }
        public decimal? Parameter33 { get; set; }

        public decimal? Parameter34 { get; set; } 
        public decimal? Parameter35 { get; set; }

        public decimal? Parameter36 { get; set; }
        public virtual Meter Meter { get; set; }
    }

    /// <summary>
    /// �����ܱ�ߣ��ֶ������ϴ��Ĳ���
    /// </summary>
    public class UnintelligentMeterParam
    {
        private string _Mode;
        /// <summary>
        /// ����ģʽ Сʱ(hourly)����(daily)����(monthly)
        /// </summary>
        public string Mode { get { return _Mode.ToLower(); } set { _Mode = value; } }
        /// <summary>
        /// ��ʼʱ��
        /// </summary>
        public DateTime StartTime { get; set; }
        //public DateTime FinishTime { get; set; }
        /// <summary>
        /// ��߶���
        /// </summary>
        public decimal UseValue { get; set; }
        /// <summary>
        /// �豸id
        /// </summary>
        public int MeterId { get; set; }
        /// <summary>
        /// �豸��������id,���ֵ�parameter
        /// </summary>
        public int ParameterTypeId { get; set; }

    }
	
    /// <summary>
    /// ��ѯ����¼����ʷ��¼����(MeterId��StartTime��FinishTime)
    /// </summary>
    public class GetOriginalHistory
    {
        /// <summary>
        /// �豸id
        /// </summary>
        public int MeterId;

        /// <summary>
        /// ��ʼʱ��
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime FinishTime { get; set; }

        /// <summary>
        /// ����id,��Parameter��
        /// </summary>
        public string  ParameterIds { get; set; }

        /// <summary>
        /// ���������������½����
        /// </summary>
        public int Count { get; set; }
    }
}
