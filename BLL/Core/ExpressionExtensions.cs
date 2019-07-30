using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.BLL
{
    /// <summary>
    /// 谓词表达式构建器
    /// </summary>
    public static class ExpressionExtensions
    {

        public static IOrderedQueryable<TObject> OrderBy<TObject>(this IQueryable<TObject> source, string property)
        {
            return ApplyOrder<TObject>(source, property, "OrderBy");
        }
        public static IOrderedQueryable<TObject> OrderByDescending<TObject>(this IQueryable<TObject> source, string property)
        {
            return ApplyOrder<TObject>(source, property, "OrderByDescending");
        }
        public static IOrderedQueryable<TObject> ThenBy<TObject>(this IOrderedQueryable<TObject> source, string property)
        {
            return ApplyOrder<TObject>(source, property, "ThenBy");
        }
        public static IOrderedQueryable<TObject> ThenByDescending<TObject>(this IOrderedQueryable<TObject> source, string property)
        {
            return ApplyOrder<TObject>(source, property, "ThenByDescending");
        }
        static IOrderedQueryable<TObject> ApplyOrder<TObject>(this IQueryable<TObject> source, string property, string methodName)
        {
            string[] props = property.Split('.');
            Type type = typeof(TObject);
            ParameterExpression arg = Expression.Parameter(type, "x");
            Expression expr = arg;
            //foreach (string prop in props)
            //{
            //    // use reflection (not ComponentModel) to mirror LINQ
            //    PropertyInfo pi = type.GetProperty(prop);
            //    expr = Expression.Property(expr, pi);
            //    type = pi.PropertyType;
            //}

            //var fields = propertyName.Split('.');
            var fieldstr = props[props.Length - 1];
            MemberExpression member = Expression.PropertyOrField(arg, props[0]);

            for (var i = 1; i < props.Length; i++)
            {
                member = Expression.Property(member, props[i]);
            }
            type = member.Type;
            Type delegateType = typeof(Func<,>).MakeGenericType(typeof(TObject), type);
            LambdaExpression lambda = Expression.Lambda(delegateType, member, arg);

            object result = typeof(Queryable).GetMethods().Single(
                    method => method.Name == methodName
                            && method.IsGenericMethodDefinition
                            && method.GetGenericArguments().Length == 2
                            && method.GetParameters().Length == 2)
                    .MakeGenericMethod(typeof(TObject), type)
                    .Invoke(null, new object[] { source, lambda });
            return (IOrderedQueryable<TObject>)result;
        }


        public static Expression<Func<TFirstParam, TResult>>
            Compose<TFirstParam, TIntermediate, TResult>(
            this Expression<Func<TFirstParam, TIntermediate>> first,
            Expression<Func<TIntermediate, TResult>> second)
        {
            var param = Expression.Parameter(typeof(TFirstParam), "param");

            var newFirst = first.Body.Replace(first.Parameters[0], param);
            var newSecond = second.Body.Replace(second.Parameters[0], newFirst);

            return Expression.Lambda<Func<TFirstParam, TResult>>(newSecond, param);
        }
        public static IEnumerable<TSource> SafeFilter<TSource, TKey>
            (this IQueryable<TSource> source,
            Expression<Func<TSource, TKey>> keySelector,
            HashSet<TKey> filterSet,
            int threshold = 500)
        {
            if (filterSet.Count > threshold)
            {
                var selector = keySelector.Compile();
                return source.AsEnumerable()
                    .Where(x => filterSet.Contains(selector(x))); //In memory
            }
            return source.Where(keySelector.Compose(
                key => filterSet.AsEnumerable().Contains(key)));     //In SQL
        }
        public static Expression Replace(this Expression expression,
            Expression searchEx, Expression replaceEx)
        {
            return new ReplaceVisitor(searchEx, replaceEx).Visit(expression);
        }

        //------------------------------------------------------------------

        /// <summary>
        /// 创建一个值恒为 <c>true</c> 的表达式。
        /// </summary>
        /// <typeparam name="T">表达式方法类型</typeparam>
        /// <returns>一个值恒为 <c>true</c> 的表达式。</returns>
        public static Expression<Func<T, bool>> True<T>() { return p => true; }

        /// <summary>
        /// 创建一个值恒为 <c>false</c> 的表达式。
        /// </summary>
        /// <typeparam name="T">表达式方法类型</typeparam>
        /// <returns>一个值恒为 <c>false</c> 的表达式。</returns>
        public static Expression<Func<T, bool>> False<T>() { return f => false; }

        /// <summary>
        /// 使用 Expression.OrElse 的方式拼接两个 System.Linq.Expression。
        /// </summary>
        /// <typeparam name="T">表达式方法类型</typeparam>
        /// <param name="left">左边的 System.Linq.Expression 。</param>
        /// <param name="right">右边的 System.Linq.Expression。</param>
        /// <returns>拼接完成的 System.Linq.Expression。</returns>
        public static Expression<T> Or<T>(this Expression<T> left, Expression<T> right)
        {
            return MakeBinary(left, right, Expression.OrElse);
        }

        /// <summary>
        /// 使用 Expression.AndAlso 的方式拼接两个 System.Linq.Expression。
        /// </summary>
        /// <typeparam name="T">表达式方法类型</typeparam>
        /// <param name="left">左边的 System.Linq.Expression 。</param>
        /// <param name="right">右边的 System.Linq.Expression。</param>
        /// <returns>拼接完成的 System.Linq.Expression。</returns>
        public static Expression<T> And<T>(this Expression<T> left, Expression<T> right)
        {
            return MakeBinary(left, right, Expression.AndAlso);
        }

        /// <summary>
        /// 使用自定义的方式拼接两个 System.Linq.Expression。
        /// </summary>
        /// <typeparam name="T">表达式方法类型</typeparam>
        /// <param name="left">左边的 System.Linq.Expression 。</param>
        /// <param name="right">右边的 System.Linq.Expression。</param>
        /// <param name="func"> </param>
        /// <returns>拼接完成的 System.Linq.Expression。</returns>
        private static Expression<T> MakeBinary<T>(this Expression<T> left, Expression<T> right, Func<Expression, Expression, Expression> func)
        {
            //Debug.Assert(func != null, "func != null");
            return MakeBinary((LambdaExpression)left, right, func) as Expression<T>;
        }

        /// <summary>
        /// 拼接两个 <paramref>
        ///        <name>System.Linq.Expression</name>
        ///      </paramref>  ，两个 <paramref>
        ///                         <name>System.Linq.Expression</name>
        ///                       </paramref>  的参数必须完全相同。
        /// </summary>
        /// <param name="left">左边的 <paramref>
        ///                          <name>System.Linq.Expression</name>
        ///                        </paramref> </param>
        /// <param name="right">右边的 <paramref>
        ///                           <name>System.Linq.Expression</name>
        ///                         </paramref> </param>
        /// <param name="func">表达式拼接的具体逻辑</param>
        /// <returns>拼接完成的 <paramref>
        ///                  <name>System.Linq.Expression</name>
        ///                </paramref> </returns>
        private static LambdaExpression MakeBinary(this LambdaExpression left, LambdaExpression right, Func<Expression, Expression, Expression> func)
        {
            var data = Combinate(right.Parameters, left.Parameters).ToArray();
            right = ParameterReplace.Replace(right, data) as LambdaExpression;
            //Debug.Assert(right != null, "right != null");
            return Expression.Lambda(func(left.Body, right.Body), left.Parameters.ToArray());
        }

        private static IEnumerable<KeyValuePair<T, T>> Combinate<T>(IEnumerable<T> left, IEnumerable<T> right)
        {
            var a = left.GetEnumerator();
            var b = right.GetEnumerator();
            while (a.MoveNext() && b.MoveNext())
                yield return new KeyValuePair<T, T>(a.Current, b.Current);
        }


        //public static Func<TObject, dynamic> GenericSelector<TObject>(string fields)
        //{
        //    var parameter = Expression.Parameter(typeof(TObject), "x");
        //    var a = new { };
        //    var typelist = new List<Type>();
        //    var fieldlsit = new List<string>();
        //    var bindings = fields.Split(',').Select(o => o.Trim()).Select(field =>
        //    {
        //        var f = GetSearchMember(parameter, field);
        //        typelist.Add(f.Type);
        //        fieldlsit.Add(f.Member.Name);
        //        return DynamicExpression.Bind(f.Member, f);
        //    });
        //    var t = GetAnonymousType(fieldlsit.ToArray(), typelist.ToArray());
        //    var xType = Expression.New(t);
        //    var xInit = DynamicExpression.MemberInit(xType, bindings);
        //    //var t = DynamicExpression.MemberInit(Expression.New(dya bindings);
        //    var lambda = Expression.Lambda<Func<TObject, dynamic>>(xInit, parameter);
        //    return lambda.Compile();
        //}

        //private static Type GetAnonymousType(string[] propertyNames, Type[] propertyTypes)
        //{
        //    List<AnonymousMetaProperty> properties = new List<AnonymousMetaProperty>();
        //    for (int i = 0; i < propertyNames.Length; ++i)
        //    {
        //        properties.Add(new AnonymousMetaProperty(propertyNames[i], propertyTypes[i]));
        //    }
        //    return AnonymousTypeHelper.GetAnonymousType(properties.ToArray()).GetClrType();
        //}  


        public static MemberExpression GetSearchMember(ParameterExpression parameter, string key)
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
    }

    #region class: ParameterReplace
    internal sealed class ParameterReplace : ExpressionVisitor
    {
        public static Expression Replace(Expression e, IEnumerable<KeyValuePair<ParameterExpression, ParameterExpression>> paramList)
        {
            var item = new ParameterReplace(paramList);
            return item.Visit(e);
        }

        private readonly Dictionary<ParameterExpression, ParameterExpression> _parameters;

        public ParameterReplace(IEnumerable<KeyValuePair<ParameterExpression, ParameterExpression>> paramList)
        {
            _parameters = paramList.ToDictionary(p => p.Key, p => p.Value, new ParameterEquality());
        }

        protected override Expression VisitParameter(ParameterExpression p)
        {
            ParameterExpression result;
            if (_parameters.TryGetValue(p, out result))
                return result;
            return base.VisitParameter(p);
        }

        #region class: ParameterEquality
        private class ParameterEquality : IEqualityComparer<ParameterExpression>
        {
            public bool Equals(ParameterExpression x, ParameterExpression y)
            {
                if (x == null || y == null)
                    return false;

                return x.Type == y.Type;
            }

            public int GetHashCode(ParameterExpression obj)
            {
                if (obj == null)
                    return 0;

                return obj.Type.GetHashCode();
            }
        }
        #endregion
    }
    #endregion



    internal class ReplaceVisitor : ExpressionVisitor
    {
        private readonly Expression from, to;
        public ReplaceVisitor(Expression from, Expression to)
        {
            this.from = from;
            this.to = to;
        }
        public override Expression Visit(Expression node)
        {
            return node == from ? to : base.Visit(node);
        }
    }
}
