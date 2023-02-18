using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyPasswordManager.Core.ApplicationServices;
using MyPasswordManager.Core.UseCases.AddNewSecret;
using MyPasswordManager.Tests.SecretsDoubles;

namespace MyPasswordManager.Tests
{
    [TestClass]
    public class AddNewSecretUseCaseShould
    {
        [TestMethod]
        public async Task CreateANewEncodedSecret_AndVerifyIfOk()
        {
            const string encryptionKey = "AbcDef123456";
            const string textToEncode = "Hello World";
            const string saltText = "MySalt";
            const string category = "Test category";
            var fakeSecretRepository = new FakeSecretRepository();
            var useCase = new AddNewSecretUseCase(fakeSecretRepository, new CryptoService());
            var secretResponse = await useCase.Execute(new AddSecretRequest
            {
                Category = category,
                Notes = string.Empty,
                Login = "Login",
                Password = textToEncode,
                Title = "My super site",
                Url = "https://url",
                EncryptionKey = encryptionKey,
                Salt = saltText
            });
            secretResponse.IsSuccess.Should().BeTrue();

            var secrets = await fakeSecretRepository.GetAllSecrets();
            secrets.Count.Should().Be(1);
            var currentSecret = secrets.First();
            currentSecret.Category.Should().Be(category);
            currentSecret.Password.Should().Be("9G6tTXhAgdPbtutNdZzBocdx4y4CWjorx3paqvd5k8M=");
        }
    }
}
