using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Web;
using System.Web.SessionState;
using Ploosqva.ImageUtils;

namespace Ploosqva.PdfViewer
{
    /// <summary>
    /// Handler which displays an image of the requested page.
    /// It requires page, doc, w and h query strings.
    /// <br/>
    /// page - nage number (int)<br/>
    /// doc - base64-encoded file path<br/>
    /// w, h - width and height
    /// </summary>
    public class PdfPageImageHandler : IHttpHandler, IReadOnlySessionState
    {
        private int page;
        private string document;
        private int width;
        private int height;

        #region Implementation of IHttpHandler

        public void ProcessRequest(HttpContext context)
        {
            int.TryParse(context.Request["page"], out page);
            if (page < 0) page = 0;

            document = context.Request["doc"];

            if (!int.TryParse(context.Request["w"], out width))
                width = 200;

            if (!int.TryParse(context.Request["h"], out height))
                height = 50;

            Bitmap pageImage;

            string cacheKey = document + page + width + height;

            /// If page hasn't been opened before it is read from pdf document
            /// and seved in cache. Subsequent requests will use the object stored earlier
            if (context.Cache[cacheKey] == null)
            {
                string sourceFile = Encoding.UTF8.GetString(Convert.FromBase64String(document));

                pageImage = ImageResizer.Resize(PdfToImage.Convert(sourceFile, page), height, width);

                context.Cache.Insert(cacheKey, pageImage);
            }
            else
            {
                pageImage = (Bitmap) context.Cache[cacheKey];
            }

            /// send image contents to user browser
            pageImage.Save(context.Response.OutputStream, ImageFormat.Jpeg);

            context.Response.ContentType = "image/jpeg";
            context.Response.StatusCode = 200;
            context.Response.End();
        }

        public bool IsReusable
        {
            get { return true; }
        }

        #endregion
    }
}