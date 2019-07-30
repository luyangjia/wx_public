
namespace WxPay2017.API.DAL.EmpModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Meter.Group")]
    public partial class Group
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Group()
        {
            LogInfos = new HashSet<LogInfo>();
            SceneModeMeters = new HashSet<SceneModeMeter>();
            MeterGroups = new HashSet<MeterGroup>();
        }

        public int Id { get; set; }

        public int GroupTypeId { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(400)]
        public string Description { get; set; }

        public bool Enable { get; set; }

        [StringLength(128)]
        public string RegeneratorId { get; set; }

        [StringLength(256)]
        public string RegeneratorName { get; set; }

        public DateTime? UpdatingTime { get; set; }

        public virtual Dictionary GroupType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LogInfo> LogInfos { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SceneModeMeter> SceneModeMeters { get; set; }

        public virtual User Regenerator { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MeterGroup> MeterGroups { get; set; }
    }
}
