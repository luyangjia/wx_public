using WxPay2017.API.DAL.EmpModels;
using WxPay2017.API.VO;
using WxPay2017.API.VO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WxPay2017.API.BLL
{
    public static class DictionaryExtension
    {
        public static DictionaryData ToViewData(this Dictionary node, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (node == null)
                return null;
            var model = new DictionaryData()
            {
                Id = node.Id,
                Code = node.Code,
                TreeId = node.TreeId,
                GbCode = node.GbCode,
                FirstValue = node.FirstValue,
                SecondValue = node.SecondValue,
                ThirdValue = node.ThirdValue,
                FourthValue = node.FourthValue,
                FifthValue = node.FifthValue,
                ChineseName = node.ChineseName,
                EnglishName = node.EnglishName,
                Enable = node.Enable,
                IsTypeOfStatisticalAnalysis = node.Code == "EnergyCategory" && node.HasMeterResult(),
                EquText = node.EquText,
                EquValue = node.EquValue,
                Description = node.Description
            };

            if ((suffix & CategoryDictionary.Parent) == CategoryDictionary.Parent && node.Parent() != null)
                model.Parent = node.Parent().ToViewData();
            if ((suffix & CategoryDictionary.Parameter) == CategoryDictionary.Parameter && node.Code == "EnergyCategory")
            {
                if (model.Parameters == null)
                    model.Parameters = new List<DictionaryData>();
                model.Parameters = node.Parameters().Select(x => x.ToViewData(suffix)).ToList();
            }
            return model;
        }
        public static IEnumerable<DictionaryData> ToViewList(this IQueryable<Dictionary> nodes, CategoryDictionary suffix = CategoryDictionary.None)
        {
            return nodes.ToList().Select(x => x.ToViewData(suffix));
        } 

        private static bool HasMeterResult(this Dictionary node)
        {
            try
            {
                return node.Descendants(true).SelectMany(n => n.MetersByEnergyCategory).Any(x => x.MeterMonthlyResults.Count > 0);
            }
            catch (Exception)
            {
                var ctx = new EmpContext();
                var ids = node.Descendants(true).Select(x => x.Id);
                return ctx.Dictionaries.Where(x => ids.Contains(x.Id)).SelectMany(n => n.MetersByEnergyCategory).Any(x => x.MeterMonthlyResults.Count > 0);

            }
        }

        public static Dictionary ToModel(this DictionaryData node)
        {
            var model = new Dictionary()
            {
                Id = node.Id,
                Code = node.Code,
                GbCode = node.GbCode,
                FirstValue = node.FirstValue,
                SecondValue = node.SecondValue,
                ThirdValue = node.ThirdValue,
                FourthValue = node.FourthValue,
                FifthValue = node.FifthValue,
                ChineseName = node.ChineseName,
                EnglishName = node.EnglishName,
                Enable = node.Enable,
                Description = node.Description
            };
            return model;
        }

        private static IEnumerable<Dictionary> GetAllCategories(this Dictionary node)
        {
            IEnumerable<Dictionary> list = null;
            switch (node.Code)
            {
                case "EnergyCategory":
                    list = SystemInfo.AllEnergyCategories;
                    break;
                case "BuildingCategory":
                    list = SystemInfo.AllBuildingCategories;
                    break;
                default:
                    throw new System.IndexOutOfRangeException("字典代码（Dictionary.Code）不在指定范围内！");

            }
            return list;
        }

        /// <summary>
        /// 直系子级
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static IEnumerable<Dictionary> Children(this Dictionary node)
        {
            return node.GetAllCategories().Where(DictionaryExtension.ChildrenFunc(node));
        }

        private static Func<Dictionary, bool> ChildrenFunc(Dictionary node)
        {
            Regex regx = new Regex(node.TreeId + "-\\d{2}$");
            return x => regx.IsMatch(x.TreeId);
            //Func<Dictionary, bool> func = x => node.Id != x.Id && node.FirstValue == x.FirstValue
            //    && (!node.SecondValue.HasValue && x.SecondValue.HasValue || x.SecondValue == node.SecondValue)
            //    && (!node.ThirdValue.HasValue && x.ThirdValue.HasValue == node.SecondValue.HasValue || x.ThirdValue == node.ThirdValue)
            //    && (!node.FourthValue.HasValue && x.FourthValue.HasValue == node.ThirdValue.HasValue || x.FourthValue == node.FourthValue)
            //    && (!node.FifthValue.HasValue && x.FifthValue.HasValue == node.FourthValue.HasValue || x.FifthValue == node.FifthValue);
            //return func;
        }

        /// <summary>
        /// 所有后代
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static IEnumerable<Dictionary> Descendants(this Dictionary node, bool includeSelf = false)
        {
            return node.GetAllCategories().Where(DescendantsFunc(node, includeSelf));
        }

        private static Func<Dictionary, bool> DescendantsFunc(Dictionary node, bool includeSelf)
        {
            return x => x.TreeId.StartsWith(node.TreeId + (includeSelf ? string.Empty : "-"));
            //Func<Dictionary, bool> func = x => (includeSelf || node.Id != x.Id) && node.FirstValue == x.FirstValue
            //    && (!node.SecondValue.HasValue || (x.SecondValue == node.SecondValue
            //    && (!node.ThirdValue.HasValue || (x.ThirdValue == node.ThirdValue
            //    && (!node.FourthValue.HasValue || (x.FourthValue == node.FourthValue
            //    && (!node.FifthValue.HasValue || x.FifthValue == node.FifthValue)))))));
            //return func;
        }

        /// <summary>
        /// 所有前代
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static IEnumerable<Dictionary> Ancestors(this Dictionary node)
        {
            return node.GetAllCategories().Where(AncestorsFunc(node));
        }

        private static Func<Dictionary, bool> AncestorsFunc(Dictionary node)
        {
            var segs = node.TreeId.Split('-');
            Expression<Func<Dictionary, bool>> exp = x => false;
            var reducer = "";
            if (segs.Length > 0)
            {
                for (int i = 1; i < segs.Length - 1; i++)
                {
                    reducer += (i == 1 ? segs[0] + "-" : "-") + segs[i];
                    Regex regx = new Regex(reducer + "$");
                    Expression<Func<Dictionary, bool>> lambda = x => regx.IsMatch(x.TreeId);
                    exp = exp.Or(lambda);
                }
            }
            return exp.Compile();
            //Func<Dictionary, bool> func = x => node.Id != x.Id && node.FirstValue == x.FirstValue &&
            //            (!node.SecondValue.HasValue && !x.FirstValue.HasValue ||
            //            (!node.ThirdValue.HasValue && !x.SecondValue.HasValue || (!x.SecondValue.HasValue || node.SecondValue == x.SecondValue && (
            //            (!node.FourthValue.HasValue && !x.ThirdValue.HasValue || (!x.ThirdValue.HasValue || node.ThirdValue == x.ThirdValue && (
            //            (!node.FifthValue.HasValue && !x.FourthValue.HasValue || (!x.FourthValue.HasValue || node.FourthValue == x.FourthValue)))))))));
            //return func;
        }

        /// <summary>
        /// 当前对象的父对象
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static Dictionary Parent(this Dictionary node)
        {
            return node.GetAllCategories().FirstOrDefault(x => x.TreeId == node.TreeId.Substring(0, node.TreeId.Length - 3));
            //return node.GetAllCategories().FirstOrDefault(x => node.Id != x.Id && node.FirstValue == x.FirstValue &&
            //            (!node.SecondValue.HasValue && !x.FirstValue.HasValue || (node.FirstValue == x.FirstValue && (
            //            (!node.ThirdValue.HasValue && !x.SecondValue.HasValue || (node.SecondValue == x.SecondValue && (
            //            (!node.FourthValue.HasValue && !x.ThirdValue.HasValue || (node.ThirdValue == x.ThirdValue && (
            //            (!node.FifthValue.HasValue && !x.FourthValue.HasValue || node.FourthValue == x.FourthValue)))))))))));
        }


        /// <summary>
        /// 当前对象的根对象
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static Dictionary Root(this Dictionary node)
        {
            return node.GetAllCategories().FirstOrDefault(x => node.FirstValue == x.FirstValue && !x.SecondValue.HasValue);
        }
        /// <summary>
        /// 获取直系子对象列表
        /// </summary>
        /// <param name="node"></param>
        /// <param name="predicate">过滤条件</param>
        /// <returns></returns>
        public static IEnumerable<Dictionary> Children(this Dictionary node, Expression<Func<Dictionary, bool>> predicate)
        {
            return node.Children().Where(predicate.Compile());
        }
        /// <summary>
        /// 获取所有后代对象列表
        /// </summary>
        /// <param name="node"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IEnumerable<Dictionary> Descents(this Dictionary node, Expression<Func<Dictionary, bool>> predicate)
        {
            return node.Descendants().Where(predicate.Compile());
        }

        /// <summary>
        /// 获取所有前代对象
        /// </summary>
        /// <param name="node"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IEnumerable<Dictionary> Ancestors(this Dictionary node, Expression<Func<Dictionary, bool>> predicate)
        {
            return node.Ancestors().Where(predicate.Compile());
        }

        public static IEnumerable<Dictionary> Parameters(this Dictionary node)
        {
            if (node == null || node.Code != "EnergyCategory") return null;
            var ecs = node.Descendants(true).Select(x => x.Id);
            var ctx = new EmpContext();
            return ctx.Meters.Where(x => ecs.Contains(x.EnergyCategoryId)).SelectMany(x => x.MeterMonthlyResults).Where(x => x.Parameter.Type.SecondValue.HasValue && x.Parameter.Type.SecondValue.Value == 1).Select(x => x.Parameter.Type).Distinct().ToList();
        }
    }
}
