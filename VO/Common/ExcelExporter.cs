using WxPay2017.API.VO;
using ExcelReport;
using System;
using System.Collections.Generic;
using System.Linq;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;

namespace WxPay2017.API.VO.Common
{
    public class ExcelExporter
    {
        /// <summary>
        /// 用电监控
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <param name="buildingName">建筑(楼栋)名称(表头)</param>
        /// <param name="path">模板文件夹地址</param>
        public void ElecMonitoring(string filename, List<PowerMeterMonitoringData> dataSource, string buildingName, string path)
        {
            //数据源GetMetersUnderBuildingLevel
            var workbookParameterContainer = new WorkbookParameterContainer();
            workbookParameterContainer.Load(string.Format("{0}{1}.xml", path, "ElectricityMonitoring"));
            SheetParameterContainer sheetParameterContainer = workbookParameterContainer["用电监控"];

            int num = 1;
            filename = filename == null ? "用电监控" : filename;

            ExportHelper.ExportToWeb(string.Format("{0}{1}.xls", path, "ElectricityMonitoring"), filename + ".xls",
                new SheetFormatter("用电监控",
                //表头前缀
                    new PartFormatter(sheetParameterContainer["tabulatingPrefix"], buildingName),
                //表头
                    new PartFormatter(sheetParameterContainer["tabulating"], filename),
                //签章时间
                    new PartFormatter(sheetParameterContainer["timeNow"], string.Format("{0}年{1}月{2}日", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)),
                    new TableFormatter<PowerMeterMonitoringData>(sheetParameterContainer["roomNo"], dataSource,
                        new CellFormatter<PowerMeterMonitoringData>(sheetParameterContainer["no"], t => num++),
                        new CellFormatter<PowerMeterMonitoringData>(sheetParameterContainer["roomNo"], t => t.BuildingName),
                        new CellFormatter<PowerMeterMonitoringData>(sheetParameterContainer["elecNo"], t => t.BrandName),
                        new CellFormatter<PowerMeterMonitoringData>(sheetParameterContainer["elecValue"], t => t.RealValue),
                        new CellFormatter<PowerMeterMonitoringData>(sheetParameterContainer["elecFee"], t => t.LeftValue),
                        new CellFormatter<PowerMeterMonitoringData>(sheetParameterContainer["controlMode"], t => t.ControlModelName),
                        new CellFormatter<PowerMeterMonitoringData>(sheetParameterContainer["status"], t => t.Status)
                        )

                       )
                );

        }


        /// <summary>
        /// 收费明细报表(导出Excel)
        /// </summary>
        /// <param name="filename">下载文件名</param>
        /// <param name="dataSource">数据源</param>
        /// <param name="path">模板路径</param>
        public void PaymentRecord(string filename, List<PaymentRecordData> dataSource, string timeRange, string path)
        {
            var workbookParameterContainer = new WorkbookParameterContainer();
            workbookParameterContainer.Load(string.Format("{0}{1}.xml", path, "PaymentRecords"));
            SheetParameterContainer sheetParameterContainer = workbookParameterContainer["收费明细报表"];

            int num = 1;
            filename = filename == null ? "收费明细报表" : filename;

            ExportHelper.ExportToWeb(string.Format("{0}{1}.xls", path, "PaymentRecords"), filename + ".xls",
                new SheetFormatter("收费明细报表",
                    new PartFormatter(sheetParameterContainer["tabulating"], filename),
                //时间段
                    new PartFormatter(sheetParameterContainer["timeRange"], timeRange),
                //总计
                    new PartFormatter(sheetParameterContainer["total"], dataSource.Sum(o => o.Value)),// + "元"
                //签章时间
                    new PartFormatter(sheetParameterContainer["tabulationTime"], string.Format("{0}年{1}月{2}日", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)),
                    new TableFormatter<PaymentRecordData>(sheetParameterContainer["no"], dataSource,
                        new CellFormatter<PaymentRecordData>(sheetParameterContainer["no"], t => num++),
                        new CellFormatter<PaymentRecordData>(sheetParameterContainer["building"], t => t.BuildingName),
                        new CellFormatter<PaymentRecordData>(sheetParameterContainer["receiver"], t => t.ReceiverName),
                        new CellFormatter<PaymentRecordData>(sheetParameterContainer["room"], t => t.RoomName),
                        new CellFormatter<PaymentRecordData>(sheetParameterContainer["payer"], t => t.PayerName),
                        new CellFormatter<PaymentRecordData>(sheetParameterContainer["value"], t => t.Value),
                        new CellFormatter<PaymentRecordData>(sheetParameterContainer["time"], t => t.PaymentTime.ToString()),
                        new CellFormatter<PaymentRecordData>(sheetParameterContainer["method"], t => t.PayMethodName)
                        )

                       )
                );
        }

        /// <summary>
        /// 收费员汇总表
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="dataSource"></param>
        /// <param name="path"></param>
        public void PaymentSummaryReport(string filename, List<PaymentSummaryData> dataSource, string path)
        {
            var workbookParameterContainer = new WorkbookParameterContainer();
            workbookParameterContainer.Load(string.Format("{0}{1}.xml", path, "PaymentSummary"));
            SheetParameterContainer sheetParameterContainer = workbookParameterContainer["收费员汇总表"];

            int num = 1;
            filename = filename == null ? "收费员汇总表" : filename;

            ExportHelper.ExportToWeb(string.Format("{0}{1}.xls", path, "PaymentSummary"), filename + ".xls",
                new SheetFormatter("收费员汇总表",
                    new PartFormatter(sheetParameterContainer["tabulating"], filename),
                //总计
                    new PartFormatter(sheetParameterContainer["total"], dataSource.Sum(o => o.Value)),// + "元"
                //打印时间
                    new PartFormatter(sheetParameterContainer["tabulationTime"], string.Format("{0}年{1}月{2}日", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)),
                    new TableFormatter<PaymentSummaryData>(sheetParameterContainer["no"], dataSource,
                        new CellFormatter<PaymentSummaryData>(sheetParameterContainer["no"], t => num++),
                        new CellFormatter<PaymentSummaryData>(sheetParameterContainer["receiver"], t => t.Receiver),
                        new CellFormatter<PaymentSummaryData>(sheetParameterContainer["count"], t => t.Count),
                        new CellFormatter<PaymentSummaryData>(sheetParameterContainer["value"], t => t.Value)
                        )

                       )
                );
        }


