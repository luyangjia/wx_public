using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO
{
    public class CancelAccountUserInfos
    {
        public Dictionary<int, CancelAccountUserInfo> info;
    }

    public class CancelAccountUserInfo
    {
        public OrganizationData OrgInfo { get; set; }
        public List<BuildingWithUser> UsersInBuilding = new List<BuildingWithUser>();
    }

    public class BuildingWithUser
    {
        public int BuildingId { get; set; }
        public string BuildingName { get; set; }
        public int? ParentId { get; set; }
        public string ParentName { get; set; }
        public int? ParentOfParentId { get; set; }
        public string ParentOfParentName { get; set; }
        /// <summary>
        /// 是否全建筑清空了
        /// </summary>
        public bool? isClear { get; set; }
        /// <summary>
        ///该机构下该建筑的相关用户是否销户完成
        /// <summary>
        public bool? isCanceled { get; set; }
        /// <summary>
        /// Money保存需要返还的金额。注意只有isCanceled和iscleared都为true，money值才有效
        /// </summary>
        public decimal? Money { get; set; }
        public List<UserLittleData> Users { get; set; }
    }
}
