using System;
using System.Diagnostics;
using System.Web;
using System.Web.SessionState;
using Db4objects.Db4o;
using Ploosqva.Security;
using Ploosqva.WebAppFrame.Database;

namespace Ploosqva.WebAppFrame.DbFacade
{
    /// <summary>
    /// Base class for creating a web application facade
    /// </summary>
    public abstract class FacadeBase : Db4oAppFrame.FacadeBase
    {
        #region Fields
        /// <summary>
        /// The application's controller
        /// </summary>
        private static Controller ctrl;

        #endregion

        #region Properties
        /// <summary>
        /// Db4o database class
        /// </summary>
        protected new virtual IObjectContainer Db4oClient
        {
            get
            {
                if (objectContainer != null && objectContainer.Ext().IsClosed())
                    objectContainer = null;

                if (objectContainer == null)
                {
                    objectContainer = ctrl.GetDbClient(Configuration);
                }

                return objectContainer;
            }
        }
        /// <summary>
        /// Give access to current session and allows overriding for unit tests
        /// </summary>
        protected virtual HttpSessionState Session
        {
            get
            {
                return HttpContext.Current.Session;
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
        public new long GetObjectId(object item)
        {
            return Db4oClient.Ext().GetID(item);
        }

        /// <summary>
        /// Method check wheather user has appropriete rights and fires
        /// UnauthorizedAccessAttempt if not
        /// </summary>
        /// <typeparam name="T">type of right</typeparam>
        /// <param name="right">the right user needs to pass the test</param>
        public void CheckMethodInvocationRights<T>(T right)
        {
            StackTrace trace = new StackTrace();

            if (!trace.GetFrame(1).GetMethod().CheckRequiredRights(right))
                OnUnauthorizedAccessAttempt();
        }

        ///<summary>
        /// Method makes check wheather the given type is decorated with
        /// RequiredRightsAttribute and fires UnauthorizedAccessAttempt if it is not
        ///</summary>
        ///<param name="t">checked classes type</param>
        /// <typeparam name="T">type of right</typeparam>
        /// <param name="right">the right user needs to pass the test</param>
        public void CheckClassAccessRights<T>(Type t, T right)
        {
            if(!t.CheckRequiredRights(right))
                OnUnauthorizedAccessAttempt();
        }

        public override abstract void CheckUserLogonStatus();
        #endregion

        #region Events

        ///<summary>
        /// Event raised when unlogged user attempts an action which requires
        /// logging in
        ///</summary>
        public static event UnloggedUserEventHandler UnloggedUserActionAttempt;

        protected static void OnUnloggedUserActionAttempt(HttpRequest request, HttpResponse response)
        {
            UnloggedUserEventHandler handler = UnloggedUserActionAttempt;

            if (handler != null)
            {
                handler(null, new UnloggedUserEventArgs(request, response));
            }
        }

        ///<summary>
        /// Event raised when user attempts an action which requires
        /// rights he does not posess
        ///</summary>
        public new static event EventHandler UnauthorizedAccessAttempt;

        protected new static void OnUnauthorizedAccessAttempt()
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
