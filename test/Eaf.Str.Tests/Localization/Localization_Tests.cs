using System.Globalization;
using Eaf.Localization;
using Shouldly;
using Xunit;

namespace Eaf.Str.Tests.Localization
{
    // ReSharper disable once InconsistentNaming
    public class Localization_Tests : AppTestBase
    {
        [Theory]
        [InlineData("en")]
        [InlineData("en-US")]
        [InlineData("en-AU")]
        public void Simple_Localization_Test(string cultureName)
        {
            CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo(cultureName);

            Resolve<ILanguageManager>().CurrentLanguage.Name.ShouldBe("en");

            var localizationManager = Resolve<ILocalizationManager>();

            localizationManager.GetString(ProjectNameConsts.LocalizationSourceName, "Identity.UserNotInRole")
                .ShouldBe("User is not in role '{0}'.");
        }
    }
}