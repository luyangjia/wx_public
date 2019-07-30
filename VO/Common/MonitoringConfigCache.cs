using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WxPay2017.API.DAL.EmpModels;
using WxPay2017.API.VO.Common;
using WxPay2017.API.VO.Param;
using WxPay2017.API.VO;

namespace WxPay2017.API.VO.Common
{
    public  class MonitoringConfigCache
    {
        static DAL.EmpModels.EmpContext db = new DAL.EmpModels.EmpContext();
        public static Dictionary<int, List<MonitoringConfig>> buildingPriceConfigs { get; set; }
        public static Dictionary<int, List<MonitoringConfig>> orgPriceConfigs { get; set; }


        /// <summary>
        /// 初始化所有建筑和机构的定价配置
        /// </summary>
        public static void InitPriceSetting() {
            var buildings = db.Buildings.Where(o => o.Users.Any(c=>c.Roles.Any(d=>d.Id==RoleCache.PayMentRoleId))).ToList();
            var orgs = db.Organizations.Where(o => o.Users.Any(c => c.Roles.Any(d => d.Id == RoleCache.PayMentRoleId))).ToList();
            //获取有直接定价配置的机构和建筑
            var orgsWithDirectPrice = orgs.Where(o => o.ConfigDetails.Count() != 0&&o.ConfigDetails.Any(c=>c.Template.ConfigCycleSettings.Any(d=>d.BeginTime<=DateTime.Now&&d.EndTime>=DateTime.Now)));
            var buildingsWithDirectPrice = buildings.Where(o => o.ConfigDetails.Count() != 0 && o.ConfigDetails.Any(c => c.Template.ConfigCycleSettings.Any(d => d.BeginTime <= DateTime.Now && d.EndTime >= DateTime.Now)));
            //获取上级机构有定价的机构
            var orgsWithDirectPriceTreeIds=orgsWithDirectPrice.Select(o=>o.TreeId).ToList();
            var orgsParentWithPrice = orgs.Where(o => !orgsWithDirectPriceTreeIds.Contains(o.TreeId) && orgsWithDirectPriceTreeIds.Any(c => o.TreeId.StartsWith(c)));
            //获取上级机构有定价的建筑
            var buildingsParentWithPrice = buildings.Where(o => !orgsWithDirectPriceTreeIds.Contains(o.Organization.TreeId) && orgsWithDirectPriceTreeIds.Any(c => o.Organization.TreeId.StartsWith(c)));

            //获取所有相关定价配置数据
            var configsBase = db.MonitoringConfig.Where(o => orgsWithDirectPrice.Any(c => c.ConfigDetails.Any(d => o.TemplateId == d.Template.Id)) || buildingsWithDirectPrice.Any(c => c.ConfigDetails.Any(d => o.TemplateId == d.Template.Id))).ToList();

            buildingPriceConfigs = new Dictionary<int, List<MonitoringConfig>>();
            orgPriceConfigs = new Dictionary<int, List<MonitoringConfig>>();

            foreach (var orgWithDirectPrice in orgsWithDirectPrice)
            {
                List<MonitoringConfig> configs = configsBase.Where(o => orgWithDirectPrice.ConfigDetails.Any(c => o.TemplateId == c.TemplateId)).ToList();
                orgPriceConfigs.Add(orgWithDirectPrice.Id, configs);
            }

            foreach (var buildingWithDirectPrice in buildingsWithDirectPrice)
            {
                List<MonitoringConfig> configs = configsBase.Where(o => buildingWithDirectPrice.ConfigDetails.Any(c => o.TemplateId == c.TemplateId)).ToList();
                buildingPriceConfigs.Add(buildingWithDirectPrice.Id, configs);
            }

            foreach (var orgParentWithPrice in orgsParentWithPrice)
            {
                //最近的有定价的上级机构
                var pTreeid = orgsWithDirectPriceTreeIds.Where(o => orgParentWithPrice.TreeId.StartsWith(o)).OrderBy(o => o.Length).Take(1).FirstOrDefault();
                if (pTreeid != null)
                {
                    var pOrgId = orgsWithDirectPrice.FirstOrDefault(o => o.TreeId == pTreeid).Id;
                    orgPriceConfigs.Add(orgParentWithPrice.Id, orgPriceConfigs[pOrgId]);
                }
            }

            foreach (var buildingParentWithPrice in buildingsParentWithPrice)
            {
                //最近的有定价的上级机构
                var pTreeid = orgsWithDirectPriceTreeIds.Where(o => buildingParentWithPrice.Organization.TreeId.StartsWith(o)).OrderBy(o => o.Length).Take(1).FirstOrDefault();
                if (pTreeid != null)
                {
                    var pOrgId = orgsWithDirectPrice.FirstOrDefault(o => o.TreeId == pTreeid).Id;
                    orgPriceConfigs.Add(buildingParentWithPrice.Id, orgPriceConfigs[pOrgId]);
                }
            }
        }

