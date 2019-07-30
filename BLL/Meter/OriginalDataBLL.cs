using WxPay2017.API.DAL.EmpModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WxPay2017.API.VO;
using WxPay2017.API.VO.Common;
using Newtonsoft.Json;
using System.Linq.Expressions;
using System.Transactions;
using System.Globalization;

namespace WxPay2017.API.BLL
{
    public class OriginalDataBLL : Repository<OriginalData>
    {

        public OriginalDataBLL(EmpContext context = null)
            : base(context)
        {
        }
        /// <summary>
        /// 获得原始数据读值
        /// </summary>
        /// <param name="id">相应meter的id值</param>
        /// <param name="ids">parameter的参数id，多个用隔开</param>
        /// <param name="count">数量</param>
        /// <param name="count">dataSource为空，则从数据库查询，否则从此缓存查询，加快批量查询速度</param>
        /// <returns></returns>
        public Dictionary<int, List<OriginalDataForReport>> GetByParaMetersId(int id, int count, string ids,List<OriginalData> dataSource=null)
        {
            var meter = db.Meters.FirstOrDefault(o => o.Id == id);
            if (meter == null)
                return null;
            string[] array = ids.Split(',');
            Dictionary<int, List<OriginalDataForReport>> result = new Dictionary<int, List<OriginalDataForReport>>();
            foreach (string idStr in array)
            {
                int parameterId = Convert.ToInt32(idStr);
                var parameter = db.Parameters.First(o => o.Id == parameterId);
                var thirdValue = parameter.Type.ThirdValue;
                string name = "Parameter";
                if (thirdValue < 10)
                    name = name + "0" + thirdValue;
                else
                    name = name + thirdValue;

                List<OriginalData> listBase2 = null;
                List<OriginalDataForReport> list2 = null;
                var list = db.OriginalDatas.Where(o => o.MeterId == id && o.Status == DictionaryCache.ValidDataCode.Id + "");
                if (dataSource!=null)
                    listBase2 = dataSource.Where(o => o.MeterId == id && o.Status == DictionaryCache.ValidDataCode.Id + "").ToList();

                Type type = typeof(OriginalData);
                var property = type.GetProperty(name, System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                var param = Expression.Parameter(type, "x");
                var member = Expression.PropertyOrField(param, name);
                var v0 = Expression.New(typeof(OriginalDataForReport));
                var v1 = typeof(OriginalDataForReport).GetProperty("Value");

                string dataPamaStr = "CollectTime";
                var memberData = Expression.PropertyOrField(param, dataPamaStr);
                var v2 = typeof(OriginalDataForReport).GetProperty("Time");

                var v3 = typeof(OriginalDataForReport).GetProperty("ParameterId");
                var v4 = typeof(OriginalDataForReport).GetProperty("ParameterName");
                Expression bin = Expression.MemberInit(v0,
                new MemberBinding[] 
                {
                    Expression.Bind(v1, member),
                    Expression.Bind(v2, memberData)
                });
                var lambda = Expression.Lambda<Func<OriginalData, OriginalDataForReport>>(bin, param);
                if (listBase2==null)
                    list2 = list.Select(lambda.Compile()).OrderByDescending(o => o.Time).Take(count).ToList();
                else
                    list2 = listBase2.Select(lambda.Compile()).OrderByDescending(o => o.Time).Take(count).ToList();
                foreach (var l in list2)
                {
                    if (DictionaryCache.RatioParameterList.Contains(parameter.Type.Id))
                        l.Value = l.Value * meter.Rate;
                    else
                        l.Value = l.Value;
                    l.ParameterId = parameterId;
                    l.Unit = parameter.Unit;
                    l.ParameterName = parameter.Type.ChineseName;
                }
                result.Add(parameterId, list2);
            }
            return result;
        }

    }
}
