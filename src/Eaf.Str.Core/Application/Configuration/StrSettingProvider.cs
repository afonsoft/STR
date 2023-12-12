using Eaf.Configuration;
using System.Collections.Generic;

namespace Eaf.Str.Configuration
{
    public class StrSettingProvider : SettingProvider
    {
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            return new[]    
                {
                    new SettingDefinition(StrSettings.AirplaneSettings.IsAirplaneManagerEnabled, "true"),
                };
        }
    }
}
