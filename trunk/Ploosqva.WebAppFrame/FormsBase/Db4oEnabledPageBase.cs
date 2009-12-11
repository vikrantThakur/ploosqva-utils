using Ploosqva.WebAppFrame.DbFacade;

namespace Ploosqva.WebAppFrame.FormsBase
{
    /// <summary>
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