        /// <summary>
        /// 个人对账单
        /// </summary>
        /// <param name="filename">下载文件名</param>
        /// <param name="dataSource">数据源</param>
        /// <param name="path">模板路径</param>
        public void PersonalPaymentRecord(string filename, List<PaymentRecordData> dataSource, string timeRange, string path)
        {
            var workbookParameterContainer = new WorkbookParameterContainer();
            workbookParameterContainer.Load(string.Format("{0}{1}.xml", path, "PersonalPaymentRecords"));
            SheetParameterContainer sheetParameterContainer = workbookParameterContainer["个人对账单"];

            int num = 1;
            filename = filename == null ? "个人对账单" : filename;

            ExportHelper.ExportToWeb(string.Format("{0}{1}.xls", path, "PersonalPaymentRecords"), filename + ".xls",
                new SheetFormatter("个人对账单",
                    new PartFormatter(sheetParameterContainer["tabulating"], filename),
                //时间段
                    new PartFormatter(sheetParameterContainer["timeRange"], timeRange),
                //总计
                    new PartFormatter(sheetParameterContainer["total"], dataSource.Sum(o => o.Value)),//+"元"
                //签章时间
                    new PartFormatter(sheetParameterContainer["timeNow"], string.Format("{0}年{1}月{2}日", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)),
                    new TableFormatter<PaymentRecordData>(sheetParameterContainer["no"], dataSource,
                        new CellFormatter<PaymentRecordData>(sheetParameterContainer["no"], t => num++),
                        new CellFormatter<PaymentRecordData>(sheetParameterContainer["building"], t => t.BuildingName),
                        new CellFormatter<PaymentRecordData>(sheetParameterContainer["floor"], t => t.FloorName),
                        new CellFormatter<PaymentRecordData>(sheetParameterContainer["room"], t => t.RoomName),
                        new CellFormatter<PaymentRecordData>(sheetParameterContainer["payer"], t => t.PayerName),
                        new CellFormatter<PaymentRecordData>(sheetParameterContainer["value"], t => t.Value),
                        new CellFormatter<PaymentRecordData>(sheetParameterContainer["time"], t => t.PaymentTime.ToString()),
                        new CellFormatter<PaymentRecordData>(sheetParameterContainer["method"], t => t.PayMethodName)
                        )

                       )
                );
        }


        /// <summary>
        /// 欠费记录
        /// </summary>
        /// <param name="filename">下载文件名</param>
        /// <param name="dataSource">数据源</param>
        /// <param name="path">模板路径</param>
        public void ArrearsRecord(string filename, List<CreditZeroData> dataSource, string path)
        {
            var workbookParameterContainer = new WorkbookParameterContainer();
            workbookParameterContainer.Load(string.Format("{0}{1}.xml", path, "ArrearsRecords"));
            SheetParameterContainer sheetParameterContainer = workbookParameterContainer["欠费报表"];

            int num = 1;
            filename = filename == null ? "欠费报表" : filename;

            ExportHelper.ExportToWeb(string.Format("{0}{1}.xls", path, "ArrearsRecords"), filename + ".xls",
                new SheetFormatter("欠费报表",
                    new PartFormatter(sheetParameterContainer["tabulating"], filename),
                //总计
                    new PartFormatter(sheetParameterContainer["total"], dataSource.Sum(o => o.Total)),//+"元"
                //签章时间
                    new PartFormatter(sheetParameterContainer["timeNow"], string.Format("{0}年{1}月{2}日", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)),
                    new TableFormatter<CreditZeroData>(sheetParameterContainer["no"], dataSource,
                        new CellFormatter<CreditZeroData>(sheetParameterContainer["no"], t => num++),
                        new CellFormatter<CreditZeroData>(sheetParameterContainer["building"], t => t.BuildingName),
                        new CellFormatter<CreditZeroData>(sheetParameterContainer["floor"], t => t.LevelName),
                        new CellFormatter<CreditZeroData>(sheetParameterContainer["room"], t => t.RoomName),
                        new CellFormatter<CreditZeroData>(sheetParameterContainer["value"], t => t.Total),
                        new CellFormatter<CreditZeroData>(sheetParameterContainer["time"], t => t.Time)//.ToString()
                        )

                       )
                );
        }


        /// <summary>
        /// 故障设备记录
        /// </summary>
        /// <param name="filename">下载文件名</param>
        /// <param name="dataSource">数据源</param>
        /// <param name="path">模板路径</param>
        public void MalfunctionMeterRecord(string filename, List<MalfunctionMeterData> dataSource, string path)
        {
            var workbookParameterContainer = new WorkbookParameterContainer();
            workbookParameterContainer.Load(string.Format("{0}{1}.xml", path, "MalfunctionMeterRecords"));
            SheetParameterContainer sheetParameterContainer = workbookParameterContainer["故障设备报表"];

            int num = 1;
            filename = filename == null ? "故障设备报表" : filename;

            ExportHelper.ExportToWeb(string.Format("{0}{1}.xls", path, "MalfunctionMeterRecords"), filename + ".xls",
                new SheetFormatter("故障设备报表",
                //签章时间
                    new PartFormatter(sheetParameterContainer["TimeNow"], string.Format("{0}年{1}月{2}日", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)),
                    new TableFormatter<MalfunctionMeterData>(sheetParameterContainer["No"], dataSource,
                        new CellFormatter<MalfunctionMeterData>(sheetParameterContainer["No"], t => num++),
                        new CellFormatter<MalfunctionMeterData>(sheetParameterContainer["Building"], t => t.BuildingName),
                        new CellFormatter<MalfunctionMeterData>(sheetParameterContainer["Floor"], t => t.FloorName),
                        new CellFormatter<MalfunctionMeterData>(sheetParameterContainer["Room"], t => t.RoomName),
                        new CellFormatter<MalfunctionMeterData>(sheetParameterContainer["Address"], t => t.MeterAddress),
                        new CellFormatter<MalfunctionMeterData>(sheetParameterContainer["Mac"], t => t.MacAddress),
                        new CellFormatter<MalfunctionMeterData>(sheetParameterContainer["Time"], t => t.UpdateTime.ToString()),
                        new CellFormatter<MalfunctionMeterData>(sheetParameterContainer["Malfunction"], t => t.MalfunctionName)
                        )

                       )
                );
        }

        /// <summary>
        /// 充值异常
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="dataSource"></param>
        /// <param name="path"></param>
        public void RechargeAnomalyRecord(string filename, List<MalfunctionMeterData> dataSource, string path)
        {
            var workbookParameterContainer = new WorkbookParameterContainer();
            workbookParameterContainer.Load(string.Format("{0}{1}.xml", path, "RechargeAnomalyRecord"));
            SheetParameterContainer sheetParameterContainer = workbookParameterContainer["充值异常报表"];

            int num = 1;
            filename = filename == null ? "充值异常报表" : filename;

            ExportHelper.ExportToWeb(string.Format("{0}{1}.xls", path, "RechargeAnomalyRecord"), filename + ".xls",
                new SheetFormatter("充值异常报表",
                    new PartFormatter(sheetParameterContainer["tabulating"], filename),
                //签章时间
                    new PartFormatter(sheetParameterContainer["timeNow"], string.Format("{0}年{1}月{2}日", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)),
                    new TableFormatter<MalfunctionMeterData>(sheetParameterContainer["no"], dataSource,
                        new CellFormatter<MalfunctionMeterData>(sheetParameterContainer["no"], t => num++),
                        new CellFormatter<MalfunctionMeterData>(sheetParameterContainer["name"], t => t.MeterName),
                        new CellFormatter<MalfunctionMeterData>(sheetParameterContainer["malfunction"], t => t.MalfunctionName),
                        new CellFormatter<MalfunctionMeterData>(sheetParameterContainer["description"], t => t.Description),
                        new CellFormatter<MalfunctionMeterData>(sheetParameterContainer["time"], t => t.UpdateTime.ToString())
                        )

                       )
                );
        }


