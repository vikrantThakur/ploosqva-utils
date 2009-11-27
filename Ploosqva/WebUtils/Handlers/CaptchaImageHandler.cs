using System;
using System.Drawing.Imaging;
using System.Web;
using System.Web.Caching;
using System.Web.SessionState;
using Ploosqva.ImageUtils;

namespace Ploosqva.WebUtils.Handlers
{
    ///<summary>
    /// HttpHandler, which shows a captcha image
    ///</summary>
    public class CaptchaImageHandler : IHttpHandler, IReadOnlySessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            /// first querystrings are read (text length, image dimensions
            /// and cache key to hold the image text)
            int len;
            if (!int.TryParse(context.Request["len"], out len))
                len = 5;

            int width;
            if (!int.TryParse(context.Request["w"], out width))
                width = 200;

            int height;
            if (!int.TryParse(context.Request["h"], out height))
                height = 50;

            /// if guid is missing, cannot load image
            string g = context.Request["g"];
            if (string.IsNullOrEmpty(g))
            {
                context.Response.StatusCode = 404;
                context.Response.End();
            }

            CaptchaImage ci;

            /// create the image using previous text 
            /// or create new if guid has not been used before 
            if (context.Cache[g] != null)
                ci = new CaptchaImage((string)context.Cache[g], width, height);
            else
            {
                ci = new CaptchaImage(len, width, height);
                context.Cache.Insert(g, ci.Text, null, Cache.NoAbsoluteExpiration, new TimeSpan(0, context.Session.Timeout, 0));
            }

            /// send image contents to user browser
            ci.Image.Save(context.Response.OutputStream, ImageFormat.Jpeg);

            context.Response.ContentType = "image/jpeg";
            context.Response.StatusCode = 200;
            context.Response.End();
        }

        public bool IsReusable
        {
            get { return true; }
        }
    }
}
