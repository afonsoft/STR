using Eaf.Configuration;
using System.Collections.Generic;

namespace Eaf.Str.Configuration
{
    public class ProjectNameSettingProvider : SettingProvider
    {
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            return new[]    
                {
                    new SettingDefinition(ProjectNameSettings.AirplaneSettings.IsAirplaneManagerEnabled, "true"),
                };
        }
    }
}