        /// <summary>
        /// 水电用量报表
        /// </summary>
        /// <param name="filename">下载文件名</param>
        /// <param name="dataSource">数据源</param>
        /// <param name="path">模板路径</param>
        public void WaterElectricityConsumptionRecord(string filename, List<EnergyReportData> dataSource, string path, string timeRange)
        {
            var workbookParameterContainer = new WorkbookParameterContainer();
            workbookParameterContainer.Load(string.Format("{0}{1}.xml", path, "WaterElectricityConsumption"));
            SheetParameterContainer sheetParameterContainer = workbookParameterContainer["水电用量报表"];

            int num = 1;
            filename = filename == null ? "水电用量报表" : filename;

            ExportHelper.ExportToWeb(string.Format("{0}{1}.xls", path, "WaterElectricityConsumption"), filename + ".xls",
                new SheetFormatter("水电用量报表",
                    new PartFormatter(sheetParameterContainer["tabulating"], filename),
                //时间段
                    new PartFormatter(sheetParameterContainer["timeRange"], timeRange),
                //合计信息
                    new PartFormatter(sheetParameterContainer["waterToTal"], dataSource.Sum(o => o.WaterEnergyValue)),
                    new PartFormatter(sheetParameterContainer["powerTotal"], dataSource.Sum(o => o.PowerEnergyValue)),
                //签章时间
                    new PartFormatter(sheetParameterContainer["timeNow"], string.Format("{0}年{1}月{2}日", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)),
                    new TableFormatter<EnergyReportData>(sheetParameterContainer["no"], dataSource,
                        new CellFormatter<EnergyReportData>(sheetParameterContainer["no"], t => num++),
                        new CellFormatter<EnergyReportData>(sheetParameterContainer["building"], t => t.BuildingName),
                        new CellFormatter<EnergyReportData>(sheetParameterContainer["floor"], t => t.FloorName),
                        new CellFormatter<EnergyReportData>(sheetParameterContainer["room"], t => t.RoomName),
                        new CellFormatter<EnergyReportData>(sheetParameterContainer["water"], t => t.WaterEnergyValue),
                        new CellFormatter<EnergyReportData>(sheetParameterContainer["power"], t => t.PowerEnergyValue)
                        )

                       )
                );
        }




        /// <summary>
        /// 下发异常报表
        /// </summary>
        /// <param name="filename">下载文件名</param>
        /// <param name="dataSource">数据源</param>
        /// <param name="path">模板路径</param>
        public void AbnormalIssueRecord(string tabulatingPrefix, string filename, List<MetersActionData> dataSource, string path)
        {
            var workbookParameterContainer = new WorkbookParameterContainer();
            workbookParameterContainer.Load(string.Format("{0}{1}.xml", path, "AbnormalIssue"));
            SheetParameterContainer sheetParameterContainer = workbookParameterContainer["下发异常报表"];

            int num = 1;
            filename = string.IsNullOrEmpty(filename) ? "下发异常报表" : filename;
            ExportHelper.ExportToWeb(string.Format("{0}{1}.xls", path, "AbnormalIssue"), filename + ".xls",
                new SheetFormatter("下发异常报表",
                //签章时间
                    new PartFormatter(sheetParameterContainer["timeNow"], string.Format("{0}年{1}月{2}日", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)),
                //new PartFormatter(sheetParameterContainer["tabulatingPrefix"], tabulatingPrefix), //新增异常类型名为报表名前缀，暂未使用，使用时将模板AbnormalIssue-prefix-backup替换为AbnormalIssue
                    new PartFormatter(sheetParameterContainer["tabulating"], filename),
                    new TableFormatter<MetersActionData>(sheetParameterContainer["no"], dataSource,
                        new CellFormatter<MetersActionData>(sheetParameterContainer["no"], t => num++),
                        new CellFormatter<MetersActionData>(sheetParameterContainer["number"], t => t.Id),
                        new CellFormatter<MetersActionData>(sheetParameterContainer["meterName"], t => t.MeterName),
                        new CellFormatter<MetersActionData>(sheetParameterContainer["macAddress"], t => t.MeterMacAddress),
                        new CellFormatter<MetersActionData>(sheetParameterContainer["actionName"], t => t.ActionName),
                        new CellFormatter<MetersActionData>(sheetParameterContainer["commandStatusName"], t => t.CommandStatusName),
                        new CellFormatter<MetersActionData>(sheetParameterContainer["time"], t => t.ActionTime)
                        )
                       )
                );
        }

        /// <summary>
        /// 链路检查异常报表
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="dataSource"></param>
        /// <param name="path"></param>
        public void LinkCheckRecord(string filename, List<MetersActionData> dataSource, string path)
        {
            var workbookParameterContainer = new WorkbookParameterContainer();
            workbookParameterContainer.Load(string.Format("{0}{1}.xml", path, "LinkCheck"));
            SheetParameterContainer sheetParameterContainer = workbookParameterContainer["链路检查异常报表"];

            int num = 1;
            filename = filename == null ? "链路检查异常报表" : filename;
            ExportHelper.ExportToWeb(string.Format("{0}{1}.xls", path, "LinkCheck"), filename + ".xls",
                new SheetFormatter("链路检查异常报表",
                    new PartFormatter(sheetParameterContainer["tabulating"], filename),
                //签章时间
                    new PartFormatter(sheetParameterContainer["timeNow"], string.Format("{0}年{1}月{2}日", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)),
                    new TableFormatter<MetersActionData>(sheetParameterContainer["no"], dataSource,
                        new CellFormatter<MetersActionData>(sheetParameterContainer["no"], t => num++),
                        new CellFormatter<MetersActionData>(sheetParameterContainer["number"], t => t.Id),
                        new CellFormatter<MetersActionData>(sheetParameterContainer["meterName"], t => t.MeterName),
                        new CellFormatter<MetersActionData>(sheetParameterContainer["macAddress"], t => t.MeterMacAddress),
                        new CellFormatter<MetersActionData>(sheetParameterContainer["actionName"], t => t.ActionName),
                        new CellFormatter<MetersActionData>(sheetParameterContainer["commandStatusName"], t => t.CommandStatusName),
                        new CellFormatter<MetersActionData>(sheetParameterContainer["time"], t => t.ActionTime)
                        )
                       )
                );
        }

