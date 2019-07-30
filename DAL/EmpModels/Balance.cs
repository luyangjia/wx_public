namespace WxPay2017.API.DAL.EmpModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Bill.Balance")]
    public partial class Balance
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Balance()
        {
            //Meters = new HashSet<Meter>();
            //Parameters = new HashSet<Parameter>();
            BalanceDetails = new HashSet<BalanceDetail>();
        }

        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// ����Id
        /// </summary>
        public int TargetId { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        public int TargetCategory { get; set; }

        /// <summary>
        /// �ܺ�����
        /// </summary>
        public int EnergyCategory { get; set; }

        /// <summary>
        /// �ܺĵ���
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// �ܺ�������
        /// </summary>
        public decimal EnergyConsumption { get; set; }

        /// <summary>
        /// �������
        /// </summary>
        public decimal Overplus { get; set; }

        /// <summary>
        /// Ԥ����
        /// </summary>
        public decimal Prepay { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        public decimal Subsidy { get; set; }

        /// <summary>
        /// ����\���ֽ��ֵ
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
        /// ���ڻ���
        /// </summary>
        public decimal BadDebt { get; set; }

        /// <summary>
        /// ����ʣ��
        /// </summary>
        public decimal Total { get; set; }
        public decimal? TotalSubsidy { get; set; }
        public decimal? TotalRecharge { get; set; }
        public decimal? TotalCashCharge { get; set; }

        /// <summary>
        /// ���ڽ�������
        /// </summary>
        public DateTime AuditDate { get; set; }

        /// <summary>
        /// �����ݴ�������
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// ������id
        /// </summary>
        [Required]
        [StringLength(128)]
        public string OperatorId { get; set; }

        //public virtual Dictionary BalanceCategory { get; set; }

        public virtual ICollection<BalanceDetail> BalanceDetails { get; set; }

    }
}
