using Db4objects.Db4o;

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
        internal IObjectContainer GetDbClient()
        {
            return Db4oHttpModule.Client;
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
