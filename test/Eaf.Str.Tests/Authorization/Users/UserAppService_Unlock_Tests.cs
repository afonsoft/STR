using System.Threading.Tasks;
using Eaf.Application.Services.Dto;
using Eaf.Authorization;
using Eaf.Middleware.Authorization;
using Eaf.Middleware.Authorization.Users;
using Eaf.MultiTenancy;
using Shouldly;
using Xunit;

namespace Eaf.Str.Tests.Authorization.Users
{
    // ReSharper disable once InconsistentNaming
    public class UserAppService_Unlock_Tests : UserAppServiceTestBase
    {
        private readonly UserManager _userManager;
        private readonly LogInManager _loginManager;

        public UserAppService_Unlock_Tests()
        {
            _userManager = Resolve<UserManager>();
            _loginManager = Resolve<LogInManager>();

            CreateTestUsers();
        }

        [Fact]
        public async Task Should_Unlock_User()
        {
            //Arrange

            await _userManager.InitializeOptionsAsync(EafSession.TenantId);
            var user = await GetUserByUserNameAsync("jnash");

            //Pre conditions
            (await _userManager.IsLockedOutAsync(user)).ShouldBeFalse();
            user.IsLockoutEnabled?.ShouldBeTrue();

            //Try wrong password until lockout
            EafLoginResultType loginResultType;
            do
            {
                loginResultType = (await _loginManager.LoginAsync(user.UserName, "wrong-password", EafTenantBase.DefaultTenantName)).Result;
            } while (loginResultType != EafLoginResultType.LockedOut);

            (await _userManager.IsLockedOutAsync(await GetUserByUserNameAsync("jnash"))).ShouldBeTrue();

            //Act

            await UserAppService.UnlockUser(new EntityDto<long>(user.Id));

            //Assert

            (await _loginManager.LoginAsync(user.UserName, "wrong-password", EafTenantBase.DefaultTenantName)).Result.ShouldBe(EafLoginResultType.InvalidPassword);
        }
    }
}