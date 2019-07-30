using WxPay2017.API.DAL.EmpModels;
using WxPay2017.API.VO.Param;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace WxPay2017.API.VO
{
    #region 0912
    //public partial class MaintenanceData
    //{
    //    public int Id { get; set; }
    //    public int MaintenanceCategoryId { get; set; }
    //    public int StateId { get; set; }
    //    public string UserId { get; set; }
    //    public string Title { get; set; }
    //    public string Content { get; set; }
    //    public int? Picture { get; set; }
    //    public int ObjectCategoryId { get; set; }
    //    public string OperateObjectId { get; set; }
    //    public DateTime CreateDate { get; set; }
    //    public string Comment { get; set; }
    //    public int? Rating { get; set; }
    //    public string OperatorDiscription { get; set; }

    //    //public DictionaryData MaintenanceCategory { get; set; }        
    //    //public DictionaryData State { get; set; }
    //    //public UserData User { get; set; }
    //    //public DictionaryData ObjectCategory { get; set; }




    //}
    #endregion

    public partial class MaintenanceData
    {

        public int Id { get; set; }

        public int MaintenanceCategoryId { get; set; }

        public int StateId { get; set; }

        public string UserId { get; set; }

        public int BuildingId { get; set; }

        public string ApproverId { get; set; }

        public string OperatorId { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public int? PictureId { get; set; }

        public DateTime CreateDate { get; set; }

        public int? Rating { get; set; }

        public DateTime? MaintenanceTime { get; set; }

        public int? PurchasingId { get; set; }

        public UserData User { get; set; }
        public UserData Approver { get; set; }
        public UserData Operator { get; set; }
        //public DictionaryData State { get; set; }
        public string StateName { get; set; }
        public string BuildingName { get; set; }
        //图片链接
        public string PictureUrl { get; set; }

    }

    /// <summary>
    /// 上传参数，获取特定活动状态的报修单
    /// </summary>
    public class StateUser
    {
        //分页
        public Pagination pagination;
        //活动状态id
        public int[] stateIds;
        //用户类型
        public string type;
    }


}
