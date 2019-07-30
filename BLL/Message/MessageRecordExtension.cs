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
    public static class MessageRecordExtension
    {
        public static MessageRecordData ToViewData(this MessageRecord node, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (node == null)
                return null;
            var model = new MessageRecordData()
            {
                Id = node.Id,
                MessageId = node.MessageId,
                UserId = node.UserId,
                IsReaded = node.IsReaded,
                IsEnable = node.IsEnable,
                IsDeleted = node.IsDeleted,
                ReadedTime = node.ReadedTime,
                Message = ((suffix & CategoryDictionary.Message) == CategoryDictionary.Message) && node.Message != null ? node.Message.ToViewData() : null
            };
            return model;
        }


        public static IList<MessageRecordData> ToViewList(this IQueryable<MessageRecord> nodes, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (nodes == null)
                return null;
            Dictionary<int, MessageData> dicMessages = new Dictionary<int, MessageData>();
            var nodeList = nodes.ToList();
            var results = nodeList.Select(node => new MessageRecordData()
            {
                Id = node.Id,
                MessageId = node.MessageId,
                UserId = node.UserId,
                IsReaded = node.IsReaded,
                IsEnable = node.IsEnable,
                IsDeleted = node.IsDeleted,
                ReadedTime = node.ReadedTime,
                Message = ((suffix & CategoryDictionary.Message) == CategoryDictionary.Message) && node.Message != null ? node.Message.ToShortViewData() : null
            }).ToList();

            MessageBLL messageBLL = new MessageBLL();
            for (int i = 0; i < nodeList.Count(); i++)
            {
                if (results[i].Message == null)
                    if (nodeList[i].Message != null)
                        results[i].Message = nodeList[i].Message.ToViewData();
                    else
                    {
                        if (dicMessages.Keys.Contains(nodeList[i].MessageId))
                            results[i].Message = dicMessages[nodeList[i].MessageId];
                        else
                        {
                            results[i].Message = messageBLL.Find(nodeList[i].MessageId).ToViewData();
                            dicMessages.Add(nodeList[i].MessageId, results[i].Message);
                        }
                    }
            }
            return results;


        }

        public static MessageRecord ToModel(this MessageRecordData node)
        {
            return new MessageRecord()
            {
                Id = node.Id,
                MessageId = node.MessageId,
                UserId = node.UserId,
                IsReaded = node.IsReaded,
                IsEnable = node.IsEnable,
                IsDeleted = node.IsDeleted,
                ReadedTime = node.ReadedTime,

            };
        }
    }
}
