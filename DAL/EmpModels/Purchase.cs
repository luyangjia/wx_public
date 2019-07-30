namespace WxPay2017.API.DAL.EmpModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Repair.Purchase")]
    #region 0912
        //public partial class Purchase
    //{
    //    public int Id { get; set; }

    //    public int MaintenanceId { get; set; }

    //    [Required]
    //    [StringLength(128)]
    //    public string CurrentOperatorId { get; set; }

    //    [StringLength(128)]
    //    public string ApproverId { get; set; }

    //    [Required]
    //    [StringLength(256)]
    //    public string MaterialName { get; set; }

    //    public int MaterialNum { get; set; }

    //    public decimal MaterialPrice { get; set; }

    //    [StringLength(512)]
    //    public string Description { get; set; }

    //    public bool? IsAdopt { get; set; }

    //    public DateTime CreateDate { get; set; }

    //    public DateTime? ApplyDate { get; set; }

    //    public virtual Maintenance Maintenance { get; set; }

    //    public virtual User PurchaseCurrentOperator { get; set; }

    //    public virtual User PurchaseApprover { get; set; }
    //}
    #endregion

    public partial class Purchase
    {
        public int Id { get; set; }

        public int MaintenanceId { get; set; }

        [Required]
        [StringLength(128)]
        public string CurrentOperatorId { get; set; }

        [StringLength(128)]
        public string ApproverId { get; set; }

        [Required]
        [StringLength(256)]
        public string MaterialName { get; set; }

        public int MaterialNum { get; set; }

        public decimal MaterialPrice { get; set; }

        [StringLength(512)]
        public string Description { get; set; }

        public bool? IsAdopt { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime? ApplyDate { get; set; }

        public virtual Maintenance Maintenance { get; set; }

        public virtual User CurrentOperator { get; set; }

        public virtual User Approver { get; set; }

        public virtual Maintenance MaintenancePurchase { get; set; }
    }

}
