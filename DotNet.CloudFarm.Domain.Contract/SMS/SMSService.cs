using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.CloudFarm.Domain.Contract.SMS
{
    public interface SMSService
    {
        public int SendSMSUserCaptcha(string mobile, string code);
    }
}
