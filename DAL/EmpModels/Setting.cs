namespace WxPay2017.API.DAL.EmpModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Web.Setting")]
    public partial class Setting
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Setting()
        { 
        }

        [Key]
        public Guid AppID { get; set; }

        [DataType(dataType:DataType.Text)]
        public string Data { get; set; }
         
        [StringLength(255)]
        public string Title { get; set; }

        [DataType(dataType: DataType.Text)]
        public string Weather { get; set; }

        public decimal ElectricityPrice { get; set; }

        public decimal WaterPrice { get; set; }
        [StringLength(50)]
        public string ProjectType { get; set; }
        [StringLength(50)]
        public string IPAddress { get; set; }
        public decimal? ElectricityPrePay { get; set; }
        public decimal? WaterPrePay { get; set; }

        public string AdministrativeCode { get; set; }
        /// <summary>
        /// 启用策略类型 90001,90031,90006,90002
        /// </summary>
        public string ConfigIds { get; set; }
        /// <summary>
        /// 学号正则表达式
        /// </summary>
        public string Regex { get; set; }

        public decimal? HeatPrice { get; set; }
        /// <summary>
        /// 第一学期开始时间
        /// </summary>
        public DateTime? FirstSemesterBeginTime { get; set; }
        /// <summary>
        /// 第一学期结束时间
        /// </summary>
        public DateTime? FirstSemesterEndTime { get; set; }
        /// <summary>
        /// 第二学期开始时间
        /// </summary>
        public DateTime? SecondSemesterBeginTime { get; set; }
        /// <summary>
        /// 第二学期结束时间
        /// </summary>
        public DateTime? SecondSemesterEndTime { get; set; }
        /// <summary>
        /// 系统上线开始计费时间
        /// </summary>
        public DateTime? BillStartTime { get; set; }
        /// <summary>
        /// 按月补贴不发放补贴月份
        /// </summary>
        [StringLength(50)]
        public string  NotGiveSubsidieMonth{ get;set; }
        /// <summary>
        /// 支付宝移动端回调确认缴费地址
        /// </summary>
        [StringLength(100)]
        public string  AliPayAddress{ get;set; }
        /// <summary>
        /// 极光推送APPKey
        /// </summary>
        [StringLength(50)]
        public string  JpushAppkey{ get;set; }
        /// <summary>
        /// 极光推送秘钥
        /// </summary>
        [StringLength(50)]
        public string  JpushSecret{ get;set; }
        /// <summary>
        /// 阿里大鱼短信发送者名称
        /// </summary>
        [StringLength(50)]
        public string  SMSSenderName{ get;set; }
        /// <summary>
        /// 阿里大鱼APPKey
        /// </summary>
        [StringLength(50)]
        public string  SMSAppkey{ get;set; }
        /// <summary>
        /// 阿里大鱼秘钥
        /// </summary>
        [StringLength(50)]
        public string  SMSSecret{ get;set; }
        /// <summary>
        /// 阿里短信模板
        /// </summary>
        [StringLength(50)]
        public string  SMSTemplateId{ get;set; }
        /// <summary>
        /// 邮件服务器地址
        /// </summary>
        [StringLength(50)]
        public string  SmtpServer{ get;set; }
        /// <summary>
        /// 邮件服务器端口
        /// </summary>
        public int? SmtpPort { get; set; }
        /// <summary>
        /// 系统邮件发送用户名
        /// </summary>
        [StringLength(50)]
        public string  SmtpUser{ get;set; }
         /// <summary>
         /// 系统邮件发送者密码
         /// </summary>
         [StringLength(50)]
         public string  SmtpPwd{ get;set; }
         /// <summary>
         /// 邮件SSL加密是否启用,暂不设置，默认true
         /// </summary>
         public bool? SmtpEnableSsl { get; set; }
         /// <summary>
         /// 邮件密码检查是否启用,暂不设置，默认true
         /// </summary>
         public bool? SmtpPwdCheckEnable { get; set; }

         public decimal? WarningAmount { get; set; }
         public decimal? BalancedetectionMaxdifference { get; set; }
         public bool? IsNeedPwdForUpdate { get; set; }
         [StringLength(50)]
         public string UpdatePwd { get; set; }
         public decimal? CampusPowerLimit { get; set; }
         public decimal? BuidlingPowerLimit { get; set; }
         public DateTime? StopChargingTime { get; set; }

    }
}
