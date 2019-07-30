using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO.Data
{
    public class DanlysisData
    {
        public object task { get; set; }
        public List<DateTime> times = new List<DateTime>();
        public Dictionary<String, decimal?> delta = new Dictionary<String, decimal?>();
        public Dictionary<String, decimal?> Maximum = new Dictionary<String, decimal?>();
        public Dictionary<String, decimal?> Minimum = new Dictionary<String, decimal?>();
        public ListChildrenData top =new ListChildrenData();
        public ListData children = new ListData();
        public ListData factor = new ListData();
        public WxPay2017.API.VO.Param.Pagination pagination =new WxPay2017.API.VO.Param.Pagination();
    }
    public class DicData
    {
        public DateTime Key { get; set; }
        public decimal? Sum { get; set; }
    }
    public class ListData
    {
        public List<ListChildrenData> list = new List<ListChildrenData>();
        public Dictionary<String, decimal?> group = new Dictionary<String, decimal?>();
        public List<decimal?> values=new List<decimal?>();
        public List<DateTime> times = new List<DateTime>();
        public Dictionary<String, decimal?> Maximum = new Dictionary<String, decimal?>();
        public Dictionary<String, decimal?> Minimum = new Dictionary<String, decimal?>();
        public decimal total;
    }
    public class ListChildrenData
    {
        public Dictionary<String, decimal?> group = new Dictionary<String, decimal?>();
        public Dictionary<String, decimal?> Maximum = new Dictionary<String, decimal?>();
        public Dictionary<String, decimal?> Minimum = new Dictionary<String, decimal?>();
        public List<decimal?> values = new List<decimal?>();
        public List<DateTime> times = new List<DateTime>();
        public decimal? total;
        public int id;
        public string name;
        public string unit;
        public IEnumerable<EnergyData> data=new List<EnergyData>();

    }
}
