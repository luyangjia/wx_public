using WxPay2017.API.VO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO
{
    public class ObejctToPayData
    {
        public CategoryDictionary Category { get; set; }//对象类型
        public string Name { get; set; }//对象名
        public string PictureAddress { get; set; }//图片地址
        public NowBill CostAccounts = new NowBill();//获取账户（余额、补贴、子账户充值，补贴总金额）
        public int Id { get; set; }//对象Id
        
    }
    //public class CostAccounts
    //{
    //    public string Payment_id { get; set; }//缴费账户Id
    //    public decimal CashAccount { get; set; }//现金账户
    //    public decimal SubsidyAccount { get; set; }//补贴账户
    //    public decimal ChildAccount { get; set; }//子账户补贴和充值总金额
    //    public decimal? ThisPeriodPowerCost { get; set; }//本周期用电消费金额
    //    public decimal? ThisPeriodWaterCost { get; set; }//本周期用水消费金额
    //    public decimal? ThisPeriodPowerValue { get; set; }//本周期用电量
    //    public decimal? ThisPeriodWaterValue { get; set; }//本周期用水量
    //    public string IsPowerOff { get; set; }//是否停水、断电
    //    public DateTime? ThisPeriodPowerFinishTime { get; set; }//统计截止时间
    //    public DateTime? ThisPeriodPowerBeginTime { get; set; }//本周期开始时间
    //    public DateTime? ThisPeriodWaterFinishTime { get; set; }//统计截止时间
    //    public DateTime? ThisPeriodWaterBeginTime { get; set; }//本周期开始时间

    //}
}
