using MyPasswordManager.Core.ApplicationServices;

namespace MyPasswordManager.Core.UseCases.GetAllSecrets
{
    public class GetAllSecretsUseCase
    {
        private readonly AllSecretsReader _allSecretsReader;

        public GetAllSecretsUseCase(AllSecretsReader allSecretsReader)
        {
            _allSecretsReader = allSecretsReader;
        }

        public async Task<GetAllSecretsResponse> Execute(GetAllSecretsRequest getAllSecretsRequest)
        {
            var allSecrets = await _allSecretsReader.GetAllSecrets(getAllSecretsRequest.EncryptionKey, getAllSecretsRequest.Salt);

            return new GetAllSecretsResponse
            {
                IsSuccess = true,
                SecretResponses = allSecrets.ToList()
            };
        }
    }
}
