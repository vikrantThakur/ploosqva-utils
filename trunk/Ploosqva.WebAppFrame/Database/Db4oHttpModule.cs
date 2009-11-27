using System;
using System.Configuration;
using System.Net;
using System.Net.Sockets;
using System.Web;
using Db4objects.Db4o;

namespace Ploosqva.WebAppFrame.Database
{
    /// <summary>
    /// This module opens a client connection to a db4o database server.
    /// By default client tries to connect to embedded server
    /// To change default behavior, HttpApplicationState variables 
    /// dbServerEmbedded, dbHost, dbPort, dbUser and dbPass have to be set.
    /// </summary>
    class Db4oHttpModule : IHttpModule
    {
        /// <summary>
        /// keys for the named parameters in Web.config 
        /// </summary>
        internal const string KEY_DB4O_CLIENT = "db4oClient";

        private static int port = 10000;
        private static string host = "localhost";
        private static string user;
        private static string password;
        private static bool isServerEmbedded = true;

        /// <summary>
        /// Initializes the Db4oHttpModule, and sets server data from HttpApplicationState
        /// (if set)
        /// </summary>
        /// <param name="application">application class</param>
        public void Init(HttpApplication application)
        {
            application.EndRequest += Application_EndRequest;

            if (application.Application["dbServerEmbedded"] != null
                && application.Application["dbServerEmbedded"] is bool)
                isServerEmbedded = (bool) application.Application["dbServerEmbedded"];

            if (!isServerEmbedded)
            {
                if (application.Application["dbPort"] != null
                    && application.Application["dbPort"] is int)
                    port = (int) application.Application["dbPort"];

                if (application.Application["dbHost"] != null
                    && application.Application["dbHost"] is string)
                    host = (string) application.Application["dbHost"];

                if (application.Application["dbUser"] != null
                    && application.Application["dbUser"] is string)
                    user = (string) application.Application["dbUser"];

                if (application.Application["dbPass"] != null
                    && application.Application["dbPass"] is string)
                    password = (string) application.Application["dbPass"];
            }
        }

        /// <summary>
        /// Returns an existing client connection to the caller. 
        /// If client connection does not exist - it will be created
        /// </summary>
        public static IObjectContainer Client
        {
            get
            {
                HttpContext context = HttpContext.Current;

                IObjectContainer objectClient = context.Items[KEY_DB4O_CLIENT] as IObjectContainer;

                if (objectClient == null)
                {
                    /// If opening the database fails, it can mean that wrong port is set or
                    /// that the main web app is not running
                    try
                    {
                        if (isServerEmbedded)
                            objectClient = Db4oServerModule.Server.OpenClient();
                        else
                            objectClient = Db4oFactory.OpenClient(host, port, user, password);
                    }
                    catch (SocketException)
                    {
                        throw new Exception(string.Format("Cannot connect to database. "
                            + "Is the db4o server running on {1} and listening on port {0}?",
                            port, host));
                    }

                    context.Items[KEY_DB4O_CLIENT] = objectClient;
                }

                return objectClient;
            }
        }

        /// <summary>
        /// Closes db4o client on request end
        /// </summary>
        /// <param name="sender">The HttpApplication</param>
        /// <param name="e"></param>
        private static void Application_EndRequest(object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication)sender;
            HttpContext context = application.Context;

            IObjectContainer objectClient = context.Items[KEY_DB4O_CLIENT] as IObjectContainer;

            if (objectClient != null)
            {
                objectClient.Close();
            }

            context.Items[KEY_DB4O_CLIENT] = null;
        }

        public void Dispose()
        {
        }
    }
}
