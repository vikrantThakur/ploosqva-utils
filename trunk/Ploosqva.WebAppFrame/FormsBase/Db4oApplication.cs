using System;
using System.Web;
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

        protected virtual void Application_Start(object sender, EventArgs e)
        {
            Db4oServerModule.InitServer(file, port, user, pass, null);
        }

        protected virtual void Application_End(object sender, EventArgs e)
        {
            FacadeBase.CloseDatabaseServer();
        }
    }
}
