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
    public static class ActivityRecordExtension
    {
        #region 0912ActivityRecord
        //public static ActivityRecordData ToViewData(this ActivityRecord node, CategoryDictionary suffix = CategoryDictionary.None)
        //{
        //    if (node == null)
        //        return null;
        //    var  model = new ActivityRecordData()
        //    {
        //        Id = node.Id,
        //        TargetId = node.TargetId,
        //        TargetTypeId = node.TargetTypeId,
        //        CurrentOperatorId = node.CurrentOperatorId,
        //        State = node.StateId,
        //        Description = node.Description,
        //        CreateDate = node.CreateDate,
        //        NextOperator = node.NextOperator,
        //        //TargetType = node.TargetType.ToViewData(),
        //        //CurrentOperator = 
        //        //ActivityState = node.ActivityState.ToViewData()

                
        //    };
        //    return model;
        //}

        //public static IList<ActivityRecordData> ToViewList(this IQueryable<ActivityRecord> nodes, CategoryDictionary suffix = CategoryDictionary.None)
        //{
        //    if (nodes == null)
        //        return null;
        //    var nodeList = nodes.ToList();
        //    var results = nodeList.Select(node => new ActivityRecordData()
        //    {
        //        Id = node.Id,
        //        TargetId = node.TargetId,
        //        TargetTypeId = node.TargetTypeId,
        //        CurrentOperatorId = node.CurrentOperatorId,
        //        State = node.StateId,
        //        Description = node.Description,
        //        CreateDate = node.CreateDate,
        //        NextOperator = node.NextOperator,

        //    }).ToList();
        //    return results;
        //}

        //public static ActivityRecord ToModel(this ActivityRecordData node)
        //{
        //    return new ActivityRecord()
        //    {
        //        Id = node.Id,
        //        TargetId = node.TargetId,
        //        TargetTypeId = node.TargetTypeId,
        //        CurrentOperatorId = node.CurrentOperatorId,
        //        StateId = node.State,
        //        Description = node.Description,
        //        CreateDate = node.CreateDate,
        //        NextOperator = node.NextOperator,

        //    };
        //}
        #endregion

        #region ActivityRecord
        public static ActivityRecordData ToViewData(this ActivityRecord node, CategoryDictionary suffix = CategoryDictionary.None)
        {
            UserBLL userBLL = new UserBLL();
            MaintenanceBLL maintenanceBLL = new MaintenanceBLL();
            if (node == null)
                return null;
            return new ActivityRecordData()
            {
                Id = node.Id,
                TargetId = node.TargetId,
                PublisherId = node.PublisherId,
                StateId = node.StateId,
                Description = node.Description,
                CreateDate = node.CreateDate,
                //Target = node.TargetId != null ? node.MaintenanceTarget == null ? maintenanceBLL.Find(node.TargetId).ToViewData() : node.MaintenanceTarget.ToViewData() : null,
                Publisher = (suffix & CategoryDictionary.User) == CategoryDictionary.User ? node.PublisherId != null ? node.Publisher == null ? userBLL.Find(node.PublisherId).ToViewData() : node.Publisher.ToViewData() : null:null,
                StateName = DictionaryCache.Get()[(int)node.StateId].ChineseName
            };
        }

        public static IList<ActivityRecordData> ToViewList(this IQueryable<ActivityRecord> nodes, CategoryDictionary suffix = CategoryDictionary.None)
        {
            UserBLL userBLL = new UserBLL();
            MaintenanceBLL maintenanceBLL = new MaintenanceBLL();
            if (nodes == null)
                return null;
            var nodeList = nodes.ToList();
            var results = nodeList.Select(node => new ActivityRecordData()
            {
                Id = node.Id,
                TargetId = node.TargetId,
                PublisherId = node.PublisherId,
                StateId = node.StateId,
                Description = node.Description,
                CreateDate = node.CreateDate,
                //Target = node.TargetId != null ? node.MaintenanceTarget == null ? maintenanceBLL.Find(node.TargetId).ToViewData() : node.MaintenanceTarget.ToViewData() : null,
                //Publisher = node.PublisherId != null ? node.Publisher == null ? userBLL.Find(node.PublisherId).ToViewData() : node.Publisher.ToViewData() : null,
                StateName = DictionaryCache.Get()[(int)node.StateId].ChineseName

            }).ToList();
            return results;
        }

        public static ActivityRecord ToModel(this ActivityRecordData node)
        {
            return new ActivityRecord()
            {
                Id = node.Id,
                TargetId = node.TargetId,
                PublisherId = node.PublisherId,
                StateId = node.StateId,
                Description = node.Description,
                CreateDate = node.CreateDate,

            };
        }
        #endregion


    }
}
