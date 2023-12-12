using Eaf.Dependency;
using Eaf.Middleware.Configuration;
using Microsoft.Extensions.Configuration;
using System;
using System.Reflection;

namespace Eaf.Str.Test.Base.Configuration
{
    public class TestAppConfigurationAccessor : IAppConfigurationAccessor, ISingletonDependency
    {
        public IConfigurationRoot Configuration { get; }

        public TestAppConfigurationAccessor()
        {
            Configuration = AppConfigurations.Get(
                typeof(ProjectNameTestBaseModule).GetAssembly().GetDirectoryPathOrNull()
            );
        }
    }
}