using System;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;

namespace Ploosqva.Db4oAppFrame
{
    /// <summary>
    /// Base class for creating a web application facade
    /// </summary>
    public abstract class FacadeBase
    {
        #region Fields
        /// <summary>
        /// The database client class used by SessionManager to access the database
        /// </summary>
        protected static IObjectContainer objectContainer;

        protected IConfiguration config;
        #endregion

        #region Properties
        /// <summary>
        /// Db4o database class
        /// </summary>
        protected virtual IObjectContainer Db4oClient
        {
            get
            {
                if (objectContainer != null && objectContainer.Ext().IsClosed())
                    objectContainer = null;

                if (objectContainer == null)
                {
                    throw new NotImplementedException("Implement opening the IObjectClient!");
                }

                return objectContainer;
            }
        }

        protected virtual IConfiguration Configuration
        {
            get
            {
                if (config == null)
                    config = Db4oFactory.NewConfiguration();

                return config;
            }
        }
        #endregion

        #region Methods

        /// <summary>
        /// Returns the database-unique id for an object
        /// </summary>
        public long GetObjectId(object item)
        {
            return Db4oClient.Ext().GetID(item);
        }

        /// <summary>
        /// Method checks wheather user is logged in and fires
        /// UnloggedUserActionAttempt is not
        /// </summary>
        public abstract void CheckUserLogonStatus();
        #endregion

        #region Events

        ///<summary>
        /// Event raised when user attempts an action which requires
        /// rights he does not posess
        ///</summary>
        public static event EventHandler UnauthorizedAccessAttempt;

        protected static void OnUnauthorizedAccessAttempt()
        {
            EventHandler handler = UnauthorizedAccessAttempt;

            if (handler != null)
            {
                handler(null, new EventArgs());
            }
        }

        #endregion
    }
}
