using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.CloudFarm.Domain.Model.Base
{
    public enum OrderStatus
    {
        /// <summary>
        /// 交易关闭
        /// </summary>
        [Description("交易关闭")]
        Close = -1,

        /// <summary>
        /// 待支付
        /// </summary>
        [Description("待支付")]
        Init = 0,

        /// <summary>
        /// 已支付
        /// </summary>
        [Description("已支付")]
        Paid = 1,


        /// <summary>
        /// 待确认结算
        /// </summary>
        [Description("待确认结算")]
        WaitingConfirm = 2,


        //TODO：状态待确定：育肥，待赎回

        /// <summary>
        /// 完成
        /// </summary>
        [Description("完成")]
        Complete = 10
    }
}
