using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using EMP2016.API.DAL.EmpModels;
using EMP2016.API.VO.Common;
using EMP2016.API.VO.Param;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using EMP2016.API.VO;
using EMP2016.API.BLL;
using EMP2016.API.VO.Data;
using System.Threading.Tasks;
using System.IO;
using System.Web.Hosting;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Claims;
using EMP2016.API.BLL.Identity;
using System.Linq.Expressions;
using Microsoft.AspNet.Identity;
using System.Transactions;

namespace EMP2016.API.WEBAPI.Areas.V1.Controllers
{
    //[Authorize]
    /// <summary>
    /// 品牌
    /// </summary>
    [Authorize]
    [RoutePrefix("api")]
    public class BrandsController : ApiController
    {
        private BrandBLL brandBLL = new BrandBLL();
        private MeterBLL meterBLL = new MeterBLL();
        private ParameterBLL paramBLL = new ParameterBLL();
        private DictionaryBLL dictBLL = new DictionaryBLL();
        public BrandsController()
        {
            meterBLL = new MeterBLL(brandBLL.db, this.User.Identity.Name);
            paramBLL = new ParameterBLL(brandBLL.db);
            dictBLL = new DictionaryBLL(brandBLL.db);
        }

        /// <summary>
        /// 获取品牌
        /// </summary>
        /// <param name="pagination"> 分页条件</param>
        /// <param name="suffix">包含的附加信息的类型</param>
        /// <returns>品牌数据对象集合</returns>
        [ResponseType(typeof(IVoData<BrandData>))]
        public IHttpActionResult GetBrands([FromUri]Pagination pagination, [FromUri]CategoryDictionary suffix = CategoryDictionary.None)
        {
            var list = brandBLL.Filter(o => 1 == 1, ref pagination).ToViewList(suffix).ToList();
            return Ok(new { List = list, Pagination = pagination });
        }

        /// <summary>
        /// 获取品牌详细
        /// </summary>
        /// <param name="id">品牌Id</param>
        /// <param name="suffix">后缀参数(该接口未使用该参数，不传)</param>
        /// <returns>品牌数据</returns>
        [ResponseType(typeof(BrandData))]
        public IHttpActionResult GetBrand(int id, [FromUri]CategoryDictionary suffix = CategoryDictionary.None)
        {
            Brand brand = brandBLL.Find(id);
            if (brand == null)
            {
                return NotFound();
            }

            return Ok(brand.ToViewData(suffix));
        }

        /// <summary>
        /// 更新品牌
        /// </summary>
        /// <param name="id">品牌id</param>
        /// <param name="brand">设备品牌数据对象</param>
        /// <returns>HTTP状态码(成功200、失败400、未找到404、错误500)</returns>
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBrand(int id, BrandData brand)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != brand.Id)
            {
                return BadRequest();
            }

