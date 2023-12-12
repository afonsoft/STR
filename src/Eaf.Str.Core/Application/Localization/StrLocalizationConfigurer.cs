using Eaf.Configuration.Startup;
using Eaf.Localization.Dictionaries;
using Eaf.Localization.Dictionaries.Xml;
using System;

namespace Eaf.Str.Localization
{
    public static class StrLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(
                    StrConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(StrLocalizationConfigurer).GetAssembly(),
                        "Eaf.Str.Application.Localization.Str"
                    )
                )
            );
        }
    }
}