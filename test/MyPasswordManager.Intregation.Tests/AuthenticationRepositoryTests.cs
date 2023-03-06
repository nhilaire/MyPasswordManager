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
    public class AuthenticationRepositoryTests
    {
        private readonly IConfigurationRoot _config;
        private readonly CosmosDbConfiguration _cosmosDbConfiguration;
        private readonly ServiceProvider _serviceProvider;
        public AuthenticationRepositoryTests()
        {
            _cosmosDbConfiguration = new CosmosDbConfiguration();
            _config = new ConfigurationBuilder().AddUserSecrets<AuthenticationRepositoryTests>().Build();
            _config.GetSection("CosmosDbConfiguration").Bind(_cosmosDbConfiguration);

            var services = new ServiceCollection();
            services.AddSingleton(_cosmosDbConfiguration);
            _serviceProvider = services.RegisterCosmosDbRepository().BuildServiceProvider();
        }

        [TestMethod]
        public void Authenticate_Success()
        {
            var login = _config["login"];
            var password = _config["password"];

            var repository = _serviceProvider.GetRequiredService<ISecret>();
            var result = repository.Authenticate(new LoginInfos(login, password));
            result.Should().BeTrue();
        }

        [TestMethod]
        public void Authenticate_Fail()
        {
            var repository = _serviceProvider.GetRequiredService<ISecret>();
            var result = repository.Authenticate(new LoginInfos("login", "badpwd"));
            result.Should().BeFalse();
        }
    }
}