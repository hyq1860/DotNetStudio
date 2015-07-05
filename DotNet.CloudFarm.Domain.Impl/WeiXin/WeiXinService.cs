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

        public WeiXinService(IWeiXinMessageDataAccess access)
        {
            this.messageDataAccess = access;
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

    }
}
