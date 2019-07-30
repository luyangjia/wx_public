using WxPay2017.API.VO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO.Param
{
    /// <summary>
    /// 水电控操作参数
    /// </summary>
    public class RegulatorOperateParam
    {
        /// <summary>
        /// 被操作对象id， 一般是建筑
        /// </summary>
        public int TargetId { get; set; }

        /// <summary>
        /// 被操作对象类型， 一般是建筑
        /// </summary>
        public CategoryDictionary TargetCategory { get; set; }

        /// <summary>
        /// 操作类型
        /// 即：要进行什么操作
        /// 注： 值为dictionary表中的Operation的id
        /// </summary>
        public int Operation { get; set; }

        /// <summary>
        /// 如果是充值， 此处填 缴费人
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 能耗类型， 注：水电账号合一， 则选择无类型
        /// </summary>
        public int EnergyCategory { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal Value { get; set; }

        /// <summary>
        /// 描述， optional
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 操作员
        /// </summary>
        public string Operator { get; set; }

        /// <summary>
        /// 结算日期
        /// </summary>
        public DateTime? SettleDate { get; set; }
        /// <summary>
        /// 纠错使用，修改对象的id
        /// </summary>
        public int? OriginalId { get; set; }
    }
}
