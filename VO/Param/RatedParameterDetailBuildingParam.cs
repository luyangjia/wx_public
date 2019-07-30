using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO.Param
{
    /// <summary>
    /// 下发参数下发情况查询参数
    /// </summary>
    public class RatedParameterDetailBuildingParam
    {
        public RatedParameterDetailBuildingParam(int ratedParameterId, int[] buildingId, bool isSend = false, bool isSuccess = false, bool isNeedBuildingInfo = false)
        {
            RatedParameterId = ratedParameterId;
            BuildingId = buildingId;
            IsSend = isSend;
            IsSuccess = isSuccess;
            IsNeedBuildingInfo = isNeedBuildingInfo;
            if (buildingId==null||buildingId.Count() == 0)
                BuildingId = new []{-1};

        }
        /// <summary>
        /// 下发参数id
        /// </summary>
        public int RatedParameterId { get; set; }
        /// <summary>
        /// 建筑id
        /// </summary>
        public int[] BuildingId { get; set; }
        /// <summary>
        /// 是否下发
        /// </summary>
        public bool IsSend { get; set; }
        /// <summary>
        /// 是否下发成功
        /// </summary>
        public bool IsSuccess { get; set; }
        /// <summary>
        /// 是否显示建筑信息(建筑id、建筑名)
        /// </summary>
        public bool IsNeedBuildingInfo { get; set; }

        //public RatedParameterDetailBuildingParam(int ratedParameterId, int buildingId = -1, bool isSend = false, bool isSuccess = false, bool isNeedBuildingInfo = false)
        //{
        //    this._RatedParameterId = ratedParameterId;
        //    this._BuildingId = buildingId;
        //    this._IsSend = isSend;
        //    this._IsSuccess = isSuccess;
        //    this._IsNeedBuildingInfo = isNeedBuildingInfo;
        //}
        //private int _RatedParameterId;
        //private int _BuildingId;
        //private bool _IsSend;
        //private bool _IsSuccess;
        //private bool _IsNeedBuildingInfo;

        //public int RatedParameterId
        //{
        //    get { return _RatedParameterId; }
        //    set { _RatedParameterId = value; }
        //}
        //public int BuildingId 
        //{
        //    get { return _BuildingId; }
        //    set { _BuildingId = value; }
        //}
        //public bool IsSend 
        //{
        //    get { return _IsSend; }
        //    set { _IsSend = value; }
        //}
        //public bool IsSuccess 
        //{
        //    get { return _IsSuccess; }
        //    set { _IsSuccess = value; }
        //}
        //public bool IsNeedBuildingInfo 
        //{
        //    get { return _IsNeedBuildingInfo; }
        //    set { _IsNeedBuildingInfo = value; }
        //}
    }
}
