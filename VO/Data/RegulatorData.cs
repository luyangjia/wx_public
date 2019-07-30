using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO
{
    /// <summary>
    /// 水电控数据
    /// </summary>
    public class RegulatorData
    {
        /// <summary>
        /// 对象id
        /// </summary>
        public int TargetId { get; set; }

        /// <summary>
        /// 对象类型
        /// </summary>
        public int TargetCategory { get; set; }


        /// <summary>
        /// 状态
        /// </summary>
        public DictionaryData Status { get; set; }

        /// <summary>
        /// 建筑信息
        /// </summary>
        public BuildingData Building { get; set; }

        /// <summary>
        /// 相关住户
        /// </summary>
        public ICollection<UserData> Users { get; set; }


        /// <summary>
        /// 对象缴费账号
        /// </summary>
        public UserData Account { get; set; }

        /// <summary>
        /// 设备信息
        /// </summary>
        public ICollection<MeterData> Meters { get; set; }

        /// <summary>
        /// 账户信息
        /// </summary>
        public BalanceData Balance { get; set; }

    }

}
