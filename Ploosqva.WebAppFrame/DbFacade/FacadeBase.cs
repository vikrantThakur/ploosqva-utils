using Db4objects.Db4o;
using Ploosqva.WebAppFrame.Database;

namespace Ploosqva.WebAppFrame.DbFacade
{
    /// <summary>
    /// Base class for creating a web application facade
    /// </summary>
    public abstract class FacadeBase
    {
        #region Fields
        /// <summary>
        /// The application's controller
        /// </summary>
        private static Controller ctrl;
        /// <summary>
        /// The database client class used by SessionManager to access the database
        /// </summary>
        private static IObjectContainer objectContainer;
        #endregion

        #region Properties
        /// <summary>
        /// Db4o database class
        /// </summary>
        protected internal IObjectContainer Db4oClient
        {
            get
            {
                if (objectContainer == null)
                    objectContainer = ctrl.GetDbClient();

                return objectContainer;
            }
        }

        #endregion

        #region Methods
        protected FacadeBase()
        {
            if (ctrl == null)
                ctrl = Controller.GetController();
        }

        /// <summary>
        /// Commits all changes and closes the database
        /// </summary>
        public static void CloseDatabaseServer()
        {
            Controller.CloseDatabaseServer();
        }

        /// <summary>
        /// Returns the database-unique id for an object
        /// </summary>
        public long GetObjectId(object item)
        {
            return Db4oClient.Ext().GetID(item);
        }
        #endregion
    }
}
