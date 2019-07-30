using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WxPay2017.API.DAL.EmpModels;
using WxPay2017.API.VO.Common;
using WxPay2017.API.VO;

namespace WxPay2017.API.BLL
{
    public static class CommandQueueExtension
    {
        #region CommandQueue
        public static CommandQueueData ToViewData(this CommandQueue node, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (node == null)
                return null;
            return new CommandQueueData()
            {
                Id = node.Id,
                //MeterActionId = node.MeterActionId,
                CommandType = node.CommandType,
                CommandTime = node.CommandTime,
                SendCount = node.SendCount,
                SendSource = node.SendSource,
                CommandValue = node.CommandValue,
                IsReply = node.IsReply,
                ReplyTime = node.ReplyTime,
                ReplyValue = node.ReplyValue,
                CommandNum = node.CommandNum,
                CommandGroup = node.CommandGroup,
                GatewayIpAddress = node.GatewayIpAddress,
                Rs485Addr = node.Rs485Addr,
                Port = node.Port,
                isSucc = node.isSucc,
                isErr = node.isErr,
                Priority = node.Priority,

            };
        }

        public static IList<CommandQueueData> ToViewList(this IQueryable<CommandQueue> nodes, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (nodes == null)
                return null;
            var nodeList = nodes.ToList();
            var results = nodeList.Select(node => new CommandQueueData()
            {
                Id = node.Id,
                CommandType = node.CommandType,
                CommandTime = node.CommandTime,
                SendCount = node.SendCount,
                SendSource = node.SendSource,
                CommandValue = node.CommandValue,
                IsReply = node.IsReply,
                ReplyTime = node.ReplyTime,
                ReplyValue = node.ReplyValue,
                CommandNum = node.CommandNum,
                CommandGroup = node.CommandGroup,
                GatewayIpAddress = node.GatewayIpAddress,
                Rs485Addr = node.Rs485Addr,
                Port = node.Port,
                isSucc = node.isSucc,
                isErr = node.isErr,
                Priority = node.Priority,

            }).ToList();
            return results;
        }

        public static CommandQueue ToModel(this CommandQueueData node)
        {
            return new CommandQueue()
            {
                Id = node.Id,
                CommandType = node.CommandType,
                CommandTime = node.CommandTime,
                SendCount = node.SendCount,
                SendSource = node.SendSource,
                CommandValue = node.CommandValue,
                IsReply = node.IsReply,
                ReplyTime = node.ReplyTime,
                ReplyValue = node.ReplyValue,
                CommandNum = node.CommandNum,
                CommandGroup = node.CommandGroup,
                GatewayIpAddress = node.GatewayIpAddress,
                Rs485Addr = node.Rs485Addr,
                Port = node.Port,
                isSucc = node.isSucc,
                isErr = node.isErr,
                Priority = node.Priority,

            };
        }
        #endregion

    }
}
