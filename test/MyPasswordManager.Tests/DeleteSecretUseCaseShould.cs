using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyPasswordManager.Core.UseCases.DeleteSecret;
using MyPasswordManager.Tests.SecretsDoubles;

namespace MyPasswordManager.Tests
{
    [TestClass]
    public class DeleteSecretUseCaseShould
    {
        [TestMethod]
        public async Task DeleteSecret()
        {
            const string id = "Id1";

            var fakeSecretRepository = new FakeSecretRepository();
            fakeSecretRepository.InitSomeDatas();

            var useCase = new DeleteSecretUseCase(fakeSecretRepository);
            var secretResponse = await useCase.Execute(new DeleteSecretRequest
            {
                Id = id,
            });
            secretResponse.IsSuccess.Should().BeTrue();

            var foundedSecret = (await fakeSecretRepository.GetAllSecrets()).FirstOrDefault(x => x.Id == id);
            foundedSecret.Should().BeNull();
        }
    }
}
