
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using WxPay2017.API.DAL;
using WxPay2017.API.DAL.EmpModels;
using WxPay2017.API.VO;
using WxPay2017.API.VO.Common;

namespace WxPay2017.API.BLL
{
    public class BuildingBLL : Repository<Building>
    {
        OrganizationBLL organizationBLL = new OrganizationBLL();


        public BuildingBLL(EmpContext context = null, string userName = null)
            : base(context, string.IsNullOrEmpty(userName) ? null : (Expression<Func<Building, bool>>)(x => x.Organization.Users.Any(u => u.UserName == userName)))
        {
            organizationBLL = new OrganizationBLL(this.db);
        }

        public IQueryable<Building> GetChildren(int id)
        {
            return db.Buildings.Where(b => id == b.ParentId.Value && b.Enable);
        }

        /// <summary>
        /// 获取与指定机构关联的一级建筑
        /// </summary>
        /// <param name="organizationId">机构编号</param>
        /// <returns></returns>
        public IQueryable<Building> GetPrimaryByOrg(int organizationId, bool includeDisable = false)
        {
            var organization = organizationBLL.Find(organizationId);
            var buidlings = db.Buildings.Where(b => b.Organization.TreeId.StartsWith(organization.TreeId) && b.Enable && (includeDisable ? true : b.TypeDict.Enable));
            var result = from bl in buidlings
                         join bc in buidlings
                         on bl.ParentId equals bc.Id into temp
                         from eb in temp.DefaultIfEmpty()
                         where eb == null
                         select bl;
            return result;
        }

        /// <summary>
        /// 获得机构下所有建筑id集合
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="includeDisable"></param>
        /// <returns></returns>
        public List<int> GetAllChildBuildingIds(int organizationId, bool includeDisable = false)
        {
            //获得机构下的所有机构
            var orgs = organizationBLL.GetDescendants(organizationId, includeDisable).ToList();
            List<int> childBuildingIds = new List<int>();
            //获得机构下所有顶级建筑
            var topBuildingsThis = GetPrimaryByOrg(organizationId, includeDisable).ToList();
            if (topBuildingsThis.Count() > 0)
                childBuildingIds.AddRange(topBuildingsThis.Select(o => o.Id).ToList());
            //遍历每栋建筑，得到建筑的下属所有不在集合中的建筑
            foreach (var building in topBuildingsThis)
            {
                var cBuildings = this.GetDescents(building, includeDisable).Where(o => !childBuildingIds.Contains(o.Id) && o.Enable).Select(o => o.Id).ToList();
                childBuildingIds.AddRange(cBuildings);
            }

            foreach (var org in orgs)
            {
                //获得机构下所有顶级建筑
                var topBuildings = GetPrimaryByOrg(org.Id, includeDisable).ToList();
                if (topBuildings.Count() > 0)
                    childBuildingIds.AddRange(topBuildings.Select(o => o.Id).ToList());
                //遍历每栋建筑，得到建筑的下属所有不在集合中的建筑
                foreach (var building in topBuildings)
                {
                    var cBuildings = this.GetDescents(building, includeDisable).Where(o => !childBuildingIds.Contains(o.Id) && o.Enable).Select(o => o.Id).ToList();
                    childBuildingIds.AddRange(cBuildings);
                }
            }
            return childBuildingIds;

        }

        /// <summary>
        /// 获取所有当前及后代对象列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IQueryable<Building> GetDescents(int id, bool includeSelf = true)
        {
            var node = this.Find(id);
            return this.GetDescents(node);
        }

        /// <summary>
        /// 获取所有当前及后代对象列表
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public IQueryable<Building> GetDescents(Building node, bool includeSelf = true)
        {
            if (includeSelf)
            {
                return db.Buildings.Where(b => b.TreeId == node.TreeId || b.TreeId.StartsWith(node.TreeId + "-") && b.Enable);
            }
            else
            {
                return db.Buildings.Where(b => b.TreeId.StartsWith(node.TreeId + "-") && b.Enable);
            }
        }


        /// <summary>
        /// 获取同辈对象列表
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public IQueryable<BuildingTree> GetPeers(Building node, bool includeSelf = true)
        {
            //var str = node.ParentId.HasValue ? (node.ParentId.Value + "-" + node.Id) : (node.Id.ToString());
            var str = includeSelf ? "" : " AND Id <> '{1}' ";
            var treenode = this.db.Database.SqlQuery<BuildingTree>(string.Format(" SELECT * FROM [Building].[BuildingTree] WHERE Id = {0} AND Enable = 1 ", node.Id)).FirstOrDefault();
            if (treenode == null) throw new KeyNotFoundException();
            var list = this.db.Database.SqlQuery<BuildingTree>(string.Format(" SELECT * FROM [Building].[BuildingTree] WHERE Rank = {0} AND Enable = 1" + str, treenode.Rank, treenode.Id)).AsQueryable();
            return list;

        }

        /// <summary>
        /// 获取缴费用户的建筑对象
        /// </summary>
        /// <param name="category">用户</param>
        /// <param name="targetId">用户id</param>
        /// <returns></returns>
        public DAL.EmpModels.Building GetUserBuilding(CategoryDictionary category, string targetId)
        {
            Building building = null;
            try
            {
                if (category == CategoryDictionary.User)
                    building = this.db.Buildings.FirstOrDefault(o => o.Users.Any(u => u.Id == targetId && u.Roles.Any(r => r.Id == RoleCache.PayMentRoleId)));
            }
            catch(Exception e)
            {
                throw e;
            }

            return building;            
        }

    }
}