using System.Linq;
using System.Threading.Tasks;
using Eaf.Configuration;
using Eaf.Middleware.Authorization.Users;
using Eaf.Middleware.Configuration;
using Shouldly;
using Xunit;

namespace Eaf.Str.Tests.Authorization.Users
{
    public class UserManager_Tests : UserAppServiceTestBase
    {
        private readonly ISettingManager _settingManager;
        private readonly UserManager _userManager;

        public UserManager_Tests()
        {
            _settingManager = Resolve<ISettingManager>();
            _userManager = Resolve<UserManager>();

            LoginAsDefaultTenantAdmin();
        }

        [Fact]
        public async Task Should_Create_User_With_Random_Password_For_Tenant()
        {
            await _settingManager.ChangeSettingForApplicationAsync(EafMiddlewareSettingNames.UserManagement.PasswordComplexity.RequireUppercase, "true");
            await _settingManager.ChangeSettingForApplicationAsync(EafMiddlewareSettingNames.UserManagement.PasswordComplexity.RequireNonAlphanumeric, "true");
            await _settingManager.ChangeSettingForApplicationAsync(EafMiddlewareSettingNames.UserManagement.PasswordComplexity.RequiredLength, "6");

            var randomPassword = "r5q9y6t2";

            randomPassword.Length.ShouldBeGreaterThanOrEqualTo(10);
            randomPassword.Any(char.IsUpper).ShouldBeTrue();
            randomPassword.Any(char.IsLetterOrDigit).ShouldBeTrue();
        }
    }
}