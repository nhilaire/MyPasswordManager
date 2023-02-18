using MyPasswordManager.Core.Domain.Secrets;

namespace MyPasswordManager.Core.UseCases.DeleteSecret
{
    public class DeleteSecretUseCase
    {
        private readonly ISecret _secret;

        public DeleteSecretUseCase(ISecret secret)
        {
            _secret = secret;
        }

        public async Task<DeleteSecretResponse> Execute(DeleteSecretRequest deleteSecretRequest)
        {
            await _secret.Delete(deleteSecretRequest.Id);
            return new DeleteSecretResponse
            {
                IsSuccess = true
            };
        }
    }
}
