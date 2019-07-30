using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WxPay2017.API.DAL.EmpModels;
using WxPay2017.API.VO.Common;
using WxPay2017.API.VO.Param;
using WxPay2017.API.VO;


namespace WxPay2017.API.VO.Common
{
    public class DictionaryCache
    {
        public static List<int>  RatioParameterList = new List<int>() { 60023,60004, 60005, 60006, 60007, 60008, 60009, 60010, 60011, 60012, 60019, 60020, 60021, 60022 };
        public static List<int> EnergyStatusIdNotUsed = new List<int>{ 40001, 40002 };
        public static List<int> EnergyStatusIdUsed = new List<int> { 40004, 40003 };
        public static List<int> MeterReadValueParameterList = new List<int>() { 60019, 60023,60055};
        public static List<int> MeterBalanceParameterList = new List<int>() { 60094 };
        //电表设备类型
        public static List<int> MeterTypeElecMeterList = new List<int>() { 50105, 50106, 50107, 50108, 50109, 50110, 50111, 50112, 50113 };
        //水表设备类型
        public static List<int> MeterTypeWaterMeterList = new List<int>() { 50201, 50202, 50203, 50204, 50205, 50206, 50207, 50208, 50209, 50210, 50211, 50212 };

        //开机设备状态
        public static List<int> MeterStatusOn = new List<int>() { 530003, 530005, 530009, 530011 };
        //异常设备状态
        public static List<int> MeterStatusError = new List<int>() { 530001, 530006, 530007, 530009,530010 };
        //异常断电状态
        public static List<int> MeterStatusOffError = new List<int>() { 530001, 530006, 530007 };

        //后续调整为需要显示的字段
        public static List<int> MeterRealTimeParameterList = new List<int>() { 60019, 60023, 60055 };

        //状态为申购的报修单
        public static List<int> PurchasingOrder = new List<int> { 410011, 410012, 410013, 410014 };
        //处理中的报修单
        public static List<int> ProcessingOrder = new List<int> { 410001, 410002, 410003, 410004, 410005, 410021, 410010 };

        //在线充值
        public static List<int> ChargeOnline = new List<int> { 370001, 370002, 370003 };

        //充值方式
        public static List<int> PaymentMethods = new List<int> { 370001, 370002, 370003, 370005 };



        public static Dictionary<int, Dictionary> dictionaryCache = new Dictionary<int, Dictionary> ();
        //static DAL.EmpModels.EmpContext db = new DAL.EmpModels.EmpContext();

        /// <summary>
        /// 11超时
        /// </summary>
        public static Dictionary CommandStatusOverTime = Get().Values.FirstOrDefault(o => o.TreeId == "CommandStatus-02");
        /// <summary>
        /// 21待下发
        /// </summary>
        public static Dictionary CommandStatusWait = Get().Values.FirstOrDefault(o => o.TreeId == "CommandStatus-03");
        /// <summary>
        /// 31已下发
        /// </summary>
        public static Dictionary CommandStatusAlreadySend = Get().Values.FirstOrDefault(o => o.TreeId == "CommandStatus-04");
        /// <summary>
        /// 41已完成
        /// </summary>
        public static Dictionary CommandStatusAlreadySucess = Get().Values.FirstOrDefault(o => o.TreeId == "CommandStatus-05");
        /// <summary>
        /// 51已重发
        /// </summary>
        public static Dictionary CommandStatusAlreadyReSend = Get().Values.FirstOrDefault(o => o.TreeId == "CommandStatus-06");
        /// <summary>
        /// 61已忽略
        /// </summary>
        public static Dictionary CommandStatusAlreadyIgnore = Get().Values.FirstOrDefault(o => o.TreeId == "CommandStatus-07");
        /// <summary>
        /// 7超出界定范围
        /// </summary>
        public static Dictionary CommandOverLimit = Get().Values.FirstOrDefault(o => o.TreeId == "CommandStatus-01-06");

