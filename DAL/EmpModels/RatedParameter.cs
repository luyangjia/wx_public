
namespace WxPay2017.API.DAL.EmpModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Meter.RatedParameter")]
    public partial class RatedParameter
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RatedParameter()
        {
            RatedParameterDetail = new HashSet<RatedParameterDetail>();
        }

        public int Id { get; set; }

        public int? RatedParameterTypeId { get; set; }

        public int? BrandId { get; set; }

        public int? ParameterId { get; set; }

        [Required]
        [StringLength(128)]
        public string Description { get; set; }

        public decimal? MinValue { get; set; }

        public decimal? MaxValue { get; set; }

        [StringLength(2048)]
        public string Code { get; set; }

        public DateTime SettingTime { get; set; }

        public decimal? PPF { get; set; }

        public decimal? RPF { get; set; }
        public decimal? PPFMax { get; set; }
        public decimal? PPFMin { get; set; }
        public decimal? RPFMax { get; set; }
        public decimal? RPFMin { get; set; }
        public virtual Dictionary Dictionary { get; set; }

        public virtual Brand Brand { get; set; }

        public virtual Parameter Parameter { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RatedParameterDetail> RatedParameterDetail { get; set; }


    }
}
