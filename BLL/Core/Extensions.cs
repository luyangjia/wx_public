using WxPay2017.API.DAL.EmpModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WxPay2017.API.VO;
using WxPay2017.API.VO.Common;
using WxPay2017.API.VO.Param;
using System.Linq.Expressions;
using System.Reflection;
using System.Data.Entity;
using System.Data;
using System.Data.OleDb;
using System.Transactions;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;
using System.Security.Claims;
using WxPay2017.API.BLL.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using WxPay2017.API.DAL.AuthModels;
using Newtonsoft.Json.Linq;

namespace WxPay2017.API.BLL
{
    public static class Extensions
    {
        public static async Task<ApplicationUser> GetUser(this IIdentity identity)
        {
            var ctx = new AuthContext();
            var store = new UserStore<ApplicationUser>(ctx);
            var user = await store.FindByNameAsync(identity.Name);
            return user;
        }
        //public static string GetUserName(this IIdentity identity);


        public static bool ImportBatchBasicData(DataSet dataSet, CategoryDictionary suffix = CategoryDictionary.All)
        {
            int? int_null = null;
            var ctx = new EmpContext();
            using (var tran = ctx.Database.BeginTransaction())
            {
                try
                {
                    var list_prior_brands = new List<Brand>();
                    var list_prior_parameters = new List<Parameter>();
                    var list_prior_organizations = new List<Organization>();
                    var list_lazy_organizations = new List<Organization>();
                    var list_prior_buildings = new List<Building>();
                    var list_lazy_buildings = new List<Building>();
                    var list_prior_meters = new List<Meter>();
                    var list_lazy_meters = new List<Meter>();
                    //var cmd_identity_insert = " SET IDENTITY_INSERT {0} {1} ";
                    #region  设备型号
                    var brandNparam = dataSet.Tables["厂家型号"];
                    if (brandNparam != null && (suffix & CategoryDictionary.Brand) == CategoryDictionary.Brand)
                    {
                        list_prior_brands = brandNparam.AsEnumerable().OfType<DataRow>().GroupBy(g => g["型号ID"].ToString()).Where(x => !string.IsNullOrEmpty(x.Key)).Select(x =>
                           new Brand()
                           {
                               Id = Convert.ToInt32(x.Key),
                               Name = x.Select(n => n["型号名称"].ToString()).FirstOrDefault(),
                               MeterType = x.Select(n => Convert.ToInt32(n["型号类型"])).FirstOrDefault(),
                               Producer = x.Select(n => n["生产厂家"].ToString()).FirstOrDefault()
                           }).Distinct().ToList();
                    #endregion

                        #region 设备参数
                        list_prior_parameters = brandNparam.AsEnumerable().OfType<DataRow>().Where(r => !string.IsNullOrEmpty(r["所属参数"].ToString().Trim())).Select(row =>
                            new Parameter()
                            {
                                Id = Convert.ToInt32(row["编号"]),
                                BrandId = list_prior_brands.FirstOrDefault(n => n.Name == row["型号名称"].ToString()).Id,
                                TypeId = Convert.ToInt32(row["所属参数"]),
                                Unit = row["参数单位"].ToString()
                            }).Distinct().ToList();
                        #endregion
                    }

                    #region 组织机构
                    var dtOrganization = dataSet.Tables["组织机构"];
                    if (dtOrganization != null && (suffix & CategoryDictionary.Organization) == CategoryDictionary.Organization)
                    {
                        foreach (DataRow dr in dtOrganization.Rows)
                        {
                            var org = new Organization()
                            {
                                Id = Convert.ToInt32(dr["当前编号"]),
                                ParentId = string.IsNullOrEmpty(dr["上级编号"].ToString().Trim()) ? int_null : Convert.ToInt32(dr["上级编号"]),
                                Name = dr["机构名称"].ToString(),
                                AliasName = "",
                                Initial = "",
                                Type = Convert.ToInt32(dr["机构类型"]),
                                ManagerCount = string.IsNullOrEmpty(dr["管理者数量"].ToString().Trim()) ? int_null : Convert.ToInt32(dr["管理者数量"]),
                                CustomerCount = string.IsNullOrEmpty(dr["学生数量"].ToString().Trim()) ? int_null : Convert.ToInt32(dr["学生数量"]),
                                Enable = true,
                                Description = ""
                            };
                            if (org.ParentId.HasValue)
                            {
                                list_lazy_organizations.Add(org);
                            }
                            else
                            {
                                list_prior_organizations.Add(org);
                            }
                        }
                    }
                    #endregion

                    #region 建筑信息表
                    var dtBuilding = dataSet.Tables["建筑信息"];
                    if (dtBuilding != null && (suffix & CategoryDictionary.Building) == CategoryDictionary.Building)
                    {
                        foreach (DataRow dr in dtBuilding.Rows)
                        {
                            var build = new Building()
                            {
                                Id = Convert.ToInt32(dr["当前编号"]),
                                ParentId = string.IsNullOrEmpty(dr["上级编号"].ToString().Trim()) ? int_null : Convert.ToInt32(dr["上级编号"].ToString().Trim()),
                                OrganizationId = Convert.ToInt32(dr["所属组织机构"]),
                                BuildingCategoryId = Convert.ToInt32(dr["建筑类型"]),
                                GbCode = dr["国标编码"].ToString().Trim(),
                                Name = dr["建筑名称"].ToString().Trim(),
                                AliasName = dr["建筑别名"].ToString().Trim(),
                                Type = Convert.ToInt32(dr["建筑分类"]),
                                ManagerCount = Convert.ToInt32(dr["管理者人数"]),
                                CustomerCount = Convert.ToInt32(dr["学生人数"]),
                                TotalArea = string.IsNullOrEmpty(dr["建筑面积"].ToString().Trim()) ? int_null : Convert.ToInt32(dr["建筑面积"].ToString().Trim()),
                                Enable = true,
                                Description = "",
                                Year = string.IsNullOrEmpty(dr["建筑年代"].ToString().Trim()) ? int_null : Convert.ToInt32(dr["建筑年代"].ToString().Trim()),
                                UpFloor = string.IsNullOrEmpty(dr["层数"].ToString().Trim()) ? int_null : Convert.ToInt32(dr["层数"].ToString().Trim())
                            };
                            if (build.ParentId.HasValue)
                            {
                                list_lazy_buildings.Add(build);
                            }
                            else
                            {
                                list_prior_buildings.Add(build);
                            }

                        }
                    }
                    #endregion

                    #region 设备信息
                    var dtMeter = dataSet.Tables["设备信息"];
                    if (dtMeter != null && (suffix & CategoryDictionary.Meter) == CategoryDictionary.Meter)
                    {
                        var dtMeterRows = dtMeter.AsEnumerable().OfType<DataRow>().Where(r => !string.IsNullOrEmpty(r["设备名称"].ToString().Trim()));
                        foreach (DataRow dr in dtMeterRows)
                        {
                            var meter = new Meter()
                            {
                                Id = Convert.ToInt32(dr["当前编号"]),
                                ParentId = string.IsNullOrEmpty(dr["上级编号"].ToString().Trim()) ? int_null : Convert.ToInt32(dr["上级编号"]),
                                BuildingId = Convert.ToInt32(dr["所属建筑"]),
                                EnergyCategoryId = Convert.ToInt32(dr["能源参数"]),
                                BrandId = Convert.ToInt32(dr["厂家型号"]),
                                GbCode = dr["国标编码"].ToString().Trim(),
                                Name = dr["设备名称"].ToString().Trim(),
                                Type = Convert.ToInt32(dr["设备类型"]),
                                Enable = true,
                                Description = dr["表物理地址"].ToString().Trim(),
                                Rate = Convert.ToInt32(dr["变比倍率"].ToString().Trim()),
                                MacAddress = dr["MAC地址"].ToString().Trim(),
                                GetInterval = 20,
                                Address = dr["安装位置"].ToString().Trim(),
                                Access = dr["IP"].ToString().Trim(),

                            };
                            if (meter.ParentId.HasValue)
                            {
                                list_lazy_meters.Add(meter);
                            }
                            else
                            {
                                list_prior_meters.Add(meter);
                            }
                        }
                    }
                    #endregion

                    //ctx.Brands.AddRange(list_prior_brands); 
                    list_prior_brands.ImportToDatabase();
                    list_prior_parameters.ImportToDatabase();
                    list_prior_organizations.ImportToDatabase();
                    list_lazy_organizations.ImportToDatabase();
                    list_prior_buildings.ImportToDatabase();
                    list_lazy_buildings.ImportToDatabase();
                    list_prior_meters.ImportToDatabase();
                    list_lazy_meters.ImportToDatabase();
                    //ctx.Database.ExecuteSqlCommand(string.Format(cmd_identity_insert, "Meter.Brand", "ON"));
                    //ctx.SaveChanges();
                    //ctx.Database.ExecuteSqlCommand(string.Format(cmd_identity_insert, "Meter.Brand", "OFF"));

                    //ctx.Database.ExecuteSqlCommand(string.Format(cmd_identity_insert, "Meter.Parameter", "ON"));
                    //ctx.Parameters.AddRange(list_prior_parameters);
                    //ctx.SaveChanges();
                    //ctx.Database.ExecuteSqlCommand(string.Format(cmd_identity_insert, "Meter.Parameter", "OFF"));

                    //ctx.Database.ExecuteSqlCommand(string.Format(cmd_identity_insert, "Organization.Organization", "ON"));
                    //ctx.Organizations.AddRange(list_prior_organizations);
                    //ctx.SaveChanges();
                    //ctx.Organizations.AddRange(list_lazy_organizations);
                    //ctx.SaveChanges();
                    //ctx.Database.ExecuteSqlCommand(string.Format(cmd_identity_insert, "Organization.Organization", "OFF"));


                    //ctx.Database.ExecuteSqlCommand(string.Format(cmd_identity_insert, "Building.Building", "ON"));
                    //ctx.Buildings.AddRange(list_prior_buildings);
                    //ctx.SaveChanges();
                    //ctx.Buildings.AddRange(list_lazy_buildings);
                    //ctx.SaveChanges();
                    //ctx.Database.ExecuteSqlCommand(string.Format(cmd_identity_insert, "Building.Building", "OFF"));


                    //ctx.Database.ExecuteSqlCommand(string.Format(cmd_identity_insert, "Meter.Meter", "ON"));
                    //ctx.Meters.AddRange(list_prior_meters);
                    //ctx.SaveChanges();
                    //ctx.Meters.AddRange(list_lazy_meters);
                    //ctx.SaveChanges();
                    //ctx.Database.ExecuteSqlCommand(string.Format(cmd_identity_insert, "Meter.Meter", "OFF"));

                    tran.Commit();
                }
                catch (Exception ex)
                {
                    //Ilog.WriteLogError("新基础数据导入错误" + ex.Message);

                    throw ex;
                }
            }

            return true;
        }

