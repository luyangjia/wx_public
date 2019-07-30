using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO.Common
{
    [Flags]
    public enum CategoryDictionary : long
    {
        /// <summary>
        /// 0: 空
        /// </summary>
        [Display(Name = "空")]
        None = 0x00000000,

        /// <summary>
        /// 1: 建筑
        /// </summary>
        [Display(Name = "建筑")]
        Building = 0x00000001,

        /// <summary>
        /// 2: 设备
        /// </summary>
        [Display(Name = "设备")]
        Meter = 0x00000002,

        /// <summary>
        /// 4: 机构
        /// </summary>
        [Display(Name = "机构")]
        Organization = 0x00000004,

        /// <summary>
        /// 8: 能耗类型
        /// </summary>
        [Display(Name = "能耗类型")]
        EnergyCategory = 0x00000008,

        /// <summary>
        /// 16: 建筑导则分类
        /// </summary>
        [Display(Name = "建筑导则分类")]
        BuidlingCategory = 0x00000010,


        /// <summary>
        /// 32: 设备类型
        /// </summary>
        [Display(Name = "设备类型")]
        MeterType = 0x00000020,

        /// <summary>
        /// 64: 品牌
        /// </summary>
        [Display(Name = "设备品牌")]
        Brand = 0x00000040,

        /// <summary>
        /// 128: 建筑类型
        /// </summary>
        [Display(Name = "建筑类型")]
        BuildingType = 0x00000080,

        /// <summary>
        /// 256: 管理用户
        /// </summary>
        [Display(Name = "管理用户")]
        Manager = 0x00000100,

        /// <summary>
        /// 512: 能耗用户
        /// </summary>
        [Display(Name = "能耗用户")]
        User = 0x00000200,


        /// <summary>
        /// 768: 管理用户
        /// </summary>
        [Display(Name = "用户")]
        AllUser = 0x00000300,


        /// <summary>
        /// 1024: 角色
        /// </summary>
        [Display(Name = "角色")]
        Role = 0x00000400,



        /// <summary>
        /// 2048: 设备参数
        /// </summary>
        [Display(Name = "设备参数")]
        Parameter = 0x00000800,

        /// <summary>
        /// 4096: 权限
        /// </summary>
        [Display(Name = "权限")]
        Permission = 0x00001000,

        /// <summary>
        /// 8192: 人事机构
        /// </summary>
        [Display(Name = "人事机构")]
        Org = 0x00002000,

        /// <summary>
        /// 16384: 字典
        /// </summary>
        [Display(Name = "字典")]
        Dictionary = 0x00004000,

        /// <summary>
        /// 32768: 父对象
        /// </summary>
        [Display(Name = "父对象")]
        Parent = 0x00008000,

        /// <summary>
        /// 65536: 子对象（列表）
        /// </summary>
        [Display(Name = "子对象（列表）")]
        Children = 0x00010000,

        /// <summary>
        /// 131072: 扩展字段
        /// </summary>
        [Display(Name = "扩展字段")]
        ExtensionField = 0x00020000,


        /// <summary>
        /// 262144: 坐标
        /// </summary>
        [Display(Name = "坐标")]
        Coordinate = 0x00040000,

        /// <summary>
        /// 524288: 实时参数
        /// </summary>
        [Display(Name = "实时参数")]
        Momentary = 0x00080000,

        /// <summary>
        /// 1048576: 省份
        /// </summary>
        [Display(Name = "省份")]
        Province = 0x00100000,

        /// <summary>
        /// 2097152: 城市
        /// </summary>
        [Display(Name = "城市")]
        City = 0x00200000,

        /// <summary>
        /// 4194304: 行政区
        /// </summary>
        [Display(Name = "行政区")]
        District = 0x00400000,

        /// <summary>
        /// 7340032: 地区
        /// </summary>
        [Display(Name = "地区")]
        Region = 0x00700000,

        /// <summary>
        /// 8388608: Descent
        /// </summary>
        [Display(Name = "后代对象列表")]
        Descendant = 0x00800000,

        /// <summary>
        /// 16777216: Descent
        /// </summary>
        [Display(Name = "先辈对象列表")]
        Ancestor = 0x01000000,

        /// <summary>
        /// 33554432: Message
        /// </summary>
        [Display(Name = "消息")]
        Message = 0x02000000,

        /// <summary>
        /// 67108864: MessageRecord
        /// </summary>
        [Display(Name = "消息记录")]
        MessageRecord = 0x04000000,

        /// <summary>
        /// 134217728: Balance
        /// </summary>
        [Display(Name = "账户结算信息")]
        Balance = 0x08000000,

        /// <summary>
        /// 268435456: Account
        /// </summary>
        [Display(Name = "账户信息")]
        Account = 0x10000000,

        /// <summary>
        /// 536,870,912: Gateway
        /// </summary>
        [Display(Name = "网关")]
        Gateway = 0x20000000,

        /// <summary>
        /// 1,073,741,824: Sibling
        /// </summary>
        [Display(Name = "兄弟")]
        Sibling = 0x40000000,

        /// <summary>
        /// 2,147,483,648: 用户分类
        /// </summary>
        [Display(Name = "用户分类")]
        UserType = 0x80000000,

        /// <summary>
        /// 4,294,967,295: 全部
        /// </summary>
        [Display(Name = "全部")]
        All = 0xFFFFFFFF
    }
}
