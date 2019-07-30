using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace WxPay2017.API.VO.Param
{
    /// <summary>
    /// 分页
    /// </summary>
    public class Pagination
    {
        public Pagination()
        {
            Index = 1;
            Size = 50;
            Count = 0;
            All = 0;
            NoPaging = true;
            //Sort = new JObject();
            //Sort = new Dictionary<string, string>();
        }
        /// <summary>
        /// 页码
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// 每页条数
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 总条数
        /// </summary>
        public int All { get; set; }

        /// <summary>
        /// 是否分页， 默认值： false，表示要分页
        /// </summary>
        public bool NoPaging { get; set; }

        /// <summary>
        /// 排序字段列表
        /// </summary>
        public string Sort { get; set; }

        public bool isDesc { get; set; }
        //public Dictionary<string, string> Sort { get; set; }

        public Dictionary<string, object> Filter { get; set; }

        public string Fields { get; set; }
    }
}
