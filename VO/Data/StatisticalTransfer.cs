namespace WxPay2017.API.VO
{
    using WxPay2017.API.DAL.EmpModels;
    using WxPay2017.API.VO.Common;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Data;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    public class StatisticalTransfer
    {
        public int StatisticalId { get; set; }
        public int? StatisticalParentId { get; set; }
        public string StatisticalTreeId { get; set; }
        public string StatisticalName { get; set; }
        public StatisticalWay StatisticalWay { get; set; }
        public int EnergyCategoryId { get; set; }
        public string EnergyCategoryName { get; set; }
        public decimal? FormulaParam1 { get; set; }
        public IEnumerable<Meter> meters;
        public IEnumerable<Meter> Meters
        {
            get
            {
                //if ((meters == null && meterDatas != null) || ((meters == null || meters.Count() == 0) && (meterDatas != null && meterDatas.Count() != 0)))
                //    return meterDatas.Select(m => new Meter { Name = m.Name, Id = m.Id, ParentId = m.ParentId, TreeId = m.TreeId });
                //else
                return meters;
            }
            set
            {
                meters = value;

            }
        }
        private IEnumerable<MeterData> meterDatas;
        public IEnumerable<MeterData> MeterDatas
        {
            get
            {
                return meterDatas;
            }
            set
            {
                meterDatas = value;
                if (meterDatas.Count() > 0)
                    Meters = meterDatas.Select(m => new Meter { Name = m.Name, Id = m.Id, ParentId = m.ParentId, TreeId = m.TreeId });
                else
                    Meters = new List<Meter>();
            }
        }

    }
}