        /// <summary>
        /// 参数检查异常记录
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="dataSource"></param>
        /// <param name="path"></param>
        public void ParameterCheckRecord(string filename, List<MetersActionData> dataSource, string path)
        {
            var workbookParameterContainer = new WorkbookParameterContainer();
            workbookParameterContainer.Load(string.Format("{0}{1}.xml", path, "ParameterCheck"));
            SheetParameterContainer sheetParameterContainer = workbookParameterContainer["参数检查异常报表"];

            int num = 1;
            filename = filename == null ? "参数检查异常报表" : filename;
            ExportHelper.ExportToWeb(string.Format("{0}{1}.xls", path, "ParameterCheck"), filename + ".xls",
                new SheetFormatter("参数检查异常报表",
                    new PartFormatter(sheetParameterContainer["tabulating"], filename),
                //签章时间
                    new PartFormatter(sheetParameterContainer["timeNow"], string.Format("{0}年{1}月{2}日", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)),
                    new TableFormatter<MetersActionData>(sheetParameterContainer["no"], dataSource,
                        new CellFormatter<MetersActionData>(sheetParameterContainer["no"], t => num++),
                        new CellFormatter<MetersActionData>(sheetParameterContainer["number"], t => t.Id),
                        new CellFormatter<MetersActionData>(sheetParameterContainer["meterName"], t => t.MeterName),
                        new CellFormatter<MetersActionData>(sheetParameterContainer["macAddress"], t => t.MeterMacAddress),
                        new CellFormatter<MetersActionData>(sheetParameterContainer["actionName"], t => t.ActionName),
                        new CellFormatter<MetersActionData>(sheetParameterContainer["commandStatusName"], t => t.CommandStatusName),
                        new CellFormatter<MetersActionData>(sheetParameterContainer["time"], t => t.ActionTime)
                        )
                       )
                );
        }



        /// <summary>
        /// 用电分析
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="dataSource"></param>
        /// <param name="title">表格标题</param>
        /// <param name="path"></param>
        public void ElecAnalysisRecord(EnergyAnalysisFullData fullData, List<EnergyAnalysisData> dataSource, string path)
        {
            var workbookParameterContainer = new WorkbookParameterContainer();
            workbookParameterContainer.Load(string.Format("{0}{1}.xml", path, "ElecAnalysis"));
            SheetParameterContainer sheetParameterContainer = workbookParameterContainer["用电分析"];

            ExportHelper.ExportToWeb(string.Format("{0}{1}.xls", path, "ElecAnalysis"), "用电分析" + ".xls",
                new SheetFormatter("用电分析",
                //表头前缀
                    new PartFormatter(sheetParameterContainer["tabulatingPrefix"], fullData.tabulatingPrefix),
                //表头
                    new PartFormatter(sheetParameterContainer["tabulating"], fullData.title),
                //时间段
                    new PartFormatter(sheetParameterContainer["timeRange"], fullData.timeRange),
                //签章时间
                    new PartFormatter(sheetParameterContainer["timeNow"], fullData.timeNow),
                    new TableFormatter<EnergyAnalysisData>(sheetParameterContainer["no"], dataSource,
                        new CellFormatter<EnergyAnalysisData>(sheetParameterContainer["no"], t => t.no),
                        new CellFormatter<EnergyAnalysisData>(sheetParameterContainer["valueNow"], t => t.valueNow),
                        new CellFormatter<EnergyAnalysisData>(sheetParameterContainer["valueLast"], t => t.valueLast),
                        new CellFormatter<EnergyAnalysisData>(sheetParameterContainer["anValue"], t => t.anValue),
                        new CellFormatter<EnergyAnalysisData>(sheetParameterContainer["anValueAdd"], t => t.anValueAdd),
                        new CellFormatter<EnergyAnalysisData>(sheetParameterContainer["momValue"], t => t.momValue),
                        new CellFormatter<EnergyAnalysisData>(sheetParameterContainer["momValueAdd"], t => t.momValueAdd),
                        new CellFormatter<EnergyAnalysisData>(sheetParameterContainer["time"], t => t.time)
                        )
                       )
                );
        }

        /// <summary>
        /// 用水分析
        /// </summary>
        /// <param name="fullData"></param>
        /// <param name="dataSource"></param>
        /// <param name="path"></param>
        public void WaterAnalysisRecord(EnergyAnalysisFullData fullData, List<EnergyAnalysisData> dataSource, string path)
        {
            var workbookParameterContainer = new WorkbookParameterContainer();
            workbookParameterContainer.Load(string.Format("{0}{1}.xml", path, "WaterAnalysis"));
            SheetParameterContainer sheetParameterContainer = workbookParameterContainer["用水分析"];

            ExportHelper.ExportToWeb(string.Format("{0}{1}.xls", path, "WaterAnalysis"), "用水分析" + ".xls",
                new SheetFormatter("用水分析",
                //表头
                    new PartFormatter(sheetParameterContainer["tabulating"], fullData.title),
                //时间段
                    new PartFormatter(sheetParameterContainer["timeRange"], fullData.timeRange),
                //签章时间
                    new PartFormatter(sheetParameterContainer["timeNow"], fullData.timeNow),
                    new TableFormatter<EnergyAnalysisData>(sheetParameterContainer["no"], dataSource,
                        new CellFormatter<EnergyAnalysisData>(sheetParameterContainer["no"], t => t.no),
                        new CellFormatter<EnergyAnalysisData>(sheetParameterContainer["valueNow"], t => t.valueNow),
                        new CellFormatter<EnergyAnalysisData>(sheetParameterContainer["valueLast"], t => t.valueLast),
                        new CellFormatter<EnergyAnalysisData>(sheetParameterContainer["anValue"], t => t.anValue),
                        new CellFormatter<EnergyAnalysisData>(sheetParameterContainer["anValueAdd"], t => t.anValueAdd),
                        new CellFormatter<EnergyAnalysisData>(sheetParameterContainer["momValue"], t => t.momValue),
                        new CellFormatter<EnergyAnalysisData>(sheetParameterContainer["momValueAdd"], t => t.momValueAdd),
                        new CellFormatter<EnergyAnalysisData>(sheetParameterContainer["time"], t => t.time)
                        )
                       )
                );
        }

        /// <summary>
        /// 网关台账
        /// </summary>
        /// <param name="filename">打印文件名</param>
        /// <param name="dataSource">数据源</param>
        /// <param name="path">路径</param>
        public void GatewayStandingBook(string tabulatingPrefix, string filename, List<GatewayMeterData> dataSource, string path)
        {
            var workbookParameterContainer = new WorkbookParameterContainer();
            workbookParameterContainer.Load(string.Format("{0}{1}.xml", path, "GatewayStandingBook"));
            SheetParameterContainer sheetParameterContainer = workbookParameterContainer["网关台账"];

            int num = 1;
            filename = filename == null ? "网关台账" : filename;

            ExportHelper.ExportToWeb(string.Format("{0}{1}.xls", path, "GatewayStandingBook"), filename + ".xls",
                new SheetFormatter("网关台账",
                    new PartFormatter(sheetParameterContainer["tabulatingPrefix"], tabulatingPrefix),
                    new PartFormatter(sheetParameterContainer["tabulating"], filename),
                //签章时间
                    new PartFormatter(sheetParameterContainer["timeNow"], string.Format("{0}年{1}月{2}日", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)),
                    new RepeaterFormatter<GatewayMeterData>(sheetParameterContainer["rpt_Start"],
                        sheetParameterContainer["rpt_End"], dataSource,
                        new CellFormatter<GatewayMeterData>(sheetParameterContainer["no"], t => num++),
                //new CellFormatter<GatewayMeterData>(sheetParameterContainer["BuildingName"], t => t.BuildingName),
                        new CellFormatter<GatewayMeterData>(sheetParameterContainer["gatewayName"], t => t.gatewayName),
                        new CellFormatter<GatewayMeterData>(sheetParameterContainer["brandName"], t => t.brandName),
                        new CellFormatter<GatewayMeterData>(sheetParameterContainer["access"], t => t.access),
                        new CellFormatter<GatewayMeterData>(sheetParameterContainer["gbcode"], t => t.gbcode),
                        new CellFormatter<GatewayMeterData>(sheetParameterContainer["portNumber"], t => t.portNumber), // t => string.Format("{0}___{1}", t.BuildingName, t.Address)
                        new CellFormatter<GatewayMeterData>(sheetParameterContainer["supplyRegion"], t => t.supplyRegion),
                        new CellFormatter<GatewayMeterData>(sheetParameterContainer["manufactory"], t => t.manufactor),
                        new CellFormatter<GatewayMeterData>(sheetParameterContainer["setupAddress"], t => t.setupAddress)
                        )
                    )
                );



        }


