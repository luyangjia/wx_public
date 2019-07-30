namespace WxPay2017.API.VO
{
    using System;
     
    public partial class MomentaryValueData
    {
        public int Id { get; set; }

        public int MeterId { get; set; }

        public int ParameterId { get; set; }
         
        public string ParameterName { get; set; }

        public int ParameterType { get; set; }
        
        public string Unit { get; set; }

        public decimal Value { get; set; }

        public decimal OriginalValue { get; set; }

        public DateTime Time { get; set; }

        public virtual MeterData Meter { get; set; }

        public virtual ParameterData Parameter { get; set; }
           
    }
}