        //维修完成
        public static Dictionary ActivityStatusFinish = Get().Values.FirstOrDefault(o => o.TreeId == "ActivityStatus-02-02");
        //放弃维修[报修单作废]
        public static Dictionary ActivityStatusFailed = Get().Values.FirstOrDefault(o => o.TreeId == "ActivityStatus-00-05");
        //申请维修
        public static Dictionary ActivityStatusApplyMaintenance = Get().Values.FirstOrDefault(o => o.TreeId == "ActivityStatus-00");
        //评价
        public static Dictionary ActivityStatusEstimate = Get().Values.FirstOrDefault(o => o.TreeId == "ActivityStatus-00-03");
        

        //附件分类为报修单
        public static Dictionary AttachmentTypeMaintenance = Get().Values.FirstOrDefault(o => o.TreeId == "AttachmentType-04");

        //附件格式为图片
        public static Dictionary AttachmentFormatPicture = Get().Values.FirstOrDefault(o => o.TreeId == "AttachmentFormat-01");


        public static Dictionary MonitoringConfigTypeQuota = Get().Values.FirstOrDefault(o => o.TreeId == "MonitoringConfigType-01");
        public static Dictionary MonitoringConfigTypePrice = Get().Values.FirstOrDefault(o => o.TreeId == "MonitoringConfigType-02");
        public static Dictionary MonitoringConfigTypeSubsidy = Get().Values.FirstOrDefault(o => o.TreeId == "MonitoringConfigType-03");
        public static Dictionary MonitoringConfigTypeTarget = Get().Values.FirstOrDefault(o => o.TreeId == "MonitoringConfigType-04");
        public static Dictionary MonitoringConfigTypeWarning = Get().Values.FirstOrDefault(o => o.TreeId == "MonitoringConfigType-05");
        public static Dictionary MonitoringConfigTypeGuaranteePower = Get().Values.FirstOrDefault(o => o.TreeId == "MonitoringConfigType-11");

        public static Dictionary MeterTypeGateway = Get().Values.FirstOrDefault(o => o.TreeId == "MeterType-00-01-01");

        
        public static Dictionary TemplateQuota = Get().Values.FirstOrDefault(o => o.TreeId == "MonitoringConfigType-06");
        public static Dictionary TemplatePrice = Get().Values.FirstOrDefault(o => o.TreeId == "MonitoringConfigType-07");
        public static Dictionary TemplateSubsidy = Get().Values.FirstOrDefault(o => o.TreeId == "MonitoringConfigType-08");
        public static Dictionary TemplateTarget = Get().Values.FirstOrDefault(o => o.TreeId == "MonitoringConfigType-09");
        public static Dictionary TemplateWarning = Get().Values.FirstOrDefault(o => o.TreeId == "MonitoringConfigType-10");
        public static Dictionary TemplateTimeStrategy = Get().Values.FirstOrDefault(o => o.TreeId == "MonitoringConfigType-12");

        public static Dictionary OrgTypeClass = Get().Values.FirstOrDefault(o => o.TreeId == "OrganizationType-02-01");

        public static Dictionary ConfigToBuilding = Get().Values.FirstOrDefault(o => o.TreeId == "MonitoringConfigTargetType-02");
        public static Dictionary ConfigToOrg = Get().Values.FirstOrDefault(o => o.TreeId == "MonitoringConfigTargetType-01");
        public static Dictionary ConfigToMeter   = Get().Values.FirstOrDefault(o => o.TreeId == "MonitoringConfigTargetType-03");
        public static Dictionary ConfigToGroup = Get().Values.FirstOrDefault(o => o.TreeId == "MonitoringConfigTargetType-04");
        

