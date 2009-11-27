using System;
using Db4objects.Db4o;

namespace Ploosqva.WebAppFrame.Database
{
    ///<summary>
    /// Arguments for events using the database server/client
    ///</summary>
    public class Db4oEventArgs : EventArgs, IDisposable
    {
        ///<summary>
        /// D4bo client (an be null at some events)
        ///</summary>
        public IObjectContainer Database { get; private set; }
        /// <summary>
        /// D4bo server (an be null at some events)
        /// </summary>
        public IObjectServer Server { get; private set; }

        internal Db4oEventArgs(IObjectServer objectServer, IObjectContainer objectContainer)
        {
            Database = objectContainer;
            Server = objectServer;
        }

        public void Dispose()
        {
            if (Database != null)
                Database.Close();
        }
    }
}
