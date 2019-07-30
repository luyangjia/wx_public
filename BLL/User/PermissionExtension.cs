using WxPay2017.API.DAL.EmpModels;
using WxPay2017.API.VO.Common;
using WxPay2017.API.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.BLL
{
    public static class PermissionExtension
    {
        public static PermissionData ToViewData(this Permission node, CategoryDictionary suffix = CategoryDictionary.None)
        {
            var model = new PermissionData()
            {
                Id = node.Id,
                ParentId = node.ParentId,
                Title = node.Title,
                Area = node.Area,
                Controller = node.Controller,
                Action = node.Action,
                Value = node.Value,
                HttpMethod = node.HttpMethod,
                Url = node.Url,
                IsNav = node.IsNav,
                Actived = node.Actived,
                Disabled = node.Disabled,
                Sort = node.Sort,
                Description = node.Description,
                CategoryID = node.CategoryID,
                CategoryTitle = node.CategoryTitle,
                Icon = node.Icon
            };
            if ((suffix & CategoryDictionary.Children) == CategoryDictionary.Children && node.Children.Count > 0)
            {
                model.Children = node.Children.Select(x => x.ToViewData(suffix)).ToList();
            }
            if ((suffix & CategoryDictionary.Parent) == CategoryDictionary.Parent && node.Parent != null)
            {
                model.Parent = node.Parent.ToViewData(CategoryDictionary.None);
            }
            if ((suffix & CategoryDictionary.ExtensionField) == CategoryDictionary.ExtensionField)
            {
                var ctx = new EmpContext();
                model.ExtensionFields = ctx.ExtensionFields.Where(x => x.Table == "Permission" && x.JoinId == node.Id).ToViewList();
            }
            return model;

        }


        public static IEnumerable<PermissionData> ToViewList(this IQueryable<Permission> nodes, CategoryDictionary suffix = CategoryDictionary.None)
        {
            return nodes.ToList().Select(x => x.ToViewData(suffix));
        }

        public static Permission ToModel(this PermissionData node)
        {
            return new Permission()
            {
                Id = node.Id,
                ParentId = node.ParentId,
                Title = node.Title,
                Area = node.Area,
                Controller = node.Controller,
                Action = node.Action,
                Value = node.Value,
                HttpMethod = node.HttpMethod,
                Url = node.Url,
                IsNav = node.IsNav,
                Actived = node.Actived,
                Disabled = node.Disabled,
                Sort = node.Sort,
                Description = node.Description,
                CategoryID = node.CategoryID,
                CategoryTitle = node.CategoryTitle,
                Icon = node.Icon
            };
        }
    }
}