        /// <summary>
        /// 电表台账
        /// </summary>
        /// <param name="filename">打印文件名</param>
        /// <param name="dataSource">数据源</param>
        /// <param name="path">路径</param>
        public void ElecMeterStandingBook(string filename, List<ElecMeterData> dataSource, string path)
        {
            var workbookParameterContainer = new WorkbookParameterContainer();
            workbookParameterContainer.Load(string.Format("{0}{1}.xml", path, "ElecMeterStandingBook"));
            SheetParameterContainer sheetParameterContainer = workbookParameterContainer["电表台账"];

            int num = 1;
            filename = filename == null ? "电表台账" : filename;

            ExportHelper.ExportToWeb(string.Format("{0}{1}.xls", path, "ElecMeterStandingBook"), filename + ".xls",
                new SheetFormatter("电表台账",
                //签章时间
                    new PartFormatter(sheetParameterContainer["TimeNow"], string.Format("{0}年{1}月{2}日", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)),
                    new RepeaterFormatter<ElecMeterData>(sheetParameterContainer["rpt_Start"],
                        sheetParameterContainer["rpt_End"], dataSource,
                        new CellFormatter<ElecMeterData>(sheetParameterContainer["No"], t => num++),
                        new CellFormatter<ElecMeterData>(sheetParameterContainer["energyCategory"], t => t.energyCategory),
                        new CellFormatter<ElecMeterData>(sheetParameterContainer["floor"], t => t.floor),
                        new CellFormatter<ElecMeterData>(sheetParameterContainer["room"], t => t.room),
                        new CellFormatter<ElecMeterData>(sheetParameterContainer["electricName"], t => t.electricName),
                        new CellFormatter<ElecMeterData>(sheetParameterContainer["setupMode"], t => t.setupMode),
                        new CellFormatter<ElecMeterData>(sheetParameterContainer["electricPrecision"], t => t.electricPrecision),
                        new CellFormatter<ElecMeterData>(sheetParameterContainer["ptVoltageRatio"], t => t.ptVoltageRatio),
                        new CellFormatter<ElecMeterData>(sheetParameterContainer["basicCurrent"], t => t.basicCurrent),
                        new CellFormatter<ElecMeterData>(sheetParameterContainer["turndownRatio"], t => t.turndownRatio), //string.Format("{0}___{1}", t.BuildingName, t.Address)
                        new CellFormatter<ElecMeterData>(sheetParameterContainer["communicationSpeed"], t => t.communicationSpeed),
                        new CellFormatter<ElecMeterData>(sheetParameterContainer["brandName"], t => t.brandName),
                        new CellFormatter<ElecMeterData>(sheetParameterContainer["gbCode"], t => t.gbCode),
                        new CellFormatter<ElecMeterData>(sheetParameterContainer["macAddress"], t => t.macAddress),
                        new CellFormatter<ElecMeterData>(sheetParameterContainer["communicationPort"], t => t.communicationPort),
                        new CellFormatter<ElecMeterData>(sheetParameterContainer["port"], t => t.port),
                        new CellFormatter<ElecMeterData>(sheetParameterContainer["supplyRegion"], t => t.supplyRegion),
                        new CellFormatter<ElecMeterData>(sheetParameterContainer["manufactory"], t => t.manufactory),
                        new CellFormatter<ElecMeterData>(sheetParameterContainer["initialValue"], t => t.initialValue),
                        new CellFormatter<ElecMeterData>(sheetParameterContainer["communicationProtocol"], t => t.communicationProtocol),
                        new CellFormatter<ElecMeterData>(sheetParameterContainer["baudRate"], t => t.baudRate)

                        )
                    )
                );



        }


        /// <summary>
        /// 水表台账
        /// </summary>
        /// <param name="filename">打印文件名</param>
        /// <param name="dataSource">数据源</param>
        /// <param name="path">路径</param>
        public void WaterMeterStandingBook(string filename, List<WaterMeterData> dataSource, string path)
        {
            var workbookParameterContainer = new WorkbookParameterContainer();
            workbookParameterContainer.Load(string.Format("{0}{1}.xml", path, "WaterMeterStandingBook"));
            SheetParameterContainer sheetParameterContainer = workbookParameterContainer["水表台账"];

            int num = 1;
            filename = filename == null ? "水表台账" : filename;

            ExportHelper.ExportToWeb(string.Format("{0}{1}.xls", path, "WaterMeterStandingBook"), filename + ".xls",
                new SheetFormatter("水表台账",
                    new PartFormatter(sheetParameterContainer["tabulating"], filename),
                //签章时间
                    new PartFormatter(sheetParameterContainer["timeNow"], string.Format("{0}年{1}月{2}日", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)),
                    new RepeaterFormatter<WaterMeterData>(sheetParameterContainer["rpt_Start"],
                        sheetParameterContainer["rpt_End"], dataSource,
                        new CellFormatter<WaterMeterData>(sheetParameterContainer["no"], t => num++),
                        new CellFormatter<WaterMeterData>(sheetParameterContainer["energyCategory"], t => t.energyCategory),
                        new CellFormatter<WaterMeterData>(sheetParameterContainer["floor"], t => t.floor),
                        new CellFormatter<WaterMeterData>(sheetParameterContainer["room"], t => t.room),
                        new CellFormatter<WaterMeterData>(sheetParameterContainer["waterName"], t => t.waterName),
                        new CellFormatter<WaterMeterData>(sheetParameterContainer["setupMode"], t => t.setupMode),
                        new CellFormatter<WaterMeterData>(sheetParameterContainer["waterPrecision"], t => t.waterPrecision),
                        new CellFormatter<WaterMeterData>(sheetParameterContainer["caliber"], t => t.caliber),
                        new CellFormatter<WaterMeterData>(sheetParameterContainer["waterGage"], t => t.waterGage),
                        new CellFormatter<WaterMeterData>(sheetParameterContainer["flowRate"], t => t.flowRate),
                        new CellFormatter<WaterMeterData>(sheetParameterContainer["communicationProtocol"], t => t.communicationProtocol),
                        new CellFormatter<WaterMeterData>(sheetParameterContainer["communicationSpeed"], t => t.communicationSpeed),
                        new CellFormatter<WaterMeterData>(sheetParameterContainer["brandName"], t => t.brandName),
                        new CellFormatter<WaterMeterData>(sheetParameterContainer["gbCode"], t => t.gbCode),
                        new CellFormatter<WaterMeterData>(sheetParameterContainer["macAddress"], t => t.macAddress),
                        new CellFormatter<WaterMeterData>(sheetParameterContainer["communicationPort"], t => t.communicationPort),
                        new CellFormatter<WaterMeterData>(sheetParameterContainer["port"], t => t.port),
                        new CellFormatter<WaterMeterData>(sheetParameterContainer["supplyRegion"], t => t.supplyRegion),
                        new CellFormatter<WaterMeterData>(sheetParameterContainer["manufactory"], t => t.manufactory),
                        new CellFormatter<WaterMeterData>(sheetParameterContainer["initialValue"], t => t.initialValue),
                        new CellFormatter<WaterMeterData>(sheetParameterContainer["baudRate"], t => t.baudRate),
                        new CellFormatter<WaterMeterData>(sheetParameterContainer["override"], t => t.meterOverride)
                        )
                    )
                );



        }


