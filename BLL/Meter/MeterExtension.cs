//using Meter;
using WxPay2017.API.DAL.EmpModels;
using WxPay2017.API.VO.Common;
using WxPay2017.API.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WxPay2017.API.BLL
{
    public static class MeterExtension
    {
        public static MeterData ToViewData(this Meter node, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (node == null)
                return null;
            var model = new MeterData()
            {
                Id = node.Id,
                ParentId = node.ParentId,
                TreeId = node.TreeId,
                Rank = node.Rank,
                Parent = (suffix & CategoryDictionary.Parent) == CategoryDictionary.Parent && node.ParentId.HasValue ? node.Parent.ToViewData() : null,
                HasChildren = node.Children.Count,
                BuildingId = node.BuildingId,
                RelayElecState=node.RelayElecState,
                PaulElecState=node.PaulElecState,
                Building = (suffix & CategoryDictionary.Building) == CategoryDictionary.Building ? node.Building.ToViewData() : null,
                EnergyCategoryId = node.EnergyCategoryId,
                EnergyCategory = (suffix & CategoryDictionary.EnergyCategory) == CategoryDictionary.EnergyCategory ? (DictionaryCache.Get()[node.EnergyCategoryId].ToViewData()) : null,
                BrandId = node.BrandId,
                BrandName = node.Brand == null ? null : node.Brand.Name,
                Brand = (suffix & CategoryDictionary.Brand) == CategoryDictionary.Brand ? node.Brand.ToViewData() : null,
                CoordinateMapId = node.CoordinateMapId,
                Coordinate2dId = node.Coordinate2dId,
                IsControlByHand = node.IsControlByHand,
                Coordinate3dId = node.Coordinate3dId,
                Code = node.Code,
                GbCode = node.GbCode,
                Name = node.Name,
                AliasName = node.AliasName,
                Initial = node.Initial,
                Type = node.Type,
                TypeDict = (suffix & CategoryDictionary.Dictionary) == CategoryDictionary.Dictionary || (suffix & CategoryDictionary.MeterType) == CategoryDictionary.MeterType ? DictionaryCache.Get()[node.Type].ToViewData() : null,
                Access = node.Access,
                Sequence = node.Sequence,
                Address = node.Address,
                MacAddress = node.MacAddress,
                Enable = node.Enable,
                StatusNote = node.StatusNote,
                GetInterval = node.GetInterval,
                BranchName = node.BranchName,
                BranchEnable = node.BranchEnable,
                Rate = node.Rate,
                Description = node.Description,
                IsTurnOn = node.IsTurnOn,
                SetupMode = node.SetupMode,
                PortNumber = node.PortNumber,
                Rs485Address = node.Rs485Address,
                ActivePrecise = node.ActivePrecise,
                ReactivePrecise = node.ReactivePrecise,
                BasicCurrent = node.BasicCurrent,
                ComProtocol = node.ComProtocol,
                SpeedRate = node.SpeedRate,
                IsHarmonic = node.IsHarmonic,
                Precision = node.Precision,
                PtRate = node.PtRate,
                RangeRatio = node.RangeRatio,
                Caliber = node.Caliber,
                Hydraulic = node.Hydraulic,
                Flow = node.Flow,
                //SetupPosition = node.SetupPosition,
                SupplyRegion = node.SupplyRegion,
                Manufactor = node.Manufactor,
                SetupDate = node.SetupDate,
                InitialValue = node.InitialValue,
                PortDescription = node.PortDescription,
                EffectiveBasePrice = node.EffectiveBasePrice,
                EffectiveModel = node.EffectiveModel,
                Status = node.MeterStatus.Where(s => s.Enabled).ToList().OrderByDescending(o => o.MeterMessageType.FirstValue).ThenByDescending(o => o.MeterMessageType.SecondValue).Select(s => s.ToViewData()).ToList(),
                Organization = ((suffix & CategoryDictionary.Organization) == CategoryDictionary.Organization) && node.Building != null && node.Building.Organization != null ? node.Building.Organization.ToViewData() : null,
            };
            if ((suffix & CategoryDictionary.Momentary) == CategoryDictionary.Momentary)
            {
                model.MomentaryValues = node.MomentaryValues.Select(x => x.ToViewData()).ToList();
            }
            if ((suffix & CategoryDictionary.Children) == CategoryDictionary.Children)
            {
                model.Children = node.Children.Select(x => x.ToViewData(suffix & CategoryDictionary.Momentary)).ToList();
            }
            if ((suffix & CategoryDictionary.Descendant) == CategoryDictionary.Descendant)
            {
                model.Descendants = node.Descendants(false).Select(x => x.ToViewData()).ToList();
            }
            if ((suffix & CategoryDictionary.Ancestor) == CategoryDictionary.Ancestor)
            {
                model.Ancestors = node.Ancestors().Select(x => x.ToViewData()).ToList();
            }
            EmpContext ctx = null;
            if ((suffix & CategoryDictionary.Gateway) == CategoryDictionary.Gateway)
            {
                if (ctx == null) ctx = new EmpContext();
                var m = ctx.Meters.FirstOrDefault(x => x.Type == 50101 && x.Access == node.Access);
                model.Gateway = m == null ? null : m.ToViewData(suffix & CategoryDictionary.Momentary);

            }
            if ((suffix & CategoryDictionary.Sibling) == CategoryDictionary.Sibling)
            {
                if (ctx == null) ctx = new EmpContext();
                model.Siblings = ctx.Meters.Where(x => x.ParentId == node.ParentId && x.Access == node.Access).ToList().Select(x => x.ToViewData(CategoryDictionary.Children | CategoryDictionary.Momentary)).ToList();

            }
            if ((suffix & CategoryDictionary.ExtensionField) == CategoryDictionary.ExtensionField)
            {
                if (ctx == null) ctx = new EmpContext();
                model.ExtensionFields = ctx.ExtensionFields.Where(x => x.Table == "Meter" && x.JoinId == node.Id).ToViewList();
            }
            return model;
        }
        public static MeterData ToViewData(this MeterFullInfo node, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (node == null)
                return null;
            var model = new MeterData()
            {
                Id = node.Id,
                ParentId = node.ParentId,
                TreeId = node.TreeId,
                Rank = node.Rank,
                Parent = (suffix & CategoryDictionary.Parent) == CategoryDictionary.Parent && node.ParentId.HasValue ? new MeterData
                {
                    Id = (int)node.ParentId,
                    Name = node.ParentName
                } : null,
                HasChildren = (node.HasChildren == null ? 0 : (int)node.HasChildren),
                BuildingId = node.BuildingId,
                Building = (suffix & CategoryDictionary.Building) == CategoryDictionary.Building && node.BuildingId.HasValue ? new BuildingData
                {
                    Id = (int)node.BuildingId,
                    Name = node.BuildingName,
                    OrganizationId = node.OrganizationId,
                    BuildingCategoryId = node.BuildingCategoryId == null ? -1 : (int)node.BuildingCategoryId,
                    GbCode = node.BuildingGBCode,
                } : null,
                EnergyCategoryId = node.EnergyCategoryId,
                EnergyCategory = (suffix & CategoryDictionary.EnergyCategory) == CategoryDictionary.EnergyCategory ? new DictionaryData
                {
                    Id = node.EnergyCategoryId,
                    ChineseName = DictionaryCache.Get()[node.EnergyCategoryId].ChineseName
                } : null,
                BrandId = node.BrandId,
                Brand = (suffix & CategoryDictionary.Brand) == CategoryDictionary.Brand ? new BrandData
                {
                    Id = node.BrandId,
                    Name = node.brandName,
                    Description = node.BrandDescription,
                    MeterTypeName = node.TypeName,
                    Model = node.Model,
                    Producer = node.Producer
                } : null,
                CoordinateMapId = node.CoordinateMapId,
                Coordinate2dId = node.Coordinate2dId,
                Coordinate3dId = node.Coordinate3dId,
                Code = node.Code,
                GbCode = node.GbCode,
                Name = node.Name,
                AliasName = node.AliasName,
                Initial = node.Initial,
                Type = node.Type,
                Access = node.Access,
                Sequence = node.Sequence,
                Address = node.Address,
                MacAddress = node.MacAddress,
                Enable = node.Enable,
                StatusNote = node.StatusNote,
                GetInterval = node.GetInterval,
                BranchName = node.BranchName,
                BranchEnable = node.BranchEnable,
                Rate = node.Rate,
                Description = node.Description
            };
            if ((suffix & CategoryDictionary.ExtensionField) == CategoryDictionary.ExtensionField)
            {
                var ctx = new EmpContext();
                model.ExtensionFields = ctx.ExtensionFields.Where(x => x.Table == "Meter" && x.JoinId == node.Id).ToViewList();
            }
            return model;
        }

        public static MeterShortData ToShortViewData(this Meter node, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (node == null)
                return null;
            var model = new MeterShortData()
            {
                Id = node.Id,
                ParentId = node.ParentId,
                TreeId = node.TreeId,
                Rank = node.Rank,
                BuildingId = node.BuildingId,
                EnergyCategoryId = node.EnergyCategoryId,
                BrandId = node.BrandId,
                CoordinateMapId = node.CoordinateMapId,
                Coordinate2dId = node.Coordinate2dId,
                Coordinate3dId = node.Coordinate3dId,
                Code = node.Code,
                RelayElecState = node.RelayElecState,
                PaulElecState = node.PaulElecState,
                GbCode = node.GbCode,
                Name = node.Name,
                AliasName = node.AliasName,
                Initial = node.Initial,
                Type = node.Type,
                Access = node.Access,
                Sequence = node.Sequence,
                Address = node.Address,
                MacAddress = node.MacAddress,
                Enable = node.Enable,
                StatusNote = node.StatusNote,
                GetInterval = node.GetInterval,
                BranchName = node.BranchName,
                BranchEnable = node.BranchEnable,
                Rate = node.Rate,
                Description = node.Description,
                IsTurnOn = node.IsTurnOn,
                SetupMode = node.SetupMode,
                PortNumber = node.PortNumber
            };

            return model;
        }

        public static GisMeterData ToGisData(this Meter node, int layer = 0)
        {
            if (node == null)
                return null;
            var model = new GisMeterData()
            {
                Id = node.Id,
                TreeId = node.TreeId,
                Rank = node.Rank,
                Name = node.Name,
                EnergyCategoryId = node.EnergyCategoryId,
                EnergyCategoryName = DictionaryCache.Get()[node.EnergyCategoryId].ChineseName,
                BrandId = node.BrandId,
                BrandName = node.Brand.Name,
                BrandType = node.Brand.MeterType,
                Type = node.Type,
                RelayElecState = node.RelayElecState,
                PaulElecState = node.PaulElecState,
                TypeName = node.TypeDict.ChineseName,
                MacAddress = node.MacAddress,
                State = node.Enable ? 1 : 0,
                Address = node.Address,
                Status = node.MeterStatus.Where(x => x.Enabled).ToList().Select(x => x.ToViewData()).ToList()
            };
            if (layer > 0)
            {
                foreach (var n in node.Children)
                {
                    model.Children.Add(n.ToGisData(--layer));
                }
            }
            return model;
        }
        public static IEnumerable<MeterData> ToViewList(this IQueryable<Meter> nodes, CategoryDictionary suffix = CategoryDictionary.None)
        {
            return nodes.ToList().Select(n => n.ToViewData(suffix)).ToList();
        }
        public static IList<MeterData> ToViewListShort(this IEnumerable<Meter> nodes, CategoryDictionary suffix = CategoryDictionary.None)
        {
            return nodes.Select(node => new MeterData()
            {
                Id = node.Id,
                ParentId = node.ParentId,
                TreeId = node.TreeId,
                Rank = node.Rank,
                Parent = (suffix & CategoryDictionary.Parent) == CategoryDictionary.Parent && node.ParentId.HasValue ? node.Parent.ToViewData() : null,
                BuildingId = node.BuildingId,
                BuildingName = (node.Building == null ? null : node.Building.Name),
                Building = (suffix & CategoryDictionary.Building) == CategoryDictionary.Building ? node.Building.ToViewData() : null,
                EnergyCategoryId = node.EnergyCategoryId,
                EnergyCategory = (suffix & CategoryDictionary.EnergyCategory) == CategoryDictionary.EnergyCategory ? DictionaryCache.Get()[node.EnergyCategoryId].ToViewData() : null,
                BrandId = node.BrandId,
                BrandName = (node.Brand == null ? null : node.Brand.Name),
                Brand = (suffix & CategoryDictionary.Brand) == CategoryDictionary.Brand ? node.Brand.ToViewData() : null,
                CoordinateMapId = node.CoordinateMapId,
                Coordinate2dId = node.Coordinate2dId,
                Coordinate3dId = node.Coordinate3dId,
                IsControlByHand = node.IsControlByHand,
                Code = node.Code,
                GbCode = node.GbCode,
                Name = node.Name,
                RelayElecState = node.RelayElecState,
                PaulElecState = node.PaulElecState,
                AliasName = node.AliasName,
                Initial = node.Initial,
                Type = node.Type,
                TypeDict = (suffix & CategoryDictionary.Dictionary) == CategoryDictionary.Dictionary || (suffix & CategoryDictionary.MeterType) == CategoryDictionary.MeterType ? DictionaryCache.Get()[node.Type].ToViewData() : null,
                Access = node.Access,
                Sequence = node.Sequence,
                Address = node.Address,
                MacAddress = node.MacAddress,
                Enable = node.Enable,
                StatusNote = node.StatusNote,
                GetInterval = node.GetInterval,
                BranchName = node.BranchName,
                BranchEnable = node.BranchEnable,
                Rate = node.Rate,
                Description = node.Description,
                IsTurnOn = node.IsTurnOn,
                SetupMode = node.SetupMode,
                PortNumber = node.PortNumber,
                EffectiveBasePrice = node.EffectiveBasePrice,
                EffectiveModel = node.EffectiveModel,
                ActivePrecise = node.ActivePrecise,
                ReactivePrecise = node.ReactivePrecise,
                BasicCurrent = node.BasicCurrent,
                ComProtocol = node.ComProtocol,
                SpeedRate = node.SpeedRate,
                IsHarmonic = node.IsHarmonic,
                Precision = node.Precision,
                PtRate = node.PtRate,
                RangeRatio = node.RangeRatio,
                Caliber = node.Caliber,
                Hydraulic = node.Hydraulic,
                Flow = node.Flow,
                //SetupPosition = node.SetupPosition,
                SupplyRegion = node.SupplyRegion,
                Manufactor = node.Manufactor,
                SetupDate = node.SetupDate,
                InitialValue = node.InitialValue,
                PortDescription = node.PortDescription,

                Organization = (suffix & CategoryDictionary.Organization) == CategoryDictionary.Organization && node.Building != null && node.Building.Organization != null ? node.Building.Organization.ToViewData() : null
            }).ToList();
        }

        public static IEnumerable<MeterData> ToViewList(this IEnumerable<MeterFullInfo> nodes, CategoryDictionary suffix = CategoryDictionary.None)
        {
            var nodeList = nodes.ToList();
            var results = nodes.Select(p => new MeterData()
            {
                Id = p.Id,
                ParentId = p.ParentId,
                TreeId = p.TreeId,
                Rank = p.Rank,
                Parent = (suffix & CategoryDictionary.Parent) == CategoryDictionary.Parent && p.ParentId.HasValue ? new MeterData
                {
                    Id = (int)p.ParentId,
                    Name = p.ParentName
                } : null,
                HasChildren = (p.HasChildren == null ? 0 : (int)p.HasChildren),
                BuildingId = p.BuildingId,
                Building = (suffix & CategoryDictionary.Building) == CategoryDictionary.Building && p.BuildingId.HasValue ? new BuildingData
                {
                    Id = (int)p.BuildingId,
                    Name = p.BuildingName,
                    OrganizationId = p.OrganizationId,
                    BuildingCategoryId = p.BuildingCategoryId == null ? -1 : (int)p.BuildingCategoryId,
                    GbCode = p.BuildingGBCode,
                } : null,
                EnergyCategoryId = p.EnergyCategoryId,
                EnergyCategory = (suffix & CategoryDictionary.EnergyCategory) == CategoryDictionary.EnergyCategory ? new DictionaryData
                {
                    Id = p.EnergyCategoryId,
                    ChineseName = DictionaryCache.Get()[p.EnergyCategoryId].ChineseName
                } : null,
                BrandId = p.BrandId,
                Brand = (suffix & CategoryDictionary.Brand) == CategoryDictionary.Brand ? new BrandData
                {
                    Id = p.BrandId,
                    Name = p.brandName,
                    Description = p.BrandDescription,
                    MeterTypeName = p.TypeName,
                    Model = p.Model,
                    Producer = p.Producer
                } : null,
                CoordinateMapId = p.CoordinateMapId,
                Coordinate2dId = p.Coordinate2dId,
                Coordinate3dId = p.Coordinate3dId,
                Code = p.Code,
                GbCode = p.GbCode,
                Name = p.Name,
                AliasName = p.AliasName,
                Initial = p.Initial,
                Type = p.Type,
                Access = p.Access,
                Sequence = p.Sequence,
                Address = p.Address,
                MacAddress = p.MacAddress,
            
                Enable = p.Enable,
                StatusNote = p.StatusNote,
                GetInterval = p.GetInterval,
                BranchName = p.BranchName,
                BranchEnable = p.BranchEnable,
                Rate = p.Rate,
                Description = p.Description,
                IsTurnOn = p.IsTurnOn
            });
            if ((suffix & CategoryDictionary.ExtensionField) == CategoryDictionary.ExtensionField)
            {
                var ctx = new EmpContext();
                results.Join(ctx.ExtensionFields.Where(x => x.Table == "Building"), o => o.Id, f => f.JoinId, (o, f) =>
                {
                    if (o.ExtensionFields == null) o.ExtensionFields = new List<ExtensionFieldData>();
                    o.ExtensionFields.Add(f.ToViewData());
                    return o;
                }).ToList();
            }
            return results;
        }

        public static Meter ToModel(this MeterData node)
        {
            var model = new Meter()
            {
                Id = node.Id,
                ParentId = node.ParentId,
                TreeId = node.TreeId,
                Rank = node.Rank,
                IsControlByHand = node.IsControlByHand,
                BuildingId = node.BuildingId,
                EnergyCategoryId = node.EnergyCategoryId,
                BrandId = node.BrandId,
                CoordinateMapId = node.CoordinateMapId,
                Coordinate2dId = node.Coordinate2dId,
                Coordinate3dId = node.Coordinate3dId,
                Code = node.Code,
                EffectiveBasePrice = node.EffectiveBasePrice,
                EffectiveModel = node.EffectiveModel,
                GbCode = node.GbCode,
                Name = node.Name,
                AliasName = node.AliasName,
                Initial = node.Initial,
                RelayElecState = node.RelayElecState,
                PaulElecState = node.PaulElecState,
                Type = node.Type,
                Access = node.Access,
                Sequence = node.Sequence,
                Address = node.Address,
                MacAddress = node.MacAddress,
                Enable = node.Enable,
                StatusNote = node.StatusNote,
                GetInterval = node.GetInterval,
                BranchName = node.BranchName,
                BranchEnable = node.BranchEnable,
                Rate = node.Rate,
                Description = node.Description,
                IsTurnOn = node.IsTurnOn,
                SetupMode = node.SetupMode,
                PortNumber = node.PortNumber,
                Rs485Address = node.Rs485Address,

                ActivePrecise = node.ActivePrecise,
                ReactivePrecise = node.ReactivePrecise,
                BasicCurrent = node.BasicCurrent,
                ComProtocol = node.ComProtocol,
                SpeedRate = node.SpeedRate,
                IsHarmonic = node.IsHarmonic,
                Precision = node.Precision,
                PtRate = node.PtRate,
                RangeRatio = node.RangeRatio,
                Caliber = node.Caliber,
                Hydraulic = node.Hydraulic,
                Flow = node.Flow,
                //SetupPosition = node.SetupPosition,
                SupplyRegion = node.SupplyRegion,
                Manufactor = node.Manufactor,
                SetupDate = node.SetupDate,
                InitialValue = node.InitialValue,
                PortDescription = node.PortDescription
            };

            return model;
        }

        public static MeterDiagram ToViewDiagram(this Meter node/*,  int layer = 0 */)
        {
            var model = new MeterDiagram()
            {
                Id = node.Id,
                ParentId = node.ParentId,
                TreeId = node.TreeId,
                Rank = node.Rank,
                ParentName = node.Parent == null ? null : node.Parent.Name,
                BrandId = node.BrandId,
                Type = node.Type,
                EnergyCategoryId = node.EnergyCategoryId,
                TypeName = node.TypeDict == null ? null : DictionaryCache.Get()[node.EnergyCategoryId].ChineseName,
                BrandType = node.Brand == null ? null : node.Brand.Name,
                Name = node.Name,
                Enable = node.Enable,
                Access = node.Access,
                Address = node.Address,
                RelayElecState = node.RelayElecState,
                PaulElecState = node.PaulElecState,
                HasPrivilege = 0,
                MacAddress = node.MacAddress,
                HasChildren = node.Children.Count(),
                IsTurnOn = node.IsTurnOn
            };

            var ctx = new EmpContext();
            model.ExtensionFields = ctx.ExtensionFields.Where(x => x.Table == "Meter" && x.JoinId == node.Id).ToViewList();

            return model;
        }

        public static MeterDiagram ToViewDiagram(this MeterFullInfo node/*,  int layer = 0 */)
        {
            var model = new MeterDiagram()
            {
                Id = node.Id,
                ParentId = node.ParentId,
                TreeId = node.TreeId,
                Rank = node.Rank,
                ParentName = node.ParentName,
                BrandId = node.BrandId,
                Type = node.Type,
                EnergyCategoryId = node.EnergyCategoryId,
                TypeName = node.TypeName,
                BrandType = node.brandName,
                Name = node.Name,
                Enable = node.Enable,
                Access = node.Access,
                Address = node.Address,
                HasPrivilege = 0,
                MacAddress = node.MacAddress,
                HasChildren = (node.HasChildren == null ? 0 : (int)node.HasChildren),
                IsTurnOn = node.IsTurnOn
            };
            var ctx = new EmpContext();
            model.ExtensionFields = ctx.ExtensionFields.Where(x => x.Table == "Meter" && x.JoinId == node.Id).ToViewList();


            return model;
        }


        #region Descendant  Ancestor
        /// <summary>
        /// 获取所有后代对象列表
        /// </summary>
        /// <param name="node"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IEnumerable<Meter> Descents(this Meter node, Expression<Func<Meter, bool>> predicate)
        {
            return node.Descendants().Where(predicate.Compile());
        }

        /// <summary>
        /// 获取所有前代对象
        /// </summary>
        /// <param name="node"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IEnumerable<Meter> Ancestors(this Meter node, Expression<Func<Meter, bool>> predicate)
        {
            return node.Ancestors().Where(predicate.Compile());
        }

        /// <summary>
        /// 所有后代
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static IEnumerable<Meter> Descendants(this Meter node, bool includeSelf = false)
        {
            var ctx = new EmpContext();
            return ctx.Meters.Where(DescendantsFunc(node, includeSelf)).AsEnumerable();
        }

        private static Func<Meter, bool> DescendantsFunc(Meter node, bool includeSelf)
        {
            //return x => x.TreeId.StartsWith(node.TreeId + (includeSelf ? string.Empty : "-"));
            return x => x.TreeId.StartsWith(node.TreeId + "-") || (includeSelf && x.TreeId == node.TreeId);
            //Func<Dictionary, bool> func = x => (includeSelf || node.Id != x.Id) && node.FirstValue == x.FirstValue
            //    && (!node.SecondValue.HasValue || (x.SecondValue == node.SecondValue
            //    && (!node.ThirdValue.HasValue || (x.ThirdValue == node.ThirdValue
            //    && (!node.FourthValue.HasValue || (x.FourthValue == node.FourthValue
            //    && (!node.FifthValue.HasValue || x.FifthValue == node.FifthValue)))))));
            //return func;
        }


        /// <summary>
        /// 所有前代
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static IEnumerable<Meter> Ancestors(this Meter node)
        {
            var ctx = new EmpContext();

            return ctx.Meters.Where(AncestorsFunc(node));
        }

        private static Func<Meter, bool> AncestorsFunc(Meter node)
        {
            var segs = node.TreeId.Split('-');
            Expression<Func<Meter, bool>> exp = x => false;
            var reducer = "";
            if (segs.Length > 0)
            {
                for (int i = 0; i < segs.Length - 1; i++)
                {
                    reducer += i == 0 ? segs[i] : ("-" + segs[i]);
                    Regex regx = new Regex("^" + reducer + "$");
                    Expression<Func<Meter, bool>> lambda = x => regx.IsMatch(x.TreeId);
                    exp = exp.Or(lambda);
                }
            }
            return exp.Compile();
            //Func<Dictionary, bool> func = x => node.Id != x.Id && node.FirstValue == x.FirstValue &&
            //            (!node.SecondValue.HasValue && !x.FirstValue.HasValue ||
            //            (!node.ThirdValue.HasValue && !x.SecondValue.HasValue || (!x.SecondValue.HasValue || node.SecondValue == x.SecondValue && (
            //            (!node.FourthValue.HasValue && !x.ThirdValue.HasValue || (!x.ThirdValue.HasValue || node.ThirdValue == x.ThirdValue && (
            //            (!node.FifthValue.HasValue && !x.FourthValue.HasValue || (!x.FourthValue.HasValue || node.FourthValue == x.FourthValue)))))))));
            //return func;
        }
        #endregion
    }
}
