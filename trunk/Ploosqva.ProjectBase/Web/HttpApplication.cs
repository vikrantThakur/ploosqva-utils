using System;
using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;
using log4net.Config;

namespace Ploosqva.ProjectBase.Web
{
    /// <summary>
    /// Summary description for IpcsHttpApplication
    /// </summary>
    public class HttpApplication : System.Web.HttpApplication, IContainerAccessor
    {
        /// <summary>
        /// Implements <see cref="IContainerAccessor" /> so that Castle facilities
        /// can gain access to the <see cref="System.Web.HttpApplication" />.
        /// </summary>
        public IWindsorContainer Container
        {
            get { return windsorContainer; }
        }

        /// <summary>
        /// Provides a globally available access to the <see cref="IWindsorContainer" /> instance.
        /// </summary>
        public static IWindsorContainer WindsorContainer
        {
            get { return windsorContainer; }
        }

        /// <summary>
        /// Code that runs on application startup
        /// </summary>
        public virtual void Application_Start(object sender, EventArgs e)
        {
            // Initialize log4net
            XmlConfigurator.Configure();

            // Create the Windsor Container for IoC.
            // Supplying "XmlInterpreter" as the parameter tells Windsor 
            // to look at web.config for any necessary configuration.
            windsorContainer = new WindsorContainer(new XmlInterpreter());
        }

        public virtual void Application_End(object sender, EventArgs e)
        {
            windsorContainer.Dispose();
        }

        public virtual void Application_Error(object sender, EventArgs e) { }

        public virtual void Session_Start(object sender, EventArgs e) { }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// The Session_End event is raised only when the sessionstate mode
        /// is set to InProc in the Web.config file. If session mode is set to StateServer 
        /// or SQLServer, the event is not raised.
        /// </remarks>
        public virtual void Session_End(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Gets instantiated on <see cref="Application_Start" />.
        /// </summary>
        private static IWindsorContainer windsorContainer;
    }
}
