using System.Collections.Generic;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace TestApplication.Windsor.TestCases.BasedOn
{
    public class FromThisAssemblyWherePredicate : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                AllTypes.FromThisAssembly().Where(t => !(t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
                );
        }
    }
}