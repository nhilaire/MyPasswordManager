using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyPasswordManager.Intregation.Tests
{
    [TestClass]
    public class AuthenticationControllerTests
    {
        private static WebApplicationFactory<Program> _factory;

        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            _factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                });
        }

        [ClassCleanup]
        public static void ClassCleanUp()
        {
            _factory.Dispose();
        }

        [TestMethod]
        public async Task Login_WithInvalidCredentials_ReturnsKo()
        {
            var client = _factory.CreateClient();

            var page = await client.GetAsync("/");
            await AssertForm(page);

            var result = await Login(client, "login", "badpassword");
            result.RequestMessage.RequestUri.PathAndQuery.Should().Be("/?error=true");

            page = await client.GetAsync(result.RequestMessage.RequestUri.PathAndQuery);
            page.EnsureSuccessStatusCode();
            var content = await page.Content.ReadAsStringAsync();
            content.Should().Contain(@"<p>Access denied !</p>");
        }

        [TestMethod]
        public async Task Login_WithValidCredentials_ReturnsOk()
        {
            (string login, string password) = GetCredentials();
            var client = _factory.CreateClient();

            var page = await client.GetAsync("/");
            await AssertForm(page);

            var result = await Login(client, login, password);

            result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            result.RequestMessage.RequestUri.PathAndQuery.Should().Be("/list");
        }

        [TestMethod]
        public async Task CannotShareAuthenticationBetweenSession()
        {
            (string login, string password) = GetCredentials();
            var client = _factory.CreateClient();
            var client2 = _factory.CreateClient();

            var result = await Login(client, login, password);
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var content = await client.GetStringAsync("/list");
            var content2 = await client2.GetStringAsync("/list");

            content.Should().NotContain("You're not allowed to visit this page.");
            content2.Should().Contain("You're not allowed to visit this page.");
        }

        private static (string login, string password) GetCredentials()
        {
            var config = new ConfigurationBuilder().AddUserSecrets<AuthenticationRepositoryTests>().Build();
            return (config["login"], config["password"]);
        }

        private static async Task<HttpResponseMessage> Login(HttpClient client, string login, string password)
        {
            using var formData = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("Login", login),
                new KeyValuePair<string, string>("Password", password),
            });
            var result = await client.PostAsync("/auth/signin", formData);
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            return result;
        }

        private static async Task AssertForm(HttpResponseMessage page)
        {
            page.EnsureSuccessStatusCode();
            var content = await page.Content.ReadAsStringAsync();

            content.Should().Contain(@"<form action=""/auth/signin"" method=""post"">");
            content.Should().Contain(@"<input type=""text"" name=""Login"" placeholder=""login"" autocomplete=""off"" />");
            content.Should().Contain(@"<input type=""password"" name=""Password"" placeholder=""Password"" />");
        }
    }
}
