using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.CloudFarm.Domain.Contract.SMS
{
    /// <summary>
    /// 短信接口
    /// </summary>
    public interface ISMSService
    {
        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <param name="code">验证码</param>
        /// <param name="expireMinute">过期时间（分钟）</param>
        /// <returns>0为成功</returns>
        int SendSMSUserCaptcha(string mobile, string code, int expireMinute);

        /// <summary>
        /// 发送订单提交成功短信
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <param name="orderId">订单金额</param>
        /// <param name="totalPrice">总价</param>
        /// <returns>0为成功</returns>
        int SendSMSOrderCreated(string mobile, long orderId, decimal totalPrice);

        /// <summary>
        /// 发送预售商城短信
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        int SendSMSPreOrderCreated(string mobile,string date);
    }
}
