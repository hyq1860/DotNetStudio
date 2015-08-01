using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNet.CloudFarm.Domain.Contract.WeiXin;
using DotNet.CloudFarm.Domain.Model.WeiXin;
namespace DotNet.CloudFarm.Domain.Impl.WeiXin
{
    public class WeiXinService :IWeiXinService
    {
        private IWeiXinMessageDataAccess messageDataAccess;

        private IWeixinPayLogDataAccess payLogDataAccess;

        public WeiXinService(IWeiXinMessageDataAccess access)
        {
            this.messageDataAccess = access;
        }

        public WeiXinService(IWeixinPayLogDataAccess pAccess)
        {
            this.payLogDataAccess = pAccess;
        }

        public WeiXinService(IWeiXinMessageDataAccess messageAccess,IWeixinPayLogDataAccess payAccess)
        {
            this.messageDataAccess = messageAccess;
            this.payLogDataAccess = payAccess;
        }

        /// <summary>
        /// 添加自动回复关键字数据
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public int AutoReplyMessageInsert(WeixinAutoReplyMessageModel message)
        {
            return messageDataAccess.AddAutoReplyMessage(message);
        }

        /// <summary>
        /// 获取所有自动回复关键字数据(未删除)
        /// </summary>
        /// <returns></returns>
        public IList<WeixinAutoReplyMessageModel> AutoReplyMessageGetAll()
        {
            return messageDataAccess.GetAllAutoReplyMessage();
        }


        /// <summary>
        /// 检查keyword是否存在
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public bool AutoReplyMessageCheckKeyword(string keyword)
        {
            return messageDataAccess.CheckKeyword(keyword);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        public void AutoReplyMessageUpdate(WeixinAutoReplyMessageModel model)
        {
            messageDataAccess.Update(model);
        }

        /// <summary>
        /// 更新status
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        public void AutoReplyMessageUpdateStatus(int id, int status)
        {
            messageDataAccess.UpdateAutoReplyMessageStatus(id, status);
        }
        /// <summary>
        /// 根据keword获取实体
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public WeixinAutoReplyMessageModel AutoReplyMessageGetByKeyword(string keyword)
        {
            return messageDataAccess.GetAutoReplyMessageByKeyword(keyword);
        }


        public int InsertWeixinPayLog(WeixinPayLog weixinPayLog)
        {
            return payLogDataAccess.Insert(weixinPayLog);
        }
    }
}
