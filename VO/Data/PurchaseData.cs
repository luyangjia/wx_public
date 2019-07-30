using WxPay2017.API.DAL.EmpModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;


namespace WxPay2017.API.VO
{
    public partial class PurchaseData
    {
        public int Id { get; set; }
        public int MaintenanceId{ get; set; }
        public string CurrentOperatorId{ get; set; }
        public string ApproverId{ get; set; }
        public string MaterialName{ get; set; }
        public int MaterialNum{ get; set; }
        public decimal MaterialPrice{ get; set; }
        public string Description{ get; set; }
        public bool? IsAdopt{ get; set; }
        public DateTime CreateDate{ get; set; }
        public DateTime? ApplyDate{ get; set; }
        public UserData Approver { get; set; }
        public UserData CurrentOperator { get; set; }
        //public MaintenanceData Maintenance { get; set; }
    }
}
