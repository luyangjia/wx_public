using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;
using System.Linq.Expressions;
using WxPay2017.API.DAL.EmpModels;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Dynamic;
using WxPay2017.API.VO;
using System.Reflection;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using WxPay2017.API.VO.Param;
using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace WxPay2017.API.BLL
{

    public partial class Repository<TObject> : IRepository<TObject> where TObject : class
    {

        public EmpContext db;
        public bool shareContext = false;
        private Expression<Func<TObject, bool>> _predicate;

        public Repository(EmpContext context = null, Expression<Func<TObject, bool>> predicate = null)
        {
            this.db = context == null ? new EmpContext() : context;
            if (predicate == null)
            {
                this._predicate = i => true;
            }
            else
            {
                this._predicate = predicate;
            }

            //shareContext = true;
        }

        protected DbSet<TObject> DbSet
        {
            get
            {
                return db.Set<TObject>();
            }
        }
        protected IQueryable<TObject> ALL
        {
            get
            {
                return DbSet.Where(this._predicate);
            }
        }

        public void Dispose()
        {
            if (shareContext && (db != null))
                db.Dispose();
        }

        public virtual IQueryable<TObject> All()
        {
            return DbSet.Where(this._predicate);
        }

        public virtual IQueryable<TObject> Filter(Expression<Func<TObject, bool>> predicate)
        {
            return ALL.Where(predicate).AsQueryable<TObject>();
        }
        public int Count(Expression<Func<TObject, bool>> predicate)
        {
            return ALL.Count(predicate);
        }

        public virtual IQueryable<TObject> Filter(Expression<Func<TObject, bool>> filter, out int total, int index = 1, int size = 50)
        {
            int skipCount = (index - 1) * size;
            var _resetSet = filter != null ? ALL.Where(filter).AsQueryable() :
                ALL.AsQueryable();
            _resetSet = skipCount == 0 ? _resetSet.Take(size) :
                _resetSet.Skip(skipCount).Take(size);
            total = _resetSet.Count();
            return _resetSet.AsQueryable();
        }

        public virtual IQueryable<TObject> Filter<TKey>(Expression<Func<TObject, bool>> filter, Expression<Func<TObject, TKey>> orderby, ref Pagination pagination)
        {
            var _list = filter != null ? ALL.Where(filter).AsQueryable() : ALL.AsQueryable();
            pagination.All = _list.Count();
            if (!pagination.NoPaging)
            {
                _list = _list.OrderBy(orderby).Skip((pagination.Index - 1) * pagination.Size).Take(pagination.Size);
            }
            pagination.Count = pagination.All % pagination.Size == 0 ? (pagination.All / pagination.Size) : ((pagination.All / pagination.Size) + 1);
            return _list.AsQueryable();
        }

        public virtual IQueryable<TObject> Filter(Expression<Func<TObject, bool>> filter, ref Pagination pagination, string userName = null)
        {
            string expressStr = null;
            var _list = filter != null ? ALL.Where(filter) : ALL;
            string sql = _list.ToString();
            sql = sql.Replace("\r\n", " ");
            try
            {
                sql = sql.Substring(sql.IndexOf(" AS [Extent1]"));
            }
            catch
            { }
            if (!sql.Contains("WHERE"))
                sql = sql + " where 1=1 and ";
            else
                sql = sql + " and ";
            if (userName != null)
                sql = sql.Replace("@p__linq__0", "'" + userName + "'");
            if (pagination == null) pagination = new Pagination();
            if (pagination.Filter != null && pagination.Filter.Keys.Count > 0)
            {
                //_list = _SEARCH(_list, pagination.Filter);
                //_list = _Filter(_list, pagination.Filter);
                _list = MYSEARCH(_list, pagination.Filter, ref expressStr);
            }
            JObject sorts = new JObject();
            if (string.IsNullOrEmpty(pagination.Sort))
            {
                sorts.Add("id", "asc");
            }
            else
            {
                sorts = JsonConvert.DeserializeObject<JObject>(pagination.Sort);
                //sorts.Add(pagination.Sort, pagination.isDesc==true?"desc":"asc");
            }
            //if (pagination.Sort == null) pagination.Sort = new JObject();
            //Expression<Func<TObject, TKey>> orderby;
            //if (pagination.Sort.Count == 0) pagination.Sort.Add("id", "asc");
            //JToken tokens = pagination.Sort;
            //JObject sorts = new JObject();
            //foreach (var item in pagination.Sort.Keys)
            //{
            //    sorts.Add(item, pagination.Sort[item]);
            //}

            //先用sql方式尝试，避免载入过多数据
            if (userName != null)
                try
                {
                    var modelName = _list.GetType().ToString();
                    modelName = modelName.Substring(0, modelName.Length - 1);
                    modelName = modelName.Substring(modelName.LastIndexOf(".") + 1);
                    DAL.EmpModels.Building building = new Building();

                    var types = Assembly.GetAssembly(building.GetType()).GetTypes();
                    string FrameworkName = "";
                    foreach (var type in types)
                    {
                        if (type.Name == (modelName))
                        {
                            FrameworkName = (type.GetCustomAttribute(typeof(TableAttribute)) as TableAttribute).Name;
                            FrameworkName = FrameworkName.Substring(0, FrameworkName.IndexOf("."));
                        }
                    }
                    var expressStrCount = string.Format(" SELECT count(*) FROM [{0}].[{1}] " + sql + "  {2} ", FrameworkName, modelName, expressStr);
                    if (expressStrCount.Trim().EndsWith(" and"))
                        expressStrCount = expressStrCount.Substring(0, expressStrCount.LastIndexOf("and") - 1);
                    pagination.All = this.db.Database.SqlQuery<int>(expressStrCount).AsQueryable().ToList()[0];
                    string sortStr = "";
                    string id = "";
                    foreach (var sort in sorts)
                    {
                        id = sort.Key;
                        sortStr = sort.Value + "";
                    }

                    if (sorts.Count == 1)
                        if (pagination.NoPaging == true)
                        {
                            pagination.Index = 1;
                            pagination.Size = pagination.All;
                            var expressStrSarch = string.Format(" SELECT * FROM [{0}].[{1}]  " + sql + "   {2} ", FrameworkName, modelName, expressStr);
                            if (expressStrSarch.Trim().EndsWith(" and"))
                                expressStrSarch = expressStrSarch.Substring(0, expressStrSarch.LastIndexOf("and") - 1);
                            expressStrSarch = expressStrSarch + " order by " + id + " " + sortStr;
                            return this.db.Database.SqlQuery<TObject>(expressStrSarch).AsQueryable();
                        }
                        else
                        {

                            // string id=pagination.Sort == null ? "id " : pagination.Sort;
                            var expressStrSarch = string.Format("select * from [{0}].[{1}]  " + sql + "  {3}  in (( select  {3}  from (select ROW_NUMBER() OVER (ORDER BY  {3} {4} ) AS pos, {3}  from [{0}].[{1}] where {2}) as T where  T.pos > {5}* ( {6}- 1 ) AND T.pos <={5} * {6}))order by  {3}  {4}",
                                FrameworkName, modelName, expressStr, id, sortStr, pagination.Size, pagination.Index);
                            return this.db.Database.SqlQuery<TObject>(expressStrSarch).AsQueryable();
                        }

                }
                catch { }

            JToken tokens = sorts;// pagination.Sort;
            var flag = false;

            IOrderedQueryable<TObject> _orderlist = null;
            pagination.All = _list.Count();

            var parameter = Expression.Parameter(typeof(TObject), "x");
            foreach (JProperty opt in tokens)
            {

                //var selector = GenericSelector<TObject>(parameter, opt.Name);
                //var property = typeof(TObject).GetProperty(opt.Name, System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                //if (property == null) throw new Exception(string.Format("未找到相应的属性名{0}!", opt.Name));

                if (opt.Value.ToString().Equals("desc", StringComparison.OrdinalIgnoreCase))
                {
                    if (!flag)
                    {
                        _orderlist = _list.OrderByDescending(opt.Name);
                        //_orderlist = _list.OrderByDescending(selector);
                        flag = true;
                    }
                    else
                    {
                        //_orderlist = _orderlist.ThenByDescending(selector);
                        _orderlist = _orderlist.ThenByDescending(opt.Name);
                    }
                }
                else
                {
                    if (!flag)
                    {
                        _orderlist = _list.OrderBy(opt.Name);// _list.AsQueryable().Provider.CreateQuery<TObject>(exp) as IOrderedQueryable<TObject>; 
                        //_orderlist = _list.OrderBy(selector);// _list.AsQueryable().Provider.CreateQuery<TObject>(exp) as IOrderedQueryable<TObject>; 
                        flag = true;
                    }
                    else
                    {
                        _orderlist = _orderlist.ThenBy(opt.Name);
                        //_orderlist = _orderlist.ThenBy(selector);
                    }
                }
                //property.SetValue(entity, JsonConvert.DeserializeObject(JsonConvert.SerializeObject(opt.Value), property.PropertyType));
                _list = _orderlist;
            }
            DateTime d25 = DateTime.Now;
            if (!pagination.NoPaging)
            {

                _list = _list.Skip((pagination.Index - 1) * pagination.Size).Take(pagination.Size);
            }
            pagination.Count = pagination.All % pagination.Size == 0 ? (pagination.All / pagination.Size) : ((pagination.All / pagination.Size) + 1);
            //pagination.Sort = (d24 - d23).Seconds*1000+(d24 - d23).Milliseconds + "";
            
            return _list;
        }

        //public virtual JArray Filter(Expression<Func<TObject, bool>> filter, ref Pagination pagination)
        //{
        //    var list = this.Filter(filter, ref pagination, "");
        //    if (!string.IsNullOrEmpty(pagination.Fields))
        //    {
        //        var parameter = Expression.Parameter(typeof(TObject), "x");
        //        return JArray.FromObject(list.Select(GenericSelector(parameter, pagination.Fields)));
        //    }
        //    return JArray.FromObject(list);

        //    // expression "o => new Data { Field1 = o.Field1, Field2 = o.Field2 }"

        //    // compile to Func<Data, Data>
        //}

        public virtual Task<IQueryable<TObject>> FilterAsync(Expression<Func<TObject, bool>> filter, ref Pagination pagination)
        {

            var _list = filter != null ? ALL.Where(filter).AsQueryable() : ALL.AsQueryable();
            if (pagination == null) pagination = new Pagination();
            if (pagination.Filter != null && pagination.Filter.Keys.Count > 0)
            {
                //_list = _SEARCH(_list, pagination.Filter);
                //_list = _Filter(_list, pagination.Filter);
                string tempStr = "";
                _list = MYSEARCH(_list, pagination.Filter, ref tempStr);
            }
            JObject sorts = new JObject();
            if (string.IsNullOrEmpty(pagination.Sort))
            {
                sorts.Add("id", "asc");
            }
            else
            {
                sorts = JsonConvert.DeserializeObject<JObject>(pagination.Sort);
            }
            //if (pagination.Sort == null) pagination.Sort = new JObject();
            //Expression<Func<TObject, TKey>> orderby;
            //if (pagination.Sort.Count == 0) pagination.Sort.Add("id", "asc");
            //JToken tokens = pagination.Sort;
            //JObject sorts = new JObject();
            //foreach (var item in pagination.Sort.Keys)
            //{
            //    sorts.Add(item, pagination.Sort[item]);
            //}
            JToken tokens = sorts;// pagination.Sort;
            var flag = false;
            IOrderedQueryable<TObject> _orderlist = null;
            pagination.All = _list.Count();
            foreach (JProperty opt in tokens)
            {

                //var selector = GenericSelector<TObject>(parameter, opt.Name);
                //var property = typeof(TObject).GetProperty(opt.Name, System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                //if (property == null) throw new Exception(string.Format("未找到相应的属性名{0}!", opt.Name));

                if (opt.Value.ToString().Equals("desc", StringComparison.OrdinalIgnoreCase))
                {
                    if (!flag)
                    {
                        _orderlist = _list.OrderByDescending(opt.Name);
                        //_orderlist = _list.OrderByDescending(selector);
                        flag = true;
                    }
                    else
                    {
                        //_orderlist = _orderlist.ThenByDescending(selector);
                        _orderlist = _orderlist.ThenByDescending(opt.Name);
                    }
                }
                else
                {
                    if (!flag)
                    {
                        _orderlist = _list.OrderBy(opt.Name);// _list.AsQueryable().Provider.CreateQuery<TObject>(exp) as IOrderedQueryable<TObject>; 
                        //_orderlist = _list.OrderBy(selector);// _list.AsQueryable().Provider.CreateQuery<TObject>(exp) as IOrderedQueryable<TObject>; 
                        flag = true;
                    }
                    else
                    {
                        _orderlist = _orderlist.ThenBy(opt.Name);
                        //_orderlist = _orderlist.ThenBy(selector);
                    }
                }
                //property.SetValue(entity, JsonConvert.DeserializeObject(JsonConvert.SerializeObject(opt.Value), property.PropertyType));
                _list = _orderlist;
            }
            if (!pagination.NoPaging)
            {

                _list = _list.Skip((pagination.Index - 1) * pagination.Size).Take(pagination.Size);
            }
            pagination.Count = pagination.All % pagination.Size == 0 ? (pagination.All / pagination.Size) : ((pagination.All / pagination.Size) + 1);
            return Task.Run(() => { return _list.AsQueryable(); });
        }

        public bool Contains(Expression<Func<TObject, bool>> predicate)
        {
            return ALL.Count(predicate) > 0;
        }

        public virtual TObject Find(params object[] keys)
        {
            return DbSet.Find(keys);
        }

        public virtual TObject Find(Expression<Func<TObject, bool>> predicate)
        {
            return ALL.FirstOrDefault(predicate);
        }

        public virtual TObject Create(TObject tobject)
        {
            var name = tobject.GetType().Name;
            if (name == "Building" || name == "Meter" || name == "Organization")
                VO.Common.MeterCache.Init();
            if (name == "User" || name == "Building" || name == "Organization")
                VO.Common.UserCache.Init();
            var newEntry = DbSet.Add(tobject);
            try
            {
                if (!shareContext)
                    db.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                MyConsole.Log(dbEx);
                throw dbEx;
            }
            catch (Exception dbEx)
            {
                MyConsole.Log(dbEx);
                throw dbEx;
            }
            return newEntry;
        }

        public virtual int Count()
        {
            return ALL.Count();
        }

        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="tobject"></param>
        /// <returns></returns>
        public virtual int Delete(TObject tobject)
        {
            var name = tobject.GetType().Name;
            if (name == "Building" || name == "Meter" || name == "Organization")
                VO.Common.MeterCache.Init();
            if (name == "User" || name == "Building" || name == "Organization")
                VO.Common.UserCache.Init();
            try
            {
                var entry = db.Entry(tobject);
                DbSet.Remove(tobject);
                if (!shareContext)
                    return db.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Console.WriteLine(dbEx.Message);
                throw dbEx;
            }
            return 0;
        }

        /// <summary>
        /// 更新对象
        /// </summary>
        /// <param name="TObject"></param>
        /// <returns></returns>
        public virtual int Update(TObject tobject)
        {
            var name = tobject.GetType().Name;
            if (name == "Building" || name == "Meter" || name == "Organization")
                VO.Common.MeterCache.Init();
            if (name == "User" || name == "Building" || name == "Organization")
                VO.Common.UserCache.Init();
            try
            {
                var entry = db.Entry(tobject);
                //DbSet.Attach(tobject);
                entry.State = EntityState.Modified;
                if (!shareContext)
                    return db.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Console.WriteLine(dbEx.Message);
                throw dbEx;
            }
            catch (Exception dbEx)
            {
                Console.WriteLine(dbEx.Message);
                throw dbEx;
            }
            return 0;
        }

        public virtual TObject Update(JObject option, params object[] id)
        {

            var entity = Find(id);
            var name = entity.GetType().Name;
            if (name.StartsWith("Building_") || name.StartsWith("Meter_") || name.StartsWith("Organization_"))
                VO.Common.MeterCache.Init();
            if (name.StartsWith("User_") || name.StartsWith("Building_") || name.StartsWith("Organization_"))
                VO.Common.UserCache.Init();
            JToken tokens = option;
            foreach (JProperty opt in tokens)
            {
                if (opt.Name.Equals("id", StringComparison.OrdinalIgnoreCase)) continue;
                if (opt.Name.Equals("hasChildren", StringComparison.OrdinalIgnoreCase)) continue;
                var property = entity.GetType().GetProperty(opt.Name, System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                if (property == null) throw new Exception(string.Format("未找到相应的属性名{0}!", opt.Name));
                //if (property.PropertyType.IsAbstract || (property.PropertyType.Name.ToString()!="String" && property.PropertyType.IsClass)) continue;
                property.SetValue(entity, JsonConvert.DeserializeObject(JsonConvert.SerializeObject(opt.Value), property.PropertyType));
            }

            if (!shareContext)
                db.SaveChanges();
            return entity;

        }

        /// <summary>
        /// 删除数据集
        /// </summary>
        /// <param name="predicate">获取数据集的表达式</param>
        /// <returns></returns>
        public virtual int Delete(Expression<Func<TObject, bool>> predicate)
        {

            var objects = Filter(predicate);
            if (objects != null && objects.Count() > 0)
            {
                var name = objects.FirstOrDefault().GetType().Name;
                if (name == "Building" || name == "Meter" || name == "Organization")
                    VO.Common.MeterCache.Init();
                if (name == "User" || name == "Building" || name == "Organization")
                    VO.Common.UserCache.Init();
            }
            foreach (var obj in objects)
                DbSet.Remove(obj);
            if (!shareContext)
                return db.SaveChanges();
            return 0;
        }

        public virtual ConfirmMessageData DeleteConfirm(TObject t, string constraint)
        {
            var list = constraint.Split(',').Select(x => x.Trim()).ToList();
            return DeleteConfirm(t, list);
        }

        public virtual ConfirmMessageData DeleteConfirm(TObject t, IEnumerable<string> constraint = null)
        {
            var name = t.GetType().Name;
            if (name == "Building" || name == "Meter" || name == "Organization")
                VO.Common.MeterCache.Init();
            if (name == "User" || name == "Building" || name == "Organization")
                VO.Common.UserCache.Init();
            if (constraint == null) constraint = new List<string>() { "*" };
            var msgs = new List<string>();
            var result = new ConfirmMessageData();
            var properties = t.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            foreach (var p in properties)
            {
                if (typeof(IEnumerable).IsAssignableFrom(p.PropertyType))
                {
                    //var t = p.PropertyType.GetMethods();
                    MethodInfo m = p.PropertyType.GetMethod("get_Count");
                    if (m != null)
                    {
                        int count = 0;
                        int.TryParse(m.Invoke(p.GetValue(t), null).ToString(), out count);
                        if (count > 0)
                        {
                            var attr = p.GetCustomAttribute<DisplayAttribute>();
                            var title = attr == null ? "" : p.GetCustomAttribute<DisplayAttribute>().Name;
                            if (constraint.Count(x => x == "*" || x == p.Name) > 0)
                                msgs.Add(string.Format("当前对象具有{0}个关联{1}数据, 无法删除!", count, string.IsNullOrEmpty(title) ? p.Name : title));
                        }
                    }
                }
            }
            if (msgs.Count > 0)
            {
                result.Messages = msgs;
                result.Status = System.Net.HttpStatusCode.Forbidden;
            }
            return result;
        }

        /// <summary>
        /// 修改数据集的一个或多个属性
        /// </summary>
        /// <param name="predicate">数据集表达式</param>
        /// <param name="target"></param>
        /// <returns></returns>
        public virtual int Update(Expression<Func<TObject, bool>> predicate, JObject target)
        {
            var objects = Filter(predicate);
            if (objects != null && objects.Count() > 0)
            {
                var name = objects.FirstOrDefault().GetType().Name;
                if (name == "Building" || name == "Meter" || name == "Organization")
                    VO.Common.MeterCache.Init();
                if (name == "User" || name == "Building" || name == "Organization")
                    VO.Common.UserCache.Init();
            }
            foreach (var obj in objects)
            {
                JToken tokens = target;
                foreach (JProperty p in tokens)
                {
                    if (p.Name.Equals("id", StringComparison.OrdinalIgnoreCase)) continue;
                    var property = obj.GetType().GetProperty(p.Name, System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                    if (property == null) throw new Exception(string.Format("未找到相应的属性名{0}!", p.Name));
                    property.SetValue(obj, JsonConvert.DeserializeObject(JsonConvert.SerializeObject(p.Value), property.PropertyType));
                }
            }

            if (!shareContext)
                return db.SaveChanges();
            return 0;
        }
        /// <summary>
        /// 复杂条件查询
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public virtual void Search(CommonSearchNode parameter, ref IQueryable<TObject> list)
        {
            //IQueryable<TObject> list = null;
            list = this.db.Database.SqlQuery<TObject>(parameter.GetSearchSQL()).AsQueryable();

            if (parameter.Pagination != null)
            {
                parameter.Pagination.All = Count(parameter);
                if (parameter.Pagination.Size != 0)
                    parameter.Pagination.Count = parameter.Pagination.All / parameter.Pagination.Size;
            }

            // return null;
        }
        /// <summary>
        /// 复杂条件查询数量
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public virtual int Count(CommonSearchNode parameter)
        {
            IQueryable<int> list = null;
            list = this.db.Database.SqlQuery<int>(parameter.GetCountSQL()).AsQueryable();

            return list.ToList()[0];
        }
        public virtual decimal? SumValue(CommonSearchNode parameter)
        {
            IQueryable<decimal?> list = null;
            list = this.db.Database.SqlQuery<decimal?>(parameter.GetSumValueSql()).AsQueryable();
            return list.ToList()[0];
        }
    }
}