         /// <summary>
        /// 获取指定建筑/机构的定价配置
        /// </summary>
        public static List<MonitoringConfig> GetPriceSetting(CategoryDictionary category, int id)
        {
            List<MonitoringConfig> config = new List<MonitoringConfig>();
            if (category == CategoryDictionary.Building)
            {
                var building = db.Buildings.FirstOrDefault(o => o.Id == id);
                return GetPriceSetting(building);
            }
            else
            {
                var org = db.Organizations.FirstOrDefault(o => o.Id == id);
                return GetPriceSetting(org);
            }
        }
        /// <summary>
        /// 获取指定建筑的定价配置
        /// </summary>
        public static List<MonitoringConfig> GetPriceSetting(Building building)
        {
            List<MonitoringConfig> configs = new List<MonitoringConfig>();
            configs = db.MonitoringConfig.Where(o => building.ConfigDetails.Any(c => o.TemplateId == c.TemplateId && c.Template.ConfigCycleSettings.Any(d => d.BeginTime <= DateTime.Now && d.EndTime >= DateTime.Now))).ToList();
            if (configs.Count == 0)
            {
                //无直接定价查找上级机构定价
                //最近的有定价的上级机构
                var orgWithDirectPrice = db.Organizations.Where(o =>building.Organization.TreeId.StartsWith(o.TreeId)&& o.ConfigDetails.Count() != 0 && o.ConfigDetails.Any(c => c.Template.ConfigCycleSettings.Any(d => d.BeginTime <= DateTime.Now && d.EndTime >= DateTime.Now))).OrderBy(o=>o.TreeId.Length).Take(1).FirstOrDefault();
                if (orgWithDirectPrice != null)
                {
                    configs = db.MonitoringConfig.Where(o => orgWithDirectPrice.ConfigDetails.Any(c => o.TemplateId == c.TemplateId && c.Template.ConfigCycleSettings.Any(d => d.BeginTime <= DateTime.Now && d.EndTime >= DateTime.Now))).ToList();
                }
            }
            if (configs.Count() > 0 && !buildingPriceConfigs.Keys.Contains(building.Id))
                buildingPriceConfigs.Add(building.Id, configs);
            else if (configs.Count() > 0 )
                buildingPriceConfigs[building.Id]= configs;
            return configs;
        }

        /// <summary>
        /// 获取指定建筑的定价配置
        /// </summary>
        public static List<MonitoringConfig> GetPriceSetting(Organization org)
        {
            List<MonitoringConfig> configs = new List<MonitoringConfig>();
            configs = db.MonitoringConfig.Where(o => org.ConfigDetails.Any(c => o.TemplateId == c.TemplateId && c.Template.ConfigCycleSettings.Any(d => d.BeginTime <= DateTime.Now && d.EndTime >= DateTime.Now))).ToList();
            if (configs.Count == 0)
            {
                //无直接定价查找上级机构定价
                //最近的有定价的上级机构
                var orgWithDirectPrice = db.Organizations.Where(o => org.TreeId.StartsWith(o.TreeId) && o.ConfigDetails.Count() != 0 && o.ConfigDetails.Any(c => c.Template.ConfigCycleSettings.Any(d => d.BeginTime <= DateTime.Now && d.EndTime >= DateTime.Now))).OrderBy(o => o.TreeId.Length).Take(1).FirstOrDefault();
                if (orgWithDirectPrice != null)
                {
                    configs = db.MonitoringConfig.Where(o => orgWithDirectPrice.ConfigDetails.Any(c => o.TemplateId == c.TemplateId && c.Template.ConfigCycleSettings.Any(d => d.BeginTime <= DateTime.Now && d.EndTime >= DateTime.Now))).ToList();
                }
            }
            if (configs.Count() > 0 && !buildingPriceConfigs.Keys.Contains(org.Id))
                buildingPriceConfigs.Add(org.Id, configs);
            else if (configs.Count() > 0)
                buildingPriceConfigs[org.Id] = configs;
            return configs;
        }

    }
}
