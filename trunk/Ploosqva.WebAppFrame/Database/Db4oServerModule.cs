using System;
using System.IO;
using System.Reflection;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;

namespace Ploosqva.WebAppFrame.Database
{
    /// <summary>
    /// ServerModule manages opening and closing of a db4o server:
    /// - the server is opened on the first request
    /// - the server is closed when the application is offloaded
    /// </summary>
    public class Db4oServerModule : IDisposable
    {
        /// <summary>
        /// Static server object
        /// </summary>
        private static IObjectServer objectServer;
        private static IConfiguration configuration = Db4oFactory.Configure();

        private static string dbFilePath;
        private static int dbPort;
        private static string dbUser, dbPass;

        public static event Db4oEventHandler NewDatabaseCreated;

        protected static void OnNewDatabaseCreated(Db4oEventArgs args)
        {
            Db4oEventHandler handler = NewDatabaseCreated;

            if (handler != null)
            {
                handler(null, args);
            }
        }

        /// <summary>
        /// Registers an alias for persistent classes namespace and assembly. The current assembly is temporary 
        /// and renamed after each recompilation, that is why it can not be used for persistence. An alias name is used 
        /// to persist the classes 
        /// </summary>
        /// <returns></returns>
        private static IConfiguration Configure()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            String assemblyName = "Bizobjects.*, " + assembly.GetName().ToString().Substring(0, assembly.GetName().ToString().LastIndexOf("Version") - 2);

            configuration.AddAlias(new WildcardAlias("Bizobjects.*, Bizobjects", assemblyName));

            return configuration;
        }

        /// <summary>
        /// Initializes server configutration. Set port to 0 to open server in embedded mode
        /// </summary>
        /// <param name="filePath">path to database file</param>
        /// <param name="port">port used by server (0 for embedded server)</param>
        /// <param name="user">db username (not used in embedded mode)</param>
        /// <param name="pass">db user password (not used in embedded mode)</param>
        /// <param name="config">custom configurtion. leave null to use defaults</param>
        public static void InitServer(string filePath, int port, string user, string pass,
            IConfiguration config)
        {
            if (objectServer == null)
            {
                if (config != null)
                    configuration = config;

                dbPort = port;
                dbFilePath = filePath;
                dbUser = user;
                dbPass = pass;
            }
        }

        /// <summary>
        /// Opens a server connection to the db4o database on a request 
        /// </summary>
        internal static IObjectServer Server
        {
            get
            {
                bool newDb = false;

                if (objectServer == null)
                {
                    if (!File.Exists(dbFilePath))
                        newDb = true;

                    objectServer = Db4oFactory.OpenServer(Configure(), dbFilePath, dbPort);

                    if (newDb)
                    {
                        IObjectContainer c = objectServer.OpenClient();

                        OnNewDatabaseCreated(new Db4oEventArgs(c));
                    }

                    if (!string.IsNullOrEmpty(dbUser) && !string.IsNullOrEmpty(dbPass))
                        objectServer.GrantAccess(dbUser, dbPass);
                }

                return objectServer;
            }
        }

        /// <summary>
        /// Method to close the server connection
        /// </summary>
        public static void CloseServer()
        {
            if (objectServer != null)
            {
                objectServer.Close();
            }

            objectServer = null;
        }

        public void Dispose()
        {
            CloseServer();
        }
    }
}