namespace WxPay2017.API.DAL.EmpModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Foundation.ExtensionField")]
    public partial class ExtensionField
    {
        [Key]
        public int Id { get; set; }

        [StringLength(128)]
        public string Database { get; set; }

        //[Key]
        //[Column(Order = 0)]
        public string Schema { get; set; }

        //[Key]
        //[Column(Order = 1)]
        public string Table { get; set; }

        //[Key]
        //[Column(Order = 2)]
        public string Column { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? FinishTime { get; set; }

        public int JoinId { get; set; }

        //[Key]
        //[Column(Order = 3)]
        [StringLength(256)]
        public string Value { get; set; }

        [StringLength(64)]
        public string ValueType { get; set; }

        //[Key]
        //[Column(Order = 4)]
        public string ChineseName { get; set; }

        [StringLength(128)]
        public string EnglishName { get; set; }

        //[Key]
        //[Column(Order = 5)]
        public bool Enable { get; set; }

        [StringLength(256)]
        public string Description { get; set; }

        //public virtual Organization Organization { get; set; }
    }
}
