using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO.Param
{
    /// <summary>
    /// 对象关系参数
    /// </summary>
    /// <typeparam name="TPrimaryKey">主键集</typeparam>
    /// <typeparam name="TForeignKey">外键集</typeparam>
    public class RelationshipModel<TPrimaryKey, TForeignKey>
    {
        /// <summary>
        /// Primary Keys, 主键
        /// </summary>
        public IList<TPrimaryKey> PKS { get; set; }

        /// <summary>
        /// Foreign Keys， 外键值
        /// </summary>
        public IList<TForeignKey> FKS { get; set; }
    }
}
