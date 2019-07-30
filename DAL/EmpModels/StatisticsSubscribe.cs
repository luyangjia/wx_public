namespace WxPay2017.API.DAL.EmpModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Message.StatisticsSubscribe")]
    public partial class StatisticsSubscribe
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public StatisticsSubscribe()
        {
            StatisticsSubscribeHistory = new HashSet<StatisticsSubscribeHistory>();
        }

        public int Id { get; set; }

        public int CategoryDictionary { get; set; }

        public int TargetId { get; set; }

        public int EnergyCategoryId { get; set; }

        [Required]
        [StringLength(1000)]
        public string Emails { get; set; }

        public DateTime? SubDate { get; set; }

        //public bool? IsPeriodMonth { get; set; }

        public string Period { get; set; }

        public DateTime From { get; set; }

        public DateTime? To { get; set; }



        public virtual Dictionary Dictionary { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StatisticsSubscribeHistory> StatisticsSubscribeHistory { get; set; }


    }
}
