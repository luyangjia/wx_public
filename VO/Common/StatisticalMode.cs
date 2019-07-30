using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO.Common
{
    /// <summary>
    /// 统计类型
    /// </summary>
    public enum StatisticalModes
    {
        [Display(Name = "组织机构")]
        Organization = 1,
        [Display(Name = "建筑类型")]
        BuildingCategory = 2,
        [Display(Name = "建筑对象")]
        Building = 3,
        [Display(Name = "能耗类型")]
        EnergyCategory = 4,
        [Display(Name = "设备对象")]
        Meter = 5
    }
}
