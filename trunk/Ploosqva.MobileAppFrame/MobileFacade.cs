using Db4objects.Db4o;

namespace Ploosqva.MobileAppFrame
{
    public abstract class FacadeBase : Db4oAppFrame.FacadeBase
    {
        protected readonly string dbPath;

        protected FacadeBase(string path)
        {
            dbPath = path;
        }

        public abstract override void CheckUserLogonStatus();

        protected override IObjectContainer Db4oClient
        {
            get
            {
                if (objectContainer != null && objectContainer.Ext().IsClosed())
                    objectContainer = null;

                if (objectContainer == null)
                {
                    objectContainer = Db4oFactory.OpenFile(dbPath);
                }

                return objectContainer;
            }
        }
    }
}
