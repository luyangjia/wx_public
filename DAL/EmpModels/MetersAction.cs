namespace WxPay2017.API.DAL.EmpModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Meter.MetersAction")]
    public partial class MetersAction
    {
        public MetersAction()
        {
            Children = new HashSet<MetersAction>();
        }

        public int Id { get; set; }

        public int MeterId { get; set; }

        public bool IsOk { get; set; }

        public bool? IsPowerOffByMoney { get; set; }

        public bool? IsPowerOffByTime { get; set; }

        public int ActionId { get; set; }

        public DateTime AddTime { get; set; }

        public DateTime? ActionTime { get; set; }

        public DateTime? AnswerTIme { get; set; }

        [StringLength(128)]
        public string AnswerValue { get; set; }

        public int? SendTimes { get; set; }

        public int? Priority { get; set; }

        [StringLength(30)]
        public string GroupNum { get; set; }

        public int? ParentId { get; set; }

        [StringLength(2048)]
        public string Description { get; set; }

        public int? CommandStatus { get; set; }


        public virtual Meter Meter { get; set; }

        public virtual Dictionary Action { get; set; }

        public virtual MetersAction Parent { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [Display(Name = "子对象")]
        public virtual ICollection<MetersAction> Children { get; set; }
        /// <summary>
        /// 要配置的值
        /// </summary>
        public string SettingValue { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CommandQueue> CommandQueue { get; set; }

        public virtual Dictionary CommandStatusDic { get; set; }

    }
}
