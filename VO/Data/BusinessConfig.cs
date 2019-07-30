using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO.Data
{
    public class BusinessConfig
    {
        /// <summary>
        /// 收费类型：
        /// 水90031，电90001，租金90074，物业管理90075，停车90077，公摊90078
        /// </summary>
        public int EnergyCategoryId { get; set; }
        /// <summary>
        /// 用户类型
        /// 目前可取：商场：20002，住宅：20027，后续可以增加细分类，进行分类定价
        /// </summary>
        public int BuildingCategoryId { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 单位面积:260007,总额:260008,每单位用能：260001
        /// </summary>
        public int WayId { get; set; }
        /// <summary>
        /// 扣费时间，按月扣费，只取日期字段
        /// 每日0时进行结算前一天的数据，对前一周期未扣费的项目扣费
        /// </summary>
        public DateTime DeductionTime { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public decimal Value { get; set; }
    }
}