        public static Dictionary PeriodYear = Get().Values.FirstOrDefault(o => o.TreeId == "Period-01-01");
        public static Dictionary PeriodMonth = Get().Values.FirstOrDefault(o => o.TreeId == "Period-02-02");
        public static Dictionary PeriodWeek = Get().Values.FirstOrDefault(o => o.TreeId == "Period-03-03");
        public static Dictionary PeriodDay = Get().Values.FirstOrDefault(o => o.TreeId == "Period-04-04");
        public static Dictionary PeriodHour = Get().Values.FirstOrDefault(o => o.TreeId == "Period-05-05");
        public static Dictionary PeriodTEveryTime = Get().Values.FirstOrDefault(o => o.TreeId == "Period-06");
        public static Dictionary PeriodTCustomer = Get().Values.FirstOrDefault(o => o.TreeId == "Period-07-07");
        public static Dictionary PeriodSemester = Get().Values.FirstOrDefault(o => o.TreeId == "Period-08");

        public static Dictionary ParameterFlow = Get().Values.FirstOrDefault(o => o.TreeId == "Parameter-02-01-01");
        public static Dictionary ParameterActivePower = Get().Values.FirstOrDefault(o => o.TreeId == "Parameter-01-01-19");
        public static Dictionary ParameterRestPower = Get().Values.FirstOrDefault(o => o.TreeId == "Parameter-01-00-26");
        public static Dictionary ParameterUsedPower = Get().Values.FirstOrDefault(o => o.TreeId == "Parameter-01-01-25");
        public static List<int> ParametersPower = new List<int> { ParameterActivePower.Id, ParameterUsedPower .Id};
        public static List<int> ParametersWater = new List<int> { ParameterFlow.Id };
        public static Dictionary ParameterClockVoltage = Get().Values.FirstOrDefault(o => o.TreeId == "Parameter-01-00-34");

        public static List<int> ParametersAllBaseEnergy = new List<int> ();
        //statisticscontroller中的配置对比
        //var pList = DictionaryCache.Get()[(int)config.EnergyCategoryId].ToViewData(CategoryDictionary.Parameter).Parameters.Select(o => o.Id).ToList();

        public static Dictionary BuildingCategoryCulture = Get().Values.FirstOrDefault(o => o.TreeId == "BuildingCategory-04");
        public static Dictionary BuildingCategoryALL = Get().Values.FirstOrDefault(o => o.TreeId == "BuildingCategory");

        public static Dictionary BuildingTypeBuilding = Get().Values.FirstOrDefault(o => o.TreeId == "BuildingType-03");
        public static Dictionary BuildingTypeLevel = Get().Values.FirstOrDefault(o => o.TreeId == "BuildingType-05");
        public static Dictionary BuildingTypeRoom = Get().Values.FirstOrDefault(o => o.TreeId == "BuildingType-07");


        public static Dictionary BalancePowerSubsidy = Get().Values.FirstOrDefault(o => o.TreeId == "BalanceType-01");
        public static Dictionary BalancePowerMoney = Get().Values.FirstOrDefault(o => o.TreeId == "BalanceType-02");
        public static Dictionary BalanceWaterSubsidy = Get().Values.FirstOrDefault(o => o.TreeId == "BalanceType-03");
        public static Dictionary BalanceWaterMoney = Get().Values.FirstOrDefault(o => o.TreeId == "BalanceType-04");


        public static Dictionary RelayElecStateCreditLow = Get().Values.FirstOrDefault(o => o.Id == 530004);
        public static Dictionary RelayElecStateCreditZero = Get().Values.FirstOrDefault(o => o.Id == 530005);
        public static Dictionary RelayElecStateNormal = Get().Values.FirstOrDefault(o => o.Id == 530003);
        public static Dictionary RelayElecStateOFF = Get().Values.FirstOrDefault(o => o.Id == 530002);
        public static Dictionary RelayElecStateCreditZeroAndOFF = Get().Values.FirstOrDefault(o => o.Id == 530008);
        public static Dictionary ParameterCredit = Get().Values.FirstOrDefault(o => o.TreeId == "Parameter-01-00-32");

