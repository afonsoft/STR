using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Shouldly;
using Xunit;
using Eaf.Middleware.Authorization.Users.Profile;
using Eaf.Middleware.Authorization.Users.Profile.Dto;
using Eaf.Middleware.Authorization.Users;
using Eaf.Application.Services.Dto;

namespace Eaf.Str.Tests.Authorization.Users
{
    // ReSharper disable once InconsistentNaming
    public class UserAppService_Delete_Tests : UserAppServiceTestBase
    {
        [Fact]
        public async Task Should_Delete_User()
        {
            //Arrange
            CreateTestUsers();

            var user = await GetUserByUserNameOrNullAsync("artdent");
            user.ShouldNotBe(null);

            //Act
            await UserAppService.DeleteUser(new EntityDto<long>(user.Id));

            //Assert
            user = await GetUserByUserNameOrNullAsync("artdent");
            user.IsDeleted.ShouldBe(true);
        }
    }
}