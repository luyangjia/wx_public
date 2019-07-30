using WxPay2017.API.VO.Common;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WxPay2017.API.BLL
{
    public partial class Repository<TObject> : IRepository<TObject> where TObject : class
    {
        public static string[] symbols = new string[] { "$not", "$in", "$nin", "$or", "$ne", "$gte", "$gt", "$lte", "$lt", "$regex", "$exist", "$eq" };

        private Expression REGEX(ParameterExpression parameter, string key, JObject obj)
        {
            List<Expression> list = new List<Expression>();
            JToken token = obj;
            foreach (JProperty property in token)
            {
                var type = property.Value.GetType().Name;
                if (type == "JObject")
                {
                    list.Add(Expression.Equal(GetSearchMember(parameter, key), REGEX(parameter, property.Name, property.Value as JObject)));
                }
                else if (type == "JArray")
                {
                    list.Add(REGEX(parameter, property.Name, property.Value as JArray));
                }
                else
                {
                    list.Add(REGEX(parameter, property.Name, property.Value as JValue));
                }

            }
            return list.Aggregate(Expression.And);
        }

        private BinaryExpression REGEX(ParameterExpression parameter, string key, JValue value)
        {
            MemberExpression member = GetSearchMember(parameter, key);

            var pattern = new Regex(string.Format(@"{0}", value), RegexOptions.IgnoreCase);
            MethodInfo method = typeof(Regex).GetMethod("IsMatch", new[] { typeof(string) });


            if ((member.Type.IsGenericType && member.Type.GetGenericTypeDefinition().Equals(typeof(Nullable<>))))
            {
                var v = value == null ? null : Convert.ChangeType(value, Nullable.GetUnderlyingType(member.Type));
                var hasValueExpression = Expression.Property(member, "HasValue");
                if (v == null) return Expression.Equal(Expression.IsFalse(hasValueExpression), Expression.Constant(false));
                var valueExpression = Expression.Property(member, "Value");
                return Expression.And(hasValueExpression, Expression.Equal(Expression.Call(Expression.Constant(pattern), method, member), Expression.Constant(true)));
            }
            else
            {
                return Expression.Equal(Expression.Call(Expression.Constant(pattern), method, Expression.Coalesce(member, Expression.Constant(""))), Expression.Constant(true));
            }
        }

        private Expression REGEX(ParameterExpression parameter, string key, JArray array)
        {
            //string vt = array.Type.ToString();
            //return array.Select(obj => REGEX(parameter, obj as JObject)).Aggregate(Expression.Or);


            List<Expression> list = new List<Expression>();
            //string s = value.Values().FirstOrDefault().Type.ToString();
            foreach (JToken token in array)
            {
                var type = token.GetType().Name;
                //var type = token.Type.ToString();
                if (type == "JObject")
                {
                    list.Add(REGEX(parameter, key, token as JObject));
                }
                else
                {
                    list.Add(REGEX(parameter, key, token as JValue));
                }
            }
            return list.Aggregate(Expression.Or);
        }


        public Expression EQ(ParameterExpression parameter, string key, JValue value)
        {
            var member = GetSearchMember(parameter, key);
            if ((member.Type.IsGenericType && member.Type.GetGenericTypeDefinition().Equals(typeof(Nullable<>))))
            {
                var v = value == null ? null : Convert.ChangeType(value, Nullable.GetUnderlyingType(member.Type));
                var hasValueExpression = Expression.Property(member, "HasValue");
                if (v == null) return Expression.IsFalse(hasValueExpression);
                var valueExpression = Expression.Property(member, "Value");
                return Expression.AndAlso(hasValueExpression, Expression.Equal(valueExpression, Expression.Constant(v)));

                //if (v == null) return Expression.Equal(member, Expression.Constant(v));
                //return Expression.Equal(member, Expression.Constant(Convert.ChangeType(v, member.Type)));
            }
            else
            {
                return Expression.Equal(member, Expression.Constant(Convert.ChangeType(value, member.Type)));
            }
        }

        public MemberExpression GetSearchMember(ParameterExpression parameter, string key)
        {
            if (key.StartsWith("$")) return null;
            var fields = key.Split('.');
            var fieldstr = fields[fields.Length - 1];
            MemberExpression member = Expression.PropertyOrField(parameter, fields[0]);

            for (var i = 1; i < fields.Length; i++)
            {
                member = Expression.PropertyOrField(member, fields[i]);
            }
            return member;
        }

        public Expression SEARCH(ParameterExpression parameter, string key, JToken tokens)
        {
            List<Expression> list = new List<Expression>();
            List<Expression> sublist = new List<Expression>();
            var type = tokens.GetType().Name;
            if (type == "JArray")
            {
                #region JArray
                if (key == "$or")
                {
                    return tokens.Select(t =>
                    {
                        return t.Select(x => SEARCH(parameter, (x as JProperty).Name, (x as JProperty).Value)).Aggregate(Expression.And);
                    }).Aggregate(Expression.Or);
                }  //end of if(symbols.Contains("key"))
                else
                {
                    return tokens.Select(t => SEARCH(parameter, key, t)).Aggregate(Expression.Or);
                }
                //return tokens.Select(t => SEARCH(parameter, key, t)).Aggregate(Expression.Or);
                #endregion
            }
            else if (type == "JObject")
            {
                #region JObject
                if (symbols.Contains(key))
                {
                    sublist = new List<Expression>();
                    foreach (JProperty property in tokens)
                    {
                        var member = GetSearchMember(parameter, property.Name);
                        //if (symbols.Contains(property.Name)) throw new Exception("语法错误");
                        var vtype = property.Value.GetType().Name;
                        switch (key)
                        {
                            case "$not":
                                //list.Add(Expression.Not(SEARCH(parameter, property.Name, property.Value)));
                                sublist.Add(SEARCH(parameter, property.Name, property.Value));
                                //list.Add(Expression.Not(sublist.Aggregate(Expression.Add)));
                                //if (vtype == "JObject") list.Add(SEARCH(parameter, property.Name, property.Value as JObject));
                                //if (vtype == "JArray") list.Add(SEARCH(parameter, property.Name, property.Value as JArray));
                                //if (vtype == "JValue") list.Add(SEARCH(parameter, property.Name, property.Value as JValue));
                                break;
                            case "$or":
                            //if (vtype == "JObject")
                            //{
                            //    sublist.Add(SEARCH(parameter, property.Name, property.Value as JObject));
                            //}
                            //else if (vtype == "JArray") sublist.Add(SEARCH(parameter, property.Name, property.Value as JArray));
                            ////if (vtype == "JValue") sublist.Add(property.Value.Select(t => SEARCH(parameter, property.Name, t)).Aggregate(Expression.Or));
                            //else if (vtype == "JValue") sublist.Add(SEARCH(parameter, property.Name, property.Value as JValue));
                            ////list.Add(property.Value.Select(t => SEARCH(parameter, property.Name, t)).Aggregate(Expression.Or));
                            //break;
                            case "$in":
                                if (vtype == "JObject") throw new Exception("语法错误");
                                list.Add(property.Value.Select(t => SEARCH(parameter, property.Name, t)).Aggregate(Expression.Or));
                                break;
                            case "$nin":
                                if (vtype == "JObject") throw new Exception("语法错误");
                                list.Add(Expression.Not(property.Value.Select(t => SEARCH(parameter, property.Name, t)).Aggregate(Expression.Or)));
                                break;
                            case "$ne":
                                if (vtype != "JValue") throw new Exception("语法错误");
                                list.Add(Expression.Not(EQ(parameter, property.Name, property.Value as JValue)));
                                //list.Add(Expression.NotEqual(member, Expression.Constant(Convert.ChangeType(property.Value, member.Type), member.Type));
                                break;
                            case "$eq":
                                if (vtype != "JValue") throw new Exception("语法错误");
                                list.Add(EQ(parameter, property.Name, property.Value as JValue));
                                break;
                            case "$gte":
                                if (vtype != "JValue") throw new Exception("语法错误");
                                list.Add(Expression.GreaterThanOrEqual(member, Expression.Constant(Convert.ChangeType(property.Value, member.Type), member.Type)));
                                break;
                            case "$gt":
                                if (vtype != "JValue") throw new Exception("语法错误");
                                list.Add(Expression.GreaterThan(member, Expression.Constant(Convert.ChangeType(property.Value, member.Type), member.Type)));
                                break;
                            case "$lte":
                                if (vtype != "JValue") throw new Exception("语法错误");
                                list.Add(Expression.LessThanOrEqual(member, Expression.Constant(Convert.ChangeType(property.Value, member.Type), member.Type)));
                                break;
                            case "$lt":
                                if (vtype != "JValue") throw new Exception("语法错误");
                                list.Add(Expression.LessThan(member, Expression.Constant(Convert.ChangeType(property.Value, member.Type), member.Type)));
                                break;
                            case "$regex":
                                if (vtype == "JObject") throw new Exception("语法错误");
                                if (vtype == "JArray") list.Add(REGEX(parameter, property.Name, property.Value as JArray));
                                if (vtype == "JValue") list.Add(REGEX(parameter, property.Name, property.Value as JValue));
                                break;

                            default:
                                break;
                        }
                    }
                    if (key == "$not")
                    {
                        list.Add(Expression.Not(sublist.Aggregate(Expression.And)));
                    }
                    if (key == "$or")
                    {
                        list.Add(sublist.Aggregate(Expression.Or));
                    }
                }
                else
                {
                    var member = GetSearchMember(parameter, key);
                    foreach (JProperty property in tokens)
                    {
                        if (!symbols.Contains(property.Name)) throw new Exception("语法错误");
                        var vtype = property.Value.GetType().Name;
                        switch (property.Name)
                        {
                            case "$not":
                                list.Add(Expression.Not(SEARCH(parameter, key, property.Value)));
                                break;
                            case "$or":
                            case "$in":
                                if (vtype == "JObject") throw new Exception("语法错误");
                                list.Add(property.Value.Select(t => SEARCH(parameter, key, t)).Aggregate(Expression.Or));
                                break;
                            case "$nin":
                                if (vtype == "JObject") throw new Exception("语法错误");
                                list.Add(Expression.Not(property.Value.Select(t => SEARCH(parameter, key, t)).Aggregate(Expression.Or)));
                                break;
                            case "$ne":
                                if (vtype != "JValue") throw new Exception("语法错误");
                                list.Add(Expression.Not(EQ(parameter, key, property.Value as JValue)));
                                break;
                            case "$eq":
                                if (vtype != "JValue") throw new Exception("语法错误");
                                list.Add(EQ(parameter, key, property.Value as JValue));
                                break;
                            case "$gte":
                                if (vtype != "JValue") throw new Exception("语法错误");
                                list.Add(Expression.GreaterThanOrEqual(member, Expression.Constant(Convert.ChangeType(property.Value, member.Type), member.Type)));
                                break;
                            case "$gt":
                                if (vtype != "JValue") throw new Exception("语法错误");
                                list.Add(Expression.GreaterThan(member, Expression.Constant(Convert.ChangeType(property.Value, member.Type), member.Type)));
                                break;
                            case "$lte":
                                if (vtype != "JValue") throw new Exception("语法错误");
                                list.Add(Expression.LessThanOrEqual(member, Expression.Constant(Convert.ChangeType(property.Value, member.Type), member.Type)));
                                break;
                            case "$lt":
                                if (vtype != "JValue") throw new Exception("语法错误");
                                list.Add(Expression.LessThan(member, Expression.Constant(Convert.ChangeType(property.Value, member.Type), member.Type)));
                                break;
                            case "$regex":
                                if (vtype == "JObject") throw new Exception("语法错误");
                                if (vtype == "JArray") list.Add(REGEX(parameter, key, property.Value as JArray));
                                if (vtype == "JValue") list.Add(REGEX(parameter, key, property.Value as JValue));
                                break;

                            default:
                                break;
                        }
                    }
                }
                #endregion
                return list.Aggregate(Expression.And);
            }
            else //type == "JValue"
            {
                #region JValue
                if (symbols.Contains(key))
                {
                    throw new Exception("语法错误");
                }
                else
                {
                    return EQ(parameter, key, tokens as JValue);
                }
                #endregion
            }
        }

        public IQueryable<TObject> MYSEARCH(IQueryable<TObject> _list, Dictionary<string, object> dict, ref string expressStr)
        {
            List<Expression> list_exp = new List<Expression>();
            Expression bin = Expression.Empty();
            var parameter = Expression.Parameter(typeof(TObject), "x");
            foreach (var key in dict.Keys)
            {
                var value = dict[key];
                var vt = value == null ? "" : value.GetType().Name;
                if (vt != "JArray" && vt != "JObject")
                {
                    list_exp.Add(EQ(parameter, key, value == null ? null : new JValue(value)));
                }
                else
                {
                    list_exp.Add(SEARCH(parameter, key, value as JToken));
                }
            }
            bin = list_exp.Aggregate(Expression.And);// Expression.Equal(Expression.PropertyOrField(parameter, name), exp_value);
            var lambda = Expression.Lambda<Func<TObject, Boolean>>(bin, parameter);
            expressStr = bin.ToString();
            expressStr = expressStr.Substring(1);
            expressStr = expressStr.Substring(0, expressStr.Length - 1);
            expressStr = Regex.Replace(expressStr, "x.([^.]*).HasValue AndAlso", "");
            //expressStr = Regex.Replace(expressStr, "IsFalse(x.([^.]*).HasValue)", "");
            for (; ; )
            {
                if (expressStr.Contains("IsFalse(x."))
                {
                    string s1 = expressStr.Substring(0, expressStr.IndexOf("IsFalse(x."));
                    string s2 = expressStr.Substring(expressStr.IndexOf("IsFalse(x.") + 10);
                    string s3 = s2.Substring(0, s2.IndexOf(".HasValue)"));
                    string s4 = s2.Substring(s2.IndexOf(".HasValue)") + 10);
                    expressStr = s1 + s3 + " is null " + s4;
                }
                else
                    break;
            }
            expressStr = expressStr.Replace("x.", "");
            expressStr = expressStr.Replace("==", "=");
            expressStr = expressStr.Replace(".Value", "");
            expressStr = expressStr.Replace("= True", "=1");
            expressStr = expressStr.Replace("= False", "=0");
            expressStr = expressStr.Replace("\"", "\'");
            //Regex.Replace(expressStr, "", "x.(.*).HasValue AndAlso");
            var result = _list.Where(lambda.Compile()).AsQueryable();
            return result;
        }
        //public IQueryable<TObject> MYSEARCH(IQueryable<TObject> _list, Dictionary<string, object> dict)
        //{
        //    List<Expression> list_exp = new List<Expression>();
        //    Expression bin = Expression.Empty();
        //    var parameter = Expression.Parameter(typeof(TObject), "x");
        //    foreach (var key in dict.Keys)
        //    {
        //        var value = dict[key];
        //        var vt = value == null ? "" : value.GetType().Name;
        //        if (vt != "JArray" && vt != "JObject")
        //        {
        //            list_exp.Add(EQ(parameter, key, value == null ? null : new JValue(value)));
        //        }
        //        else
        //        {
        //            list_exp.Add(SEARCH(parameter, key, value as JToken));
        //        }
        //    }
        //    bin = list_exp.Aggregate(Expression.And);// Expression.Equal(Expression.PropertyOrField(parameter, name), exp_value);
        //    var lambda = Expression.Lambda<Func<TObject, Boolean>>(bin, parameter);

        //    return _list.Where(lambda.Compile()).AsQueryable();
        //}
        public Expression<Func<TObject, dynamic>> GenericSelector
            (ParameterExpression parameter, string fields)
        {
            var list = new List<ParameterExpression>();
            var mems = new List<MemberExpression>();
            var xNew = Expression.New(typeof(TObject));
            var bindings = fields.Split(',').Select(o => o.Trim()).Select(field =>
            {
                var f = GetSearchMember(parameter, field);
                return Expression.Bind(f.Member, f);
            });
            var xInit = Expression.MemberInit(xNew, bindings);

            var lambda = Expression.Lambda<Func<TObject, dynamic>>(xInit, parameter);
            return lambda;
        }


    }
}
