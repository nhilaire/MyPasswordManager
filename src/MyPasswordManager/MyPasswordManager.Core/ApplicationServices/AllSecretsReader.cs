using MyPasswordManager.Core.Domain.Secrets;
using MyPasswordManager.Core.UseCases.GetAllSecrets;

namespace MyPasswordManager.Core.ApplicationServices
{
    public class AllSecretsReader
    {
        private readonly ISecret _secret;
        private readonly CryptoService _cryptoService;

        public AllSecretsReader(ISecret secret, CryptoService cryptoService)
        {
            _secret = secret;
            _cryptoService = cryptoService;
        }

        public async Task<IReadOnlyList<SecretResponse>> GetAllSecrets(string encryptionKey, string salt)
        {
            var result = new List<SecretResponse>();
            var allSecrets = await _secret.GetAllSecrets();
            foreach (var secret in allSecrets)
            {
                var decodedPassword = _cryptoService.Decrypt(secret.Password, encryptionKey, salt);
                result.Add(new SecretResponse
                {
                    Id = secret.Id,
                    Category = secret.Category,
                    Notes = secret.Notes,
                    Login = secret.Login,
                    Title = secret.Title,
                    Url = secret.Url,
                    DecodedPassword = decodedPassword
                });
            }
            return result;
        }
    }
}
