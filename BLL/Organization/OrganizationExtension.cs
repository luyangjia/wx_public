using WxPay2017.API.DAL.EmpModels;
using WxPay2017.API.VO.Common;
using WxPay2017.API.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WxPay2017.API.BLL
{
    public static class OrganizationExtension
    {
        public static OrganizationData ToViewData(this Organization node, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (node == null)
                return null;
            var model = new OrganizationData()
            {
                Id = node.Id,
                ParentId = node.ParentId,
                TreeId = node.TreeId,
                Rank = node.Rank,
                Parent = (suffix & CategoryDictionary.Parent) == CategoryDictionary.Parent && node.ParentId.HasValue ? node.Parent.ToViewData(suffix) : null,
                HasChildren = node.Children.Count,
                Children = (suffix & CategoryDictionary.Children) == CategoryDictionary.Children ? node.Children.ToList().Select(c => c.ToViewData()).ToList() : null,
                Name = node.Name,
                AliasName = node.AliasName,
                Initial = node.Initial,
                Type = node.Type,
                TypeDict = (suffix & CategoryDictionary.Dictionary) == CategoryDictionary.Dictionary ? node.TypeDict.ToViewData() : null,
                ManagerCount = node.ManagerCount,
                CustomerCount = node.CustomerCount,
                ProvinceId = node.ProvinceId,
                CityId = node.CityId,
                DistrictId = node.DistrictId,
                Enable = node.Enable,
                Users = (suffix & CategoryDictionary.User) == CategoryDictionary.User ? node.Users.ToList().Select(u => u.ToViewData()).ToList() : null,
                Buildings = (suffix & CategoryDictionary.Building) == CategoryDictionary.Building ? node.Buildings.ToList().Select(b => b.ToViewData()).ToList() : null,
                Description = node.Description
            };
            if ((suffix & CategoryDictionary.Descendant) == CategoryDictionary.Descendant)
            {
                model.Descendants = node.Descendants(false).Select(x => x.ToViewData()).ToList();
            }
            if ((suffix & CategoryDictionary.Ancestor) == CategoryDictionary.Ancestor)
            {
                model.Ancestors = node.Ancestors().Select(x => x.ToViewData()).ToList();
            }

            if ((suffix & CategoryDictionary.ExtensionField) == CategoryDictionary.ExtensionField)
            {
                var ctx = new EmpContext();
                model.ExtensionFields = ctx.ExtensionFields.Where(x => x.Table == "Organization" && x.JoinId == node.Id).ToViewList();
            }
            return model;
        }

        public static OrganizationShortData ToShortViewData(this Organization node, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (node == null)
                return null;
            var model = new OrganizationShortData()
            {
                Id = node.Id,
                ParentId = node.ParentId,
                TreeId = node.TreeId,
                Rank = node.Rank,
                Name = node.Name,
                AliasName = node.AliasName,
                Initial = node.Initial,
                Type = node.Type,
                ManagerCount = node.ManagerCount,
                CustomerCount = node.CustomerCount,
                ProvinceId = node.ProvinceId,
                CityId = node.CityId,
                DistrictId = node.DistrictId,
                Enable = node.Enable,
                Description = node.Description
            };

            return model;
        }

        public static IEnumerable<OrganizationData> ToViewList(this IQueryable<Organization> nodes, CategoryDictionary suffix = CategoryDictionary.None)
        {
            return nodes.ToList().Select(n => n.ToViewData(suffix));
        }

        public static Organization ToModel(this OrganizationData node)
        {
            var model = new Organization()
            {
                Id = node.Id,
                Type = node.Type,
                ParentId = node.ParentId,
                Name = node.Name,
                AliasName = node.AliasName,
                Initial = node.Initial,
                ManagerCount = node.ManagerCount,
                CustomerCount = node.CustomerCount,
                Enable = node.Enable,
                Description = node.Description,
                ProvinceId = node.ProvinceId,
                CityId = node.CityId,
                DistrictId = node.DistrictId
            };

            return model;
        }


        public static OrganizationDiagram ToViewDiagram(this Organization node, int layer = 0, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (node.Parent != null)
                node.Parent.Parent = null;
            var model = new OrganizationDiagram()
            {
                Id = node.Id,
                ParentId = node.ParentId,
                TreeId = node.TreeId,
                Rank = node.Rank,
                Name = node.Name,
                AliasName = node.AliasName,
                Initial = node.Initial,
                ManagerCount = node.ManagerCount,
                CustomerCount = node.CustomerCount,
                Enable = node.Enable,
                Description = node.Description,
                HasChildren = node.Children.Count,
                Parent = (suffix & CategoryDictionary.Parent) == CategoryDictionary.Parent ? (node.Parent == null ? null : node.Parent.ToViewDiagram()) : null,
                Buildings = (suffix & CategoryDictionary.Building) == CategoryDictionary.Building ? node.Buildings.ToList().Select(b => b.ToViewData()).ToList() : null
            };

            if (layer > 0 && node.Children != null && node.Children.Count() != 0 && (suffix & CategoryDictionary.Children) == CategoryDictionary.Children)
            {
                model.Children = node.Children.Select(o => o.ToViewDiagram(--layer));
            }
            else
            {
                model.Children = null;
            }
            if ((suffix & CategoryDictionary.ExtensionField) == CategoryDictionary.ExtensionField)
            {
                var ctx = new EmpContext();
                model.ExtensionFields = ctx.ExtensionFields.Where(x => x.Table == "Organization" && x.JoinId == node.Id).ToViewList();
            }

            return model;
        }

        public static IQueryable<Organization> Peers(this Organization node, bool includeSelf = false)
        {
            var ctx = new OrganizationBLL();
            var list = ctx.Filter(x => x.Rank == node.Rank || (includeSelf && x.Id == node.Id)).AsQueryable();
            return list;
        }



        #region Descendant  Ancestor
        /// <summary>
        /// 获取所有后代对象列表
        /// </summary>
        /// <param name="node"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IEnumerable<Organization> Descents(this Organization node, Expression<Func<Organization, bool>> predicate)
        {
            return node.Descendants().Where(predicate.Compile());
        }

        /// <summary>
        /// 获取所有前代对象
        /// </summary>
        /// <param name="node"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IEnumerable<Organization> Ancestors(this Organization node, Expression<Func<Organization, bool>> predicate)
        {
            return node.Ancestors().Where(predicate.Compile());
        }

        /// <summary>
        /// 所有后代
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static IEnumerable<Organization> Descendants(this Organization node, bool includeSelf = false)
        {
            var ctx = new EmpContext();
            return ctx.Organizations.Where(DescendantsFunc(node, includeSelf)).AsEnumerable();
        }

        private static Func<Organization, bool> DescendantsFunc(Organization node, bool includeSelf)
        {
            //return x => x.TreeId.StartsWith(node.TreeId + (includeSelf ? string.Empty : "-"));
            return x => x.TreeId.StartsWith(node.TreeId + "-") || (includeSelf && x.TreeId == node.TreeId);
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
        public static IEnumerable<Organization> Ancestors(this Organization node)
        {
            var ctx = new EmpContext();
            return ctx.Organizations.Where(AncestorsFunc(node));
        }

        private static Func<Organization, bool> AncestorsFunc(Organization node)
        {
            var segs = node.TreeId.Split('-');
            Expression<Func<Organization, bool>> exp = x => false;
            var reducer = "";
            if (segs.Length > 0)
            {
                for (int i = 0; i < segs.Length - 1; i++)
                {
                    reducer += i == 0 ? segs[i] : ("-" + segs[i]);
                    Regex regx = new Regex("^" + reducer + "$");
                    Expression<Func<Organization, bool>> lambda = x => regx.IsMatch(x.TreeId);
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
        #endregion

    }
}
