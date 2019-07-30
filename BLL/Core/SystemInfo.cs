using WxPay2017.API.DAL.EmpModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using WxPay2017.API.VO.Common;

namespace WxPay2017.API
{
    public class SystemInfo
    {
        static SystemInfo()
        {
            //Refresh();
        }


        #region EnergyCategory
        private static List<Dictionary> _ALL_ENERGY_CATEGORIES;
        /// <summary>
        /// 获取此站点所有能耗类型
        /// </summary>
        public static List<Dictionary> AllEnergyCategories
        {
            get
            {
                if (_ALL_ENERGY_CATEGORIES == null)
                {
                    //var ctx = DictionaryCache.Get().Values;
                    var db = VO.Common.DictionaryCache.Get().Values;
                    _ALL_ENERGY_CATEGORIES = (from x in db
                                              where x.Code == "EnergyCategory"
                                                  && x.Enable
                                              //!!!!!!!!!!!!!!导致死锁!!!!!!!!!1
                                              //&& x.MetersByEnergyCategory.Count > 0
                                              select x into temp
                                              from d in db
                                              where d.Code == temp.Code
                                              && d.FirstValue == temp.FirstValue
                                              && (d.SecondValue == temp.SecondValue || !d.SecondValue.HasValue)
                                              && (d.ThirdValue == temp.ThirdValue || !d.ThirdValue.HasValue)
                                              && (d.FourthValue == temp.FourthValue || !d.FourthValue.HasValue)
                                              && (d.FifthValue == temp.FifthValue || !d.FifthValue.HasValue)
                                              select d).Distinct().ToList();

                    //_ALL_ENERGY_CATEGORIES = ctx.Where(x => x.Code == "EnergyCategory"
                    //    && x.Enablex`
                    //    //&& x.MetersByEnergyCategory.Count > 0
                    //    && x.MetersByEnergyCategory.Select(m=> m.EnergyCategoryDict).Where(ec=> 
                    //                            x.FirstValue == ec.FirstValue
                    //                          && (x.SecondValue == ec.SecondValue || !x.SecondValue.HasValue)
                    //                          && (x.ThirdValue == ec.ThirdValue || !x.ThirdValue.HasValue)
                    //                          && (x.FourthValue == ec.FourthValue || !x.FourthValue.HasValue)
                    //                          && (x.FifthValue == ec.FifthValue || !x.FifthValue.HasValue) 
                    //    ).Count() > 0
                    //    )
                    //        .AsNoTracking();
                    //var list = ctx.Where(x => x.Code == "EnergyCategory"
                    //    && x.Enable
                    //    && x.MetersByEnergyCategory.Count > 0
                    //    );

                    //var list2 = ctx.Where(x=> x.FirstValue == )

                }
                return _ALL_ENERGY_CATEGORIES;
            }
        }

        private static List<Dictionary> _ALL_ENERGY_ROOTS;
        /// <summary>
        /// 获取所有可用能耗类型的根节点列表
        /// </summary>
        public static List<Dictionary> AllEnergyCategoryRoots
        {
            get
            {
                if (_ALL_ENERGY_ROOTS == null)
                {
                    _ALL_ENERGY_ROOTS = AllEnergyCategories.Where(x => !x.SecondValue.HasValue).ToList();

                }
                return _ALL_ENERGY_ROOTS;
            }
        }

        #endregion

        #region Building Category
        private static List<Dictionary> _ALL_BUILDING_CATEGORIES;

        public static List<Dictionary> AllBuildingCategories
        {
            get
            {
                if (_ALL_BUILDING_CATEGORIES == null)
                {
                    var ctx = DictionaryCache.Get().Values;
                    _ALL_BUILDING_CATEGORIES = (from x in ctx
                                                where x.Code == "BuildingCategory"
                                                  && x.Enable
                                                  && x.BuildingsByCategory.Count > 0
                                                select x into temp
                                                from d in ctx
                                                where d.Code == temp.Code
                                                && d.FirstValue == temp.FirstValue
                                                && (d.SecondValue == temp.SecondValue || !d.SecondValue.HasValue)
                                                && (d.ThirdValue == temp.ThirdValue || !d.ThirdValue.HasValue)
                                                && (d.FourthValue == temp.FourthValue || !d.FourthValue.HasValue)
                                                && (d.FifthValue == temp.FifthValue || !d.FifthValue.HasValue)
                                                select d).Distinct().ToList();

                    //_ALL_BUILDING_CATEGORIES = ctx.Where(x => x.Code == "BuildingCategory"
                    //    && x.Enable
                    //    //&& x.BuildingsByCategory.Count > 0
                    //    ).AsNoTracking();


                }
                return _ALL_BUILDING_CATEGORIES;
            }
        }

        private static List<Dictionary> _ALL_BUILDING_CATEGORY_ROOTS;
        public static List<Dictionary> AllBuildingCategoryRoots
        {
            get
            {
                if (_ALL_BUILDING_CATEGORY_ROOTS == null)
                {
                    var ctx = DictionaryCache.Get().Values;
                    _ALL_BUILDING_CATEGORY_ROOTS = AllBuildingCategories.Where(x => !x.SecondValue.HasValue).ToList();

                }
                return _ALL_BUILDING_CATEGORY_ROOTS;
            }
        }

        #endregion

        #region MeterResult