        /// <summary>
        /// 3
        /// </summary>
        public static Dictionary BillTypePay = Get().Values.FirstOrDefault(o => o.TreeId == "BillType-03");
        /// <summary>
        /// 1
        /// </summary>
        public static Dictionary BillTypeAuto = Get().Values.FirstOrDefault(o => o.TreeId == "BillType-01");
        /// <summary>
        /// 2
        /// </summary>
        public static Dictionary BillTypeManual = Get().Values.FirstOrDefault(o => o.TreeId == "BillType-02");
        /// <summary>
        /// 4
        /// </summary>
        public static Dictionary BillTypeSubsidy = Get().Values.FirstOrDefault(o => o.TreeId == "BillType-04");
        /// <summary>
        /// 6
        /// </summary>
        public static Dictionary BillTypePrePay = Get().Values.FirstOrDefault(o => o.TreeId == "BillType-06");
        /// <summary>
        /// 5
        /// </summary>
        public static Dictionary BillTypeSubsidyOverTime = Get().Values.FirstOrDefault(o => o.TreeId == "BillType-05");
        /// <summary>
        /// 统一账号缴费,7
        /// </summary>
        public static Dictionary BillTypeUnifiedAccount = Get().Values.FirstOrDefault(o => o.TreeId == "BillType-07");

        /// <summary>
        /// 1
        /// </summary>
        public static Dictionary PayMethodAlipay = Get().Values.FirstOrDefault(o => o.TreeId == "PayMethod-01");
        /// <summary>
        /// 4
        /// </summary>
        public static Dictionary PayMethodWaterElectCtrl = Get().Values.FirstOrDefault(o => o.TreeId == "PayMethod-04");
        /// <summary>
        /// 5
        /// </summary>
        public static Dictionary PayMethodCash = Get().Values.FirstOrDefault(o => o.TreeId == "PayMethod-05");
        /// <summary>
        /// 6
        /// </summary>
        public static Dictionary PayMethodRefund = Get().Values.FirstOrDefault(o => o.TreeId == "PayMethod-06");
        /// <summary>
        /// 7
        /// </summary>
        public static Dictionary PayMethodBadBalance = Get().Values.FirstOrDefault(o => o.TreeId == "PayMethod-07");
        /// <summary>
        /// 8
        /// </summary>
        public static Dictionary PayMethodSubsidyOverTime = Get().Values.FirstOrDefault(o => o.TreeId == "PayMethod-08");
        /// <summary>
        /// 9
        /// </summary>
        public static Dictionary PayMethodPrePay = Get().Values.FirstOrDefault(o => o.TreeId == "PayMethod-09");
        /// <summary>
        /// 10
        /// </summary>
        public static Dictionary PayMethodDeductionPrePay = Get().Values.FirstOrDefault(o => o.TreeId == "PayMethod-10");
        /// <summary>
        /// 11
        /// </summary>
        public static Dictionary PayMethodCashCorrect = Get().Values.FirstOrDefault(o => o.TreeId == "PayMethod-11");

        public static Dictionary SubscribeTypeByRole = Get().Values.FirstOrDefault(o => o.TreeId == "SubscribeType-01");
        public static Dictionary SubscribeTypeByUser = Get().Values.FirstOrDefault(o => o.TreeId == "SubscribeType-02");

        public static Dictionary ValidDataCode = Get().Values.FirstOrDefault(o => o.TreeId == "OriginalDataStatus-01");
        public static Dictionary InvalidDataCode = Get().Values.FirstOrDefault(o => o.TreeId == "OriginalDataStatus-00");

        public static Dictionary PowerCateogry = Get().Values.FirstOrDefault(o => o.TreeId == "EnergyCategory-01");
        public static Dictionary WaterCategory = Get().Values.FirstOrDefault(o => o.TreeId == "EnergyCategory-02");
        public static Dictionary HeatCategory = Get().Values.FirstOrDefault(o => o.TreeId == "EnergyCategory-04");
        public static Dictionary AllCategory = Get().Values.FirstOrDefault(o => o.TreeId == "EnergyCategory");

