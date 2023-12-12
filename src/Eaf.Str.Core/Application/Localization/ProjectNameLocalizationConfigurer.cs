using Eaf.Configuration.Startup;
using Eaf.Localization.Dictionaries;
using Eaf.Localization.Dictionaries.Xml;
using System;

namespace Eaf.Str.Localization
{
    public static class ProjectNameLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(
                    ProjectNameConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(ProjectNameLocalizationConfigurer).GetAssembly(),
                        "Eaf.Str.Application.Localization.ProjectName"
                    )
                )
            );
        }
    }
}