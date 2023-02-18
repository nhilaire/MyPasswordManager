using MyPasswordManager.Core.Domain.Secrets;
using MyPasswordManager.Core.ApplicationServices;

namespace MyPasswordManager.Core.UseCases.AddNewSecret
{
    public class AddNewSecretUseCase
    {
        private readonly ISecret _secret;
        private readonly CryptoService _cryptoService;

        public AddNewSecretUseCase(ISecret secret, CryptoService cryptoService)
        {
            _secret = secret;
            _cryptoService = cryptoService;
        }

        public async Task<AddSecretResponse> Execute(AddSecretRequest addSecretRequest)
        {
            var encoded = _cryptoService.Encrypt(addSecretRequest.Password, addSecretRequest.EncryptionKey, addSecretRequest.Salt);
            var secret = new Secret(Guid.NewGuid().ToString(), addSecretRequest.Category, addSecretRequest.Title, addSecretRequest.Notes, addSecretRequest.Login, encoded, addSecretRequest.Url);
            var result = await _secret.AddSecret(secret);

            return new AddSecretResponse
            {
                IsSuccess = result
            };
        }
    }
}
