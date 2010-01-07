using System;
using System.Net.Sockets;
using System.Web;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;

namespace Ploosqva.WebAppFrame.Database
{
    /// <summary>
    /// This module opens a client connection to a db4o database server.
    /// By default client tries to connect to embedded server
    /// To change default behavior, HttpApplicationState variables 
    /// dbServerEmbedded, dbHost, dbPort, dbUser and dbPass have to be set.
    /// </summary>
    public class Db4oHttpModule : IHttpModule
    {
        /// <summary>
        /// keys for the named parameters in Web.config 
        /// </summary>
        internal const string KEY_DB4O_CLIENT = "db4oClient";

        private static int dbPort = 10000;
        private static string dbHost = "localhost";
        private static string dbUser;
        private static string dbPass;
        private static bool isEmbeddedServer = true;
        private static readonly IConfiguration config = Db4oFactory.NewConfiguration();

        ///<summary>
        /// Configuration used to open database clients
        ///</summary>
        public static IConfiguration Configuration
        {
            get
            {
                return config;
            }
        }

        ///<summary>
        /// Initializes database client connection parameters
        ///</summary>
        ///<param name="isServerEmbedded">if true, will try to connect to a locally running db4o server and will ignore the other parameters</param>
        ///<param name="host">server host address/ip. If null, localhost is used</param>
        ///<param name="port">server port. If null, 10000 is used</param>
        ///<param name="user">db username as set using Db4oServerModule#Init</param>
        ///<param name="password">db user password as set using Db4oServerModule#Init</param>
        public static void InitClient(bool isServerEmbedded, string host, int? port,
            string user, string password)
        {
            isEmbeddedServer = isServerEmbedded;

            if (!isServerEmbedded)
            {
                if (port != null)
                    dbPort = (int)port;

                if (!string.IsNullOrEmpty(host))
                    dbHost = host;

                if (!string.IsNullOrEmpty(user))
                    dbUser = user;

                if (!string.IsNullOrEmpty(password))
                    dbPass = password;
            }
        }

        /// <summary>
        /// Initializes the Db4oHttpModule, and sets server data from HttpApplicationState
        /// (if set)
        /// </summary>
        /// <param name="application">application class</param>
        public void Init(HttpApplication application)
        {
            application.EndRequest += Application_EndRequest;
        }

        /// <summary>
        /// Returns an existing client connection to the caller. 
        /// If client connection does not exist - it will be created
        /// </summary>
        public static IObjectContainer GetClient(IConfiguration configuration)
        {
            HttpContext context = HttpContext.Current;

            IObjectContainer objectClient = context.Items[KEY_DB4O_CLIENT] as IObjectContainer;

            if (objectClient == null)
            {
                /// If opening the database fails, it can mean that wrong port is set or
                /// that the main web app is not running
                try
                {
                    if (isEmbeddedServer)
                        objectClient = Db4oServerModule.Server.OpenClient(configuration);
                    else
                        objectClient = Db4oFactory.OpenClient(configuration, dbHost, dbPort, dbUser, dbPass);
                }
                catch (SocketException)
                {
                    throw new Exception(string.Format("Cannot connect to database. "
                        + "Is the db4o server running on {1} and listening on port {0}?",
                        dbPort, dbHost));
                }

                context.Items[KEY_DB4O_CLIENT] = objectClient;
            }

            return objectClient;
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
