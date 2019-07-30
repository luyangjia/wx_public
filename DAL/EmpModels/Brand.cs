namespace WxPay2017.API.DAL.EmpModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Meter.Brand")]
    public partial class Brand
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Brand()
        {
            Meters = new HashSet<Meter>();
            Parameters = new HashSet<Parameter>();
            RatedParameters = new HashSet<RatedParameter>();
            RatedParameterDetails = new HashSet<RatedParameterDetail>();
        }

        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(128)]
        public string Name { get; set; }

        public int MeterType { get; set; }

        [StringLength(128)]
        public string Producer { get; set; }

        [StringLength(128)]
        public string Model { get; set; }

        [StringLength(256)]
        public string Description { get; set; }

        public bool IsControllable { get; set; }
        public bool IsFJNewcapSystem { get; set; }

        public virtual Dictionary TypeDict { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [Display(Name = "…Ë±∏")]
        public virtual ICollection<Meter> Meters { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [Display(Name = "∂¡≤Œ")]
        public virtual ICollection<Parameter> Parameters { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RatedParameter> RatedParameters { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RatedParameterDetail> RatedParameterDetails { get; set; }
    }
}
