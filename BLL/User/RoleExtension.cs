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
    public static class RoleExtension
    {
        public static RoleData ToViewData(this Role node, CategoryDictionary suffix = CategoryDictionary.None)
        {
            return new RoleData()
            {
                Id = node.Id,
                Name = node.Name
            };
        }
        public static IEnumerable<RoleData> ToViewList(this IQueryable<Role> nodes, CategoryDictionary suffix = CategoryDictionary.None)
        {
            return nodes.ToList().Select(x => x.ToViewData(suffix));
        }

        public static Role ToModel(this RoleData node)
        {
            return new Role()
            {
                Id = node.Id,
                Name = node.Name
            };
        }
    }
}
