using Db4objects.Db4o;
using Db4objects.Db4o.Config;

namespace Ploosqva.WebAppFrame.Database
{
    public class Controller
    {
        private static Controller self;

        private Controller()
        {
        }

        /// <summary>
        /// Creates a Controller and returns it
        /// </summary>
        internal static Controller GetController()
        {
            if (self == null)
                self = new Controller();

            return self;
        }

        /// <summary>
        /// Pobiera połączenie z bazą. Jeżeli dla tej sesji nie istniało, zostanie otwarte nowe
        /// </summary>
        internal IObjectContainer GetDbClient(IConfiguration config)
        {
            return Db4oHttpModule.GetClient(config);
        }

        /// <summary>
        /// Commits all changes and closes the database
        /// </summary>
        internal static void CloseDatabaseServer()
        {
            Db4oServerModule.CloseServer();
        }
    }
}
