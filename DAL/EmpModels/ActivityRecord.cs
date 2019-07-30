namespace WxPay2017.API.DAL.EmpModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Repair.ActivityRecord")]
    #region 9.12
    
    
    //public partial class ActivityRecord
    //{
    //    public int Id { get; set; }

    //    public int TargetId { get; set; }

    //    public int TargetTypeId { get; set; }

    //    [Required]
    //    [StringLength(128)]
    //    public string CurrentOperatorId { get; set; }

    //    public int StateId { get; set; }

    //    [Required]
    //    [StringLength(512)]
    //    public string Description { get; set; }

    //    public DateTime CreateDate { get; set; }

    //    [Required]
    //    [StringLength(128)]
    //    public string NextOperator { get; set; }

    //    public virtual Dictionary TargetType { get; set; }

    //    public virtual Dictionary ActivityState { get; set; }

    //    public virtual User CurrentOperator { get; set; }
    //}

    #endregion

    public partial class ActivityRecord
    {
        public int Id { get; set; }

        public int TargetId { get; set; }

        [Required]
        [StringLength(128)]
        public string PublisherId { get; set; }

        public int StateId { get; set; }

        [Required]
        [StringLength(512)]
        public string Description { get; set; }

        public DateTime CreateDate { get; set; }

        public virtual Dictionary State { get; set; }

        public virtual User Publisher { get; set; }

        public virtual Maintenance MaintenanceTarget { get; set; }
    }

}