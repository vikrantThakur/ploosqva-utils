using System.Reflection;
using Castle.Core.Configuration;
using Castle.Facilities.NHibernateIntegration;
using Castle.Facilities.NHibernateIntegration.Builders;
using FluentNHibernate;
using NHibernate.Cfg;
using Ploosqva.DesignByContract;

namespace Ploosqva.ProjectBase.NHibernate
{
    public class FluentNHConfigBuilder : IConfigurationBuilder
    {
        public Configuration GetConfiguration(IConfiguration facilityConfiguration)
        {
            var defaultConfigurationBuilder = new DefaultConfigurationBuilder();
            var configuration = defaultConfigurationBuilder.GetConfiguration(facilityConfiguration);

            IConfiguration mappingsConfiguration = facilityConfiguration.Children["mappingsAssembly"];

            Check.Require(mappingsConfiguration != null, "mappingsAssembly element missing in NHibernateIntegration facility configuration");

            Assembly mappings = Assembly.Load(mappingsConfiguration.Value);
            configuration.AddMappingsFromAssembly(mappings);
            
            return configuration;
        }
    }
}
