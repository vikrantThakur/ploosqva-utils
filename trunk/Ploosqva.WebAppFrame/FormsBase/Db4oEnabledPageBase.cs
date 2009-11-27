using Ploosqva.WebAppFrame.DbFacade;

namespace Ploosqva.WebAppFrame.FormsBase
{
    /// <summary>
    /// Every page should extend this class to enable globalization, server-side
    /// ViewState and access to the SessionManager class
    /// </summary>
    public abstract class Db4oEnabledPageBase : System.Web.UI.Page
    {
        /// <summary>
        /// The entrypoint to the application layer for all webpages. The SessionManager
        /// has to be used to run all business bahavior.
        /// </summary>
        public abstract FacadeBase Facade { get; }
    }
}
