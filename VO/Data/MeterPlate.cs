namespace WxPay2017.API.VO
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Runtime.Serialization;

    public partial class MeterPlate
    {
        /// <summary>
        /// ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// �豸����
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// ��ѹ
        /// </summary>
        public decimal? Voltage { get; set; }

        //public decimal? 
    }
}
