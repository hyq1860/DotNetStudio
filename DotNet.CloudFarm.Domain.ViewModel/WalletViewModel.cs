using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.CloudFarm.Domain.ViewModel
{
    /// <summary>
    /// 钱包ViewModel
    /// </summary>
    public class WalletViewModel
    {
        /// <summary>
        /// 当前收益(尚未赎回的已支付订单的金额总和)已支付的收益之和
        /// </summary>
        public decimal CurrentIncome { get; set; }

        /// <summary>
        /// 预期收益
        /// </summary>
        public decimal ExpectIncome { get; set; }

        /// <summary>
        /// 预期年化收益率
        /// </summary>
        public decimal YearEarningRate { get; set; }

        /// <summary>
        /// 累计收益
        /// </summary>
        public decimal TotalIncome { get; set; }

        /// <summary>
        /// 累计养殖数量
        /// </summary>
        public int TotalProductCount { get; set; }

        /// <summary>
        /// 累计投入
        /// </summary>
        public decimal TotalInvestment { get; set; }

        /// <summary>
        /// 生长时间
        /// </summary>
        public int GrowDay { get; set; }

        

        //收益要从活动结束后开始计算

        //预期年化收益率 每期年化的平均

        //育肥进度只取最后一期的未赎回的进度

        //投资报表 赎回后的
    }
}
