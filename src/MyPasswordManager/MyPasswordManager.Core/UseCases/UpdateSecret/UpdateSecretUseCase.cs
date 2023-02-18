using MyPasswordManager.Core.ApplicationServices;
using MyPasswordManager.Core.Domain.Secrets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPasswordManager.Core.UseCases.UpdateSecret
{
    public class UpdateSecretUseCase
    {
        private readonly ISecret _secret;
        private readonly CryptoService _cryptoService;

        public UpdateSecretUseCase(ISecret secret, CryptoService cryptoService)
        {
            _secret = secret;
            _cryptoService = cryptoService;
        }

        public async Task<UpdateSecretResponse> Execute(UpdateSecretRequest updateSecretRequest)
        {
            var encodedPassword = _cryptoService.Encrypt(updateSecretRequest.Password, updateSecretRequest.EncryptionKey, updateSecretRequest.Salt);

            var result = await _secret.UpdateSecret(new Secret(updateSecretRequest.Id, 
                updateSecretRequest.Category, 
                updateSecretRequest.Title, 
                updateSecretRequest.Notes,
                updateSecretRequest.Login,
                encodedPassword,
                updateSecretRequest.Url));

            return new UpdateSecretResponse
            {
                IsSuccess = result
            };
        }
    }
}
