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
    public static class SubscribeExtension
    {
        public static SubscribeData ToViewData(this Subscribe node, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (node == null)
                return null;
            var model = new SubscribeData()
            {
                Id = node.Id,
                TypeId = node.TypeId,
                Name = node.Name,
                Description = node.Description,
                TargetTypeId = node.TargetTypeId,
                TargetId = node.TargetId,
                Enabled = node.Enabled,
                MessageTypeId = node.MessageTypeId,
                ReceivingModelId = node.ReceivingModelId,
                Type = node.Type == null ? DictionaryCache.Get()[node.TypeId].ToViewData() : node.Type.ToViewData(),
                TargetType = node.TargetType == null ? DictionaryCache.Get()[node.TargetTypeId].ToViewData() : node.TargetType.ToViewData(),
                MessageType = node.MessageType == null ? DictionaryCache.Get()[node.MessageTypeId].ToViewData() : node.MessageType.ToViewData(),
                ReceivingModel = node.ReceivingModel == null ? DictionaryCache.Get()[node.ReceivingModelId].ToViewData() : node.ReceivingModel.ToViewData(),
            };
            string TypeStr = DictionaryCache.Get()[node.TargetTypeId].Description;
            if (TypeStr != null)
            {
                UserBLL userBLL = new UserBLL();
                RoleBLL roleBLL = new RoleBLL();
              
                if (TypeStr.ToLower() == "role")
                {
                    model.Target = roleBLL.Find(model.TargetId).ToViewData();
                }
                else
                {
                    model.Target = userBLL.Find(model.TargetId).ToViewData();
                }
            }
            return model;
        }

        public static IList<SubscribeData> ToViewList(this IQueryable<Subscribe> nodes, CategoryDictionary suffix = CategoryDictionary.None)
        {
            //Dictionary<int, MessageData> dicMessages = new Dictionary<int, MessageData>();
            var nodeList = nodes.ToList();
            var results = nodeList.Select(node => new SubscribeData()
            {
                Id = node.Id,
                TypeId = node.TypeId,
                Name = node.Name,
                Description = node.Description,
                TargetTypeId = node.TargetTypeId,
                TargetId = node.TargetId,
                Enabled = node.Enabled,
                MessageTypeId = node.MessageTypeId,
                ReceivingModelId = node.ReceivingModelId,

            }).ToList();

            for (int i = 0; i < nodeList.Count(); i++)
            {
                results[i].MessageType = DictionaryCache.Get()[results[i].MessageTypeId].ToViewData();
                results[i].ReceivingModel = DictionaryCache.Get()[results[i].ReceivingModelId].ToViewData();
                results[i].TargetType = DictionaryCache.Get()[results[i].TargetTypeId].ToViewData();
                results[i].Type = DictionaryCache.Get()[results[i].TypeId].ToViewData();
                results[i].Target = nodeList[i].ToViewData().Target;
            }
            return results;
        }

        public static Subscribe ToModel(this SubscribeData node)
        {
            return new Subscribe()
            {
                Id = node.Id,
                TypeId = node.TypeId,
                Name = node.Name,
                Description = node.Description,
                TargetTypeId = node.TargetTypeId,
                TargetId = node.TargetId,
                Enabled = node.Enabled,
                MessageTypeId = node.MessageTypeId,
                ReceivingModelId = node.ReceivingModelId,
            };
        }
    }
}
