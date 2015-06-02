using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DotNet.WebSite.MVC.Extensions
{
    public static class ControllerExtensions
    {
        public static ImageResult Image(this Controller controller, Stream imageStream, string contentType)
        {
            return new ImageResult(imageStream, contentType);
        }

        public static ImageResult Image(this Controller controller, byte[] imageBytes, string contentType)
        {
            return new ImageResult(new MemoryStream(imageBytes), contentType);
        }

        public static ImageResult Image(this Controller controller, Bitmap bitmap, string contentType = "image/jpeg")
        {
            byte[] bytes = ImageToByte(bitmap);
            return new ImageResult(new MemoryStream(bytes), contentType);
        }

        private static byte[] ImageToByte(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }

        /// <summary>
        /// 获取ip地址
        /// </summary>
        /// <param name="ctrl"></param>
        /// <returns></returns>
        public static string GetIp(this Controller ctrl)
        {
            string ip=string.Empty;
            var httpContext = ctrl.HttpContext;
            
            if (httpContext != null)
            {
                var request = httpContext.Request;
                if (request != null)
                {
                    ip = request.ServerVariables["HTTP_VIA"] != null ? request.ServerVariables["HTTP_X_FORWARDED_FOR"] : request.ServerVariables["REMOTE_ADDR"];
                }
            }
            
            return ip;
        }
    }
}
