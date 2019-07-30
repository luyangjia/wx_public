
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

namespace WxPay2017.API.BLL
{
    public class OrganizationBLL : Repository<Organization>
    {

        public OrganizationBLL(EmpContext context = null, string userName = null)
            : base(context, string.IsNullOrEmpty(userName) ? null : (Expression<Func<Organization, bool>>)(o => o.Users.Any(u => u.UserName == userName)))
        {

        }

        //public OrganizationBLL(Expression<Func<Organization, bool>> predicate, EmpContext context)
        //    : base(context, predicate)
        //{
        //}

        public IQueryable<Organization> GetChildren(int id)
        {
            return db.Organizations.Where(o => o.Enable && id == o.ParentId.Value);
        }


        /// <summary>
        /// 获取建筑树关联的子机构
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public IEnumerable<Organization> GetChildrenByBldg(int bldgId)
        {
            var building = db.Buildings.FirstOrDefault(b => b.Id == bldgId);
            var childrenOrg = building.Organization.Children.Where(co => co.Enable && co.Buildings.Any(cob => cob.TreeId.StartsWith(building.TreeId + "-") || cob.TreeId == building.TreeId));
            return childrenOrg;
        }

        /// <summary>
        /// 获取所有当前及后代对象列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IQueryable<OrganizationTree> GetDescents(int id)
        {
            var node = this.Find(id);
            return this.GetDescents(node);
        }

        public IQueryable<Organization> GetDescendants(int id, bool includeSelf = false)
        {
            var node = this.Find(id);
            return this.GetDescendants(node, includeSelf);
        }


        /// <summary>
        /// 获取所有当前及后代对象列表
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public IQueryable<OrganizationTree> GetDescents(Organization node)
        {
            var str = node.ParentId.HasValue ? (node.ParentId.Value + "-" + node.Id) : (node.Id.ToString());
            var treenode = this.db.Database.SqlQuery<OrganizationTree>(string.Format(" SELECT * FROM [Organization].[OrganizationTree] WHERE Id = {0} AND Enable = 1", node.Id)).FirstOrDefault();
            if (treenode == null) throw new KeyNotFoundException();
            var list = this.db.Database.SqlQuery<OrganizationTree>(string.Format(" SELECT * FROM [Organization].[OrganizationTree] WHERE TreeId LIKE '{0}%' AND Enable = 1", treenode.TreeId)).AsQueryable();
            return list;
        }

        /// <summary>
        /// 获取所有当前及后代对象列表
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public IQueryable<Organization> GetDescendants(Organization node, bool includeSelf = false)
        {
            var list = this.db.Organizations.Where(x => x.Enable && x.TreeId.StartsWith(node.TreeId + "-") || (includeSelf && x.TreeId == node.TreeId)).AsQueryable();
            return list;
        }

        /// <summary>
        /// 获取同辈对象列表
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public IQueryable<OrganizationTree> GetPeers(Organization node, bool includeSelf = true)
        {
            //var str = node.ParentId.HasValue ? (node.ParentId.Value + "-" + node.Id) : (node.Id.ToString());
            var str = includeSelf ? "" : " AND Id <> '{0}' ";
            var treenode = this.db.Database.SqlQuery<OrganizationTree>(string.Format(" SELECT * FROM [Organization].[OrganizationTree] WHERE Id = {0} AND Enable = 1", node.Id)).FirstOrDefault();
            if (treenode == null) throw new KeyNotFoundException();
            var list = this.db.Database.SqlQuery<OrganizationTree>(string.Format(" SELECT * FROM [Organization].[OrganizationTree] WHERE Rank = '{0}' AND Enable = 1" + str, treenode.Rank)).AsQueryable();
            return list;

        }

        public void InitTopOrg()
        {
            string appId = MyConsole.GetAppString("ApplicationID");

            var settingBLL = new SettingBLL();
            //var topOrgName = BLL.MyConsole.GetAppString("TopOrgName");
            var firstOrDefault = settingBLL.Filter(o => o.AppID.ToString() == appId).FirstOrDefault();
            if (firstOrDefault != null)
            {
                var topOrgName = firstOrDefault.Title;
                var org = this.Filter(o => o.Enable && o.Type == 100001).OrderByDescending(o => o.Id).FirstOrDefault();
                if (org == null)
                {
                    //创建顶级机构和顶级建筑
                    org = new Organization();
                    org.Name = topOrgName;
                    org.Type = 100001;
                    org.Enable = true;
                    org = this.Create(org);

                }
                else
                {
                    if (org.Name != topOrgName)
                    {
                        org.Name = topOrgName;
                        this.Update(org);

                    }
                }
            }
        }
    }
}