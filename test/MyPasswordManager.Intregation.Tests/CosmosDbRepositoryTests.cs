using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyPasswordManager.Core.Domain.Login;
using MyPasswordManager.Core.Domain.Secrets;
using MyPasswordManager.Infrastructure.CosmosDb;
using MyPasswordManager.Infrastructure.Extensions;

namespace MyPasswordManager.Intregation.Tests
{
    [TestClass]
    public class CosmosDbRepositoryTests
    {
        private readonly IConfigurationRoot _config;
        private readonly CosmosDbConfiguration _cosmosDbConfiguration;
        private readonly ServiceProvider _serviceProvider;

        private const string TestBeginningId = "#TestBeginningId#";

        public CosmosDbRepositoryTests()
        {
            _cosmosDbConfiguration = new CosmosDbConfiguration();
            _config = new ConfigurationBuilder().AddUserSecrets<CosmosDbRepositoryTests>().Build();
            _config.GetSection("CosmosDbConfiguration").Bind(_cosmosDbConfiguration);

            var services = new ServiceCollection();
            services.AddSingleton(_cosmosDbConfiguration);
            _serviceProvider = services.RegisterCosmosDbRepository().BuildServiceProvider();
        }

        [TestMethod]
        public async Task CanAddSecret_And_Update_WithLoginSuccess()
        {
            const string testCategory1 = "TestCategory";
            const string testTitle1 = "TestTitle";
            const string testLogin1 = "testLogin";
            const string testPassword1 = "testPassword";
            const string testUrl1 = "TestUrl";

            var repository = _serviceProvider.GetRequiredService<ISecret>();

            LoginWithSuccess((IAuthenticate)repository);

            var secretId = TestBeginningId + Guid.NewGuid().ToString();
            await repository.AddSecret(new Secret(secretId, testCategory1, testTitle1, null, testLogin1, testPassword1, testUrl1));

            var allSecrets = await repository.GetAllSecrets();
            var currentSecret = allSecrets.First(s => s.Id == secretId);
            currentSecret.Category.Should().Be(testCategory1);
            currentSecret.Title.Should().Be(testTitle1);
            currentSecret.Url.Should().Be(testUrl1);
            currentSecret.Notes.Should().BeNull();
            currentSecret.Login.Should().Be(testLogin1);
            currentSecret.Password.Should().Be(testPassword1);

            const string testCategory2 = "TestCategory2";
            const string testTitle2 = "TestTitle2";

            await repository.UpdateSecret(new Secret(secretId, testCategory2, testTitle2, null, testLogin1, testPassword1, testUrl1));

            allSecrets = await repository.GetAllSecrets();
            currentSecret = allSecrets.First(s => s.Id == secretId);
            currentSecret.Category.Should().Be(testCategory2);
            currentSecret.Title.Should().Be(testTitle2);
            currentSecret.Url.Should().Be(testUrl1);
            currentSecret.Notes.Should().BeNull();
            currentSecret.Login.Should().Be(testLogin1);
            currentSecret.Password.Should().Be(testPassword1);

            // should be in TestCleanup, but it doesn't support async call
            await CleanContainer(repository);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task CannotAddSecretWithLoginAsFailure()
        {
            var repository = _serviceProvider.GetRequiredService<ISecret>();
            LoginWithFailure((IAuthenticate)repository);

            await repository.AddSecret(new Secret(
                TestBeginningId + Guid.NewGuid().ToString(),
                "TestCategory",
                "TestTitle",
                null,
                "testLogin",
                "testPassword",
                "TestUrl"));
        }

        public async Task CleanContainer(ISecret repository)
        {
            var allSecrets = await repository.GetAllSecrets();
            foreach (var secret in allSecrets.Where(x => x.Id.StartsWith(TestBeginningId)))
            {
                await repository.Delete(secret.Id);
            }
        }

        private void LoginWithSuccess(IAuthenticate secretRepository)
        {
            var login = _config["login"];
            var password = _config["password"];

            var result = secretRepository.Authenticate(new LoginInfos(login, password));
            result.Should().BeTrue();
        }

        private void LoginWithFailure(IAuthenticate secretRepository)
        {
            var result = secretRepository.Authenticate(new LoginInfos("badlogin", "badpassword"));
            result.Should().BeFalse();
        }
    }
}
