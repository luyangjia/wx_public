namespace WxPay2017.API.DAL.EmpModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Meter.PreProcessAlarmData")]
    public partial class PreProcessAlarmData
    {

        public PreProcessAlarmData()
        {
           
        }

        [Key]
        public Int64 Id { get; set; }
        public int BuildingId { get; set; }
       
        public int MeterId { get; set; }

        public DateTime? CollectTime { get; set; }
        public decimal Value { get; set; }
        
    }
}
