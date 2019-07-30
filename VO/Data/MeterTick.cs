namespace WxPay2017.API.VO
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Runtime.Serialization;

    /// <summary>
    /// 设备实时数据
    /// </summary>
    public partial class MeterTick
    {
        public MeterTick()
        {
            Parameters = new HashSet<ParamMeta>();
        }
        /// <summary>
        /// ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
         

        public virtual ICollection<ParamMeta> Parameters { get; set; }
    }
}
