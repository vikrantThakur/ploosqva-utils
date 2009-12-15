using System;
using System.Web;

namespace Ploosqva.WebAppFrame.DbFacade
{
    ///<summary>
    /// These event arguments allow precise action if a user is not logged in
    ///</summary>
    public class UnloggedUserEventArgs : EventArgs
    {
        ///<summary>
        /// This constructor can be used for web applications and allows actions such
        /// as redirect to login page with return url
        ///</summary>
        ///<param name="request"></param>
        ///<param name="response"></param>
        public UnloggedUserEventArgs(HttpRequest request, HttpResponse response)
        {
            Request = request;
            Response = response;
        }

        ///<summary>
        /// Response object current for the http request which fired this event
        ///</summary>
        public HttpResponse Response { get; protected set; }

        ///<summary>
        /// Response object current for the http request which fired this event
        ///</summary>
        public HttpRequest Request { get; protected set; }
    }
}