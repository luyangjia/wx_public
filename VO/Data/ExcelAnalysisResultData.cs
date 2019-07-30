using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO
{
    /// <summary>
    /// Excel导入分析后的结果
    /// </summary>
    public class ExcelAnalysisResultData<T> : ICloneable
    {
        /// <summary>
        /// Excel导入分析后的结果
        /// </summary>
        /// <param name="UserID">导入用户ID</param>
        /// <param name="ID">结果ID，可用户指定，不指定为自动生成的ID</param>
        public ExcelAnalysisResultData(string UserID, string ID = null)
        {
            if (string.IsNullOrEmpty(ID))
            {
                this.ID = UserID + DateTime.Now.Ticks.ToString() + "-" + (new Random().Next(1, 9999).ToString());
            }
            else
                this.ID = ID;
            this.IsDealDataLoaded = false;
            this.IsOver = false;
            this.Success = 0;
        }
        /// <summary>
        /// 导入批次ID
        /// </summary>
        public string ID { get; private set; }
        /// <summary>
        /// 状态信息
        /// </summary>
        public string State { get; set; }
        /// <summary>
        /// Excel所有数据条数
        /// </summary>
        public int ALL { get; set; }
        /// <summary>
        /// 是否结束
        /// </summary>
        public bool IsOver { get; set; }
        /// <summary>
        /// 等待处理的数据是否加载完毕
        /// </summary>
        public bool IsDealDataLoaded { get; set; }
        /// <summary>
        /// 成功导入数
        /// </summary>
        public int Success { get; set; }
        //不保存成功数据，如需保存，在这里取消掉注释
        //get { return SuccessDatas == null ? 0 : SuccessDatas.Count; } }
        /// <summary>
        /// 等待处理数
        /// </summary>
        public int WaitingDeal { get { return WaitingDealDatas == null ? 0 : WaitingDealDatas.Count; } }
        /// <summary>
        /// 失败导入数
        /// </summary>
        public int Failure { get { return FailureDatas == null ? 0 : FailureDatas.Count; } }
        /// <summary>
        /// 当前正在分析第？条数据
        /// 从0开始
        /// </summary>
        public int Current { get { return Success + Failure; } }
        /// <summary>
        /// 当前进度百分比
        /// </summary>
        public double Proccess { get { return ALL == 0 ? 0 : Current * 100.0 / ALL; } }

        /// <summary>
        /// 等待处理数据队列
        /// </summary>
        public Queue<ExcelAnalysisWaitingDealData<T>> WaitingDealDatas { get; set; }
        /// <summary>
        /// 成功数据(不保存成功数据，如需保存，在这里取消掉注释)
        /// </summary>
        //public List<T> SuccessDatas { get; set; }
        /// <summary>
        /// 失败数据
        /// </summary>
        public List<ExcelAnalysisFailureData> FailureDatas { get; set; }

        public object Clone()
        {

            return new ExcelAnalysisResultData<T>(null, this.ID)
            {
                ALL = this.ALL,
                FailureDatas = this.FailureDatas == null ? null : new List<ExcelAnalysisFailureData>(this.FailureDatas),
                IsDealDataLoaded = this.IsDealDataLoaded,
                IsOver = this.IsOver,
                State = this.State,
                Success = this.Success,
                //不在前台显示待处理数据
                //WaitingDealDatas = new Queue<ExcelAnalysisWaitingDealData<T>>(this.WaitingDealDatas)
            };
        }

    }

    /// <summary>
    /// Excel分析结果缓存
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class ExcelAnalysisCache<T>
    {
        public static Dictionary<string, ExcelAnalysisResultData<T>> Cache = new Dictionary<string, ExcelAnalysisResultData<T>>();
        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="id">缓存标识，使用用户ID</param>
        public static ExcelAnalysisResultData<T> Add(string id)
        {
            lock (Cache)
            {
                if (Cache.ContainsKey(id))
                {
                    if (Cache[id] != null && !Cache[id].IsOver)
                        throw new ArgumentException("您已经创建了一个导入过程，并且尚未结束，过程ID：" + Cache[id].ID);
                    Cache[id] = new ExcelAnalysisResultData<T>(id);
                }
                else
                    Cache.Add(id, new ExcelAnalysisResultData<T>(id));
                return Cache[id];
            }
        }
        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="id">缓存标识，使用用户ID</param>
        /// <param name="expired">缓存保存有效期（单位：秒）</param>
        public static void RemoveAsync(string id, int expired = 60)
        {
            new Task(() =>
            {
                DateTime start = DateTime.Now;
                while (DateTime.Now - start < TimeSpan.FromSeconds(expired))
                {
                    System.Threading.Thread.Sleep(1000);
                }
                lock (Cache)
                {
                    if (Cache.ContainsKey(id))
                        Cache.Remove(id);
                }
            }).Start();
        }
        /// <summary>
        /// 返回分析结果
        /// </summary>
        /// <param name="id">缓存标识，使用用户ID</param>
        /// <returns></returns>
        public static ExcelAnalysisResultData<T> Get(string id)
        {
            lock (Cache)
            {
                if (Cache.ContainsKey(id) && Cache[id] != null)
                {
                    lock (Cache[id])
                    {
                        return Cache[id];
                    }
                }
                return null;
            }
        }
    }
    /// <summary>
    /// 扩展方法
    /// </summary>
    public static class ExcelAnalysisExtension
    {
        /// <summary>
        /// 添加等待处理数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sourceData"></param>
        /// <param name="fileName"></param>
        /// <param name="index"></param>
        /// <param name="waitingDealData"></param>
        public static void AddWaitingDealData<T>(this ExcelAnalysisResultData<T> sourceData, string fileName, int index, T waitingDealData)
        {
            lock (sourceData)
            {
                if (sourceData.WaitingDealDatas == null)
                    sourceData.WaitingDealDatas = new Queue<ExcelAnalysisWaitingDealData<T>>(1000);
                ExcelAnalysisWaitingDealData<T> data = new ExcelAnalysisWaitingDealData<T>();
                data.SourceFileName = fileName;
                data.Index = index;
                data.Data = waitingDealData;
                sourceData.WaitingDealDatas.Enqueue(data);
            }
        }
        /// <summary>
        /// 添加成功处理数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sourceData"></param>
        /// <param name="successData"></param>
        public static void AddSuccessData<T>(this ExcelAnalysisResultData<T> sourceData, T successData)
        {
            lock (sourceData)
                sourceData.Success = sourceData.Success + 1;
            //不保存成功数据，如需保存，在这里取消掉注释
            //if (sourceData.SuccessDatas == null)
            //    sourceData.SuccessDatas = new List<T>();
            //lock(sourceData.SuccessDatas)
            //    sourceData.SuccessDatas.Add(successData);
        }
        /// <summary>
        /// 添加失败处理数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sourceData"></param>
        /// <param name="failureData"></param>
        public static void AddFailureData<T>(this ExcelAnalysisResultData<T> sourceData, ExcelAnalysisFailureData failureData)
        {
            lock (sourceData)
            {
                if (sourceData.FailureDatas == null)
                    sourceData.FailureDatas = new List<ExcelAnalysisFailureData>();
                sourceData.FailureDatas.Add(failureData);
            }
        }
        /// <summary>
        /// 添加失败处理数据
        /// </summary>
        /// <typeparam name="T">数据</typeparam>
        /// <param name="sourceData"></param>
        /// <param name="fileName">数据所在文件名</param>
        /// <param name="index">Excel行标</param>
        /// <param name="failureReason">失败原因</param>
        public static void AddFailureData<T>(this ExcelAnalysisResultData<T> sourceData, string fileName, int index, string failureReason)
        {
            lock (sourceData)
            {
                if (sourceData.FailureDatas == null)
                    sourceData.FailureDatas = new List<ExcelAnalysisFailureData>();
                sourceData.FailureDatas.Add(new ExcelAnalysisFailureData(fileName, index, failureReason));
            }
        }
    }

    /// <summary>
    /// Excel导入分析后失败数据
    /// </summary>
    public class ExcelAnalysisFailureData
    {
        /// <summary>
        /// 失败文件名
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// Excel文件行标，从2开始，1为标题
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// 失败原因
        /// </summary>
        public string FailureReason { get; set; }

        /// <summary>
        /// Excel解析失败数据
        /// </summary>
        /// <param name="fileName">失败数据所在源导入文件名</param>
        /// <param name="index">失败数据所在行标，从2开始，1为标题</param>
        /// <param name="failureReason">失败原因</param>
        public ExcelAnalysisFailureData(string fileName, int index, string failureReason)
        {
            this.FileName = fileName;
            this.Index = index;
            this.FailureReason = failureReason;
        }
    }

    /// <summary>
    /// Excel导入分析后的等待处理的数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ExcelAnalysisWaitingDealData<T>
    {
        /// <summary>
        /// 源文件名
        /// </summary>
        public string SourceFileName { get; set; }
        /// <summary>
        /// 文件
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// 成功数据
        /// </summary>
        public T Data { get; set; }
    }
}
