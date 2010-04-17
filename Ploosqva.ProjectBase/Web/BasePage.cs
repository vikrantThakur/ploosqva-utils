using System;
using System.Web.UI;

namespace Ploosqva.ProjectBase.Web
{
    public abstract class BasePage : Page
    {
        /// <summary>
        /// Page_Load of the Page Controller pattern.
        /// See http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dnpatterns/html/ImpPageController.asp
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            // Do whatever standard code which occurs on every page

            PageLoad();
        }

        protected abstract void PageLoad();

        /// <summary>
        /// Exposes accessor for the <see cref="IDaoFactoryBase" /> used by all pages.
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
