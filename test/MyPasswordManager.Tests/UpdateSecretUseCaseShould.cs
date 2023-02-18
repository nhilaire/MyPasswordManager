using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyPasswordManager.Core.ApplicationServices;
using MyPasswordManager.Core.UseCases.UpdateSecret;
using MyPasswordManager.Tests.SecretsDoubles;

namespace MyPasswordManager.Tests
{
    [TestClass]
    public class UpdateSecretUseCaseShould
    {
        [TestMethod]
        public async Task UpdateSecret_WithCorrectValues() 
        {
            const string encryptionKey = "AbcDef123456";
            const string saltText = "MySalt";

            const string newPassword = "The new password";
            const string category = "The new category";
            const string id = "Id1";
            const string newLogin = "Login";
            const string newTitle = "My super site";

            var fakeSecretRepository = new FakeSecretRepository();
            fakeSecretRepository.InitSomeDatas();

            var useCase = new UpdateSecretUseCase(fakeSecretRepository, new CryptoService());
            var secretResponse = await useCase.Execute(new UpdateSecretRequest
            {
                Id = id,
                Category = category,
                Notes = string.Empty,
                Login = newLogin,
                Password = newPassword,
                Title = newTitle,
                Url = "https://url",
                EncryptionKey = encryptionKey,
                Salt = saltText
            });
            secretResponse.IsSuccess.Should().BeTrue();

            var updateSecret = (await fakeSecretRepository.GetAllSecrets()).First(x => x.Id == id);
            updateSecret.Id.Should().Be(id);
            updateSecret.Category.Should().Be(category);
            updateSecret.Login.Should().Be(newLogin);
            updateSecret.Title.Should().Be(newTitle);
        }
    }
}