            //db.Entry(brand).State = EntityState.Modified; 
            try
            {
                brandBLL.Update(brand.ToModel());
            }
            catch (Exception ex)
            {
                if (!BrandExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw ex;
                }
            }
            return Ok();
        }

        /// <summary>
        /// 新增品牌
        /// </summary>
        /// <param name="brand">设备品牌数据对象</param>
        /// <returns>新增的品牌数据</returns>
        [ResponseType(typeof(BrandData))]
        public IHttpActionResult PostBrand(BrandData brand)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var node = brandBLL.Create(brand.ToModel());

            return Ok(node.ToViewData());
        }

        // DELETE: api/Brands/5
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">品牌id</param>
        /// <returns>已删除的品牌数据</returns>
        [ResponseType(typeof(BrandData))]
        public IHttpActionResult DeleteBrand(int id)
        {
            Brand brand = brandBLL.Find(id);
            if (brand == null)
            {
                return NotFound();
            }

            ////外键约束
            //var msg = brandBLL.DeleteConfirm(brand, "Meters, Parameters");
            //if (msg.Status != HttpStatusCode.OK) return Content<ConfirmMessageData>(msg.Status, msg);

            //brandBLL.Delete(brand);

            //return Ok(brand.ToViewData());
            using (var tran = new TransactionScope())
            {
                try
                {
                    brand.Meters.Clear();
                    brand.Parameters.Clear();
                    brand.RatedParameters.Clear();
                    brand.RatedParameterDetails.Clear();

                    var count = brandBLL.Delete(brand);
                    tran.Complete();
                    return Ok(brand.ToViewData());
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                brandBLL.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BrandExists(int id)
        {
            return brandBLL.Find(o => o.Id == id) != null;
        }


        #region Extra

        /// <summary>
        /// 更新设备品牌
        /// </summary>
        /// <param name="id">品牌id</param>
        /// <param name="brand">品牌数据对象</param>
        /// <returns>更新的品牌数据</returns>
        [ResponseType(typeof(void))]
        public IHttpActionResult PatchBrand(int id, Newtonsoft.Json.Linq.JObject brand)
        {
            try
            {
                //var node = brandBLL.Find(id);
                //if (node == null) return NotFound();
                //JToken tokens = brand;
                //foreach (JProperty p in tokens)
                //{
                //    if (p.Name.Equals("id", StringComparison.OrdinalIgnoreCase)) continue;
                //    var property = node.GetType().GetProperty(p.Name, System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                //    if (property == null) throw new Exception(string.Format("未找到相应的属性名{0}!", p.Name));
                //    property.SetValue(node, JsonConvert.DeserializeObject(JsonConvert.SerializeObject(p.Value), property.PropertyType));
                //}
                var node = brandBLL.Update(brand, id);
                return Ok(node.ToViewData());
            }
            catch (Exception ex)
            {
                if (!BrandExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw ex;
                }
            }

            //return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// 根据类型获取品牌列表 
        /// </summary>
        /// <param name="category">类型</param>
        /// <param name="id">相应类型的id值</param>
        /// <param name="suffix">后缀参数(可选Meter、MeterType、Parameter)</param>
        /// <returns>品牌数据对象集合</returns> 
        [ResponseType(typeof(IEnumerable<BrandData>))]
        [Route("brands/by/{category}/{id}")]
        public IHttpActionResult GetByCategory(CategoryDictionary category, string id, [FromUri] Pagination pagination, CategoryDictionary suffix = CategoryDictionary.None)
        {
            //var list = new List<Brand>();
            IQueryable<Brand> list;
            //var list = new JArray();
            var _id = -1;
            int.TryParse(id, out _id);
            switch (category)
            {
                case CategoryDictionary.Meter:
                    //var meter = meterBLL.Find(_id);
                    //if (meter != null & meter.Brand != null) list.Add(meter.Brand);
                    list = brandBLL.Filter(x => x.Meters.Select(m => m.Id).Contains(_id), ref pagination);
                    break;
                case CategoryDictionary.MeterType:
                    list = brandBLL.Filter(x => x.MeterType == _id, ref pagination);
                    break;
                case CategoryDictionary.Parameter:
                    list = brandBLL.Filter(x => x.Parameters.Select(p => p.Id).Contains(_id), ref pagination);
                    break;
                case CategoryDictionary.Building:
                case CategoryDictionary.BuidlingCategory:
                case CategoryDictionary.Brand:
                case CategoryDictionary.Organization:
                case CategoryDictionary.Manager:
                case CategoryDictionary.Role:
                case CategoryDictionary.Permission:
                default:
                    return BadRequest("系统不支持此功能");
                    break;
            }
            return Ok(list.ToViewList(suffix));
        }


        /// <summary>
        /// 根据类型，维护品牌集（PKS）的每一个品牌对象与相应类型对象集的主外键关系
        /// </summary>
        /// <param name="category">类型， CategoryDictionary</param>
        /// <param name="rels">{ PKS: 品牌主键集, FKS: 类型{category}对象集的主键 }
        /// </param>
        /// <returns>HTTP状态码(成功200、失败400、未找到404、错误500)</returns>
        [Route("brands/{category}/relationship")]
        public IHttpActionResult PostRelationship(CategoryDictionary category, RelationshipModel<int, int> rels)
        {
            EmpContext content1 = new EmpContext();
            var list = brandBLL.Filter(x => rels.PKS.Contains(x.Id)).ToList();
            if (list.Count != rels.PKS.Count) return BadRequest("参数出错，部分品牌信息未找到，请审查后再提交");
            switch (category)
            {
                case CategoryDictionary.Meter:
                    if (list.Count != 1) return BadRequest("参数出错，品牌参数只能一条，请审查后再提交");
                    var brand = list.FirstOrDefault();
                    var meters = meterBLL.Filter(x => rels.FKS.Contains(x.Id)).ToList();
                    if (meters.Count != rels.FKS.Count) return BadRequest("参数出错，部分参数信息未找到，请审查后再提交");
                    var obj = new JObject();
                    obj.Add("BrandId", brand.Id);
                    meterBLL.Update(x => rels.FKS.Contains(x.Id), obj);
                    //meterBLL.Update(x => rels.FKS.Contains(x.Id), new { BrandID = brand.Id });
                    break;
                case CategoryDictionary.Parameter:
                    //var parameters = paramBLL.Filter(x => rels.FKS.Contains(x.Id)).ToList();
                    var dicts = dictBLL.Filter(x => x.Code == "Parameter" && rels.FKS.Contains(x.Id)).ToList();
                    if (dicts.Count != rels.FKS.Count) return BadRequest("参数出错，部分参数信息未找到，请审查后再提交");
                    foreach (var brandId in list)
                    {
                        foreach (var param in rels.FKS)
                        {
                            Parameter parameters = new Parameter();
                            parameters.BrandId = brandId.Id;
                            parameters.TypeId = param;
                            parameters.Unit = dicts.Where(x => x.Id == param).FirstOrDefault().EquText;
                            content1.Parameters.Add(parameters);
                        }

                    }
                    content1.SaveChanges();
                    content1.Dispose();
                    //return BadRequest("系统暂不支持此功能");
                    break;
                case CategoryDictionary.Building:
                case CategoryDictionary.BuidlingCategory:
                case CategoryDictionary.MeterType:
                case CategoryDictionary.Brand:
                case CategoryDictionary.Organization:
                case CategoryDictionary.Manager:
                case CategoryDictionary.Role:
                case CategoryDictionary.Permission:
                default:
                    return BadRequest("系统不支持此功能");
                    break;
            }
            return Ok();
        }

        /// <summary>
        /// 复杂条件查询数据
        /// </summary>
        /// <param name="parameter">参数</param>
        /// <returns>品牌数据集合</returns>
        [Route("Brands/Postsearch")]
        [ResponseType(typeof(BrandData))]
        public IHttpActionResult Postsearch(CommonSearchNode parameter)
        {
            parameter.ClassName = "Brand";
            parameter.FrameworkName = "Meter";

            IQueryable<Brand> list = null;
            brandBLL.Search(parameter, ref list);
            var result = list as IQueryable<Brand>;
            return Ok(new { List = result.ToViewList(), Pagination = parameter.Pagination });


        }

        /// <summary>
        /// 品牌导入
        /// </summary>
        /// <param name="suffix">后缀参数（可选，该接口未使用该参数，不传）</param>
        /// <returns>上传的文件数据</returns>
        [Route("brands/import")]
        public async Task<IHttpActionResult> PostImport(CategoryDictionary suffix = CategoryDictionary.Brand)
        {
            var user = await this.User.Identity.GetUser();
            // 检查是否是 multipart/form-data
            if (!Request.Content.IsMimeMultipartContent("form-data"))
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

            var guid = Guid.NewGuid();
            var path = HostingEnvironment.MapPath(string.Format("/template/{0}/{1}", user.Id, guid));
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            // 设置上传目录
            var provider = new MultipartFormDataStreamProvider(path);
            // 接收数据，并保存文件

            var task = await Request.Content.ReadAsMultipartAsync(provider).ContinueWith<IHttpActionResult>(t =>
            {
                var fileInfo = new List<FileInfoData>();
                foreach (var i in provider.FileData)
                {
                    var filename = i.Headers.ContentDisposition.FileName.Trim('"');
                    var info = new FileInfo(i.LocalFileName);
                    info.CopyTo(Path.Combine(path, filename), true);
                    try
                    {
                        var ds = BLL.Extensions.GetExcelDataSet(Path.Combine(path, filename));
                        BLL.Extensions.ImportBatchBasicData(ds, suffix);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    fileInfo.Add(new FileInfoData(filename, Path.Combine(path, filename), info.Length / 1024));
                    info.Delete();
                }

                //var fileInfo = provider.FileData.Select(i =>
                //{
                //    var filename = i.Headers.ContentDisposition.FileName.Trim('"');
                //    var info = new FileInfo(i.LocalFileName);
                //    info.CopyTo(Path.Combine(path, filename), true);
                //    info.Delete();
                //    return new FileDesc(filename, Path.Combine(path, filename), info.Length / 1024);
                //});

                return Ok(fileInfo);
            });
            return Ok(new { success = guid.ToString(), token = guid.ToString() });
        }


        /// <summary>
        /// 检索品牌
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="suffix">后缀参数（可选，该接口未使用该参数，不传）</param>
        /// <returns>品牌数据对象集合</returns>
        [Route("brands/search")]
        [ResponseType(typeof(IVoData<BrandData>))]
        public IHttpActionResult PostSearch(Pagination pagination, [FromUri]CategoryDictionary suffix = CategoryDictionary.None)
        {
            var list = brandBLL.Filter(o => 1 == 1, ref pagination).ToViewList(suffix).ToList();
            return Ok(new { List = list, Pagination = pagination });
        }

        /// <summary>
        /// 传入字典表中的brandtype的firstvalue值，返回相应设备分类的品牌数据集合
        /// </summary>
        /// <param name="firstvalue">字典表中的brandtype的firstvalue值</param>
        /// <returns>品牌数据集合</returns>
        [Route("brands/brandtype/{firstvalue}")]
        public IHttpActionResult GetByBrandType(int firstvalue)
        {
            var brandMeterType = dictBLL.Filter(o => o.FirstValue == firstvalue && o.Code == DictionaryCache.BrandMeterTypeCode).ToList();
            var brandMeterTypeIds = brandMeterType.Select(o => o.Id).ToArray();
            var brands = brandBLL.Filter(b => brandMeterTypeIds.Contains(b.MeterType)).ToViewList();  //.ToList();
            //var brandIds = brands.Select(b => b.Id).ToArray();
            //var meters = meterBLL.Filter(m => brandIds.Contains(m.BrandId) && m.Enable == true).ToViewList();
            return Ok(brands);
        }
        #endregion
    }
}