        public static Dictionary EnergyStatusComplete = Get().Values.FirstOrDefault(o => o.TreeId == "EnergyStatus-02");

        public static Dictionary QuotaWayPerCapita = Get().Values.FirstOrDefault(o => o.TreeId == "QuotaWay-01");
        public static Dictionary QuotaWayPerUnitArea = Get().Values.FirstOrDefault(o => o.TreeId == "QuotaWay-02");
        public static Dictionary QuotaWayTotal = Get().Values.FirstOrDefault(o => o.TreeId == "QuotaWay-03");

        public static Dictionary PriceWayNormal = Get().Values.FirstOrDefault(o => o.TreeId == "PriceWay-01");
        public static Dictionary PriceWayByTime = Get().Values.FirstOrDefault(o => o.TreeId == "PriceWay-02");
        public static Dictionary PriceWayGradientByQuota = Get().Values.FirstOrDefault(o => o.TreeId == "PriceWay-03");
        public static Dictionary PriceWayGradientByPercentage = Get().Values.FirstOrDefault(o => o.TreeId == "PriceWay-04");
        public static Dictionary PriceWayOverRationByQuota = Get().Values.FirstOrDefault(o => o.TreeId == "PriceWay-05");
        public static Dictionary PriceWayOverRationByPercentage = Get().Values.FirstOrDefault(o => o.TreeId == "PriceWay-06");

        public static Dictionary ActionOnALL = Get().Values.FirstOrDefault(o => o.TreeId == "Action-01");
        public static Dictionary ActionOffALL = Get().Values.FirstOrDefault(o => o.TreeId == "Action-02");
        public static Dictionary ActionOnWater = Get().Values.FirstOrDefault(o => o.TreeId == "Action-03");
        public static Dictionary ActionOffWater = Get().Values.FirstOrDefault(o => o.TreeId == "Action-04");
        public static Dictionary ActionOnPower = Get().Values.FirstOrDefault(o => o.TreeId == "Action-05");
        public static Dictionary ActionOffPower = Get().Values.FirstOrDefault(o => o.TreeId == "Action-06");
        public static Dictionary ActionSetPrice = Get().Values.FirstOrDefault(o => o.TreeId == "Action-07");
        public static Dictionary ActionMalignantLoadOn = Get().Values.FirstOrDefault(o => o.TreeId == "Action-08");
        public static Dictionary ActionMalignantLoadOff = Get().Values.FirstOrDefault(o => o.TreeId == "Action-09");
        public static Dictionary ActionMalignantLoadSet = Get().Values.FirstOrDefault(o => o.TreeId == "Action-10");
        public static Dictionary ActionOverLoadOn = Get().Values.FirstOrDefault(o => o.TreeId == "Action-11");
        public static Dictionary ActionOverLoadOff = Get().Values.FirstOrDefault(o => o.TreeId == "Action-12");
        public static Dictionary ActionOverLoadSet = Get().Values.FirstOrDefault(o => o.TreeId == "Action-13");
        public static Dictionary ActionDefaultWarningValue = Get().Values.FirstOrDefault(o => o.TreeId == "Action-14");
        /// <summary>
        /// 紧急操作
        /// </summary>
        public static Dictionary ActionEmergencyOperation = Get().Values.FirstOrDefault(o => o.TreeId == "Action-38");
        /// <summary>
        /// 节假日保电
        /// </summary>
        public static Dictionary ActionGuaranteedElectricity = Get().Values.FirstOrDefault(o => o.TreeId == "Action-36");
        /// <summary>
        /// 销户
        /// </summary>
        public static Dictionary ActionCancelAccount = Get().Values.FirstOrDefault(o => o.TreeId == "Action-15");
        /// <summary>
        /// 余额变更22
        /// </summary>
        public static Dictionary ActionBalanceChange = Get().Values.FirstOrDefault(o => o.TreeId == "Action-22");
        /// <summary>
        /// 换表
        /// </summary>
        public static Dictionary ActionChangeMeter = Get().Values.FirstOrDefault(o => o.TreeId == "Action-35");

