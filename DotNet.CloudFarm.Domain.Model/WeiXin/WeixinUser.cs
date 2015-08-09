using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.CloudFarm.Domain.Model.WeiXin
{
    public class WeixinUser
    {
        public int Id { get; set; }

        public string  openid { get; set; }

        public string nickname { get; set; }

        public string headimgurl { get; set; }

        public DateTime createtime { get; set; }

    }
}
