using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyPasswordManager.Core.UseCases.GeneratePassword;

namespace MyPasswordManager.Tests
{
    [TestClass]
    public class GeneratePasswordUseCaseShould
    {
        [TestMethod]
        [DataRow(5)]
        [DataRow(15)]
        [DataRow(25)]
        public void GenerateNewPassword(int length)
        {
            var useCase = new GeneratePasswordUseCase();
            var response = useCase.Execute(new GeneratePasswordRequest { Length = length });

            response.Password.Should().NotBeNullOrEmpty();
            response.Password.Length.Should().Be(length);

            var response2 = useCase.Execute(new GeneratePasswordRequest { Length = length });

            response2.Password.Should().NotBeNullOrEmpty();
            response2.Password.Length.Should().Be(length);
            response.Password.Should().NotBe(response2.Password);

        }
    }
}