        /// <summary>
        /// init 17
        /// </summary>
        public static Dictionary ActionInitialization = Get().Values.FirstOrDefault(o => o.TreeId == "Action-17");

        /// <summary>
        /// 设置费率1定价
        /// </summary>
        public static Dictionary ActionSetPrice1 = Get().Values.FirstOrDefault(o => o.TreeId == "Action-18");
        /// <summary>
        /// 读取电表剩余金额
        /// </summary>
        public static Dictionary ActionGetRemainingAccount = Get().Values.FirstOrDefault(o => o.TreeId == "Action-19");
        /// <summary>
        /// 设置控制模式20
        /// </summary>
        public static Dictionary ActionSetControlModel = Get().Values.FirstOrDefault(o => o.TreeId == "Action-20");
        /// <summary>
        /// 拉闸合闸21
        /// </summary>
        public static Dictionary ActionSetONOFF = Get().Values.FirstOrDefault(o => o.TreeId == "Action-21");
        /// <summary>
        ///合闸 恢复模式
        /// </summary>
        public static Dictionary ActionSetONAndModel = Get().Values.FirstOrDefault(o => o.TreeId == "Action-34");
        ///// <summary>
        ///// 切换远程控制模式
        ///// </summary>
        //public static Dictionary ActionSetRemoteControl = Get().Values.FirstOrDefault(o => o.TreeId == "Action-22");
        /// <summary>
        /// 假期时套设置
        /// </summary>
        public static Dictionary ActionSetVacationTimeControlTemplate = Get().Values.FirstOrDefault(o => o.TreeId == "Action-26");
        /// <summary>
        /// 假日时套设置
        /// </summary>
        public static Dictionary ActionSetHolidayTimeControlTemplate = Get().Values.FirstOrDefault(o => o.TreeId == "Action-27");
        /// <summary>
        /// 周末时套设置
        /// </summary>
        public static Dictionary ActionSetWeekEndTimeControlTemplate = Get().Values.FirstOrDefault(o => o.TreeId == "Action-28");
        /// <summary>
        ///平时时套设置
        /// </summary>
        public static Dictionary ActionSetPeacetimeTimeControlTemplate = Get().Values.FirstOrDefault(o => o.TreeId == "Action-29");
        /// <summary>
        ///设备链路检测
        /// </summary>
        public static Dictionary ActionMeterCheckLine = Get().Values.FirstOrDefault(o => o.TreeId == "Action-30");
        /// <summary>
        ///网关链路检测
        /// </summary>
        public static Dictionary ActionGatewayCheckLine = Get().Values.FirstOrDefault(o => o.TreeId == "Action-31");
        /// <summary>
        ///设备参数检测
        /// </summary>
        public static Dictionary ActionMeterCheckConfig = Get().Values.FirstOrDefault(o => o.TreeId == "Action-32");
        /// <summary>
        ///电表余额检测
        /// </summary>
        public static Dictionary ActionMeterCheckAmount = Get().Values.FirstOrDefault(o => o.TreeId == "Action-33");
        public static Dictionary SubsidyWayPerCapita = Get().Values.FirstOrDefault(o => o.TreeId == "SubsidyWay-01");
        public static Dictionary SubsidyWayTotal = Get().Values.FirstOrDefault(o => o.TreeId == "SubsidyWay-02");

