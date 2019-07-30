using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO.Param
{
   public class CommonSearchNode
    {
        public Pagination Pagination { get; set; }
        public Dictionary<string, object> ContainValue { get; set; }
        public Dictionary<string,object> EqualValue { get; set; }
        public Dictionary<string, object> GreaterValue { get; set; }
        public Dictionary<string, object> LessValue { get; set; }
        public Dictionary<string, object> NotEqualValue { get; set; }
        public Dictionary<string, object> GreaterEqualValue { get; set; }
        public Dictionary<string, object> LessEqualValue { get; set; }

        public string ClassName { get; set; }
        public string FrameworkName { get; set; }

        public string GetSearchSQL()
        {
            string sb = "1=1 ";
            object results = VO.Common.ReflectionHelper.CreateInstance("DAL", "WxPay2017.API.DAL.Models", "List<" + ClassName + ">");

            if (LessEqualValue != null && LessEqualValue.Values.Count() > 0)
            {
                foreach (var value in LessEqualValue)
                {
                    sb = sb + " and " + value.Key + "<='" + value.Value + "'  ";
                }
            }
            if (GreaterEqualValue != null && GreaterEqualValue.Values.Count() > 0)
            {
                foreach (var value in GreaterEqualValue)
                {
                    sb = sb + " and " + value.Key + ">='" + value.Value + "'  ";
                }
            }
            if (NotEqualValue != null && NotEqualValue.Values.Count() > 0)
            {
                foreach (var value in NotEqualValue)
                {
                    if (value.Value == null)
                        sb = sb + " and " + value.Key + "  is not null  ";
                    else
                       
                        sb = sb + " and " + value.Key + "!='" + value.Value + "'  ";
                }
            }
            if (EqualValue != null && EqualValue.Values.Count() > 0)
            {
                foreach (var value in EqualValue)
                {
                    if (value.Value==null)
                        sb = sb + " and " + value.Key + " is null  ";
                    else
                        sb = sb + " and " + value.Key + "='" + value.Value + "'  ";
                }
            }
            if (ContainValue != null && ContainValue.Values.Count() > 0)
            {
                foreach (var value in ContainValue)
                {
                    sb = sb + " and " + value.Key + " like '%" + value.Value + "%'  ";
                }
            }
            if (GreaterValue != null && GreaterValue.Values.Count() > 0)
            {
                foreach (var value in GreaterValue)
                {
                    sb = sb + " and " + value.Key + ">'" + value.Value + "'  ";
                }
            }
            if (LessValue != null && LessValue.Values.Count() > 0)
            {
                foreach (var value in LessValue)
                {
                    sb = sb + " and " + value.Key + "<'" + value.Value + "'  ";
                }
            }
            string sql = "";
            string desc = " ";

            if (Pagination == null)
            {
                sql = string.Format(" SELECT * FROM [{0}].[{1}] WHERE  {2} ", FrameworkName, ClassName, sb);
            }
            else
            {
                if (Pagination.Sort != null && Pagination.isDesc == true)
                    desc = desc + "desc ";
                Pagination.NoPaging = false;
                if (Pagination.Sort != null)
                    sql = string.Format(" SELECT top {0} * FROM (SELECT ROW_NUMBER() OVER (ORDER BY {5} {6}) AS RowNumber,* FROM [{1}].[{2}] where {3}) A  WHERE RowNumber > {0}*({4}-1)   ", Pagination.Size, FrameworkName, ClassName, sb, Pagination.Index, Pagination.Sort, desc);
                else
                    sql = string.Format(" SELECT top {0} * FROM (SELECT ROW_NUMBER() OVER (ORDER BY {5} {6}) AS RowNumber,* FROM [{1}].[{2}] where {3}) A  WHERE RowNumber > {0}*({4}-1)   ", Pagination.Size, FrameworkName, ClassName, sb, Pagination.Index, "id", desc);
            }
            return sql;
        }
        public string GetCountSQL()
        {
            string sb = "1=1 ";
            object results = VO.Common.ReflectionHelper.CreateInstance("DAL", "WxPay2017.API.DAL.Models", "List<" + ClassName + ">");

            if (EqualValue != null && EqualValue.Values.Count() > 0)
            {
                foreach (var value in EqualValue)
                {
                    sb = sb + " and " + value.Key + "='" + value.Value + "'  ";
                }
            }
            if (ContainValue != null && ContainValue.Values.Count() > 0)
            {
                foreach (var value in ContainValue)
                {
                    sb = sb + " and " + value.Key + " like '%" + value.Value + "%'  ";
                }
            }
            if (GreaterValue != null && GreaterValue.Values.Count() > 0)
            {
                foreach (var value in GreaterValue)
                {
                    sb = sb + " and " + value.Key + ">'" + value.Value + "'  ";
                }
            }
            if (LessValue != null && LessValue.Values.Count() > 0)
            {
                foreach (var value in LessValue)
                {
                    sb = sb + " and " + value.Key + "<'" + value.Value + "'  ";
                }
            }

                string sql = string.Format(" SELECT Count(*) FROM [{0}].[{1}] WHERE  {2} ", FrameworkName, ClassName, sb);
                return sql;

        }

        public string GetSumValueSql()
        {
            string sb = "1=1 ";
            object results = VO.Common.ReflectionHelper.CreateInstance("DAL", "WxPay2017.API.DAL.Models", "List<" + ClassName + ">");

            if (LessEqualValue != null && LessEqualValue.Values.Count() > 0)
            {
                foreach (var value in LessEqualValue)
                {
                    sb = sb + " and " + value.Key + "<='" + value.Value + "'  ";
                }
            }
            if (GreaterEqualValue != null && GreaterEqualValue.Values.Count() > 0)
            {
                foreach (var value in GreaterEqualValue)
                {
                    sb = sb + " and " + value.Key + ">='" + value.Value + "'  ";
                }
            }

            if (EqualValue != null && EqualValue.Values.Count() > 0)
            {
                foreach (var value in EqualValue)
                {
                    sb = sb + " and " + value.Key + "='" + value.Value + "'  ";
                }
            }
            if (ContainValue != null && ContainValue.Values.Count() > 0)
            {
                foreach (var value in ContainValue)
                {
                    sb = sb + " and " + value.Key + " like '%" + value.Value + "%'  ";
                }
            }
            if (GreaterValue != null && GreaterValue.Values.Count() > 0)
            {
                foreach (var value in GreaterValue)
                {
                    sb = sb + " and " + value.Key + ">'" + value.Value + "'  ";
                }
            }
            if (LessValue != null && LessValue.Values.Count() > 0)
            {
                foreach (var value in LessValue)
                {
                    sb = sb + " and " + value.Key + "<'" + value.Value + "'  ";
                }
            }

            string sql = string.Format(" SELECT Sum(Value) FROM [{0}].[{1}] WHERE  {2} ", FrameworkName, ClassName, sb);
            return sql;
        }	
    }
}
