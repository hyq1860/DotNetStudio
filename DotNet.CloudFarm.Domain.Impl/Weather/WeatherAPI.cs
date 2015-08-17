using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DotNet.Common.Configuration;
using DotNet.Common.Utility;

namespace DotNet.CloudFarm.Domain.Impl.Weather
{
    public class WeatherAPI
    {
        public static readonly WeatherAPI InStance=new WeatherAPI();
        private WeatherAPI()
        {
        }

        private WeatherInfo weatherData;

        public WeatherInfo WeatherData
        {
            get
            {
                if (weatherData == null || weatherData.Date.AddHours(24) < DateTime.Now)
                {
                    WebClient client=new WebClient();
                    client.Encoding=Encoding.UTF8;
                    var url = string.Format("http://v.juhe.cn/weather/index?key={0}&dtype=json&cityname={1}&format=json", ConfigHelper.ParamsConfig.GetParamValue("JuHeWeatherKey"), ConfigHelper.ParamsConfig.GetParamValue("JuHeWeatherCity"));
                    var json=client.DownloadString(url);
                    if (!string.IsNullOrEmpty(json))
                    {
                        weatherData = JsonHelper.FromJson<WeatherInfo>(json);
                        weatherData.Date=DateTime.Now;
                        weatherData.Data = json;
                    }
                }
                
                return weatherData;
            }
        }
    }

    public class WeatherInfo
    {
        public DateTime Date { get; set; }
        public string error_code { get; set; }
        public string reason { get; set; }
        
        public string Data { get; set; }
    }
}