        public static Dictionary PayTypePower = Get().Values.FirstOrDefault(o => o.TreeId == "PayType-01");
        public static Dictionary PayTypeWater = Get().Values.FirstOrDefault(o => o.TreeId == "PayType-02");
        public static Dictionary PayTypeAll = Get().Values.FirstOrDefault(o => o.TreeId == "PayType-03");

        public static Dictionary ControlMeterPower = Get().Values.FirstOrDefault(o => o.TreeId == "MeterType-01-01-00-01");
        public static Dictionary ControlMeterWater = Get().Values.FirstOrDefault(o => o.TreeId == "MeterType-02-01-00-01");

        public static Dictionary MessageTypeLineOfCredit = Get().Values.FirstOrDefault(o => o.TreeId == "MessageType-05-10");
        public static Dictionary MessageTypeEnergyConsumptionEarlyWarning = Get().Values.FirstOrDefault(o => o.TreeId == "MessageType-05-09");
        /// <summary>
        /// 余额不足
        /// </summary>
        public static Dictionary MessageTypeEnergyCreditLow = Get().Values.FirstOrDefault(o => o.TreeId == "MessageType-05-07");
        /// <summary>
        /// 欠费
        /// </summary>
        public static Dictionary MessageTypeEnergyCreditZero = Get().Values.FirstOrDefault(o => o.TreeId == "MessageType-05-08");
        public static Dictionary MessageTypeEnergyQuotaLow = Get().Values.FirstOrDefault(o => o.TreeId == "MessageType-05-05");
        public static Dictionary MessageTypeEnergyQuotaZero = Get().Values.FirstOrDefault(o => o.TreeId == "MessageType-05-06");
        public static Dictionary MessageTypeSystem = Get().Values.FirstOrDefault(o => o.TreeId == "MessageType-02");
        public static Dictionary MessageTypeNotice = Get().Values.FirstOrDefault(o => o.TreeId == "MessageType-03");
        public static Dictionary MessageTypeSetQuota = Get().Values.FirstOrDefault(o => o.TreeId == "MessageType-02-04");
        public static Dictionary MessageTypeSetTarget = Get().Values.FirstOrDefault(o => o.TreeId == "MessageType-02-05");
        public static Dictionary MessageTypeEarlyAlarm = Get().Values.FirstOrDefault(o => o.TreeId == "MessageType-01");
        public static Dictionary MessageTypeGuarantee = Get().Values.FirstOrDefault(o => o.TreeId == "MessageType-04");
        public static Dictionary MessageTypeEnergyAlarm = Get().Values.FirstOrDefault(o => o.TreeId == "MessageType-05");
        public static Dictionary MessageTypeSystemParameter = Get().Values.FirstOrDefault(o => o.TreeId == "MessageType-06");
        public static List<int> MessageTypes = new List<int> { MessageTypeEarlyAlarm.Id, MessageTypeSystem.Id, MessageTypeNotice.Id, MessageTypeGuarantee.Id, MessageTypeEnergyAlarm.Id };
        public static List<int> MessagesAboutMoney = new List<int> { MessageTypeEnergyCreditLow.Id, MessageTypeEnergyCreditZero.Id, MessageTypeLineOfCredit.Id };
        public static Dictionary MessageTypeIeakage = Get().Values.FirstOrDefault(o => o.TreeId == "MessageType-01-12");
        public static Dictionary MessageTypeMalignantLoad = Get().Values.FirstOrDefault(o => o.TreeId == "MessageType-01-13");
        public static Dictionary MessageTypeOverLoad = Get().Values.FirstOrDefault(o => o.TreeId == "MessageType-01-01");
        public static Dictionary MessageTypeCashPayFail = Get().Values.FirstOrDefault(o => o.TreeId == "MessageType-01-14");
        public static Dictionary MessageTypeMobilePayFail = Get().Values.FirstOrDefault(o => o.TreeId == "MessageType-01-15");
        public static Dictionary MessageTypeAliPayInfo = Get().Values.FirstOrDefault(o => o.TreeId == "MessageType-10");