        private static List<Dictionary> _EFFECTIVE_RESULT_STATUS;

        public static List<Dictionary> EffectiveResultStatus
        {
            get
            {
                if (_EFFECTIVE_RESULT_STATUS == null)
                {
                    var ctx = DictionaryCache.Get().Values;
                    _EFFECTIVE_RESULT_STATUS = ctx.Where(x => x.Code == "EnergyStatus"
                        && x.Enable
                        && x.FirstValue > 0).ToList();
                }
                return _EFFECTIVE_RESULT_STATUS;
            }
        }

        public static List<int> EffectiveResultStatusId
        {
            get
            {
                return SystemInfo.EffectiveResultStatus.Select(x => x.Id).ToList();
            }
        }

        #endregion

        #region AllDict

        private static List<Dictionary> _AllDict_STATUS;

        public static List<Dictionary> AllDictStatus
        {
            get
            {
                if (_AllDict_STATUS == null)
                {
                    var ctx = DictionaryCache.Get().Values;
                    //var ctx = DictionaryCache.Get().Values;
                    //_AllDict_STATUS = ctx.Where(x => x.Enable).AsNoTracking().ToList();
                    _AllDict_STATUS = ctx.ToList();

                }
                return _AllDict_STATUS;
            }
        }

        public static List<int> AllDictStatusId
        {
            get
            {
                return SystemInfo.AllDictStatus.Select(x => x.Id).ToList();
            }
        }

        #endregion

        #region Parameter In Statistics

        private static List<Dictionary> _ALL_PARAMETERS;

        public static List<Dictionary> AllParameters
        {
            get
            {
                if (_ALL_PARAMETERS == null)
                {
                    EmpContext ctx = new EmpContext();
                    _ALL_PARAMETERS = (from x in ctx.MeterMonthlyResults
                                       select x.Parameter.Type).Distinct().AsNoTracking().ToList();
                   

                }
                return _ALL_PARAMETERS;
            }
        }

        #endregion


        #region MessageType


        private static List<Dictionary> _ALL_MESSAGE_TYPES;
        /// <summary>
        /// 获取此站点所有能耗类型
        /// </summary>
        public static List<Dictionary> AllMessageTypes
        {
            get
            {
                if (_ALL_MESSAGE_TYPES == null)
                {
                    var ctx = DictionaryCache.Get().Values;
                    _ALL_MESSAGE_TYPES = (from x in ctx
                                          where x.Code == "MessageType"
                                              && x.Enable
                                              && x.MessagesByType.Count > 0
                                          select x into temp
                                          from d in ctx
                                          where d.Code == temp.Code
                                          && d.FirstValue == temp.FirstValue
                                          && (d.SecondValue == temp.SecondValue || !d.SecondValue.HasValue)
                                          && (d.ThirdValue == temp.ThirdValue || !d.ThirdValue.HasValue)
                                          && (d.FourthValue == temp.FourthValue || !d.FourthValue.HasValue)
                                          && (d.FifthValue == temp.FifthValue || !d.FifthValue.HasValue)
                                          select d).Distinct().ToList();

                }
                return _ALL_MESSAGE_TYPES;
            }
        }

        private static List<Dictionary> _ALL_MESSAGE_TYPE_ROOTS;
        /// <summary>
        /// 获取所有可用能耗类型的根节点列表
        /// </summary>
        public static List<Dictionary> AllMessageTypeRoots
        {
            get
            {
                if (_ALL_MESSAGE_TYPE_ROOTS == null)
                {
                    _ALL_MESSAGE_TYPE_ROOTS = AllMessageTypes.Where(x => !x.SecondValue.HasValue).ToList();

                }
                return _ALL_MESSAGE_TYPE_ROOTS;
            }
        }
        #endregion


        /// <summary>
        /// 如果系统添加新的能耗类型，请调用此函数刷新；
        /// </summary>
        public static void Refresh()
        {
            _ALL_ENERGY_CATEGORIES = null;
            var list = AllEnergyCategories;
            _ALL_ENERGY_ROOTS = null;
            list = AllEnergyCategoryRoots;
        }



        /// 


        #region City
        private static List<City> _ALL_CITIES;
        /// <summary>
        /// 获取此站点所有能耗类型
        /// </summary>
        public static List<City> AllCities
        {
            get
            {
                if (_ALL_CITIES == null)
                {
                    EmpContext ctx = new EmpContext();
                    var ids = ctx.Buildings.Select(x => x.CityId).Concat(ctx.Organizations.Select(x => x.CityId)).Distinct().ToList();
                    _ALL_CITIES = ctx.Cities.Where(x => ids.Contains(x.Id)).ToList();

                }
                return _ALL_CITIES;
            }
        }
        #endregion

        #region Province
        private static List<Province> _ALL_PROVINCES;
        /// <summary>
        /// 获取此站点所有省份
        /// </summary>
        public static List<Province> AllProvinces
        {
            get
            {
                if (_ALL_PROVINCES == null)
                {
                    EmpContext ctx = new EmpContext();
                    var ids = ctx.Buildings.Select(x => x.ProvinceId).Concat(ctx.Organizations.Select(x => x.ProvinceId)).Distinct().ToList();
                    _ALL_PROVINCES = ctx.Provinces.Where(x => ids.Contains(x.Id)).ToList();

                }
                return _ALL_PROVINCES;
            }
        }
        #endregion

    }
}
