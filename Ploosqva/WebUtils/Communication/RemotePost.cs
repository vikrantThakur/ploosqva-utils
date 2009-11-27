using System;
using System.Collections.Specialized;
using System.Web;

namespace Ploosqva.WebUtils.Communication
{
    /// <summary>
    /// This class overcomes ASP.NET inflexibility of allowing only one form tag on a page.
    /// It works by clearing the current HttpResponse and posting a form with only the
    /// given fields. 
    /// </summary>
    public class RemotePost
    {
        private readonly NameValueCollection Inputs = new NameValueCollection();
        private readonly HttpPostMethod method;
        private readonly string formName;

        /// <summary>
        /// Create new instance of RemotePost
        /// </summary>
        /// <param name="method">method of http request</param>
        /// <param name="formName">form's name</param>
        public RemotePost(HttpPostMethod method, string formName)
        {
            this.method = method;
            this.formName = formName;
        }

        /// <summary>
        /// Add a field to the form
        /// </summary>
        public void Add(string name, string value)
        {
            Inputs.Add(name, value);
        }

        /// <summary>
        /// Sends the form to <paramref name="url"/>
        /// </summary>
        /// <param name="url">Address to post form to</param>
        public void Post(Uri url)
        {
            Url = url;

            Post();
        }

        /// <summary>
        /// Posts a form to url set to Url property
        /// </summary>
        public void Post()
        {
            HttpContext.Current.Response.Clear();

            HttpContext.Current.Response.Write("<html><head>");

            HttpContext.Current.Response.Write(string.Format("</head><body onload=\"document.{0}.submit()\">", formName));
            HttpContext.Current.Response.Write(string.Format("<form name=\"{0}\" method=\"{1}\" action=\"{2}\" >", formName, method, Url));
            for (int i = 0; i < Inputs.Keys.Count; i++)
            {
                HttpContext.Current.Response.Write(string.Format("<input name=\"{0}\" type=\"hidden\" value=\"{1}\">", Inputs.Keys[i], Inputs[Inputs.Keys[i]]));
            }
            HttpContext.Current.Response.Write("</form>");
            HttpContext.Current.Response.Write("</body></html>");

            HttpContext.Current.Response.End();
        }

        internal Uri Url { get; set; }
    }

    /// <summary>
    /// Posting method for RemotePost class
    /// </summary>
    public enum HttpPostMethod
    {
        /// <summary>
        /// Posting with hidden input fields
        /// </summary>
        Post,
        /// <summary>
        /// Posting with query strings
        /// </summary>
        Get
    }
}