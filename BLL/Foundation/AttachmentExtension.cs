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
    public static class AttachmentExtension
    {
        public static AttachmentData ToViewData(this Attachment node, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (node == null)
                return null;
            var model = new AttachmentData()
            {
                Id = node.Id,
                TargetId = node.TargetId,
                AttachmentTypeId = node.AttachmentTypeId,
                AttachmentFormatId = node.AttachmentFormatId,
                Description = node.Description,
                Size = node.Size,
                CreateTime = node.CreateTime,
                Path = node.Path,
                OriginalName = node.OriginalName,
                LogicalName = node.LogicalName,
                AttachmentType = node.AttachmentType == null ? DictionaryCache.Get()[node.AttachmentTypeId].ToViewData() : node.AttachmentType.ToViewData(),
                AttachmentFormat = node.AttachmentFormat == null ? DictionaryCache.Get()[node.AttachmentFormatId].ToViewData() : node.AttachmentFormat.ToViewData(),
            };

            string AttachmentTypeStr = node.AttachmentType == null ? DictionaryCache.Get()[node.AttachmentTypeId].Description : node.AttachmentType.Description;
            if (AttachmentTypeStr != null)
            {
                ViewMeterFullInfoBLL meterBLL = new ViewMeterFullInfoBLL();
                BuildingBLL buildingBLL = new BuildingBLL();
                OrganizationBLL organizationBLL = new OrganizationBLL();
                UserBLL userBLL = new UserBLL();
                BrandBLL brandBLL = new BrandBLL();
                MessageBLL messageBLL = new MessageBLL();
                int id = -1;
                if (model.TargetId != null && int.TryParse(model.TargetId, out id))
                {
                    if (AttachmentTypeStr.ToLower() == "meter")
                        model.Target = meterBLL.Find(id).ToViewData();
                    else if (AttachmentTypeStr.ToLower() == "building")
                        model.Target = buildingBLL.Find(id).ToViewData();
                    else if (AttachmentTypeStr.ToLower() == "organization")
                        model.Target = organizationBLL.Find(id).ToViewData();
                    else if (AttachmentTypeStr.ToLower() == "brand")
                        model.Target = brandBLL.Find(id).ToViewData();
                    else if (AttachmentTypeStr.ToLower() == "message")
                        model.Target = messageBLL.Find(id).ToViewData();
                }
                else
                {
                    model.Target = userBLL.Find(model.TargetId).ToViewData();
                }

            }
            return model;
        }

        public static IList<AttachmentData> ToViewList(this IQueryable<Attachment> nodes, CategoryDictionary suffix = CategoryDictionary.None)
        {
            Dictionary<int, MessageData> dicMessages = new Dictionary<int, MessageData>();
            var nodeList = nodes.ToList();
            var results = nodeList.Select(node => new AttachmentData()
            {
                Id = node.Id,
                TargetId = node.TargetId,
                AttachmentTypeId = node.AttachmentTypeId,
                AttachmentFormatId = node.AttachmentFormatId,
                Description = node.Description,
                Size = node.Size,
                CreateTime = node.CreateTime,
                Path = node.Path,
                OriginalName = node.OriginalName,
                LogicalName = node.LogicalName,

            }).ToList();
            for (int i = 0; i < nodeList.Count(); i++)
            {
                results[i].AttachmentFormat = DictionaryCache.Get()[results[i].AttachmentFormatId].ToViewData();
                results[i].AttachmentType = DictionaryCache.Get()[results[i].AttachmentTypeId].ToViewData();
                results[i].Target = nodeList[i].ToViewData().Target;
            }
            return results;

        }

        public static Attachment ToModel(this AttachmentData node)
        {
            return new Attachment()
            {
                Id = node.Id,
                TargetId = node.TargetId,
                AttachmentTypeId = node.AttachmentTypeId,
                AttachmentFormatId = node.AttachmentFormatId,
                Description = node.Description,
                Size = node.Size,
                CreateTime = node.CreateTime,
                Path = node.Path,
                OriginalName = node.OriginalName,
                LogicalName = node.LogicalName,

            };
        }
    }
}
