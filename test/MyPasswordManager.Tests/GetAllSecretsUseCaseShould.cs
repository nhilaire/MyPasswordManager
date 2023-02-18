using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyPasswordManager.Core.ApplicationServices;
using MyPasswordManager.Core.UseCases.GetAllSecrets;
using MyPasswordManager.Tests.SecretsDoubles;

namespace MyPasswordManager.Tests
{
    [TestClass]
    public class GetAllSecretsUseCaseShould
    {
        [TestMethod]
        public async Task ReturnAllSecretDecoded()
        {
            const string encryptionKey = "AbcDef123456";
            const string saltText = "MySalt";

            var fakeSecretRepository = new FakeSecretRepository();
            fakeSecretRepository.InitSomeDatas();

            var allSecretReader = new AllSecretsReader(fakeSecretRepository, new CryptoService());
            var useCase = new GetAllSecretsUseCase(allSecretReader);
            var response = await useCase.Execute(new GetAllSecretsRequest
            {
                EncryptionKey = encryptionKey,
                Salt = saltText
            });

            response.IsSuccess.Should().BeTrue();
            response.SecretResponses.Should().SatisfyRespectively(
                first =>
                {
                    first.Category.Should().Be("Category 1");
                    first.DecodedPassword.Should().Be("Hello World");
                },
                second =>
                {
                    second.Category.Should().Be("Category 2");
                    second.DecodedPassword.Should().Be("Love C#");
                },
                third =>
                {
                    third.Category.Should().Be("Category 3");
                    third.DecodedPassword.Should().Be("What a beautiful day !");
                },
                fourth =>
                {
                    fourth.Category.Should().Be("Category 4");
                    fourth.DecodedPassword.Should().Be("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.");
                });
        }
    }
}
