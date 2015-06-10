using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNet.Common.Configuration;

namespace DotNet.WebSite.Infrastructure.Config
{
    public class ConfigHelper
    {
        /// <summary>
        /// Gets the params config.
        /// </summary>
        public static ParamsConfiguration ParamsConfig
        {
            get { return ConfigManager.GetConfig<ParamsConfiguration>(); }
        }
    }
}
