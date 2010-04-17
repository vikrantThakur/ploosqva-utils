using System;
using System.Web;

namespace Ploosqva.ProjectBase.NHibernate.Fluent.SessionMgmt
{
    /// <summary>
    /// Implements the Open-Session-In-View pattern using <see cref="FluentNhSessionManager" />.
    /// Inspiration for this class came from Ed Courtenay at 
    /// http://sourceforge.net/forum/message.php?msg_id=2847509.
    /// </summary>
    public class FluentNhSessionModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.EndRequest += CommitAndCloseSession;
        }

        /// <summary>
        /// Commits and closes the NHibernate session provided by the supplied <see cref="FluentNhSessionManager"/>.
        /// Assumes a transaction was begun at the beginning of the request; but a transaction or session does
        /// not *have* to be opened for this to operate successfully.
        /// </summary>
        private static void CommitAndCloseSession(object sender, EventArgs e)
        {
            try
            {
                FluentNhSessionManager.Instance.CommitTransaction();
            }
            finally
            {
                // No matter what happens, make sure the session gets closed
                FluentNhSessionManager.Instance.CloseSession();
            }
        }

        public void Dispose() { }
    }
}