        /// <summary>
        /// 空白文档
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="dataSource"></param>
        /// <param name="path"></param>
        public void EmptyRecord(string filename, string path)
        {
            var workbookParameterContainer = new WorkbookParameterContainer();
            workbookParameterContainer.Load(string.Format("{0}{1}.xml", path, "EmptyRecord"));
            SheetParameterContainer sheetParameterContainer = workbookParameterContainer["空白记录"];

            filename = filename == null ? "空" : filename;

            ExportHelper.ExportToWeb(string.Format("{0}{1}.xls", path, "EmptyRecord"), filename + ".xls",
                new SheetFormatter("空白记录",
                    new PartFormatter(sheetParameterContainer["Title"], filename),
                    new PartFormatter(sheetParameterContainer["TimeNow"],
                        string.Format("{0}年{1}月{2}日", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)
                        )
                    )
                );
        }


        /// <summary>
        /// 设备操作记录
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="dataSource"></param>
        /// <param name="path"></param>
        public void MetersActionRecord(string filename, List<MetersActionRecordData> dataSource, string path)
        {
            var workbookParameterContainer = new WorkbookParameterContainer();
            workbookParameterContainer.Load(string.Format("{0}{1}.xml", path, "MetersActionRecord"));
            SheetParameterContainer sheetParameterContainer = workbookParameterContainer["设备操作记录"];

            int num = 1;
            filename = filename == null ? "设备操作记录" : filename;

            ExportHelper.ExportToWeb(string.Format("{0}{1}.xls", path, "metersActionRecord"), filename + ".xls",
                new SheetFormatter("设备操作记录",
                    new PartFormatter(sheetParameterContainer["tabulating"], filename),
                //签章时间
                    new PartFormatter(sheetParameterContainer["timeNow"], string.Format("{0}年{1}月{2}日", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)),
                    new RepeaterFormatter<MetersActionRecordData>(sheetParameterContainer["rpt_Start"],
                        sheetParameterContainer["rpt_End"], dataSource,
                        new CellFormatter<MetersActionRecordData>(sheetParameterContainer["no"], t => num++),
                        new CellFormatter<MetersActionRecordData>(sheetParameterContainer["meterName"], t => t.MeterName),
                        new CellFormatter<MetersActionRecordData>(sheetParameterContainer["action"], t => t.ActionName),
                        new CellFormatter<MetersActionRecordData>(sheetParameterContainer["meterType"], t => t.MeterTypeName),
                        new CellFormatter<MetersActionRecordData>(sheetParameterContainer["address"], t => t.MeterAddress),
                        new CellFormatter<MetersActionRecordData>(sheetParameterContainer["macAddress"], t => t.MeterMacAddress),
                        new CellFormatter<MetersActionRecordData>(sheetParameterContainer["building"], t => t.InBuildingName),
                        new CellFormatter<MetersActionRecordData>(sheetParameterContainer["actionTime"], t => t.ActionTime),
                        new CellFormatter<MetersActionRecordData>(sheetParameterContainer["answerTime"], t => t.AnswerTIme),
                        new CellFormatter<MetersActionRecordData>(sheetParameterContainer["answerValue"], t => t.AnswerValue),
                        new CellFormatter<MetersActionRecordData>(sheetParameterContainer["commandStatus"], t => t.CommandStatusName)

                        )
                    )
                );

        }

        /// <summary>
        /// 用户批量导入模板下载
        /// </summary>
        /// <param name="path">导入模板路径</param>
        /// <param name="filename">下载文件名</param>
        /// <param name="userTypeList">所有用户类型列表</param>
        /// <param name="orgList">所有机构类型列表（需带上父节点信息）</param>
        public void UserBulkImportTemplate(string path, string filename,
            List<DictionaryData> userTypeList,
            List<OrganizationData> orgList)
        {
            var workbookParameterContainer = new WorkbookParameterContainer();
            string xmlPath = string.Format("{0}{1}.xml", path, "UserBulkImportTemplate");
            if (!System.IO.File.Exists(xmlPath))
            {
                throw new System.IO.FileNotFoundException("导入模板xml不存在！");
            }
            workbookParameterContainer.Load(xmlPath);
            SheetParameterContainer sheetParameterContainerUser = workbookParameterContainer["附录-用户类型"];
            SheetParameterContainer sheetParameterContainerOrg = workbookParameterContainer["附录-机构"];

            filename = string.IsNullOrEmpty(filename) ? "用户批量导入模板" : filename;

            string templatePath = string.Format("{0}{1}.xls", path, "UserBulkImportTemplate");
            if (!System.IO.File.Exists(templatePath))
            {
                throw new System.IO.FileNotFoundException("导入模板xls不存在！");
            }
            //由于Excel有项功能，下拉序列自动增长，比如“机构1-1”，
            //下拉后第二行自动填充为“机构1-2”，而不会进行数据验证，
            //为防止这个行为，每行数据结尾都加上“-1”
            ExportHelper.ExportToWeb(templatePath, filename + ".xls",
                new SheetFormatter("附录-用户类型",
                    new TableFormatter<DictionaryData>(sheetParameterContainerUser["utype"], userTypeList,
                        new CellFormatter<DictionaryData>(sheetParameterContainerUser["utype"], t => t.ChineseName + "-" + t.Id + "-1")
                        )
                    ),
                  new SheetFormatter("附录-机构",
                    new TableFormatter<OrganizationData>(sheetParameterContainerOrg["org"], orgList,
                        new CellFormatter<OrganizationData>(sheetParameterContainerOrg["org"],
                            t => t.Parent == null ? t.Id.ToString() + "-1" :
                                (t.Parent.Parent == null ? t.Parent.Name + "-" + t.Id + "-1" :
                                t.Parent.Parent.Parent == null ? (t.Parent.Parent.Name + "-" + t.Parent.Name + "-" + t.Name + "-" + t.Id + "-1") :
                                (t.Parent.Parent.Parent.Name + "-" + t.Parent.Parent.Name + "-" + t.Parent.Name + "-" + t.Name + "-" + t.Id + "-1")
                                ))
                    ))
                );
        }

