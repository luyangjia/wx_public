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
using EMP2016.API.VO;
using EMP2016.API.BLL;
using EMP2016.API.VO.Data;
using EMP2016.API.VO.Common;
using EMP2016.API.VO.Param;

namespace EMP2016.API.WEBAPI.Areas.V1.Controllers
{
    /// <summary>
    /// 设备读参
    /// </summary>
    [Authorize]
    [RoutePrefix("api")]
    public class ParametersController : ApiController
    {
        private ParameterBLL paramBLL = new ParameterBLL();
        private MeterBLL meterBLL = new MeterBLL();
        private BrandBLL brandBLL = new BrandBLL();

        public ParametersController()
        {
            meterBLL = new MeterBLL(paramBLL.db, this.User.Identity.Name);
            brandBLL = new BrandBLL(paramBLL.db);
        }

        // GET: api/Parameters
        /// <summary>
        /// 获取所有设备读参
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="suffix">所需附加的信息类型</param>
        /// <returns>设备读参数据集合</returns>
        [ResponseType(typeof(IVoData<RoleData>))]
        public IHttpActionResult GetParameters([FromUri]Pagination pagination, [FromUri]CategoryDictionary suffix = CategoryDictionary.None)
        {
            var list = paramBLL.Filter(o => 1 == 1, ref pagination, "").ToViewList(suffix);
            return Ok(new { list = list, pagination = pagination });
        }

        // GET: api/Parameters/5
        /// <summary>
        /// 获取设备读参详细
        /// </summary>
        /// <param name="id">设备读参id</param>
        /// <returns>设备读参数据</returns>
        [ResponseType(typeof(ParameterData))]
        public IHttpActionResult GetParameter(int id, [FromUri]CategoryDictionary suffix = CategoryDictionary.None)
        {
            Parameter parameter = paramBLL.Find(id);
            if (parameter == null)
            {
                return NotFound();
            }

            return Ok(parameter.ToViewData(suffix));
        }

        // PUT: api/Parameters/5
        /// <summary>
        /// 更新设备读参
        /// </summary>
        /// <param name="id">设备读参id</param>
        /// <param name="parameter">设备读参数据对象</param>
        /// <returns>HTTP状态码(成功200、失败400、未找到404、错误500)</returns>
        [ResponseType(typeof(void))]
        public IHttpActionResult PutParameter(int id, ParameterData parameter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != parameter.Id)
            {
                return BadRequest();
            }


            try
            {
                paramBLL.Update(parameter.ToModel());
            }
            catch (Exception ex)
            {
                if (!ParameterExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw ex;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Parameters
        /// <summary>
        /// 新增设备读参
        /// </summary>
        /// <param name="parameter">设备读参数据对象</param>
        /// <returns>新增的设备读参数据</returns>
        [ResponseType(typeof(ParameterData))]
        public IHttpActionResult PostParameter(ParameterData parameter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var node = paramBLL.Create(parameter.ToModel());

            return Ok(node.ToViewData());
        }

        // DELETE: api/Parameters/5
        /// <summary>
        /// 删除设备读参
        /// </summary>
        /// <param name="id">设备读参id</param>
        /// <returns>删除的设备读参数据</returns>
        [ResponseType(typeof(ParameterData))]
        public IHttpActionResult DeleteParameter(int id)
        {
            Parameter parameter = paramBLL.Find(id);
            if (parameter == null)
            {
                return NotFound();
            }

            //外键约束
            var msg = paramBLL.DeleteConfirm(parameter);
            if (msg.Status != HttpStatusCode.OK) return Content<ConfirmMessageData>(msg.Status, msg);

            paramBLL.Delete(parameter);

            return Ok(parameter.ToViewData());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                meterBLL.Dispose();
                brandBLL.Dispose();
                paramBLL.Dispose();

            }
            base.Dispose(disposing);
        }

        private bool ParameterExists(int id)
        {
            return paramBLL.Count(e => e.Id == id) > 0;
        }

        #region Extra

        /// <summary>
        /// 根据类型获取设备读参列表 
        /// </summary>
        /// <param name="category">类型</param>
        /// <param name="id">相应类型的id值</param>
        /// <returns>设备读参数据集合</returns>
        [ResponseType(typeof(IEnumerable<ParameterData>))]
        [Route("parameters/by/{category}/{id}")]
        public IHttpActionResult GetByCategory(CategoryDictionary category, string id, [FromUri]CategoryDictionary suffix = CategoryDictionary.None)
        {
            var list = new List<Parameter>();
            var _id = 0;
            int.TryParse(id, out _id);
            switch (category)
            {
                case CategoryDictionary.Meter:
                    list = meterBLL.Filter(x => x.Id == _id).SelectMany(x => x.Brand.Parameters).ToList();
                    break;
                case CategoryDictionary.Brand:
                    list = brandBLL.Filter(x => x.Id == _id).SelectMany(x => x.Parameters).ToList();
                    break;
                case CategoryDictionary.Manager:
                case CategoryDictionary.User:
                case CategoryDictionary.Role:
                case CategoryDictionary.Building:
                case CategoryDictionary.BuidlingCategory:
                case CategoryDictionary.MeterType:
                case CategoryDictionary.Organization:
                case CategoryDictionary.Parameter:
                case CategoryDictionary.Permission:
                case CategoryDictionary.EnergyCategory:
                case CategoryDictionary.BuildingType:
                case CategoryDictionary.Org:
                default:
                    return BadRequest("系统不支持此功能");
                    break;
            }
            return Ok(list.Select(x => x.ToViewData(suffix)));
        }
        #endregion
    }
}