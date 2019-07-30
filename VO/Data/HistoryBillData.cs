using WxPay2017.API.VO.Param;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO
{
    public class HistoryBillData
    {
        public long Id { get; set; }


        public string ReceiverId { get; set; }


        public string PayerId { get; set; }

        public DateTime? BeginTime { get; set; }
        public decimal? UsedValue { get; set; }
        public decimal? UsedEnergyValue { get; set; }
        public DateTime? EndTime { get; set; }

        public decimal Value { get; set; }

        public int BillTypeId { get; set; }

        public int? PayTypeId { get; set; }
        public string OperatorId { get; set; }
        public bool IsPay { get; set; }


        public string subject { get; set; }


        public string Body { get; set; }

        public DateTime? PayMentTime { get; set; }

        public DateTime? CreateTime { get; set; }

        public bool? IsZero { get; set; }

        public bool? IsSynchro { get; set; }
        public DateTime? ZeroTime { get; set; }

        public int? PayMethodId { get; set; }


        public string TransNumber { get; set; }

        public int? EnergyCategoryId { get; set; }


        public virtual string PayMethodName { get; set; }

        public virtual string BillTypeName { get; set; }

        public virtual string PayTypeName { get; set; }

        public virtual UserData Payer { get; set; }

        public virtual UserData Receiver { get; set; }
        public virtual UserData Operator { get; set; }

        public virtual dynamic Target { get; set; }
    }

    /// <summary>
    /// 获取缴费记录，待上传参数
    /// </summary>
    public class GetPaymentRecordParam 
    {
        /// <summary>
        /// 统计开始时间
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 统计结束时间
        /// </summary>
        public DateTime FinishedTime { get; set; }
        /// <summary>
        /// 目标建筑Id,默认为空
        /// </summary>
        public int? BuildingId { get; set; }
    }

    /// <summary>
    /// 缴费记录数据
    /// </summary>
    public class PaymentRecordData 
    {
        /// <summary>
        /// 建筑名
        /// </summary>
        public string BuildingName { get; set; }
        /// <summary>
        /// 楼层名
        /// </summary>
        public string FloorName { get; set; }
        /// <summary>
        /// 房间名
        /// </summary>
        public string RoomName { get; set; }
        /// <summary>
        /// 缴费账户id
        /// </summary>
        public string PayerId { get; set; }
        /// <summary>
        /// 缴费人信息
        /// </summary>
        //public UserData Payer { get; set; }
        /// <summary>
        /// 缴费人姓名
        /// </summary>
        public string PayerName { get; set; }
        /// <summary>
        /// 缴费金额
        /// </summary>
        public decimal Value { get; set; }
        /// <summary>
        /// 缴费时间
        /// </summary>
        public DateTime? PaymentTime { get; set; }
        /// <summary>
        /// 缴费方式
        /// </summary>
        public int? PayMethodId { get; set; }
        /// <summary>
        /// 缴费方式信息
        /// </summary>
        //public DictionaryData PayMethod { get; set; }
        /// <summary>
        /// 缴费方式名称
        /// </summary>
        public string PayMethodName { get; set; }

        public string OperatorId { get; set; }
        public string OperatorName { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverId { get; set; }

    }

    /// <summary>
    /// 缴费记录结果集
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class IPaymentRecordData<T> 
    {
        /// <summary>
        /// 缴费记录
        /// </summary>
        public ICollection<T> PaymentRecords { get; set; }
        /// <summary>
        /// 分页信息
        /// </summary>
        public Pagination Pagination { get; set; }
        /// <summary>
        /// 总计
        /// </summary>
        public decimal TotalValue { get; set; }

    }

    /// <summary>
    /// 设备充值失败信息
    /// </summary>
    public class MeterChargeFailedData 
    {
        /// <summary>
        /// 房间名称
        /// </summary>
        public string BuildingName { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        public string MeterName { get; set; }
        public int MeterId { get; set; }
        /// <summary>
        /// 充值者姓名
        /// </summary>
        public string PayerName { get; set; }
        /// <summary>
        /// 学号
        /// </summary>
        public string PayerId { get; set; }
        /// <summary>
        /// 充值金额
        /// </summary>
        public decimal Value { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// historybill的id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 充值时间
        /// </summary>
        public DateTime? PayMentTime { get; set; }


    }

    /// <summary>
    /// 收费汇总表
    /// </summary>
    public class PaymentSummaryData 
    {
        public string Receiver { get; set; }
        public string Count { get; set; }
        public int Value { get; set; }
    }

}