        string getCellValue<T>(out T res, ICell cell, string columnName, bool allowNull = false)
        {
            res = default(T);
            if (cell == null || cell.CellType == CellType.Blank)
            {
                if (!allowNull)
                    return string.Format("{0}不能为空", columnName);
                else
                    return "";
            }
            Type t = typeof(T);
            int i; double d; DateTime dt; object o;
            switch (t.Name)
            {
                case "Int":
                    if (cell.CellType == CellType.Numeric)
                        i = (int)cell.NumericCellValue;
                    else if (cell.CellType == CellType.String)
                    {
                        if (!int.TryParse(cell.StringCellValue.Trim(), out i))
                            return string.Format("{0} 数值输入不正确", columnName);
                    }
                    else return string.Format("{0} 输入格式不正确", columnName);
                    o = i;
                    break;
                case "Double":
                    if (cell.CellType == CellType.Numeric)
                        d = cell.NumericCellValue;
                    else if (cell.CellType == CellType.String)
                    {
                        if (!double.TryParse(cell.StringCellValue.Trim(), out d))
                            return string.Format("{0} 数值输入不正确", columnName);
                    }
                    else return string.Format("{0} 输入格式不正确", columnName);
                    o = d;
                    break;
                case "DateTime":
                    if (cell.CellType == CellType.Numeric)
                        dt = cell.DateCellValue;
                    else if (cell.CellType == CellType.String)
                    {
                        if (!DateTime.TryParse(cell.StringCellValue.Trim(), out dt))
                            return string.Format("{0} 时间输入不正确", columnName);
                    }
                    else return string.Format("{0} 输入格式不正确", columnName);
                    o = dt;
                    break;
                default:
                    if (cell.CellType == CellType.Numeric)
                        o = ((long)cell.NumericCellValue).ToString();
                    else if (cell.CellType == CellType.String)
                    {
                        string s = cell.StringCellValue.Trim();
                        if (!allowNull && string.IsNullOrEmpty(s))
                            return string.Format("{0} 不能为空", columnName);
                        o = s;
                    }
                    else return string.Format("{0} 输入格式不正确", columnName);
                    break;
            }
            res = (T)o;
            return "";
        }
        /// <summary>
        /// 解析导入的用户模板
        /// </summary>
        /// <param name="result">解析结果</param>
        /// <param name="fullPath">导入模板路径</param>
        /// <param name="fileType">导入模板类型xls,xlsx</param>
        /// <param name="sourceFileName">导入模板原文件名，用于多文件导入时区分</param>
        /// <returns></returns>
        public void UserTemlateAnaylsis(ExcelAnalysisResultData<WxPay2017.API.DAL.EmpModels.User> result,
            string fullPath, string fileType, string sourceFileName)
        {
            #region 初始值定义
            if (result == null)
                throw new ArgumentException("导入模板不存在");
            System.IO.FileStream fs = null;
            IWorkbook workbook = null;
            ISheet sheet = null;
            IRow row = null;
            ICell cell = null;
            //固定列
            Dictionary<string, int> fixedColumns = new Dictionary<string, int>() {
                {"机构",-1}, {"学号",-1}, {"用户类型",-1}, 
                {"入学日期",-1}, {"姓名",-1}, {"性别",-1}, 
                {"身份证",-1}, {"邮箱",-1}, {"手机",-1}, 
                {"QQ",-1}, {"微信",-1}};
            #endregion
            using (fs = System.IO.File.OpenRead(fullPath))
            {
                #region 解析文件头
                if (fileType == "xls") { workbook = new HSSFWorkbook(fs); }
                else if (fileType == "xlsx") { workbook = new XSSFWorkbook(fs); }
                else throw new ArgumentException("不支持该类型文件导入");
                if (workbook == null) throw new ArgumentException("上传文件不存在");
                sheet = workbook.GetSheet("能耗用户");
                if (sheet == null) throw new ArgumentException("导入的模板不正确");
                IRow firstRow = sheet.GetRow(0);//第一行  
                if (firstRow == null) throw new ArgumentException("导入的模板不正确");
                int cellCount = firstRow.LastCellNum;//列数
                int rowCount = sheet.LastRowNum;//行数
                if (cellCount != fixedColumns.Count) throw new ArgumentException("导入的模板不正确");
                if (rowCount <= 0) throw new ArgumentException("导入的是空数据");
                #endregion
                #region 解析文件内容
                for (int i = firstRow.FirstCellNum; i < cellCount; i++)
                {
                    cell = firstRow.GetCell(i);
                    if (cell.StringCellValue != null)
                    {
                        if (fixedColumns.Keys.Contains(cell.StringCellValue))
                            fixedColumns[cell.StringCellValue] = i;
                    }
                }
                if (fixedColumns.Values.Contains(-1))
                    throw new ArgumentException("导入的模板不正确:缺少列");
                int all = rowCount;
                result.ALL += all;
                for (int m = 1; m <= rowCount; m++)
                {
                    int i = m + 1;//excel中的行标
                    row = sheet.GetRow(m);
                    #region 排除空行
                    if (row == null)
                    {
                        all--;
                        result.ALL--;
                        if (all <= 0)
                            throw new ArgumentException("导入的模板不正确:空数据");
                        continue;
                    }
                    bool _isnull = true;
                    foreach (int n in fixedColumns.Values)
                    {
                        ICell ce = row.GetCell(n);
                        if (ce != null && ce.CellType != CellType.Blank)
                        {
                            _isnull = false;
                            break;
                        }
                    }
                    if (_isnull)
                    {
                        all--;
                        result.ALL--;
                        if (all <= 0)
                            throw new ArgumentException("导入的模板不正确:空数据");
                        continue;
                    }
                    #endregion
                    #region 填充数据，合法性校验
                    ICell _cell = null;
                    WxPay2017.API.DAL.EmpModels.User udata = new WxPay2017.API.DAL.EmpModels.User();
                    string s, res; DateTime dt;
                    #region 学号
                    res = getCellValue<string>(out s, row.GetCell(fixedColumns["学号"]), "学号", true);
                    if (!string.IsNullOrEmpty(res))
                    { result.AddFailureData(sourceFileName, i, res); continue; }
                    udata.StaffNo = s;
                    #endregion
                    #region 入学日期
                    _cell = row.GetCell(fixedColumns["入学日期"]);
                    res = getCellValue<DateTime>(out dt, row.GetCell(fixedColumns["入学日期"]), "入学日期");
                    if (!string.IsNullOrEmpty(res))
                    { result.AddFailureData(sourceFileName, i, res); continue; }
                    if (dt < DateTime.Now.AddYears(-1) || dt > DateTime.Now.AddYears(1))
                    {
                        result.AddFailureData(sourceFileName, i, string.Format("入学日期不在范围内：{0}-{1}", DateTime.Now.AddYears(-1).ToString("yyyy-MM-dd"), DateTime.Now.AddYears(1).ToString("yyyy-MM-dd"))); continue;
                    }
                    udata.EnrollDate = dt;
                    #endregion
                    #region QQ
                    res = getCellValue<string>(out s, row.GetCell(fixedColumns["QQ"]), "QQ", true);
                    if (!string.IsNullOrEmpty(res))
                    { result.AddFailureData(sourceFileName, i, res); continue; }
                    udata.QQ = s;
                    #endregion
                    #region 微信
                    res = getCellValue<string>(out s, row.GetCell(fixedColumns["微信"]), "微信", true);
                    if (!string.IsNullOrEmpty(res))
                    { result.AddFailureData(sourceFileName, i, res); continue; }
                    udata.WeChat = s;
                    #endregion
                    #region 机构
                    string org;
                    res = getCellValue<string>(out org, row.GetCell(fixedColumns["机构"]), "机构");
                    if (!string.IsNullOrEmpty(res))
                    { result.AddFailureData(sourceFileName, i, res); continue; }
                    string[] orgs = org.Split('-');
                    if (orgs.Length < 2) { result.AddFailureData(sourceFileName, i, "异常机构信息"); continue; }
                    string sorgid = orgs[orgs.Length - 2].Trim();
                    if (containSBC(sorgid)) { result.AddFailureData(sourceFileName, i, "机构包含全角字符"); continue; }
                    int iorgid;
                    if (!int.TryParse(sorgid, out iorgid)) { result.AddFailureData(sourceFileName, i, "机构输入异常"); continue; }
                    udata.OrganizationId = iorgid;
                    #endregion
                    #region 用户类型
                    string utype;
                    res = getCellValue<string>(out utype, row.GetCell(fixedColumns["用户类型"]), "用户类型");
                    if (!string.IsNullOrEmpty(res))
                    { result.AddFailureData(sourceFileName, i, res); continue; }
                    string[] utypes = utype.Split('-');
                    if (utypes.Length < 2) { result.AddFailureData(sourceFileName, i, "异常用户类型信息"); continue; }
                    string sutype = utypes[utypes.Length - 2].Trim();
                    if (containSBC(sutype)) { result.AddFailureData(sourceFileName, i, "用户类型包含全角字符"); continue; }
                    int iutype;
                    if (!int.TryParse(sutype, out iutype)) { result.AddFailureData(sourceFileName, i, "用户类型输入异常"); continue; }
                    udata.UserType = iutype;
                    #endregion
                    #region 学号
                    udata.UserName = udata.StaffNo;
                    if (string.IsNullOrEmpty(udata.UserName)) { result.AddFailureData(sourceFileName, i, "学号不能为空"); continue; }
                    if (containSBC(udata.UserName)) { result.AddFailureData(sourceFileName, i, "学号包含全角字符"); continue; }
                    #endregion
                    #region 姓名
                    res = getCellValue<string>(out s, row.GetCell(fixedColumns["姓名"]), "姓名");
                    if (!string.IsNullOrEmpty(res))
                    { result.AddFailureData(sourceFileName, i, res); continue; }
                    udata.FullName = s;
                    #endregion
                    #region 手机
                    res = getCellValue<string>(out s, row.GetCell(fixedColumns["手机"]), "手机");
                    if (!string.IsNullOrEmpty(res))
                    { result.AddFailureData(sourceFileName, i, res); continue; }
                    udata.PhoneNumber = s;
                    if (containSBC(udata.PhoneNumber)) { result.AddFailureData(sourceFileName, i, "手机包含全角字符"); continue; }
                    if (!IsMobilePhone(udata.PhoneNumber)) { result.AddFailureData(sourceFileName, i, "非法手机号"); continue; }
                    #endregion
                    #region 邮箱
                    res = getCellValue<string>(out s, row.GetCell(fixedColumns["邮箱"]), "邮箱");
                    if (!string.IsNullOrEmpty(res))
                    { result.AddFailureData(sourceFileName, i, res); continue; }
                    udata.Email = s;
                    if (!IsEmail(udata.Email)) { result.AddFailureData(sourceFileName, i, "非法邮箱"); continue; }
                    #endregion
                    #region 身份证
                    res = getCellValue<string>(out s, row.GetCell(fixedColumns["身份证"]), "身份证");
                    if (!string.IsNullOrEmpty(res))
                    { result.AddFailureData(sourceFileName, i, res); continue; }
                    udata.IdentityNo = s;
                    if (!isIdentifyno(udata.IdentityNo)) { result.AddFailureData(sourceFileName, i, "非法身份证号"); continue; }
                    #endregion
                    #region 性别
                    res = getCellValue<string>(out s, row.GetCell(fixedColumns["性别"]), "性别");
                    if (!string.IsNullOrEmpty(res))
                    { result.AddFailureData(sourceFileName, i, res); continue; }
                    if (s != "男" && s != "女") { result.AddFailureData(sourceFileName, i, "非法性别"); continue; }
                    udata.Gender = s == "男";
                    #endregion
                    #endregion
                    lock (result)
                    {
                        result.AddWaitingDealData(sourceFileName, i, udata);
                    }
                }
                #endregion
            }
        }

