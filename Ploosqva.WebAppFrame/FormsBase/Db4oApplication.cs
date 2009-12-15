using System;
using System.Web;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Ploosqva.WebAppFrame.Database;
using Ploosqva.WebAppFrame.DbFacade;

namespace Ploosqva.WebAppFrame.FormsBase
{
    public class Db4oApplication : HttpApplication
    {
        protected string file;
        protected int port;
        protected string user;
        protected string pass;
        private IConfiguration config;
        protected IConfiguration Config
        {
            get
            {
                if (config == null)
                    config = Db4oFactory.Configure();

                return config;
            }
        }

        protected virtual void Application_Start(object sender, EventArgs e)
        {
            Db4oServerModule.InitServer(file, port, user, pass, config);
        }

        protected virtual void Application_End(object sender, EventArgs e)
        {
            FacadeBase.CloseDatabaseServer();
        }
    }
}
