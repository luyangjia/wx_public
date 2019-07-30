using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO
{
    /// <summary>
    /// 设备参数
    /// </summary>
    public class ParamMeta
    {
        /// <summary>
        /// 属性ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 设备属性名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 数值
        /// </summary>
        public decimal? Value { get; set; }

        /// <summary>
        /// 计量单位, 预留
        /// </summary>
        public string Units { get; set; }
    }
}
