using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO
{
    /// <summary>
    /// 读值详情
    /// </summary>
    public class DiagramDetail
    {
        public string DiagramName { get; set; }
        public string DiagramValue { get; set; }
        public DiagramDetail(string diagramName, string diagramValue)
        {
            this.DiagramName = diagramName;
            this.DiagramValue = diagramValue;
        }
    }
}
