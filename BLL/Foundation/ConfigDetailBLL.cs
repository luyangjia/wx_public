using WxPay2017.API.DAL.EmpModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WxPay2017.API.VO;
using WxPay2017.API.VO.Common;
using WxPay2017.API.VO.Param;
using System.Threading;

namespace WxPay2017.API.BLL
{
    public class ConfigDetailBLL : Repository<ConfigDetail>
    {
        MeterBLL meterBLL = new MeterBLL();
        MetersActionBLL meterActionBLL = new MetersActionBLL();
        BuildingBLL buildingBLL = new BuildingBLL();
        public ConfigDetailBLL(EmpContext context = null)
            : base(context)
        {
            meterActionBLL = new MetersActionBLL(meterBLL.db);
            buildingBLL = new BuildingBLL(meterBLL.db);
        }
        /// <summary>
        /// 更新此建筑配置相关硬件设备价格
        /// </summary>
        /// <param name="configDetail"></param>
        public void updateMetersPriceByConfigDetail(ConfigDetail configDetail,decimal? value)
        {
           if (configDetail.BuildingId==null)
               return;
           var meterIds = GetMeterIdsInRoom(configDetail,true);
            meterActionBLL.CreateByMeterIds(meterIds, DictionaryCache.ActionSetPrice.Id, DateTime.Now, value+"","");
        }

        /// <summary>
        /// 获得配置相关的设备
        /// </summary>
        /// <param name="configDetail"></param>
        /// <returns></returns>
        public List<int> GetMeterIdsInRoom(ConfigDetail configDetail,bool? isInUsed=null)
        {
            var building = buildingBLL.Find(configDetail.BuildingId);
            var bTreeId = DictionaryCache.Get()[(int)configDetail.BuildingCategoryId].TreeId;
            var eTreeId = DictionaryCache.Get()[(int)configDetail.EnergyCategoryId].TreeId;
            var meterIds = meterBLL.Filter(o =>
                //是其建筑下设备
                   (configDetail.BuildingId == o.Building.Id
                    || o.Building.TreeId.StartsWith(building.TreeId + "-"))
                       //分类正确，默认使用顶级分类
                    && (configDetail.BuildingCategoryId == o.Building.BuildingCategoryId ||
                    o.Building.BuildingCategoryDict.TreeId.StartsWith(bTreeId + "-"))
                    //&& (configDetail.EnergyCategoryId == o.EnergyCategoryId ||
                    //o.EnergyCategoryDict.TreeId.StartsWith(eTreeId + "-"))
                    && (configDetail.EnergyCategoryId == o.EnergyCategoryId)
                    &&o.TypeDict.ThirdValue==1
                    &&o.Enable
                    &&o.Access!=null
                    &&o.GbCode!=null
                    &&o.TypeDict.SecondValue==1
                    &&(isInUsed==null?true:isInUsed== o.Enable)
                ).Select(o => o.Id).ToList();

            return meterIds;
        }
    }

}
