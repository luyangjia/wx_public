
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
using System.Text;
using WxPay2017.API.VO.Param;
using WxPay2017.API.BLL.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Reflection;


namespace WxPay2017.API.BLL
{
    public class MeterBLL : Repository<Meter>
    {
        public DictionaryBLL DictionaryBLL { get; set; }

        public BuildingBLL BuildingBLL { get; set; }

        public OrganizationBLL organizationBLL { get; set; }
      

        public MeterBLL(EmpContext context = null, string userName = null)
            : base(context, string.IsNullOrEmpty(userName) ? null : (Expression<Func<Meter, bool>>)(x => x.Building.Organization.Users.Any(u => u.UserName == userName)))
        {
            if (context == null) context = new EmpContext();
            this.DictionaryBLL = new DictionaryBLL(context);
            this.BuildingBLL = new BuildingBLL(context, userName);
            this.organizationBLL = new OrganizationBLL(context, userName);
            
        }

        public IQueryable<Meter> GetChildren(IList<int> id)
        {
            return db.Meters.Where(b => b.Enable && id.Contains(b.ParentId.Value));
        }

        /// <summary>
        /// 获得对应用户集合下的设备集合
        /// </summary>
        /// <param name="result">调用返回值使用</param>
        /// <param name="users">用户集合</param>
        /// <returns>设备集合</returns>
        public IEnumerable<Meter> GetMetersByUsers(IEnumerable<Meter> result, IQueryable<DAL.EmpModels.User> users)
        {
            if (users != null && users.Count() != 0)
            {
                List<Meter> meters = new List<Meter>();
                List<string> usesrIds = new List<string>();
                foreach (var user in users)
                {
                    usesrIds.Add(user.Id);
                }
                result = db.Meters.Where(o => o.Enable && o.Building.Organization.Users.Any(c => usesrIds.Contains(c.Id)));
            }
            return result;
        }


        /// <summary>
        /// 获取所有当前及后代对象列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IQueryable<MeterTree> GetDescents(int id)
        {
            var node = this.Find(id);
            return this.GetDescents(node);
        }


        /// <summary>
        /// 获取同辈对象列表
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public IQueryable<MeterTree> GetPeers(Meter node, bool includeSelf = true)
        {
            //var str = node.ParentId.HasValue ? (node.ParentId.Value + "-" + node.Id) : (node.Id.ToString());
            var str = includeSelf ? "" : " AND Id <> '{0}' ";
            var treenode = this.db.Database.SqlQuery<MeterTree>(string.Format(" SELECT * FROM [Meter].[MeterTree] WHERE Id = {0} AND Enable = 1", node.Id)).FirstOrDefault();
            if (treenode == null) throw new KeyNotFoundException();
            var list = this.db.Database.SqlQuery<MeterTree>(string.Format(" SELECT * FROM [Meter].[MeterTree] WHERE Rank = '{0}' AND Enable = 1" + str, treenode.Rank)).AsQueryable();
            return list;

        }

        /// <summary>
        /// 获取所有符合能源分类集合的meter详情
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public IQueryable<object> GetMetersByEnergyCategories(List<int> list, int? pid = -1)
        {
            if (list.Count <= 0) return this.db.Database.SqlQuery<MeterFullInfo>(" SELECT * FROM [Meter].[MeterFullInfo] WHERE 1=0 ").AsQueryable();
            string array = string.Join(",", list.ToArray());
            string sql = "";
            if (pid == -1)
                sql = string.Format(" SELECT * FROM [Meter].[MeterFullInfo] WHERE EnergyCategoryId in ({0}) ", array);
            else if (pid == 0)
                sql = string.Format(" SELECT * FROM [Meter].[MeterFullInfo] WHERE EnergyCategoryId in ({0}) and ParentId is null ", array);
            else
                sql = string.Format(" SELECT * FROM [Meter].[MeterFullInfo] WHERE EnergyCategoryId in ({0}) and ParentId={1} AND Enable = 1", array, pid);
            var reuslt = this.db.Database.SqlQuery<MeterFullInfo>(sql).AsQueryable();
            return reuslt;
        }

        /// <summary>
        /// 获取所有当前及后代对象列表
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public IQueryable<MeterTree> GetDescents(Meter node)
        {
            var treenode = this.db.Database.SqlQuery<MeterTree>(string.Format(" SELECT * FROM [Meter].[MeterTree] WHERE Id = {0} AND Enable = 1", node.Id)).FirstOrDefault();
            if (treenode == null) throw new KeyNotFoundException();
            var list = this.db.Database.SqlQuery<MeterTree>(string.Format(" SELECT * FROM [Meter].[MeterTree] WHERE TreeId LIKE '{0}%' AND Enable = 1", treenode.TreeId)).AsQueryable();
            return list;
        }

