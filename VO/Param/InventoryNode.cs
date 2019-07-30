using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO.Param
{
    /// <summary>
    /// 盘点|结算  参数
    /// </summary>
    public class InventoryNode
    {
        /// <summary>
        /// 结算对象类型
        /// </summary>
        [Required]
        public int TargetCategory { get; set; }

        /// <summary>
        /// 结算对象
        /// </summary>
        [Required]
        public int TargetId { get; set; }

        /// <summary>
        /// 结算开始日期
        /// </summary>
        [Required]
        public DateTime Start { get; set; }

        /// <summary>
        /// 结算结束日期
        /// </summary>
        [Required]
        public DateTime End { get; set; }
    }
}