        public static DataSet GetExcelDataSet(string excelFilename)
        {
            var connectionString =
                "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + excelFilename +
                ";Extended Properties='Excel 12.0;HDR=YES;IMEX=0'";
            var ds = new DataSet();
            using (var conn = new OleDbConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    var table = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    for (var i = 0; i < table.Rows.Count; i++)
                    {
                        var tableName = table.Rows[i]["Table_Name"].ToString();
                        var strExcel = "select * from " + "[" + tableName + "]";
                        var adapter = new OleDbDataAdapter(strExcel, connectionString);
                        adapter.Fill(ds, tableName.Substring(0, tableName.Length - 1));
                    }
                    conn.Close();
                }
                catch (Exception)
                {

                    throw;
                }
            }
            return ds;
        }

        public static void ImportToDatabase<T>(this IEnumerable<T> list, bool isIdentityInsert = true)
        {
            if (list.Count() == 0) return;
            var cmd_identity_insert = " SET IDENTITY_INSERT {0} {1} ";
            var columns = "";
            var values = new List<string>();
            var flag = true;
            foreach (var item in list)
            {
                string val = "";
                foreach (var property in typeof(T).GetProperties().Where(property => !((property.PropertyType.IsClass && property.PropertyType.Name != "String") || property.PropertyType.IsAbstract)))
                {
                    System.Diagnostics.Debug.WriteLine(property.PropertyType.Name);
                    //System.Diagnostics.Debug.WriteLine(property.PropertyType.ToString());
                    //System.Diagnostics.Debug.WriteLine(property.PropertyType.IsEnum);
                    //System.Diagnostics.Debug.WriteLine(property.PropertyType.IsArray);
                    //System.Diagnostics.Debug.WriteLine(property.PropertyType.IsAbstract);
                    //System.Diagnostics.Debug.WriteLine(property.PropertyType.IsClass && property.PropertyType.Name != "String");
                    System.Diagnostics.Debug.WriteLine(property.Name);
                    System.Diagnostics.Debug.WriteLine("");
                    if (flag)
                    {
                        columns += string.Format(",[{0}]", property.Name);
                    }
                    switch (property.PropertyType.Name)
                    {
                        case "Nullable`1":
                            val += string.Format(", {0} ", property.GetValue(item) == null ? "NULL" : property.GetValue(item));
                            break;
                        case "bool":
                        case "Boolean":
                            val += string.Format(", {0}", (Convert.ToBoolean(property.GetValue(item)) ? 1 : 0));
                            break;
                        case "String":
                        default:
                            val += string.Format(",'{0}'", property.GetValue(item));
                            break;
                    }
                    //if (property.PropertyType.Name == "String")
                    //else val += string.Format(", {0}", property.GetValue(item));
                }
                values.Add(string.Format("({0})", val.Substring(1)));
                flag = false;
            }

            var table = typeof(T).GetCustomAttribute<TableAttribute>().Name;
            if (string.IsNullOrEmpty(table)) return;
            string sql = string.Format(" INSERT INTO {0} ({1})", table, columns.Substring(1));
            sql += "VALUES" + string.Join(",", values);
            if (isIdentityInsert)
            {
                sql = string.Format(cmd_identity_insert, table, " ON ") + sql;
                sql += string.Format(cmd_identity_insert, table, " OFF ");
            }
            System.Diagnostics.Debug.WriteLine(sql);

            var ctx = new EmpContext();
            using (var scope = ctx.Database.BeginTransaction())
            {
                try
                {
                    ctx.Database.ExecuteSqlCommand(sql);
                    scope.Commit();
                }
                catch (Exception ex)
                {
                    scope.Rollback();
                    throw ex;
                }
            }
        }


        ///// <summary>
        ///// 根据Brand表的型号名称BrandName
        ///// 获取型号Id
        ///// </summary>
        ///// <returns></returns>
        //public Dictionary<string, int> GetBrandIdByBrandNameDict()
        //{
        //    var getBrandIdByBrandNameDict = new Dictionary<string, int>();
        //    var dtBrand = Brand.GetBrand();
        //    for (int i = 0; i < dtBrand.Rows.Count; i++)
        //    {
        //        var brandName = dtBrand.Rows[i]["Name"].ToString();
        //        if (!getBrandIdByBrandNameDict.ContainsKey(brandName))
        //        {
        //            getBrandIdByBrandNameDict.Add(brandName, Convert.ToInt32(dtBrand.Rows[i]["Id"]));
        //        }
        //    }
        //    return getBrandIdByBrandNameDict;
        //}


        public static JArray ToViewList(this JArray node, CategoryDictionary suffix = CategoryDictionary.None)
        {
            return node;
        }
        public static JArray ToViewData(this JArray node, CategoryDictionary suffix = CategoryDictionary.None)
        {
            return node;
        }
        public static JArray ToList(this JArray node, CategoryDictionary suffix = CategoryDictionary.None)
        {
            return node;
        }
    }
}
