namespace WxPay2017.API.DAL.EmpModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Repair.Maintenance")]

    #region 0912
    //public partial class Maintenance
    //{
    //    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    //    public Maintenance()
    //    {
    //        Purchase = new HashSet<Purchase>();
    //    }

    //    public int Id { get; set; }

    //    public int MaintenanceCategoryId { get; set; }

    //    public int StateId { get; set; }

    //    [Required]
    //    [StringLength(128)]
    //    public string UserId { get; set; }

    //    [Required]
    //    [StringLength(256)]
    //    public string Title { get; set; }

    //    [Required]
    //    [StringLength(512)]
    //    public string Content { get; set; }

    //    public int? Picture { get; set; }

    //    public int ObjectCategoryId { get; set; }

    //    [StringLength(128)]
    //    public string OperateObjectId { get; set; }

    //    public DateTime CreateDate { get; set; }

    //    [StringLength(512)]
    //    public string Comment { get; set; }

    //    public int? Rating { get; set; }

    //    [Required]
    //    [StringLength(512)]
    //    public string OperatorDiscription { get; set; }

    //    public virtual Dictionary MaintenanceCategory { get; set; }

    //    public virtual Dictionary MaintenanceState { get; set; }

    //    public virtual Dictionary MaintenanceObjectCategory { get; set; }

    //    public virtual User User { get; set; }

    //    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
    //    public virtual ICollection<Purchase> Purchase { get; set; }
    //}
    #endregion

    public partial class Maintenance
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Maintenance()
        {
            ActivityRecords = new HashSet<ActivityRecord>();
            Purchases = new HashSet<Purchase>();
        }

        public int Id { get; set; }

        public int MaintenanceCategoryId { get; set; }

        public int StateId { get; set; }

        [Required]
        [StringLength(128)]
        public string UserId { get; set; }

        public int BuildingId { get; set; }

        [StringLength(128)]
        public string ApproverId { get; set; }

        [StringLength(128)]
        public string OperatorId { get; set; }

        [Required]
        [StringLength(256)]
        public string Title { get; set; }

        [Required]
        [StringLength(512)]
        public string Content { get; set; }

        public int? PictureId { get; set; }

        public DateTime CreateDate { get; set; }

        public int? Rating { get; set; }

        public DateTime? MaintenanceTime { get; set; }

        public int? PurchasingId { get; set; }

        public virtual Attachment Picture { get; set; }

        public virtual Dictionary MaintenanceCategory { get; set; }

        public virtual Dictionary State { get; set; }

        public string PictureUrl { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ActivityRecord> ActivityRecords { get; set; }

        public virtual User User { get; set; }

        public virtual User Approver { get; set; }

        public virtual User Operator { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Purchase> Purchases { get; set; }

        public virtual Building Building { get; set; }

        public virtual ICollection<Purchase> PurchaseId { get; set; }
    }
    
}
