using System.Web.UI;

namespace Ploosqva.ProjectBase.Web
{
    /// <summary>
    /// Summary description for BaseControl
    /// </summary>
    public abstract class BaseUserControl : UserControl
    {
        /// <summary>
        /// Exposes accessor for the <see cref="IDaoFactory" /> used by all pages.
        /// </summary>
        public IDaoFactoryBase DaoFactory
        {
            get
            {
                return (IDaoFactoryBase)HttpApplication.WindsorContainer[typeof(IDaoFactoryBase)];
            }
        }

        // You can expose IAnotherDbDaoFactory here
    }
}
