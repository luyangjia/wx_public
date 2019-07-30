using WxPay2017.API.DAL.EmpModels;
using WxPay2017.API.VO.Common;
using WxPay2017.API.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.BLL
{
    public static class SettingExtension
    {

        #region Setting
        public static SettingData ToViewData(this Setting node, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (node == null)
                return null;
            var model = new SettingData()
            {
                AppID = node.AppID,
                Data = node.Data,
                Title = node.Title,
                Weather = node.Weather,
				ElectricityPrice=node.ElectricityPrice,
				WaterPrice=node.WaterPrice,
                WaterPrePay=node.WaterPrePay,
                ElectricityPrePay=node.ElectricityPrePay,
                ProjectType=node.ProjectType,
                IPAddress=node.IPAddress,
                AdministrativeCode = node.AdministrativeCode,
                ConfigIds=node.ConfigIds,
                Regex = node.Regex,
                HeatPrice=node.HeatPrice,
                FirstSemesterBeginTime=node.FirstSemesterBeginTime,
                FirstSemesterEndTime=node.FirstSemesterEndTime,
                SecondSemesterBeginTime=node.SecondSemesterBeginTime,
                SecondSemesterEndTime=node.SecondSemesterEndTime,
                NotGiveSubsidieMonth=node.NotGiveSubsidieMonth,
                AliPayAddress=node.AliPayAddress,
                SMSAppkey=node.SMSAppkey,
                SMSSenderName=node.SMSSenderName,
                SMSSecret=node.SMSSecret,
                SMSTemplateId=node.SMSTemplateId,
                BalancedetectionMaxdifference=node.BalancedetectionMaxdifference,
                WarningAmount=node.WarningAmount,
                JpushAppkey=node.JpushAppkey,
                BillStartTime=node.BillStartTime,
                JpushSecret=node.JpushSecret,
                SmtpEnableSsl=node.SmtpEnableSsl,
                SmtpPort=node.SmtpPort,
                SmtpPwd=node.SmtpPwd,
                SmtpPwdCheckEnable=node.SmtpPwdCheckEnable,
                SmtpServer=node.SmtpServer,
                SmtpUser = node.SmtpUser,
                IsNeedPwdForUpdate=node.IsNeedPwdForUpdate,
                UpdatePwd=node.UpdatePwd,
                CampusPowerLimit=node.CampusPowerLimit,
                BuidlingPowerLimit=node.BuidlingPowerLimit,
                StopChargingTime = node.StopChargingTime
            };
            return model;
        }

        public static IList<SettingData> ToViewList(this IQueryable<Setting> nodes, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (nodes == null)
                return null;
            var result = nodes.ToList().Select(node => node.ToViewData(suffix)).ToList();
            return result;
        }
        public static Setting ToModel(this SettingData node)
        {
            var model = new Setting()
            {
                AppID = node.AppID,
                Data = node.Data,
                Title = node.Title,
                Weather = node.Weather,
                ElectricityPrice = node.ElectricityPrice,
                WaterPrice = node.WaterPrice,
                WaterPrePay = node.WaterPrePay,
                ElectricityPrePay = node.ElectricityPrePay,
                ProjectType = node.ProjectType,
                IPAddress = node.IPAddress,
                AdministrativeCode = node.AdministrativeCode,
                ConfigIds = node.ConfigIds,
                Regex = node.Regex,
                HeatPrice = node.HeatPrice,
                BalancedetectionMaxdifference = node.BalancedetectionMaxdifference,
                WarningAmount = node.WarningAmount,
                FirstSemesterBeginTime = node.FirstSemesterBeginTime,
                FirstSemesterEndTime = node.FirstSemesterEndTime,
                SecondSemesterBeginTime = node.SecondSemesterBeginTime,
                SecondSemesterEndTime = node.SecondSemesterEndTime,
                NotGiveSubsidieMonth = node.NotGiveSubsidieMonth,
                AliPayAddress = node.AliPayAddress,
                SMSAppkey = node.SMSAppkey,
                SMSSenderName = node.SMSSenderName,
                SMSSecret = node.SMSSecret,
                SMSTemplateId = node.SMSTemplateId,
                JpushAppkey = node.JpushAppkey,
                JpushSecret = node.JpushSecret,
                SmtpEnableSsl = node.SmtpEnableSsl,
                BillStartTime = node.BillStartTime,
                SmtpPort = node.SmtpPort,
                SmtpPwd = node.SmtpPwd,
                SmtpPwdCheckEnable = node.SmtpPwdCheckEnable,
                SmtpServer = node.SmtpServer,
                SmtpUser = node.SmtpUser,
                IsNeedPwdForUpdate = node.IsNeedPwdForUpdate,
                UpdatePwd = node.UpdatePwd,
                CampusPowerLimit = node.CampusPowerLimit,
                BuidlingPowerLimit = node.BuidlingPowerLimit,
                StopChargingTime = node.StopChargingTime

            };
            return model;
        }
        #endregion


    }
}
