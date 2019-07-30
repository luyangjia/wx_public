
namespace WxPay2017.API.DAL.EmpModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Meter.RatedParameterDetail")]
    public partial class RatedParameterDetail
    {
        public int Id { get; set; }

        public int? RatedParameterId { get; set; }

        public int? BrandId { get; set; }

        public int? MeterId { get; set; }

        public DateTime SettingTime { get; set; }

        public bool? IsSuccess { get; set; }

        [StringLength(128)]
        public string OperatorId { get; set; }

        [StringLength(256)]
        public string OperatorName { get; set; }

        public virtual Brand Brand { get; set; }

        public virtual Meter Meter { get; set; }

        public virtual RatedParameter RatedParameter { get; set; }

        public virtual User Operator { get; set; }
    }
}
