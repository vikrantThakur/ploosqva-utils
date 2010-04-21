using System;
using System.Web;
using Castle.MicroKernel;
using Castle.MicroKernel.Lifestyle;

namespace Ploosqva.ProjectBase.Web
{
    /// <summary>
    /// http://stackoverflow.com/questions/1366214/castle-project-per-session-lifestyle-with-asp-net-mvc
    /// </summary>
    public class PerSessionLifestyleManager : AbstractLifestyleManager
    {
        private readonly string PerRequestObjectID = "PerSessionLifestyleManager_" + Guid.NewGuid();

        public override bool Release(object instance)
        {
            HttpContext.Current.Session[PerRequestObjectID] = instance;

            return base.Release(instance);
        }

        public override object Resolve(CreationContext context)
        {
            if (HttpContext.Current.Session == null)
                return base.Resolve(context);

            if (HttpContext.Current.Session[PerRequestObjectID] == null)
            {
                // Create the actual object
                HttpContext.Current.Session[PerRequestObjectID] = base.Resolve(context);
            }

            return HttpContext.Current.Session[PerRequestObjectID];
        }

        public override void Dispose()
        {
        }
    }
}