        #region 字符串校验函数
        /// <summary>
        /// 判断是否包含全角字符，true包含，false不包含
        /// </summary>
        /// <param name="checkString"></param>
        /// <returns></returns>
        private bool containSBC(string checkString)
        {
            return checkString.Length != System.Text.Encoding.Default.GetByteCount(checkString);
        }

        /// <summary>  
        /// 判断输入的字符串是否是一个合法的手机号  
        /// </summary>  
        /// <param name="input"></param>  
        /// <returns></returns>  
        public static bool IsMobilePhone(string input)
        {
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("^1(3|4|5|7|8|)\\d{9}$");
            return regex.IsMatch(input);

        }

        /// <summary>  
        /// 判断输入的字符串是否是一个合法的Email地址  
        /// </summary>  
        /// <param name="input"></param>  
        /// <returns></returns>  
        public static bool IsEmail(string input)
        {
            string pattern = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(pattern);
            return regex.IsMatch(input);
        }
        /// <summary>
        /// 验证身份证是否正确 
        /// </summary> 
        /// <param name="str"></param> 
        /// <returns></returns> 
        private static bool isIdentifyno(string str)
        {
            if (str == null || str.Length != 18)
                return false;
            string number17 = str.Substring(0, 17);
            string number18 = str.Substring(17);
            string check = "10X98765432";
            int[] num = { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2 };
            int sum = 0;
            for (int i = 0; i < number17.Length; i++)
            {
                sum += Convert.ToInt32(number17[i].ToString()) * num[i];
            }
            sum %= 11;
            if (number18.Equals(check[sum].ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}