        public static Dictionary MessageTypeWideRangeError = Get().Values.FirstOrDefault(o => o.TreeId == "MessageType-01-16");

        public static Dictionary MessageSourceTypeMeter = Get().Values.FirstOrDefault(o => o.TreeId == "MessageSourceType-00");
        public static Dictionary MessageSourceTypeUser = Get().Values.FirstOrDefault(o => o.TreeId == "MessageSourceType-01");
        public static Dictionary MessageSourceTypeBuilding = Get().Values.FirstOrDefault(o => o.TreeId == "MessageSourceType-04");
        public static Dictionary MessageSourceTypeOrg = Get().Values.FirstOrDefault(o => o.TreeId == "MessageSourceType-05");
        public static Dictionary MessageSourceTypeSystem = Get().Values.FirstOrDefault(o => o.TreeId == "MessageSourceType-02");

        public static Dictionary MessageReceiveModelMail = Get().Values.FirstOrDefault(o => o.TreeId == "ReceivingModel-05");
        public static Dictionary MessageReceiveModelSMS = Get().Values.FirstOrDefault(o => o.TreeId == "ReceivingModel-02");


        public static Dictionary RatedParameterTypeOverLoad = Get().Values.FirstOrDefault(o => o.TreeId == "RatedParameterType-01");
        public static Dictionary RatedParameterTypeMalignantLoad = Get().Values.FirstOrDefault(o => o.TreeId == "RatedParameterType-04");
        public static Dictionary RatedParameterTypeMalignantLoadBlack = Get().Values.FirstOrDefault(o => o.TreeId == "RatedParameterType-05");
        public static Dictionary RatedParameterTypeMalignantLoadWhite = Get().Values.FirstOrDefault(o => o.TreeId == "RatedParameterType-06");
        /// <summary>
        /// 预警
        /// </summary>
        public static Dictionary AlarmLevelEarlyWarning = Get().Values.FirstOrDefault(o => o.TreeId == "AlarmLevel-02");
        /// <summary>
        /// 告警
        /// </summary>
        public static Dictionary AlarmLevelWarning = Get().Values.FirstOrDefault(o => o.TreeId == "AlarmLevel-03");

        public static string PowerUnit = "KWh";
        public static string WaterUnit = "吨";
        public static List<int> EnergyStatusActiveIds = Get().Values.Where(o => o.Code == "EnergyStatus"&&o.FirstValue>0).Select(o=>o.Id).ToList();
        public  static Dictionary<int, Dictionary> Get(){
            DAL.EmpModels.EmpContext db = new DAL.EmpModels.EmpContext();
            if (dictionaryCache.Count()==0)
            {
                var dics = db.Dictionaries.ToList();
                foreach (Dictionary m in dics)
                {
                    dictionaryCache.Add(m.Id, m);
                }
            }
            //水电控设备
            if (ParameterUsedPower != null && !ParametersPower.Contains(ParameterUsedPower.Id))
                ParametersPower.Add(ParameterUsedPower.Id);
            if (ParametersAllBaseEnergy!=null&&ParametersAllBaseEnergy.Count() == 0)
            {
                ParametersAllBaseEnergy.AddRange(ParametersPower);
                ParametersAllBaseEnergy.AddRange(ParametersWater);
            }
            return dictionaryCache;
        }
        public static Dictionary<int, Dictionary> Reset()
        {
            DAL.EmpModels.EmpContext db = new DAL.EmpModels.EmpContext();
            dictionaryCache = new Dictionary<int, Dictionary>();
            var dics = db.Dictionaries.Where(o => o.Enable == true).ToList();
            foreach (Dictionary m in dics)
            {
                dictionaryCache.Add(m.Id, m);
            }

            return dictionaryCache;
        }

        /// <summary>
        /// BrandMeterType  Code字段值
        /// </summary>
        public static string BrandMeterTypeCode = "BrandMeterType";

    }
}
