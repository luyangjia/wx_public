namespace WxPay2017.API.DAL.EmpModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Foundation.Attachment")]
    public partial class Attachment
    {
        public Attachment()
        {
            Maintenances = new HashSet<Maintenance>();

        }
        public int Id { get; set; }

        [StringLength(128)]
        public string TargetId { get; set; }

        public int AttachmentTypeId { get; set; }

        public int AttachmentFormatId { get; set; }

        [StringLength(80)]
        public string Description { get; set; }

        public int Size { get; set; }

        public DateTime CreateTime { get; set; }

        [Required]
        [StringLength(200)]
        public string Path { get; set; }

        [StringLength(100)]
        public string OriginalName { get; set; }

        [StringLength(100)]
        public string LogicalName { get; set; }
        [Display(Name = "附件分类")]
        public virtual EmpModels.Dictionary AttachmentType { get; set; }
        [Display(Name = "附件格式")]
        public virtual EmpModels.Dictionary AttachmentFormat { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Maintenance> Maintenances { get; set; }
    }
}
