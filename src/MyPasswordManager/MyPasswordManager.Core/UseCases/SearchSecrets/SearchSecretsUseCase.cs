using MyPasswordManager.Core.ApplicationServices;
using MyPasswordManager.Core.UseCases.GetAllSecrets;

namespace MyPasswordManager.Core.UseCases.SearchSecrets
{
    public class SearchSecretsUseCase
    {
        private readonly AllSecretsReader _allSecretsReader;

        public SearchSecretsUseCase(AllSecretsReader allSecretsReader)
        {
            _allSecretsReader = allSecretsReader;
        }

        public async Task<SearchSecretsResponse> Execute(SearchSecretsRequest searchSecretsRequest)
        {
            var allSecrets = await _allSecretsReader.GetAllSecrets(searchSecretsRequest.EncryptionKey, searchSecretsRequest.Salt);

            var result = new List<SecretResponse>();
            foreach (var secret in allSecrets)
            {
                if (secret.MatchQuery(searchSecretsRequest.Query))
                {
                    result.Add(secret);
                }
            }

            return new SearchSecretsResponse
            {
                IsSuccess = true,
                SecretResponses = result
            };
        }
    }
}
