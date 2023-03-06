using LanguageExt;
using Microsoft.Azure.Cosmos;
using MyPasswordManager.Core.Domain.Login;
using MyPasswordManager.Core.Domain.Secrets;
using System.Net;

namespace MyPasswordManager.Infrastructure.CosmosDb
{
    public class SecretsRepository : ISecret
    {
        private readonly CosmosDbConfiguration _cosmosDbConfiguration;
        private Container? _container;

        public SecretsRepository(CosmosDbConfiguration cosmosDbConfiguration)
        {
            _cosmosDbConfiguration = cosmosDbConfiguration;
        }

        public async Task<bool> AddSecret(Secret secret)
        {
            if (secret == null)
                throw new ArgumentNullException();

            var container = GetContainer();

            var existingSecret = await ReadSecret(container, secret.Id);
            var secretDao = ToSecretDAO(secret);
            return await existingSecret.MatchAsync(
                Some: foundedSecret => false,
                None: async () =>
                {
                    await container.CreateItemAsync(secretDao, new PartitionKey(SecretDAO.UniquePartition));
                    return true;
                });
        }

        public async Task<bool> UpdateSecret(Secret secret)
        {
            if (secret == null)
                throw new ArgumentNullException();

            var container = GetContainer();

            var existingSecret = await ReadSecret(container, secret.Id);
            var secretDao = ToSecretDAO(secret);
            return await existingSecret.MatchAsync(
                Some: async foundedSecret =>
                {
                    await container.ReplaceItemAsync<SecretDAO>(secretDao, foundedSecret.Id, new PartitionKey(SecretDAO.UniquePartition));
                    return true;
                },
                None: () => false);
        }

        public async Task Delete(string secretId)
        {
            var container = GetContainer();
            await container.DeleteItemAsync<SecretDAO>(secretId, new PartitionKey(SecretDAO.UniquePartition));
        }

        public async Task<IReadOnlyCollection<Secret>> GetAllSecrets()
        {
            var container = GetContainer();
            var queryDefinition = new QueryDefinition("SELECT * FROM c");
            var iterator = container.GetItemQueryIterator<SecretDAO>(queryDefinition, requestOptions: new QueryRequestOptions
            {
                PartitionKey = new PartitionKey(SecretDAO.UniquePartition)
            });

            var results = new List<Secret>();

            while (iterator.HasMoreResults)
            {
                var result = await iterator.ReadNextAsync();
                var secrets = result.Resource.Select(x => new Secret(x.Id, x.Category, x.Title, x.Notes, x.Login, x.Password, x.Url));
                results.AddRange(secrets);
            }

            return results;
        }

        public bool Authenticate(LoginInfos loginInfos)
        {
            try
            {
                var connectionString = _cosmosDbConfiguration.ConnectionString!
                                            .Replace("##login##", loginInfos.Login).Replace("##password##", loginInfos.Password);
                var cosmosClient = new CosmosClient(connectionString);
                var db = cosmosClient.GetDatabase(_cosmosDbConfiguration.DatabaseName);
                _container = db.GetContainer(_cosmosDbConfiguration.ContainerName);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private Container GetContainer()
        {
            if (_container == null)
            {
                throw new InvalidOperationException("Authenticate first");
            }
            return _container;
        }

        private static async Task<Option<SecretDAO>> ReadSecret(Container container, string id)
        {
            try
            {
                var response = await container.ReadItemAsync<SecretDAO>(id, new PartitionKey(SecretDAO.UniquePartition));
                return Option<SecretDAO>.Some(response);
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return Option<SecretDAO>.None;
            }
        }

        private static SecretDAO ToSecretDAO(Secret secret)
        {
            return new SecretDAO
            {
                Id = secret.Id,
                Password = secret.Password,
                Login = secret.Login,
                Category = secret.Category,
                Notes = secret.Notes,
                Partition = SecretDAO.UniquePartition,
                Title = secret.Title,
                Url = secret.Url
            };
        }
    }
}
