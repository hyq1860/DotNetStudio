// -----------------------------------------------------------------------
// <copyright file="JsonHelper.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace DotNet.Common.Utility
{
    using System;
    using System.Data;
    using System.IO;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public static class JsonHelper
    {
        private static JsonSerializerSettings jsonSettings;

        static JsonHelper()
        {
            IsoDateTimeConverter datetimeConverter = new IsoDateTimeConverter();
            datetimeConverter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            jsonSettings = new JsonSerializerSettings
            {
                MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore,
                NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                //TypeNameHandling = TypeNameHandling.All
            };
            jsonSettings.Converters.Add(datetimeConverter);
        }

        /// <summary>
        /// 将指定的对象序列化成 JSON 数据。
        /// </summary>
        /// <param name="obj">要序列化的对象。</param>
        /// <returns></returns>
        public static string ToJson(this object obj)
        {
            try
            {
                if (null == obj)
                    return null;

                return JsonConvert.SerializeObject(obj, Formatting.Indented, jsonSettings);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 将指定的对象序列化成 JSON 数据。
        /// </summary>
        /// <param name="obj">要序列化的对象。</param>
        /// <returns></returns>
        public static string ToJson(this object obj,string dateTimeFormat)
        {
            try
            {
                if (null == obj)
                    return null;
                IsoDateTimeConverter datetimeConverter = new IsoDateTimeConverter();
                datetimeConverter.DateTimeFormat = dateTimeFormat;

                JsonSerializerSettings jsonSettings = new JsonSerializerSettings
                {
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    //TypeNameHandling = TypeNameHandling.All
                };
                jsonSettings.Converters.Add(datetimeConverter);
                return JsonConvert.SerializeObject(obj, Formatting.Indented, jsonSettings);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 将指定的 JSON 数据反序列化成指定对象。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="json">JSON 数据。</param>
        /// <returns></returns>
        public static T FromJson<T>(this string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json, jsonSettings);
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }
    }
}
