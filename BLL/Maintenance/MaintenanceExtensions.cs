using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WxPay2017.API.VO.Common;
using WxPay2017.API.VO.Param;
using WxPay2017.API.VO;
using WxPay2017.API.DAL.EmpModels;


namespace WxPay2017.API.BLL
{
    public static class MaintenanceExtensions
    {
        #region 0912Maintenance
        //public static MaintenanceData ToViewData(this Maintenance node, CategoryDictionary suffix = CategoryDictionary.None)
        //{
        //    if (node == null)
        //        return null;
        //    return new MaintenanceData()
        //    {
        //        Id = node.Id,
        //        MaintenanceCategoryId = node.MaintenanceCategoryId,
        //        StateId = node.StateId,
        //        UserId = node.UserId,
        //        Title = node.Title,
        //        Content = node.Content,
        //        Picture = node.Picture,
        //        ObjectCategoryId = node.ObjectCategoryId,
        //        OperateObjectId = node.OperateObjectId,
        //        CreateDate = node.CreateDate,
        //        Comment = node.Comment,
        //        Rating = node.Rating,
        //        OperatorDiscription = node.OperatorDiscription
        //        //User = ((suffix & CategoryDictionary.User) == CategoryDictionary.User) && node.User != null ? node.User.ToViewData() : null,
        //        //MaintenanceCategory = node.MaintenanceCategory.ToViewData(),
        //        //State = node.MaintenanceState.ToViewData(),
        //        //ObjectCategory = node.MaintenanceObjectCategory.ToViewData()

        //    };
        //}

        //public static IList<MaintenanceData> ToViewList(this IQueryable<Maintenance> nodes, CategoryDictionary suffix = CategoryDictionary.None)
        //{
        //    if (nodes == null)
        //        return null;
        //    var nodeList = nodes.ToList();
        //    //var results = nodes.ToList().Select(x => x.ToViewData(suffix)).ToList();
        //    var results = nodeList.Select(node => new MaintenanceData()
        //    {
        //        Id = node.Id,
        //        MaintenanceCategoryId = node.MaintenanceCategoryId,
        //        StateId = node.StateId,
        //        UserId = node.UserId,
        //        Title = node.Title,
        //        Content = node.Content,
        //        Picture = node.Picture,
        //        ObjectCategoryId = node.ObjectCategoryId,
        //        OperateObjectId = node.OperateObjectId,
        //        CreateDate = node.CreateDate,
        //        Comment = node.Comment,
        //        Rating = node.Rating,
        //        OperatorDiscription = node.OperatorDiscription,

        //    }).ToList();

        //    return results;
        //}

        //public static Maintenance ToModel(this MaintenanceData node)
        //{
        //    return new Maintenance()
        //    {
        //        Id = node.Id,
        //        MaintenanceCategoryId = node.MaintenanceCategoryId,
        //        StateId = node.StateId,
        //        UserId = node.UserId,
        //        Title = node.Title,
        //        Content = node.Content,
        //        Picture = node.Picture,
        //        ObjectCategoryId = node.ObjectCategoryId,
        //        OperateObjectId = node.OperateObjectId,
        //        CreateDate = node.CreateDate,
        //        Comment = node.Comment,
        //        Rating = node.Rating,
        //        OperatorDiscription = node.OperatorDiscription,

        //    };
        //}
        #endregion

        #region Maintenance
        public static MaintenanceData ToViewData(this Maintenance node, CategoryDictionary suffix = CategoryDictionary.None)
        {
            UserBLL userBLL = new UserBLL();
            BuildingBLL buildingBLL = new BuildingBLL();
            if (node == null)
                return null;
            return new MaintenanceData()
            {
                Id = node.Id,
                MaintenanceCategoryId = node.MaintenanceCategoryId,
                StateId = node.StateId,
                UserId = node.UserId,
                BuildingId = node.BuildingId,
                ApproverId = node.ApproverId,
                OperatorId = node.OperatorId,
                Title = node.Title,
                Content = node.Content,
                PictureId = node.PictureId,
                CreateDate = node.CreateDate,
                Rating = node.Rating,
                MaintenanceTime = node.MaintenanceTime,
                PurchasingId = node.PurchasingId,
                PictureUrl = node.PictureUrl,
                User = ((suffix & CategoryDictionary.User) == CategoryDictionary.User) ? node.User.ToViewData() : null,// node.User == null ? userBLL.Find(node.UserId).ToViewData() : node.User.ToViewData(),
                Approver = ((suffix & CategoryDictionary.User) == CategoryDictionary.User) ? node.Approver.ToViewData() : null, //node.ApproverId != null ? node.Approver == null ? userBLL.Find(node.ApproverId).ToViewData() : node.Approver.ToViewData() : null,
                Operator = ((suffix & CategoryDictionary.User) == CategoryDictionary.User) ? node.Operator.ToViewData() : null, //node.OperatorId != null ? node.Operator == null ? userBLL.Find(node.OperatorId).ToViewData() : node.Operator.ToViewData() : null,
                //State = node.State.ToViewData(),
                StateName = DictionaryCache.Get()[(int)node.StateId].ChineseName,
                BuildingName = buildingBLL.Find(node.BuildingId).Name,
            };
        }

        public static IEnumerable<MaintenanceData> ToViewList(this IQueryable<Maintenance> node, CategoryDictionary suffix = CategoryDictionary.User)
        {
            return node.ToList().Select(x => x.ToViewData(suffix));
        }

        public static Maintenance ToModel(this MaintenanceData node)
        {
            return new Maintenance()
            {
                Id = node.Id,
                MaintenanceCategoryId = node.MaintenanceCategoryId,
                StateId = node.StateId,
                UserId = node.UserId,
                BuildingId = node.BuildingId,
                ApproverId = node.ApproverId,
                OperatorId = node.OperatorId,
                Title = node.Title,
                Content = node.Content,
                PictureId = node.PictureId,
                CreateDate = node.CreateDate,
                Rating = node.Rating,
                MaintenanceTime = node.MaintenanceTime,
                PurchasingId = node.PurchasingId,
                PictureUrl = node.PictureUrl

            };
        }
        #endregion


    }
}
