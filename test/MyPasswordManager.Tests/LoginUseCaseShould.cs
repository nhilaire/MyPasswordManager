using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyPasswordManager.Core.UseCases.Login;
using MyPasswordManager.Tests.LoginDoubles;

namespace MyPasswordManager.Tests
{
    [TestClass]
    public class LoginUseCaseShould
    {
        [TestMethod]
        public void WhenCorrectLoginIsGiven()
        {
            LoginUseCase useCase = BuildLoginUseCase();
            var loginResponse = useCase.Execute(new UserRequest());
            loginResponse.IsSuccess.Should().BeTrue();
        }

        [TestMethod]
        public void WhenBadLoginIsGiven_ReceiveAnError()
        {
            LoginUseCase useCase = BuildLoginUseCase();
            var loginResponse = useCase.Execute(new BadUserRequest());
            loginResponse.IsSuccess.Should().BeFalse();
        }

        private static LoginUseCase BuildLoginUseCase()
        {
            return new LoginUseCase(new FakeAuthenticate());
        }
    }
}