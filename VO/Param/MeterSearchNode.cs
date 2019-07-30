namespace WxPay2017.API.VO.Param
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Runtime.Serialization;
      
    public partial class MeterSearchNode
    {   
        
        public string ParentId { get; set; }
         
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        
        /// <summary>
        /// 设备类型
        /// </summary>
        public int? Type { get; set; }
         
        /// <summary>
        /// 变比
        /// </summary>
        public decimal Rate { get; set; }

        
        /// <summary>
        /// 状态
        /// </summary>
        public int State { get; set; }

        
        public int? Interval { get; set; }

         

         
        
        public decimal? Long { get; set; }

        
        public decimal? Lat { get; set; }
         
        public string Seat { get; set; }

        
        public bool IsAuxiliary { get; set; }

        
        public string Code { get; set; }

        public Pagination pagination { get; set; }
           

    }
}
