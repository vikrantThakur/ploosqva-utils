using System;
using Db4objects.Db4o;

namespace Ploosqva.WebAppFrame.Database
{
    public class Db4oEventArgs : EventArgs, IDisposable
    {
        public IObjectContainer Database { get; private set; }

        internal Db4oEventArgs(IObjectContainer objectContainer)
        {
            Database = objectContainer;
        }

        public void Dispose()
        {
            if (Database != null)
                Database.Close();
        }
    }
}
