using WxPay2017.API.DAL.EmpModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace WxPay2017.API.VO
{
    #region 0912
    //public partial class ActivityRecordData
    //{
    //    public int Id{ get; set; }
    //    public int TargetId{ get; set; }
    //    public int TargetTypeId{ get; set; }
    //    public string CurrentOperatorId{ get; set; }
    //    public int State{ get; set; }
    //    public string Description{ get; set; }
    //    public DateTime CreateDate{ get; set; }
    //    public string NextOperator{ get; set; }
    //    //public DictionaryData TargetType { get; set; }
    //    //public UserData CurrentOperator { get; set; }
    //    //public DictionaryData ActivityState { get; set; }


    //}
    #endregion

    public partial class ActivityRecordData
    {

        public int Id { get; set; }

        public int TargetId { get; set; }

        public string PublisherId { get; set; }

        public int StateId { get; set; }

        public string Description { get; set; }

        public DateTime CreateDate { get; set; }

        public MaintenanceData Target { get; set; }

        public UserData Publisher { get; set; }

        public string StateName { get; set; }

    }

    public class Estimate {
        public int TargetId { get; set; }
        public string PublisherId { get; set; }
        public string Description {get;set;}
        public int Rating { get;set;}

    }

}
