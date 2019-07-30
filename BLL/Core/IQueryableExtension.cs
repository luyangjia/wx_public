using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Linq;

namespace WxPay2017.API.BLL
{
    public static class IQueryableExtension
    {
       
        public static decimal SumZero<TSource>( this IQueryable<TSource> source, Expression<Func<TSource, decimal>> selector)
        {
            try
            {
                return source.Sum(selector);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("failed because the materialized value is null"))
                    return 0;
                else
                    throw ex;
            }
        }

        public static decimal? SumZero<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, decimal?>> selector)
        {
            try
            {
                return source.Sum(selector);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("failed because the materialized value is null"))
                    return 0;
                else
                    throw ex;
            }
        }

        public static int SumZero<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, int>> selector)
        {
            try
            {
                return source.Sum(selector);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("failed because the materialized value is null"))
                    return 0;
                else
                    throw ex;
            }
        }
        public static int? SumZero<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, int?>> selector)
        {
            try
            {
                return source.Sum(selector);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("failed because the materialized value is null"))
                    return 0;
                else
                    throw ex;
            }
        }
        public static decimal SumZero<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector)
        {
            try
            {
                return source.Sum(selector);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("failed because the materialized value is null"))
                    return 0;
                else
                    throw ex;
            }
        }

        public static decimal? SumZero<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal?> selector)
        {
            try
            {
                return source.Sum(selector);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("failed because the materialized value is null"))
                    return 0;
                else
                    throw ex;
            }
        }

        public static int SumZero<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector)
        {
            try
            {
                return source.Sum(selector);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("failed because the materialized value is null"))
                    return 0;
                else
                    throw ex;
            }
        }
        public static int? SumZero<TSource>(this IEnumerable<TSource> source, Func<TSource, int?> selector)
        {
            try
            {
                return source.Sum(selector);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("failed because the materialized value is null"))
                    return 0;
                else
                    throw ex;
            }
        }
    }
}
