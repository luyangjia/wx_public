using WxPay2017.API.DAL.EmpModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using WxPay2017.API.VO.Common;
using WxPay2017.API.VO.Param;
using WxPay2017.API.VO;
namespace WxPay2017.API.BLL
{
    public class MessageBLL : Repository<Message>
    {
        UserBLL userBLL = new UserBLL();
        SubscribeBLL subscribeBLL = new SubscribeBLL();
        BuildingBLL buildingBLL = new BuildingBLL();
        OrganizationBLL organizationBLL = new OrganizationBLL();
        MessageRecordBLL messageRecordBLL = new MessageRecordBLL();

        public MessageBLL(EmpContext context = null)
            : base(context)
        {
            userBLL = new UserBLL(this.db);
            subscribeBLL = new SubscribeBLL(this.db);
            buildingBLL = new BuildingBLL(this.db);
            organizationBLL = new OrganizationBLL(this.db);
            messageRecordBLL = new MessageRecordBLL(this.db);
            userBLL = new UserBLL(this.db);
            subscribeBLL = new SubscribeBLL(this.db);
        }

        public IList<MessageData> Search(CommonSearchNode parameter)
        {
            IQueryable<Message> list = null;
            if (parameter.Pagination.NoPaging == true)
                list = this.db.Database.SqlQuery<Message>(parameter.GetSearchSQL()).AsQueryable();
            else
                list = this.db.Database.SqlQuery<Message>(parameter.GetSearchSQL()).AsQueryable().Skip(parameter.Pagination.Size * (parameter.Pagination.Index - 1)).Take(parameter.Pagination.Size);

            parameter.Pagination.All = Count(parameter);
            if (parameter.Pagination.Size != 0)
                parameter.Pagination.Count = parameter.Pagination.All / parameter.Pagination.Size;
            return list.ToViewList();
        }
        public new int Count(CommonSearchNode parameter)
        {
            IQueryable<int> list = null;
            list = this.db.Database.SqlQuery<int>(parameter.GetCountSQL()).AsQueryable();

            return list.ToList()[0];
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="isNotify">是否发送通知,true,返回需要发送的消息列表，需要在web层调用notifyHub发送</param>
        /// <param name="msgList">消息内容列表</param>
        /// <returns></returns>
        public List<NotifyInfo> SendBuildingMessageToNotify(bool isNotify, List<MessageData> msgList)
        {
            List<NotifyInfo> list = new List<NotifyInfo>();
            foreach (var msg in msgList)
            {
                list.AddRange(SendMessageToNotify(isNotify, msg));
            }
            return list;
        }

        public MessageData CreateMessageData(int wayId, int sourceTypeId, int srcId, string subject, string body)
        {
            MessageData message = new MessageData();
            message.MessageTypeId = wayId;
            message.CreateDate = DateTime.Now;
            message.EndDate = null;
            message.SrcId = srcId + "";
            message.MessageSourceTypeId = sourceTypeId;
            message.Subject = subject;
            message.Body = body;
            message.Url = "";//!!!!!!!!!!!!!!后续增加缴费连接
            message.ActiveDate = DateTime.Now;
            message.NotActiveDate = DateTime.Now.AddYears(100);
            message.IsDeleted = false;
            return message;
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="isNotify">是否发送通知,true,返回需要发送的消息列表，需要在web层调用notifyHub发送</param>
        /// <param name="messageData">消息内容</param>
        ///  <param name="msg">实际发送的消息内容</param>
        /// <returns>需要发送通知的对象列表</returns>
        public List<NotifyInfo> SendMessageToNotify(bool isNotify, MessageData msg)
        {
            MessagesPostData postData = new MessagesPostData();
            postData.Message = msg;
            postData.IdsByCategory = new Dictionary<CategoryDictionary, List<string>>();
            //switch (msg.MessageSourceTypeId)
            //{
            //    case 200034:
            //        postData.IdsByCategory.Add(CategoryDictionary.Meter, new List<string> { msg.SrcId });
            //        break;
            //    case 200035:
            //        postData.IdsByCategory.Add(CategoryDictionary.User, new List<string> { msg.SrcId });
            //        break;
            //    case 200036:
            //    case 200037:
            //    case 200039:
            //        postData.IdsByCategory.Add(CategoryDictionary.Organization, new List<string> { msg.SrcId });
            //        break;
            //    case 200038:
            //        postData.IdsByCategory.Add(CategoryDictionary.Building, new List<string> { msg.SrcId });
            //        break;
            //}
            postData.IdsByCategory.Add(CategoryDictionary.Building, new List<string> { msg.SrcId });
            
            return SendMessageToNotify(isNotify, postData);
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="isNotify">是否发送通知,true,返回需要发送的消息列表，需要在web层调用notifyHub发送</param>
        /// <param name="messageData">消息内容</param>
        /// <returns>需要发送通知的对象列表</returns>
        public List<NotifyInfo> SendMessageToNotify(bool isNotify, MessagesPostData messageData)
        {
            List<NotifyInfo> result = new List<NotifyInfo>();
            MessageData data = new MessageData();
            EntityTools.EntityCopy(messageData.Message, data, "");
            data.CreateDate = DateTime.Now;
            if (data.ActiveDate.Year == 1)
                data.ActiveDate = Convert.ToDateTime("1900-1-1");
            if (data.NotActiveDate.Year == 1)
                data.NotActiveDate = Convert.ToDateTime("9999-1-1");
            //data.MessageSourceType =DictionaryCache.Get()[data.MessageSourceTypeId];
            //data.MessageType =DictionaryCache.Get()[data.MessageTypeId];

            List<string> ids = new List<string>();
            IQueryable<User> users = userBLL.Filter(o => o.IsResignOrGraduate != true&&o.SecurityStamp!=null);
            //List<string> userList = userBLL.Filter(o => o.IsResignOrGraduate != true).Select(o => o.Id).ToList();
            List<string> userList = null;
            if (messageData.IdsByCategory != null && messageData.IdsByCategory.Count() != 0)
            {
                foreach (var idsByCategory in messageData.IdsByCategory)
                {
                    switch (idsByCategory.Key)
                    {
                        case CategoryDictionary.Building:
                            var usersByBuilding = new List<string>();
                            foreach (var r in idsByCategory.Value)
                            {
                                int id = Convert.ToInt32(r);
                                string treeId = buildingBLL.Find(id).TreeId;
                                var list = new List<string>();
                                list = users.Where(o => !o.IsResignOrGraduate && o.Buildings.Any(x => x.TreeId.StartsWith(treeId + "-") || x.TreeId == treeId)).Select(o => o.Id).ToList();
                                usersByBuilding = usersByBuilding.Union(list).ToList();
                            }
                            if (userList == null)
                                userList = usersByBuilding;
                            else
                                userList = userList.Intersect(usersByBuilding).ToList();
                            break;
                        case CategoryDictionary.Role:
                            var usersByRole = new List<string>();
                            foreach (var r in idsByCategory.Value)
                            {
                                var list = new List<string>();
                                list = users.Where(o => !o.IsResignOrGraduate && o.Roles.Any(x => x.Id == r)).Select(o => o.Id).ToList();
                                usersByRole = usersByRole.Union(list).ToList();
                            }
                            if (userList == null)
                                userList = usersByRole;
                            else
                                userList = userList.Intersect(usersByRole).ToList();
                            break;
                        case CategoryDictionary.Organization:
                            var usersByOrganization = new List<string>();
                            foreach (var r in idsByCategory.Value)
                            {
                                int id = Convert.ToInt32(r);
                                var list = new List<string>();
                                string treeId = organizationBLL.Find(id).TreeId;
                                list = users.Where(o => !o.IsResignOrGraduate && (o.Organizations.Any(c => c.TreeId.StartsWith(treeId) || c.TreeId == treeId)) || (o.OrganizationId == id || userBLL.db.Organizations.FirstOrDefault(c => c.Id == id).TreeId.StartsWith(treeId))).Select(o => o.Id).ToList();
                                usersByOrganization = usersByOrganization.Union(list).ToList();
                            }
                            if (userList == null)
                                userList = usersByOrganization;
                            else
                                userList = userList.Intersect(usersByOrganization).ToList();
                            break;
                        case CategoryDictionary.Org:
                            var usersByOrg = new List<string>();
                            foreach (var r in idsByCategory.Value)
                            {
                                int id = Convert.ToInt32(r);
                                var list = new List<string>();
                                list = users.Where(o => !o.IsResignOrGraduate && o.OrganizationId == id).Select(o => o.Id).ToList();
                                usersByOrg = usersByOrg.Union(list).ToList();
                            }
                            if (userList == null)
                                userList = usersByOrg;
                            else
                                userList = userList.Intersect(usersByOrg).ToList();
                            break;
                        case CategoryDictionary.User:
                            if (userList == null)
                                userList = idsByCategory.Value;
                            else 
                                userList = userList.Intersect(idsByCategory.Value).ToList();
                            break;
                    }

                }
            }
            var node = Create(data.ToModel());
            if (userList.Count > 0)
            {
                foreach (var id in userList)
                {
                    var toUser = userBLL.Find(id);
                    if (toUser == null)
                        continue;
                    List<string> roles = new List<string>();
                    if (toUser.Roles != null)
                        foreach (var role in toUser.Roles)
                        {
                            roles.Add(role.Id);
                        }
                    var roleSub = subscribeBLL.Filter(o => o.TypeId == 290001 && o.MessageTypeId == messageData.Message.MessageTypeId && o.Enabled == true && roles.Contains(o.TargetId)).ToList();//角色订阅
                    var userSub = subscribeBLL.Filter(o => o.TypeId == 290002 && o.MessageTypeId == messageData.Message.MessageTypeId && o.Enabled == true && o.TargetId == toUser.Id).ToList();//用户退订
                    if (!((roleSub != null && roleSub.Count() > 0) && (userSub == null || (userSub != null && userSub.Count() == 0))))
                    {
                        continue;
                    }
                    if (isNotify)
                    {
                        List<int> receiveModels = new List<int>();
                        foreach (var m in userSub)
                            receiveModels.Add(m.ReceivingModelId);
                        var sub = roleSub.Where(o => !receiveModels.Contains(o.ReceivingModelId)).ToList();
                        if (sub.Count > 0)
                        {
                            //NotifyHub hub = new Hubs.NotifyHub();
                            //hub.Notify(data.ToViewData(), id, sub);
                            NotifyInfo info = new NotifyInfo();
                            info.msg = data;
                            info.userId = id;
                            info.subs = sub;
                            result.Add(info);
                        }
                    }
                    MessageRecord record = new MessageRecord();
                    record.MessageId = node.Id;
                    record.IsDeleted = false;
                    record.IsEnable = false;
                    record.IsReaded = false;
                    record.ReadedTime = null;
                    record.UserId = id;
                    messageRecordBLL.Create(record);
                }

                return result;
            }
            else
            {
                return null;
            }
        }


    }
}