        /// <summary>
        /// 根据能耗类型和设备id获取该类型顶级子设备
        /// </summary>
        /// <param name="meterId"></param>
        /// <param name="energyCategoryId"></param>
        /// <returns></returns>
        public List<int> GetTopChildByEnergyCategoryId(int meterId, int energyCategoryId)
        {
            List<int> result = new List<int>();
            Dictionary<int, string> ids = new Dictionary<int, string>();
            var meter = db.Meters.FirstOrDefault(o => o.Id == meterId);
            if (meter != null)
            {
                var meterIds = db.Meters.Where(o => o.Enable && (o.TreeId == meter.TreeId || o.TreeId.StartsWith(meter.TreeId + "-")) && o.EnergyCategoryId == energyCategoryId).Select(o => new
                {
                    o.Id,
                    o.TreeId
                }).ToList();
                foreach (var m in meterIds)
                {
                    ids.Add(m.Id, m.TreeId);
                    result.Add(m.Id);
                }
                foreach (var m in ids)
                {
                    var childs = ids.Where(o => o.Value.StartsWith(m.Value) && o.Key != m.Key);
                    foreach (var c in childs)
                    {
                        result.Remove(c.Key);
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// 根据能耗类型和设备id获取该类型顶级子设备
        /// </summary>
        /// <param name="meterId"></param>
        /// <param name="energyCategoryId"></param>
        /// <returns></returns>
        public List<int> GetTopChildByEnergyCategoryId(List<int> meterPIds, int energyCategoryId)
        {
            List<int> result = new List<int>();
            Dictionary<int, string> ids = new Dictionary<int, string>();
            var meterTreeIds = db.Meters.Where(o => o.Enable && meterPIds.Contains(o.Id)).Select(o => o.TreeId).ToList();
            if (meterTreeIds != null && meterTreeIds.Count != 0)
            {
                //查找所有子设备
                var meterIds = db.Meters.Where(o => o.Enable && (meterTreeIds.Count(c => o.TreeId == c || o.TreeId.StartsWith(c + "-")) > 0) && o.EnergyCategoryId == energyCategoryId).Select(o => new
                {
                    o.Id,
                    o.TreeId
                }).ToList();
                foreach (var m in meterIds)
                {
                    ids.Add(m.Id, m.TreeId);
                    result.Add(m.Id);
                }
                foreach (var m in ids)
                {
                    var childs = ids.Where(o => o.Value.StartsWith(m.Value) && o.Key != m.Key);
                    foreach (var c in childs)
                    {
                        result.Remove(c.Key);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 获取统计对象
        /// </summary>
        /// <param name="node">统计参数</param>
        /// <returns></returns>
        public async Task<IList<StatisticalTransfer>> GetStatisticalObj(StatisticalNode node)
        {
            IList<StatisticalTransfer> meters = null;
            switch (node.StatMode) /* 判断统计模式 */
            {
                case StatisticalModes.Organization:
                    switch (node.ChildrenMode) /* 判断子级统计模式 */
                    {
                        case StatisticalModes.EnergyCategory:
                            var energyCategories = DictionaryBLL.Get(node.EnergyCategoryId.Value).Children().Select(ec => ec.Id).ToList();
                            meters = await this.StatObjByOrg(node.TargetId.First(), energyCategories, node.StatWay.Value);
                            break;
                        case StatisticalModes.Organization: /* 子机构 */
                            var childrenOrg = organizationBLL.GetChildren(node.TargetId.First());
                            node.TargetId = await childrenOrg.Select(co => co.Id).ToListAsync();
                            goto default;
                        case StatisticalModes.BuildingCategory: /* 子建筑类型 */
                            var childrenBldgForCat = BuildingBLL.GetPrimaryByOrg(node.TargetId.First());
                            node.TargetId = await childrenBldgForCat.Select(co => co.Id).ToListAsync();
                            meters = await this.StatObjByBldg(node.TargetId, node.EnergyCategoryId.Value, node.StatWay.Value, 20004 /* TODO: 根据项目类型传参 */);
                            break;
                        case StatisticalModes.Building: /* 子建筑 */
                            var childrenBldg = await BuildingBLL.GetPrimaryByOrg(node.TargetId.First()).Select(cb => cb.Id).ToListAsync();
                            meters = await this.StatObjByBldg(childrenBldg, node.EnergyCategoryId.Value, node.StatWay.Value);
                            break;
                        default: /* 无子级模式 */
                            meters = await this.StatObjByOrg(node.TargetId, node.EnergyCategoryId.Value, node.StatWay.Value);
                            break;
                    }
                    break;
                case StatisticalModes.Building:
                    switch (node.ChildrenMode) /* 判断子级统计模式 */
                    {
                        case StatisticalModes.EnergyCategory:
                            var energyCategories = DictionaryBLL.Get(node.EnergyCategoryId.Value).Children().Select(ec => ec.Id).ToList();
                            meters = await this.StatObjByBldg(node.TargetId.First(), energyCategories, node.StatWay.Value);
                            break;
                        case StatisticalModes.Organization: /* 子机构 */
                            var buildingId = node.TargetId.First();
                            var childrenOrg = organizationBLL.GetChildrenByBldg(buildingId);
                            node.TargetId = childrenOrg.Select(co => co.Id).ToList();
                            meters = await this.StatObjByBldg(buildingId, node.EnergyCategoryId.Value, node.StatWay.Value, node.TargetId);
                            break;
                        case StatisticalModes.Building: /* 子建筑 */
                            var childrenBldg = BuildingBLL.GetChildren(node.TargetId.First());
                            node.TargetId = await childrenBldg.Select(co => co.Id).ToListAsync();
                            goto default;
                        case StatisticalModes.Meter: /* 子设备 */
                            var childrenMtr = await this.StatObjByBldg(node.TargetId, node.EnergyCategoryId.Value, node.StatWay.Value);
                            node.TargetId = (childrenMtr.FirstOrDefault()).Meters.Select(cm => cm.Id).ToList();
                            meters = await this.StatObjByMtr(node.TargetId, node.EnergyCategoryId.Value, node.StatWay.Value);
                            break;
                        //goto default;
                        //meters = await this.StatObjByBldg(node.TargetId, node.EnergyCategoryId.Value, node.StatWay.Value);
                        //break;
                        default: /* 无子级模式 */
                            meters = await this.StatObjByBldg(node.TargetId, node.EnergyCategoryId.Value, node.StatWay.Value);
                            break;
                    }
                    break;
                case StatisticalModes.Meter:
                    switch (node.ChildrenMode) /* 判断子级统计模式 */
                    {
                        case StatisticalModes.Meter: /* 子设备 */
                            var childrenMtr = this.GetChildren(node.TargetId);
                            node.TargetId = await childrenMtr.Select(co => co.Id).ToListAsync();
                            goto default;
                        default: /* 无子级模式 */
                            meters = await this.StatObjByMtr(node.TargetId, node.EnergyCategoryId.Value, node.StatWay.Value);
                            break;
                    }
                    break;
                default:
                    throw new Exception("系统目前不支持此统计模式");
            }
            return meters;
        }

        /// <summary>
        /// 通过建筑获取关联的一级设备清单
        /// </summary>
        /// <param name="buildingId">建筑编号</param>
        /// <param name="energyCategoryId">能耗类型编号</param>
        /// <param name="way">统计方式</param>
        /// <returns></returns>
        public async Task<IList<StatisticalTransfer>> StatObjByBldg(IList<int> buildingId, int energyCategoryId, StatisticalWay way)
        {
            IList<int> bids = new List<int>(buildingId.ToArray());
            IList<StatisticalTransfer> results = new List<StatisticalTransfer>();
            for (int i = 0; i < bids.Count; i++)
            {
                if (MeterCache.MetersUnderBuiding.Keys.Contains(bids[i] + "_" + energyCategoryId + "_" + way))
                {
                    var model = MeterCache.MetersUnderBuiding[bids[i] + "_" + energyCategoryId + "_" + way];
                    if (model!=null)
                    {
                        var r = new StatisticalTransfer
                        {
                            StatisticalId = model.StatisticalId,
                            StatisticalParentId = model.StatisticalId,
                            StatisticalTreeId = model.StatisticalTreeId,
                            StatisticalName = model.StatisticalName,
                            StatisticalWay = way,
                            EnergyCategoryId = energyCategoryId,
                            EnergyCategoryName = model.EnergyCategoryName,
                            FormulaParam1 = model.FormulaParam1,
                            Meters = model.Meters
                        };
                        results.Add(r);
                        
                    }
                    bids.Remove(bids[i]);
                    i--;
                }
            }
            if (bids.Count != 0)
            {
                var energyCategories = await DictionaryBLL.GetDescendantsId(energyCategoryId, true); // 获取后代能耗类型
                var energyCategory = DictionaryBLL.Get(energyCategoryId); // 获取统计的能耗类型

                // 获取一级设备统计对象
                var meters = from bp in db.Buildings.Where(b => b.Enable && bids.Contains(b.Id)) // 建筑统计对象
                             from bc in db.Buildings.Where(b => b.Enable && b.TreeId == bp.TreeId || b.TreeId.StartsWith(bp.TreeId + "-")) // 关联的建筑树
                             from m in bc.Meters.Where(m => m.Enable && energyCategories.Contains(m.EnergyCategoryId) // 能耗类型树关联的设备
                                 && m.TypeDict.ThirdValue.Value == 1
                                 && m.Enable) // 有效计量设备
                             group new { bp, m } by new
                             {
                                 bp.Id,
                                 bp.ParentId,
                                 bp.TreeId,
                                 bp.Name,
                                 ManagerCount = bp.ManagerCount,
                                 TotalArea = bp.TotalArea
                             } into g
                             select new StatisticalTransfer
                             {
                                 StatisticalId = g.Key.Id,
                                 StatisticalParentId = g.Key.ParentId,
                                 StatisticalTreeId = g.Key.TreeId,
                                 StatisticalName = g.Key.Name,
                                 StatisticalWay = way,
                                 EnergyCategoryId = energyCategoryId,
                                 EnergyCategoryName = energyCategory.ChineseName,
                                 FormulaParam1 = way == StatisticalWay.PerCapita ?
                                                         (decimal?)g.Key.ManagerCount :
                                                         (way == StatisticalWay.PerUnitArea ?
                                                             (decimal?)g.Key.TotalArea :
                                                             null), // 计算统计对象关联的办公人数或单位面积数
                                 MeterDatas = g.Select(gr => new MeterData
                                 {
                                     Id = gr.m.Id,
                                     ParentId = gr.m.ParentId,
                                 })
                                 //Meters = from ml in g.Select(gr => gr.m)
                                 //         join il in g.Select(gr => gr.m)
                                 //         on ml.ParentId equals il.Id into temp
                                 //         from em in temp.DefaultIfEmpty()
                                 //         where em == null
                                 //         select ml // 获取关联的一级设备
                             };
                IList<StatisticalTransfer> result2s = await meters.AsNoTracking().ToListAsync();
                for (int i = 0; i < result2s.Count(); i++)
                {
                    result2s[i].Meters = results[i].MeterDatas.Where(o => !result2s[i].MeterDatas.Any(c => c.Id == o.ParentId)).Select(m => new Meter { Id = m.Id }).ToList();
                }
                foreach (var result in result2s)
                {
                    results.Add(result);
                    if (!MeterCache.MetersUnderBuiding.Keys.Contains(result.StatisticalId + "_" + result.EnergyCategoryId + "_" + way))
                        MeterCache.MetersUnderBuiding.Add(result.StatisticalId + "_" + result.EnergyCategoryId + "_" + way, result);
                    else
                        MeterCache.MetersUnderBuiding[result.StatisticalId + "_" + result.EnergyCategoryId + "_" + way]=result;
                }
                var resultIds=result2s.Select(o=>o.StatisticalId).ToList();
                foreach (var bid in bids)
                {
                    if (!resultIds.Contains(bid))
                    {
                        //无此类型能耗设备
                        if (!MeterCache.MetersUnderBuiding.Keys.Contains(bid + "_" + energyCategoryId + "_" + way))
                            MeterCache.MetersUnderBuiding.Add(bid + "_" + energyCategoryId + "_" + way, null);
                        else
                            MeterCache.MetersUnderBuiding[bid + "_" + energyCategoryId + "_" + way] = null;
                    }
                }
            }
            return results;
        }

        #region 针对宿舍最底层建筑的快速设备查找
        /// <summary>
        /// 获取统计对象针对宿舍最底层建筑的快速设备查找
        /// </summary>
        /// <param name="node">统计参数</param>
        /// <returns></returns>
        public IList<StatisticalTransfer> GetStatisticalObjForDormitory(StatisticalNode node)
        {
            IList<StatisticalTransfer> meters = null;
            meters =  this.StatObjByBldgForDormitory(node.TargetId, node.EnergyCategoryId.Value, node.StatWay.Value);
            return meters;
        }

        /// <summary>
        /// 通过单个建筑获取关联的一级设备清单针对宿舍最底层建筑的快速设备查找
        /// </summary>
        /// <param name="buildingId">建筑编号</param>
        /// <param name="energyCategoryId">能耗类型编号</param>
        /// <param name="way">统计方式</param>
        /// <returns></returns>
        public IList<StatisticalTransfer> StatObjByBldgForDormitory(IList<int> buildingId, int energyCategoryId, StatisticalWay way)
        {
            var str = "";
            foreach (var bid in buildingId)
                str = str + bid + "_";
            str = str + "|" + energyCategoryId + "!" + way ;
            if (MeterCache.MetersByEnergy.Keys.Contains(str))
                return MeterCache.MetersByEnergy[str];


            int id = buildingId[0];
            var energyCategory = DictionaryCache.Get()[energyCategoryId];
            var energyCategories = DictionaryCache.Get().Values.Where(o => o.Id == energyCategoryId || o.TreeId.StartsWith(energyCategory.TreeId + "-")).Select(o => o.Id).ToList();
           
            // 获取一级设备统计对象,宿舍，只考虑1级设备
            var building = BuildingBLL.Find(id);
            var meterLists = this.Filter(o => (o.BuildingId == id||o.Building.TreeId.StartsWith(building.TreeId+"-")) && energyCategories.Contains(o.EnergyCategoryId)&& o.Enable&&o.TypeDict.ThirdValue.Value==1).ToList();
            var treeIds=meterLists.Select(o=>o.TreeId).ToList();
            meterLists = meterLists.Where(o => !meterLists.Any(c => o.TreeId.StartsWith(c.TreeId+"-"))).ToList();
            var meter = new StatisticalTransfer
            {
                StatisticalId = building.Id,
                StatisticalParentId = building.ParentId,
                StatisticalTreeId = building.TreeId,
                StatisticalName = building.Name,
                StatisticalWay = way,
                EnergyCategoryId = energyCategoryId,
                EnergyCategoryName = energyCategory.ChineseName,
                FormulaParam1 = way == StatisticalWay.PerCapita ?
                                        (decimal?)building.ManagerCount :
                                        (way == StatisticalWay.PerUnitArea ?
                                            (decimal?)building.TotalArea :
                                            null), // 计算统计对象关联的办公人数或单位面积数
                Meters = meterLists
            };

            var meters = new List<StatisticalTransfer>();
            meters.Add(meter);
            if (!MeterCache.MetersByEnergy.Keys.Contains(str))
                MeterCache.MetersByEnergy.Add(str, meters);
            else
                MeterCache.MetersByEnergy[str] = meters;
            return meters;
        }
        #endregion

        /// <summary>
        /// 通过建筑获取能耗类型关联的一级设备清单
        /// </summary>
        /// <param name="buildingId">建筑编号</param>
        /// <param name="energyCategoryId">能耗类型编号</param>
        /// <param name="way">统计方式</param>
        /// <returns></returns>
        public async Task<IList<StatisticalTransfer>> StatObjByBldg(int buildingId, IList<int> energyCategoryId, StatisticalWay way)
        {
            if (MeterCache.MetersByEnergy.Keys.Contains(getKey(buildingId, energyCategoryId, way)))
                return MeterCache.MetersByEnergy[getKey(buildingId, energyCategoryId, way)];

            // 获取一级设备统计对象
            var building = BuildingBLL.Find(buildingId);
            var chdBuildingId = db.Buildings.Where(b => b.Enable && b.TreeId == building.TreeId || b.TreeId.StartsWith(building.TreeId + "-")).Select(b => b.Id);
            var meters = from pd in db.Dictionaries.Where(d => d.Enable && energyCategoryId.Contains(d.Id))
                         from m in db.Meters
                         where (m.Enable && m.EnergyCategoryDict.TreeId == pd.TreeId || m.EnergyCategoryDict.TreeId.StartsWith(pd.TreeId + "-"))
                         && chdBuildingId.Contains(m.BuildingId.Value)
                         && m.EnergyCategoryId == m.EnergyCategoryId
                         && m.TypeDict.ThirdValue.Value == 1
                         && m.Enable
                         group new { dp = pd, m } by new
                         {
                             EcId = pd.Id,
                             EcName = pd.ChineseName
                         } into g
                         select new StatisticalTransfer
                         {
                             StatisticalId = building.Id,
                             StatisticalTreeId = building.TreeId,
                             StatisticalName = building.Name,
                             StatisticalWay = way,
                             EnergyCategoryId = g.Key.EcId,
                             EnergyCategoryName = g.Key.EcName,
                             MeterDatas = g.Select(gr => new MeterData
                             {
                                 Id = gr.m.Id,
                                 ParentId = gr.m.ParentId,
                             })
                             //Meters = from ml in g.Select(gr => gr.m)
                             //         join il in g.Select(gr => gr.m)
                             //         on ml.ParentId equals il.Id into temp
                             //         from em in temp.DefaultIfEmpty()
                             //         where em == null
                             //         select ml // 获取关联的一级设备
                         };
            var results= await meters.AsNoTracking().ToListAsync();
            for (int i = 0; i < results.Count(); i++)
            {
                results[i].Meters = results[i].MeterDatas.Where(o => !results[i].MeterDatas.Any(c => c.Id == o.ParentId)).Select(m => new Meter { Id = m.Id }).ToList();
            }
            var key = getKey(buildingId, energyCategoryId, way);
            if (!MeterCache.MetersByEnergy.Keys.Contains(key))
                MeterCache.MetersByEnergy.Add(key, results);
            else
                MeterCache.MetersByEnergy[key] = results;
            return results;
        }

        private string getKey(int buildingId, IList<int> energyCategoryId, StatisticalWay way)
        {
            var str= buildingId + "_";
            foreach (var id in energyCategoryId)
                str = str + id + "_";
            str = str + way;
            return str;
        }

        /// <summary>
        /// 通过建筑获取建筑类型关联的一级设备清单
        /// </summary>
        /// <param name="buildingId">建筑编号</param>
        /// <param name="energyCategoryId">能耗类型编号</param>
        /// <param name="way">统计方式</param>
        /// <param name="buildingCategoryId">父级建筑类型编号</param>
        /// <returns></returns>
        public async Task<IList<StatisticalTransfer>> StatObjByBldg(IList<int> buildingId, int energyCategoryId, StatisticalWay way, int buildingCategoryId)
        {
            var str = "";
            foreach (var bid in buildingId)
                str = str + bid + "_";
            str = str + "|" + energyCategoryId + "!" + way + "@" + buildingCategoryId;
            if (MeterCache.MetersByEnergy.Keys.Contains(str))
                return MeterCache.MetersByEnergy[str];

            var energyCategories = await DictionaryBLL.GetDescendantsId(energyCategoryId, true); // 获取后代能耗类型
            var buildingCategories = await DictionaryBLL.GetDescendantsId(buildingCategoryId); // 获取后代建筑类型
            var energyCategory = DictionaryBLL.Get(energyCategoryId); // 获取统计的能耗类型

            // 获取一级设备统计对象
            var meters = from bp in db.Buildings.Where(b => b.Enable && buildingId.Contains(b.Id) && buildingCategories.Contains(b.BuildingCategoryId)) // 子建筑能耗关联的建筑统计对象
                         from bc in db.Buildings.Where(b => b.Enable && b.TreeId == bp.TreeId || b.TreeId.StartsWith(bp.TreeId + "-")) // 关联的建筑树
                         from m in bc.Meters.Where(m => energyCategories.Contains(m.EnergyCategoryId) // 能耗类型树关联的设备
                             && m.TypeDict.ThirdValue.Value == 1
                             && m.Enable) // 有效计量设备
                         group new { bp, m } by new
                         {
                             bp.BuildingCategoryId,
                             bp.BuildingCategoryDict.Code,
                             bp.BuildingCategoryDict.FirstValue,
                             bp.BuildingCategoryDict.SecondValue,
                             bp.BuildingCategoryDict.ChineseName
                         } into g
                         select new StatisticalTransfer
                         {
                             StatisticalId = g.Key.BuildingCategoryId,
                             StatisticalParentId = buildingCategoryId,
                             StatisticalTreeId = g.Key.Code + "-" + g.Key.FirstValue.Value.ToString() + (g.Key.SecondValue.HasValue ? "-" + g.Key.SecondValue.Value.ToString() : string.Empty),
                             StatisticalName = g.Key.ChineseName,
                             StatisticalWay = way,
                             EnergyCategoryId = energyCategoryId,
                             EnergyCategoryName = energyCategory.ChineseName,
                             FormulaParam1 = way == StatisticalWay.PerCapita ?
                                                     (decimal?)g.Sum(gb => gb.bp.ManagerCount) :
                                                     (way == StatisticalWay.PerUnitArea ?
                                                         (decimal?)g.Sum(gb => gb.bp.TotalArea) :
                                                         null), // 计算统计对象关联的办公人数或单位面积数
                             MeterDatas = g.Select(gr => new MeterData
                             {
                                 Id = gr.m.Id,
                                 ParentId = gr.m.ParentId,
                             })
                             //Meters = from ml in g.Select(gr => gr.m)
                             //         join il in g.Select(gr => gr.m)
                             //         on ml.ParentId equals il.Id into temp
                             //         from em in temp.DefaultIfEmpty()
                             //         where em == null
                             //         select ml // 获取关联的一级设备
                         };
            var results = await meters.AsNoTracking().ToArrayAsync();
            for (int i = 0; i < results.Count(); i++)
            {
                results[i].Meters = results[i].MeterDatas.Where(o => !results[i].MeterDatas.Any(c => c.Id == o.ParentId)).Select(m => new Meter { Id = m.Id }).ToList();
            }
            if (!MeterCache.MetersByEnergy.Keys.Contains(str))
                MeterCache.MetersByEnergy.Add(str, results);
            else
                MeterCache.MetersByEnergy[str] = results;
            return results;
        }

        /// <summary>
        /// 通过建筑获取机构关联的一级设备清单
        /// </summary>
        /// <param name="organizationId">机构编号</param>
        /// <param name="energyCategoryId">能耗类型编号</param>
        /// <param name="way">统计方式</param>
        /// <param name="buildingCategoryId">父级建筑类型编号</param>
        /// <returns></returns>
        public async Task<IList<StatisticalTransfer>> StatObjByBldg(int buildingId, int energyCategoryId, StatisticalWay way, IList<int> organizationId)
        {
            var str = buildingId + "";
            str = str + "|" + energyCategoryId + "!" + way + "#";
            foreach (var item in organizationId)
            {
                str = str + item + "-";
            }
            if (MeterCache.MetersByEnergy.Keys.Contains(str))
                return MeterCache.MetersByEnergy[str];

            var energyCategories = await DictionaryBLL.GetDescendantsId(energyCategoryId, true); // 获取后代能耗类型
            var energyCategory = DictionaryBLL.Get(energyCategoryId); // 获取统计的能耗类型
            var building = BuildingBLL.Find(buildingId); // 建筑统计对象

            // 获取一级设备统计对象
            var meters = from op in db.Organizations.Where(o => o.Enable && organizationId.Contains(o.Id)) // 关联的机构
                         from oc in db.Organizations.Where(o => o.Enable && o.TreeId == op.TreeId || o.TreeId.StartsWith(op.TreeId + "-")) // 关联的机构树
                         from b in oc.Buildings.Where(b => b.Enable && b.TreeId == building.TreeId || b.TreeId.StartsWith(building.TreeId + "-")) // 关联的建筑树
                         from m in b.Meters.Where(m => energyCategories.Contains(m.EnergyCategoryId) // 能耗类型树关联的设备
                             && m.TypeDict.ThirdValue.Value == 1
                             && m.Enable) // 有效计量设备
                         group new { op, m } by new
                         {
                             op.Id,
                             op.ParentId,
                             op.TreeId,
                             op.Name,
                             ManagerCount = op.ManagerCount,
                             TotalArea = op.Buildings.Where(ob => ob.Enable && !ob.ParentId.HasValue
                                 || !op.Buildings.Select(opb => opb.Id).Contains(ob.ParentId.Value)
                                 ).Sum(ob => ob.TotalArea)
                         } into g
                         select new StatisticalTransfer
                         {
                             StatisticalId = g.Key.Id,
                             StatisticalParentId = g.Key.ParentId,
                             StatisticalTreeId = g.Key.TreeId,
                             StatisticalName = g.Key.Name,
                             StatisticalWay = way,
                             EnergyCategoryId = energyCategoryId,
                             EnergyCategoryName = energyCategory.ChineseName,
                             FormulaParam1 = way == StatisticalWay.PerCapita ?
                                                     (decimal?)g.Key.ManagerCount :
                                                     (way == StatisticalWay.PerUnitArea ?
                                                         (decimal?)g.Key.TotalArea :
                                                         null), // 计算统计对象关联的办公人数或单位面积数
                             MeterDatas = g.Select(gr => new MeterData
                             {
                                 Id = gr.m.Id,
                                 ParentId = gr.m.ParentId,
                             })
                             //Meters = from ml in g.Select(gr => gr.m)
                             //         join il in g.Select(gr => gr.m)
                             //         on ml.ParentId equals il.Id into temp
                             //         from em in temp.DefaultIfEmpty()
                             //         where em == null
                             //         select ml // 获取关联的一级设备
                         };
            var results = await meters.AsNoTracking().ToArrayAsync();
            for (int i = 0; i < results.Count(); i++)
            {
                results[i].Meters = results[i].MeterDatas.Where(o => !results[i].MeterDatas.Any(c => c.Id == o.ParentId)).Select(m => new Meter { Id = m.Id }).ToList();
            }
            if (!MeterCache.MetersByEnergy.Keys.Contains(str))
                MeterCache.MetersByEnergy.Add(str, results);
            else
                MeterCache.MetersByEnergy[str] = results;
            return results;
        }

        /// <summary>
        /// 通过机构获取关联的一级设备清单
        /// </summary>
        /// <param name="organizationId">机构编号</param>
        /// <param name="energyCategoryId">能耗类型编号</param>
        /// <param name="way">统计方式</param>
        /// <returns></returns>
        public async Task<IList<StatisticalTransfer>> StatObjByOrg(IList<int> organizationId, int energyCategoryId, StatisticalWay way)
        {
            IList<int> oids = new List<int>(organizationId.ToArray());
            IList<StatisticalTransfer> results = new List<StatisticalTransfer>();
            for (int i = 0; i < oids.Count(); i++)
            {
                if (MeterCache.MetersUnderOrg.Keys.Contains(oids[i] + "_" + energyCategoryId + "_" + way))
                {
                    var model = MeterCache.MetersUnderOrg[oids[i] + "_" + energyCategoryId + "_" + way];
                    if (model!=null)
                    {
                        var r = new StatisticalTransfer
                        {
                            StatisticalId = model.StatisticalId,
                            StatisticalParentId = model.StatisticalId,
                            StatisticalTreeId = model.StatisticalTreeId,
                            StatisticalName = model.StatisticalName,
                            StatisticalWay = way,
                            EnergyCategoryId = energyCategoryId,
                            EnergyCategoryName = model.EnergyCategoryName,
                            FormulaParam1 = model.FormulaParam1,
                            Meters = model.Meters
                        };
                        results.Add(r);
                       
                    }
                    oids.Remove(oids[i]);
                    i--;
                }
            }
            if (oids.Count != 0)
            {
               
                var energyCategories = await DictionaryBLL.GetDescendantsId(energyCategoryId, true); // 获取后代能耗类型
                var energyCategory = DictionaryBLL.Get(energyCategoryId); // 获取统计的能耗类型

                // 获取一级设备统计对象
                var meters = from op in db.Organizations.Where(o => o.Enable && oids.Contains(o.Id)) // 机构统计对象
                             from oc in db.Organizations.Where(o => o.Enable && o.TreeId == op.TreeId || o.TreeId.StartsWith(op.TreeId + "-")) // 关联的机构树
                             from b in oc.Buildings // 关联的建筑树
                             from m in b.Meters.Where(m => energyCategories.Contains(m.EnergyCategoryId) // 能耗类型树关联的设备
                                 && m.TypeDict.ThirdValue.Value == 1
                                 && m.Enable) // 有效计量设备
                             group new { op, m } by new
                             {
                                 op.Id,
                                 op.ParentId,
                                 op.TreeId,
                                 op.Name,
                                 ManagerCount = op.ManagerCount,
                                 TotalArea = op.Buildings.Where(ob => ob.Enable && !ob.ParentId.HasValue
                                     || !op.Buildings.Select(opb => opb.Id).Contains(ob.ParentId.Value)
                                     ).Sum(ob => ob.TotalArea)
                             } into g
                             select new StatisticalTransfer
                             {
                                 StatisticalId = g.Key.Id,
                                 StatisticalParentId = g.Key.ParentId,
                                 StatisticalTreeId = g.Key.TreeId,
                                 StatisticalName = g.Key.Name,
                                 StatisticalWay = way,
                                 EnergyCategoryId = energyCategoryId,
                                 EnergyCategoryName = energyCategory.ChineseName,
                                 FormulaParam1 = way == StatisticalWay.PerCapita ?
                                                         (decimal?)g.Key.ManagerCount :
                                                         (way == StatisticalWay.PerUnitArea ?
                                                             (decimal?)g.Key.TotalArea :
                                                             null), // 计算统计对象关联的办公人数或单位面积数
                                 MeterDatas = g.Select(gr => new MeterData
                                 {
                                     Id = gr.m.Id,
                                     ParentId = gr.m.ParentId,
                                 })
                             };
                IList<StatisticalTransfer> result2s = await meters.AsNoTracking().ToListAsync();

                for (int i = 0; i < result2s.Count(); i++)
                {
                    result2s[i].Meters = results[i].MeterDatas.Where(o => !result2s[i].MeterDatas.Any(c => c.Id == o.ParentId)).Select(m => new Meter { Id = m.Id }).ToList();
                }
                foreach (var result in result2s)
                {
                    results.Add(result);
                    if (!MeterCache.MetersUnderOrg.Keys.Contains(result.StatisticalId + "_" + result.EnergyCategoryId + "_" + way))
                        MeterCache.MetersUnderOrg.Add(result.StatisticalId + "_" + result.EnergyCategoryId + "_" + way, result);
                    else
                        MeterCache.MetersUnderOrg[result.StatisticalId + "_" + result.EnergyCategoryId + "_" + way] = result;
                }
                var resultIds = result2s.Select(o => o.StatisticalId).ToList();
                foreach (var oid in oids)
                {
                    if (!resultIds.Contains(oid))
                    {
                        //无此类型能耗设备
                        if (!MeterCache.MetersUnderOrg.Keys.Contains(oid + "_" + energyCategoryId + "_" + way))
                            MeterCache.MetersUnderOrg.Add(oid + "_" + energyCategoryId + "_" + way, null);
                        else
                            MeterCache.MetersUnderOrg[oid + "_" + energyCategoryId + "_" + way] = null;

                        
                    }
                }
            }
            return results;
        }

        /// <summary>
        /// 获取单个机构关联的一级设备清单
        /// </summary>
        /// <param name="organizationId">机构编号</param>
        /// <param name="energyCategoryId">能耗类型编号</param>
        /// <param name="way">统计方式</param>
        /// <returns></returns>
        public async Task<IList<StatisticalTransfer>> StatObjByOrg(int organizationId, int energyCategoryId, StatisticalWay way)
        {
            var str = organizationId + "";
            str = str + "@|" + energyCategoryId + "@!" + way + "！#";
            if (MeterCache.MetersByEnergy.Keys.Contains(str))
                return MeterCache.MetersByEnergy[str];

            var energyCategories = await DictionaryBLL.GetDescendantsId(energyCategoryId, true); // 获取后代能耗类型
            var energyCategory = DictionaryBLL.Get(energyCategoryId); // 获取统计的能耗类型
            var organization = organizationBLL.Find(organizationId); // 机构统计对象
            // 获取关联的一级设备
            var allMeters = from b in db.Buildings
                            from m in b.Meters
                            where (b.Enable && b.Organization.TreeId == organization.TreeId || b.Organization.TreeId.StartsWith(organization.TreeId + "-"))
                            && energyCategories.Contains(m.EnergyCategoryId) // 能耗类型树关联的设备
                            && m.TypeDict.ThirdValue.Value == 1
                            && m.Enable // 有效计量设备
                            select m;
            var meterList = (from ml in allMeters
                             join il in allMeters
                             on ml.ParentId equals il.Id into temp
                             from em in temp.DefaultIfEmpty()
                             where em == null
                             select ml).AsNoTracking().ToList();

            List<StatisticalTransfer> meters = new List<StatisticalTransfer>();
            // 获取一级设备统计对象
            var statObj = new StatisticalTransfer
            {
                StatisticalId = organization.Id,
                StatisticalParentId = organization.ParentId,
                StatisticalTreeId = organization.TreeId,
                StatisticalName = organization.Name,
                StatisticalWay = way,
                EnergyCategoryId = energyCategoryId,
                EnergyCategoryName = energyCategory.ChineseName,
                FormulaParam1 = way == StatisticalWay.PerCapita ?
                                        (decimal?)organization.ManagerCount :
                                        (way == StatisticalWay.PerUnitArea ?
                                            (decimal?)organization.Buildings.Where(ob => ob.Enable && !ob.ParentId.HasValue
                                            || !organization.Buildings.Select(opb => opb.Id).Contains(ob.ParentId.Value)
                                            ).Sum(ob => ob.TotalArea) :
                                            null), // 计算统计对象关联的办公人数或单位面积数
                Meters = meterList
            };
            meters.Add(statObj);
            if (!MeterCache.MetersByEnergy.Keys.Contains(str))
                MeterCache.MetersByEnergy.Add(str, meters);
            else
                MeterCache.MetersByEnergy[str] = meters;
            return meters;
        }


        public async Task<IList<StatisticalTransfer>> StatObjByOrg(int organizationId, IList<int> energyCategoryId, StatisticalWay way)
        {
            var str = organizationId + "";
            str = str + "||";
            foreach (var item in energyCategoryId)
            {
                str = str + item + "-";
            }
            str = str + "@@" + way;

            if (MeterCache.MetersByEnergy.Keys.Contains(str))
                return MeterCache.MetersByEnergy[str];

            // 获取一级设备统计对象
            var organization = this.organizationBLL.Find(organizationId);
            var chdBuildingId = db.Buildings.Where(o => o.Enable && o.Organization.TreeId == organization.TreeId || o.Organization.TreeId.StartsWith(organization.TreeId + "-")).Select(o => o.Id);
            var meters = from pd in db.Dictionaries.Where(d => d.Enable && energyCategoryId.Contains(d.Id))
                         from m in db.Meters
                         where (m.EnergyCategoryDict.TreeId == pd.TreeId || m.EnergyCategoryDict.TreeId.StartsWith(pd.TreeId + "-"))
                         && chdBuildingId.Contains(m.BuildingId.Value) && m.EnergyCategoryId == m.EnergyCategoryId
                         && m.TypeDict.ThirdValue.Value == 1
                         && m.Enable
                         group new { dp = pd, m } by new
                         {
                             EcId = pd.Id,
                             EcNmae = pd.ChineseName
                         } into g
                         select new StatisticalTransfer
                         {
                             StatisticalId = organization.Id,
                             StatisticalTreeId = organization.TreeId,
                             StatisticalName = organization.Name,
                             StatisticalWay = way,
                             EnergyCategoryId = g.Key.EcId,
                             EnergyCategoryName = g.Key.EcNmae,
                             MeterDatas = g.Select(gr => new MeterData
                             {
                                 Id = gr.m.Id,
                                 ParentId = gr.m.ParentId,
                             })
                         };
            var results = await meters.AsNoTracking().ToArrayAsync();
            for (int i = 0; i < results.Count(); i++)
            {
                results[i].Meters = results[i].MeterDatas.Where(o => !results[i].MeterDatas.Any(c => c.Id == o.ParentId)).Select(m => new Meter { Id = m.Id }).ToList();
            }
            if (!MeterCache.MetersByEnergy.Keys.Contains(str))
                MeterCache.MetersByEnergy.Add(str, results);
            else
                MeterCache.MetersByEnergy[str] = results;
            return results;
        }

        /// <summary>
        /// 通过设备获取关联的子设备清单
        /// </summary>
        /// <param name="meterId">设备编号</param>
        /// <param name="energyCategoryId">能耗类型编号</param>
        /// <param name="way">统计方式</param>
        /// <returns></returns>
        public async Task<IList<StatisticalTransfer>> StatObjByMtr(IList<int> meterId, int energyCategoryId, StatisticalWay way)
        {
            var str = "";
            foreach (var item in meterId)
            {
                str = str + item + "-";
            }

            str = str + "%" + energyCategoryId + "%%";
            str = str + "@@" + way;

            if (MeterCache.MetersByEnergy.Keys.Contains(str))
                return MeterCache.MetersByEnergy[str];

            var energyCategories = await DictionaryBLL.GetDescendantsId(energyCategoryId, true); // 获取后代能耗类型
            var energyCategory = DictionaryBLL.Get(energyCategoryId); // 获取统计的能耗类型

            // 获取一级设备统计对象
            var meters = from op in db.Meters.Where(m => m.Enable && meterId.Contains(m.Id))
                         from oc in db.Meters.Where(m => energyCategories.Contains(m.EnergyCategoryId) // 能耗类型树关联的设备
                             && m.TypeDict.ThirdValue.Value == 1
                             && m.Enable) // 有效计量设备
                         where oc.Enable && oc.TreeId == op.TreeId || oc.TreeId.StartsWith(op.TreeId + "-")
                         group new { op, oc } by new
                         {
                             op.Id,
                             op.ParentId,
                             op.TreeId,
                             op.Name,
                             ManagerCount = op.Building.ManagerCount,
                             TotalArea = op.Building.TotalArea
                         } into g
                         select new StatisticalTransfer
                         {
                             StatisticalId = g.Key.Id,
                             StatisticalParentId = g.Key.ParentId,
                             StatisticalTreeId = g.Key.TreeId,
                             StatisticalName = g.Key.Name,
                             StatisticalWay = way,
                             EnergyCategoryId = energyCategoryId,
                             EnergyCategoryName = energyCategory.ChineseName,
                             FormulaParam1 = way == StatisticalWay.PerCapita ?
                                                     (decimal?)g.Key.ManagerCount :
                                                     (way == StatisticalWay.PerUnitArea ?
                                                         (decimal?)g.Key.TotalArea :
                                                         null), // 计算统计对象关联的办公人数或单位面积数
                             MeterDatas = g.Select(gr => new MeterData
                             {
                                 Id = gr.oc.Id,
                                 ParentId = gr.oc.ParentId,
                             })
                         };
            var results = await meters.AsNoTracking().ToArrayAsync();
            for (int i = 0; i < results.Count(); i++)
            {
                results[i].Meters = results[i].MeterDatas.Where(o => !results[i].MeterDatas.Any(c => c.Id == o.ParentId)).Select(m => new Meter { Id = m.Id }).ToList();
            }
            if (!MeterCache.MetersByEnergy.Keys.Contains(str))
                MeterCache.MetersByEnergy.Add(str, results);
            else
                MeterCache.MetersByEnergy[str] = results;
            return results;
        }
        /// <summary>
        /// 获取指定时间设备最近的读值
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="parameters"></param>
        /// <param name="meter"></param>
        /// <param name="data"></param>
        public decimal GetMeterData(DateTime startTime, List<int> parameters, Meter meter, ref DateTime returnTime, ref string unit, List<OriginalData> originalData = null)
        {
            unit = "";
            OriginalData originalDataBegin = GetDataByTime(startTime, meter,originalData);
            if (originalDataBegin != null)
            {
                returnTime = originalDataBegin.CollectTime;
                var nums = meter.Brand.Parameters.OrderBy(o => o.Id).ToList();
                int num = -1;
                for (int i = 0; i < nums.Count; i++)
                    if (parameters.Contains(nums[i].TypeId))
                    {
                        num = (int)nums[i].Type.ThirdValue;
                        unit = nums[i].Unit;
                        break;
                    }
                if (num != -1)
                {
                    string pStr = "Parameter" + string.Format("{0:00}", num);
                    Type type = typeof(OriginalData);
                    PropertyInfo pi = type.GetProperty(pStr);
                    return (decimal)pi.GetValue(originalDataBegin, null);
                }
            }
            return 0;
        }
        /// <summary>
        /// 获取最近的一个读值数据
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="meter"></param>
        private OriginalData GetDataByTime(DateTime startTime, Meter meter, List<OriginalData> originalDatas = null)
        {
            OriginalData originalData;
            List<OriginalData> originalData1, originalData2;
            if (originalDatas == null)
                originalData = db.OriginalDatas.FirstOrDefault(o => o.MeterId == meter.Id && o.CollectTime == startTime);
            else
                originalData = originalDatas.FirstOrDefault(o => o.MeterId == meter.Id && o.CollectTime == startTime);
            if (originalData == null)
            {
                if (originalDatas == null)
                {
                    originalData1 = db.OriginalDatas.Where(o => o.MeterId == meter.Id && o.CollectTime > startTime).OrderBy(o => o.CollectTime).Take(1).ToList();
                    originalData2 = db.OriginalDatas.Where(o => o.MeterId == meter.Id && o.CollectTime < startTime).OrderByDescending(o => o.CollectTime).Take(1).ToList();
                }
                else
                {
                    originalData1 = originalDatas.Where(o => o.MeterId == meter.Id && o.CollectTime > startTime).OrderBy(o => o.CollectTime).Take(1).ToList();
                    originalData2 = originalDatas.Where(o => o.MeterId == meter.Id && o.CollectTime < startTime).OrderByDescending(o => o.CollectTime).Take(1).ToList();
                }
                if (originalData1.Count() == 1 && originalData2.Count() == 0)
                    originalData = originalData1[0];
                else if (originalData2.Count() == 1 && originalData1.Count() == 0)
                    originalData = originalData2[0];
                else if (originalData2.Count() == 1 && originalData1.Count() == 1)
                {
                    TimeSpan span1 = originalData1[0].CollectTime - startTime;
                    double m1 = span1.TotalMilliseconds > 0 ? span1.TotalMilliseconds : -span1.TotalMilliseconds;
                    TimeSpan span2 = originalData2[0].CollectTime - startTime;
                    double m2 = span2.TotalMilliseconds > 0 ? span2.TotalMilliseconds : -span2.TotalMilliseconds;
                    if (m1 < m2)
                        originalData = originalData1[0];
                    else
                        originalData = originalData2[0];
                }
            }
            return originalData;
        }
        /// <summary>
        /// 联动操作接口
        /// </summary>
        /// <param name="ids">对象集合</param>
        /// <param name="action">操作类型</param>
        /// <returns></returns>
        public Dictionary<int, bool> LinkageControl(List<int> ids, Dictionary action, int? value)
        {
            SocketData socketData = new SocketData();
            SynchronousSocketClient client = new SynchronousSocketClient();
            Dictionary<int, bool> record = new Dictionary<int, bool>();
            foreach (var id in ids)
            {
                if (this.Count(o => o.Id == id) > 0)
                {
                    var meter = this.Filter(o => o.Enable && o.Id == id).First();
                    socketData.MeterId = meter.Id;
                    socketData.BuildingId = meter.BuildingId;   //设备所属建筑Id
                    socketData.Type = action.EquText;   //开关类型  见Dictionary.Action.EquText值
                    socketData.IpAddress = meter.Access;//设备IP地址
                    socketData.PhyAddress = meter.MacAddress;//设备MAC地址
                    socketData.GbCode = meter.GbCode;//国标编码
                    //socketData.Value = value;
                    record.Add(id, client.StartClient(socketData));      //调用TCP_SocketClient发送数据
                }
            }
            return record;
        }
        /// <summary>
        /// 联动操作接口,触发操作
        /// </summary>
        /// <param name="category">对象分类</param>
        /// <param name="ids">对象集合</param>
        /// <param name="action">操作类型</param>
        /// <param name="isAction">是否激发操作：如果不是，则执行器反操作，如on_开头，则变为off_开头的操作</param>
        /// <returns></returns>
        public async Task<Dictionary<int, bool>> LinkageControl(CategoryDictionary category, List<int> ids, Dictionary action, int? value, bool isAction = true)
        {
            //!!!!!!!!!计费版本，不触发联动操作，设备自动执行!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            if (BLL.MyConsole.GetAppString("IsNeedSync") == "true")
                return new Dictionary<int, bool>();

            if (action.EquText == null || action.EquText == "")
                return null;
            List<int> energyCategoryIds = new List<int>();
            if (action.Description.Contains("W"))
                energyCategoryIds.Add(DictionaryCache.WaterCategory.Id);
            if (action.Description.Contains("P"))
                energyCategoryIds.Add(DictionaryCache.PowerCateogry.Id);
            List<int> meterIds = new List<int>();
            if (!isAction)
            {
                if (action.EquText.StartsWith("on_"))
                {
                    string txt = "off_" + action.EquText.Substring(3);
                    action = DictionaryCache.Get().Values.FirstOrDefault(o => o.Code == action.Code && o.EquText == txt);
                }
                else if (action.EquText.StartsWith("off_"))
                {
                    string txt = "on_" + action.EquText.Substring(4);
                    action = DictionaryCache.Get().Values.FirstOrDefault(o => o.Code == action.Code && o.EquText == txt);
                }
            }

            switch (category)
            {
                case CategoryDictionary.Building:
                    //获得建筑下面该类型设备列表
                    foreach (var bid in ids)
                    {
                        var meterInfos = await StatObjByBldg(bid, energyCategoryIds, StatisticalWay.Total);
                        var meters = meterInfos[0].Meters.Select(o => o.Id).ToList();
                        meterIds.AddRange(meters);
                    }
                    break;
                case CategoryDictionary.Meter:
                    meterIds = ids;
                    //LinkageControl(ids,action,value);
                    break;
                case CategoryDictionary.Organization:
                    //获得建筑下面该类型设备列表
                    foreach (var bid in ids)
                    {
                        var meterInfos = await StatObjByOrg(bid, energyCategoryIds, StatisticalWay.Total);
                        var meters = meterInfos[0].Meters.Select(o => o.Id).ToList();
                        meterIds.AddRange(meters);
                    }
                    break;

                default:
                    break;
            }
            return LinkageControl(meterIds, action, value);
        }
        
        /// <summary>
        /// 查找建筑下的某类型能耗计量设备
        /// </summary>
        /// <param name="building"></param>
        /// <param name="energyCategory"></param>
        /// <param name="IsControllable"></param>
        /// <param name="IsFJNewcapSystem"></param>
        /// <returns></returns>
        public IQueryable<Meter> GetMetersInBuilding(Building building, Dictionary energyCategory,bool? IsControllable=true,bool? IsFJNewcapSystem=true)
        {
            IQueryable<Meter> meters;
            var transactionOptions = new System.Transactions.TransactionOptions();
            transactionOptions.IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted;
            using (var transactionScope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Required, transactionOptions))
            {
                meters = this.Filter(o =>
                 (IsControllable == null ? true : o.Brand.IsControllable == IsControllable)
                 && (IsFJNewcapSystem == null ? true : o.Brand.IsFJNewcapSystem == IsFJNewcapSystem)//公司的可控设备
                     //属于该建筑
                 && (o.BuildingId == building.Id || o.Building.TreeId.StartsWith(building.TreeId + "-"))
                 && o.TypeDict.ThirdValue == 1//计量设备
                 && o.Enable == true
                 && (o.EnergyCategoryId == energyCategory.Id || o.EnergyCategoryDict.TreeId.StartsWith(energyCategory.TreeId + "-")));
                transactionScope.Complete();
            }
           
            return meters;
        }
        /// <summary>
        /// 查询设备余额数据最后读值
        /// </summary>
        /// <param name="id">设备id</param>
        /// <returns>null ：无效余额数据</returns>
        public MomentaryValueData GetBalanceByMeterId(int id)
        {
            try
            {
                var pid=db.Meters.FirstOrDefault(o=>o.Id==id).Brand.Parameters.FirstOrDefault(o=>DictionaryCache.MeterBalanceParameterList.Contains(o.TypeId)).Id;
                return db.MomentaryValues.Where(o => o.MeterId == id && o.ParameterId == pid).FirstOrDefault().ToViewData();//.Value;
            }catch
            {
                return null;
            }
        }
      
        /// <summary>
        /// 获取异常设备状态，更新到meterstatus表
        /// </summary>
        /// <returns></returns>
        public void SetMeterStatus()
        {
            var group = db.MeterStatus.GroupBy(o => new { o.MeterId, o.MeterMessageTypeId }).Where(o=>o.Count()>1).Select(o=>o.OrderByDescending(c=>c.Id).Skip(1)).ToList();
            foreach (var item in group)
            {
                for (int i = 0; i < item.Count(); i++)
                {
                    db.MeterStatus.Remove(item.Skip(i).Take(1).FirstOrDefault());
                }
            }
            db.SaveChanges();


            //已有故障信息
            
            var meterStatusOld = db.MeterStatus.Where(o =>!o.Meter.Enable&& o.MeterMessageTypeId > 530001 && o.Enabled).ToList();
            foreach (var item in meterStatusOld)
            {
                db.MeterStatus.Remove(item);
            }
            db.SaveChanges();
            var meterStatus = db.MeterStatus.Where(o => o.MeterMessageTypeId > 530001 && o.Enabled).ToList();
            var meterErrors = this.Filter(o => o.RelayElecState != null && DictionaryCache.MeterStatusError.Contains((int)o.RelayElecState)).Select(o => new
            {
                o.Id,
                o.RelayElecState
            }).ToList();
           

            //新故障
            var newErrors = meterErrors.Where(o => !meterStatus.Any(c => c.MeterId == o.Id && c.MeterMessageTypeId == o.RelayElecState)).ToList();
            foreach (var error in newErrors)
            {
                MeterStatus status = new MeterStatus();
                status.MeterId = error.Id;
                status.MeterMessageTypeId =(int) error.RelayElecState;
                status.Enabled = true;
                status.Value = 0;
                status.UpdateTime = DateTime.Now;
                db.MeterStatus.Add(status);
            }

            //旧故障已经解决的
            meterStatus = meterStatus.Where(o => !meterErrors.Any(c => c.Id== o.MeterId && c.RelayElecState == o.MeterMessageTypeId)).ToList();
            foreach (var status in meterStatus)
            {
                db.MeterStatus.Remove(status);
            }
            db.SaveChanges();
            //欠费数据
            //已有故障信息
            meterStatus = db.MeterStatus.Where(o => o.Meter.Enable && o.MeterMessageTypeId == DictionaryCache.MessageTypeEnergyCreditZero.Id && o.Enabled).ToList();
            meterErrors = this.Filter(o =>o.Enable&& o.RelayElecState != null && o.RelayElecState==DictionaryCache.RelayElecStateCreditZero.Id).Select(o => new
            {
                o.Id,
                o.RelayElecState
            }).ToList();

            meterStatusOld = db.MeterStatus.Where(o => !o.Meter.Enable && o.MeterMessageTypeId == DictionaryCache.MessageTypeEnergyCreditZero.Id && o.Enabled).ToList();
            foreach (var item in meterStatusOld)
            {
                db.MeterStatus.Remove(item);
            }
            db.SaveChanges();
            //新故障
            newErrors = meterErrors.Where(o => !meterStatus.Any(c => c.MeterId == o.Id)).ToList();
            foreach (var error in newErrors)
            {
                MeterStatus status = new MeterStatus();
                status.MeterId = error.Id;
                status.MeterMessageTypeId =  DictionaryCache.MessageTypeEnergyCreditZero.Id;
                status.Enabled = true;
                status.Value = 0;
                status.UpdateTime = DateTime.Now;
                db.MeterStatus.Add(status);
                
            }

            //旧故障已经解决的
            meterStatus = meterStatus.Where(o => !meterErrors.Any(c => c.Id == o.MeterId)).ToList();
            foreach (var status in meterStatus)
            {
                db.MeterStatus.Remove(status);
            }
            db.SaveChanges();

        }
      
      
    }



}