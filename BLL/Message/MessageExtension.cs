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
    public static class MessageExtension
    {
        public static IList<MessageData> ToViewList(this IQueryable<Message> nodes, CategoryDictionary suffix = CategoryDictionary.None)
        {
            return nodes.ToList().Select(node => node.ToViewData(suffix)).ToList();
           
        }
        public static MessageData ToViewData(this Message node, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (node == null)
                return null;
            var model = new MessageData()
            {
                Id = node.Id,
                MessageTypeId = node.MessageTypeId,
                CreateDate = node.CreateDate,
                EndDate = node.EndDate,
                MessageSourceTypeId = node.MessageSourceTypeId,
                SrcId = node.SrcId,
                Subject = node.Subject,
                Body = node.Body,
                Url = node.Url,
                ActiveDate = node.ActiveDate,
                IsDeleted = node.IsDeleted,
                NotActiveDate = node.NotActiveDate,
                AlertLevelId = node.AlertLevelId,   //告警等级
                MessageTypeName = node.MessageType == null ? DictionaryCache.Get()[node.MessageTypeId].ChineseName : node.MessageType.ChineseName,
                MessageSourceTypeName = node.MessageSourceType == null ? DictionaryCache.Get()[node.MessageSourceTypeId].ChineseName : node.MessageSourceType.ChineseName,
                MessageRecords = ((suffix & CategoryDictionary.MessageRecord) == CategoryDictionary.MessageRecord) ? node.MessageRecords.ToList().Select(x => x.ToViewData()).ToList() : null
            };

            string MessageSourceTypeStr = node.MessageSourceType == null ? DictionaryCache.Get()[node.MessageSourceTypeId].Description : node.MessageSourceType.Description;
            if (MessageSourceTypeStr != null)
            {
                ViewMeterFullInfoBLL meterBLL = new ViewMeterFullInfoBLL();
                BuildingBLL buildingBLL = new BuildingBLL();
                OrganizationBLL organizationBLL = new OrganizationBLL();
                UserBLL userBLL = new UserBLL();
                BrandBLL brandBLL = new BrandBLL();
                try
                {
                    int id = -1;
                    if (model.SrcId != null && int.TryParse(model.SrcId, out id))
                    {
                        if (MessageSourceTypeStr.ToLower() == "meter")
                        {
                            var meter = meterBLL.Find(id).ToViewData();
                            model.MessageSource = meter;
                            model.SenderName = meter.Name;
                        }
                        else if (MessageSourceTypeStr.ToLower() == "building")
                        {
                            var m = buildingBLL.Find(id).ToViewData();
                            model.MessageSource = m;
                            model.SenderName = m.Name;
                        }
                        else if (MessageSourceTypeStr.ToLower() == "organization")
                        {
                            var m = organizationBLL.Find(id).ToViewData();
                            model.MessageSource = m;
                            model.SenderName = m.Name;
                        }
                        else if (MessageSourceTypeStr.ToLower() == "brand")
                        {
                            var m = brandBLL.Find(id).ToViewData();
                            model.MessageSource = m;
                            model.SenderName = m.Name;
                        }
                        else if (MessageSourceTypeStr.ToLower() == "user")
                        {
                            var m = userBLL.Find(model.SrcId).ToViewData();
                            model.MessageSource = m;
                            model.SenderName = m.FullName;
                        }
                    }
                    else
                    {
                        var m = userBLL.Find(model.SrcId).ToViewData();
                        model.MessageSource = m;
                        model.SenderName = m.FullName;
                    }
                }
                catch { }
            }

            return model;
        }
        public static MessageData ToShortViewData(this Message node, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (node == null)
                return null;
            var model = new MessageData()
            {
                Id = node.Id,
                MessageTypeId = node.MessageTypeId,
                CreateDate = node.CreateDate,
                EndDate = node.EndDate,
                MessageSourceTypeId = node.MessageSourceTypeId,
                SrcId = node.SrcId,
                Subject = node.Subject,
                Body = node.Body,
                Url = node.Url,
                ActiveDate = node.ActiveDate,
                IsDeleted = node.IsDeleted,
                NotActiveDate = node.NotActiveDate,
                AlertLevelId = node.AlertLevelId,   //告警等级
                MessageTypeName = node.MessageType == null ? DictionaryCache.Get()[node.MessageTypeId].ChineseName : node.MessageType.ChineseName,
                MessageSourceTypeName = node.MessageSourceType == null ? DictionaryCache.Get()[node.MessageSourceTypeId].ChineseName : node.MessageSourceType.ChineseName,
            };
            return model;
        }


        public static Message ToModel(this MessageData node)
        {
            return new Message()
            {
                Id = node.Id,
                MessageTypeId = node.MessageTypeId,
                CreateDate = node.CreateDate,
                EndDate = node.EndDate,
                MessageSourceTypeId = node.MessageSourceTypeId,
                SrcId = node.SrcId,
                Subject = node.Subject,
                Body = node.Body,
                Url = node.Url,
                ActiveDate = node.ActiveDate,
                IsDeleted = node.IsDeleted,
                NotActiveDate = node.NotActiveDate,
                AlertLevelId = node.AlertLevelId,   //告警等级
            };
        }
    }
}
