using WxPay2017.API.DAL.EmpModels;
using WxPay2017.API.VO.Common;
using WxPay2017.API.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.BLL
{
    public static class ExtensionFieldExtension
    {
        #region ExtensionField
        public static ExtensionFieldData ToViewData(this ExtensionField node, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (node == null)
                return null;
            var model = new ExtensionFieldData()
            {
                Id = node.Id,
                Database = node.Database,
                Schema = node.Schema,
                Table = node.Table,
                Column = node.Column,
                JoinId = node.JoinId,
                StartTime = node.StartTime,
                FinishTime = node.FinishTime,
                Value = node.Value,
                ValueType = node.ValueType,
                ChineseName = node.ChineseName,
                EnglishName = node.EnglishName,
                Enable = node.Enable,
                Description = node.Description
            };
            var tableName = node.Table;
            CategoryDictionary dict = (CategoryDictionary)Enum.Parse(typeof(CategoryDictionary), tableName);
            if ((suffix & dict) == dict)
            {
                try
                {
                    var type = Type.GetType("WxPay2017.API.BLL." + tableName + "BLL");
                    var bll = Activator.CreateInstance(type);
                    model.RelatedObject = type.GetMethod("Find", new Type[] { typeof(object[]) }).Invoke(bll, new object[1] { new object[] { node.JoinId } });
                    var type2 = Type.GetType("WxPay2017.API.BLL." + tableName + "Extension");
                    System.Reflection.Assembly assm =System.Reflection.Assembly.Load("DAL");
                    Type typeModel = assm.GetType(""+tableName);
                    model.RelatedObject = type2.GetMethod("ToViewData", new Type[] { typeModel, typeof(CategoryDictionary) }).Invoke(model.RelatedObject, new object[2] { model.RelatedObject, CategoryDictionary.None });
                }catch{
                    model.RelatedObject=null;
                }
            }
            return model;
        }

        public static IList<ExtensionFieldData> ToViewList(this IQueryable<ExtensionField> nodes, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (nodes == null)
                return null;
            var nodeList = nodes.ToList();
            var results = nodeList.Select(node => new ExtensionFieldData()
            {
                Id = node.Id,
                Database = node.Database,
                Schema = node.Schema,
                Table = node.Table,
                Column = node.Column,
                JoinId = node.JoinId,
                StartTime = node.StartTime,
                FinishTime = node.FinishTime,
                Value = node.Value,
                ValueType = node.ValueType,
                ChineseName = node.ChineseName,
                EnglishName = node.EnglishName,
                Enable = node.Enable,
                Description = node.Description
            }).ToList();

            if (nodeList != null && nodeList.Count() > 0)
            {
                var tableName = nodeList[0].Table;
                CategoryDictionary dict = (CategoryDictionary)Enum.Parse(typeof(CategoryDictionary), tableName);
                if ((suffix & dict) == dict)
                {
                    var type = Type.GetType("WxPay2017.API.BLL." + tableName + "BLL");
                    var bll = Activator.CreateInstance(type);
                    if (bll != null)
                        foreach (var model in results)
                        {
                            try
                            {
                                model.RelatedObject = type.GetMethod("Find", new Type[] { typeof(object[]) }).Invoke(bll, new object[1] { new object[] { model.JoinId } });
                                var type2 = Type.GetType("WxPay2017.API.BLL." + tableName + "Extension");
                                System.Reflection.Assembly assm = System.Reflection.Assembly.Load("DAL");
                                Type typeModel = assm.GetType("" + tableName);
                                model.RelatedObject = type2.GetMethod("ToViewData", new Type[] { typeModel, typeof(CategoryDictionary) }).Invoke(model.RelatedObject, new object[2] { model.RelatedObject, CategoryDictionary.None });
                            }
                            catch
                            {
                                model.RelatedObject = null;
                            }
                        }
                }

            }
            return results;
        }

        public static ExtensionField ToModel(this ExtensionFieldData node)
        {
            return new ExtensionField()
            {
                Id = node.Id,
                Database = node.Database,
                Schema = node.Schema,
                Table = node.Table,
                Column = node.Column,
                JoinId = node.JoinId,
                StartTime = node.StartTime,
                FinishTime = node.FinishTime,
                Value = node.Value,
                ValueType = node.ValueType,
                ChineseName = node.ChineseName,
                EnglishName = node.EnglishName,
                Enable = node.Enable,
                Description = node.Description
            };
        }
        #endregion

    }
}
