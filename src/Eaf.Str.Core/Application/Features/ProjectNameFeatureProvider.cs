using Eaf.Application.Features;
using Eaf.Localization;
using Eaf.UI.Inputs;

namespace Eaf.Str.Features
{
    public class ProjectNameFeatureProvider : FeatureProvider
    {
        public override void SetFeatures(IFeatureDefinitionContext context)
        {
            context.Create(
                ProjectNameFeatures.TestCheckFeature,
                defaultValue: "false",
                displayName: L("TestCheckFeature"),
                inputType: new CheckboxInputType()
            );
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, ProjectNameConsts.LocalizationSourceName);
        }
    }
}
