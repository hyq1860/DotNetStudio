using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.CloudFarm.Domain.Model
{
    public class ApplicationConfig
    {
        private static readonly ApplicationConfig instance;
        static ApplicationConfig()
        {
            instance = new ApplicationConfig();
        }

        public static ApplicationConfig Instance
        {
            get { return instance;}
        }

        public string DbConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["YangkeConnection"].ConnectionString; }
        }
    }
}
