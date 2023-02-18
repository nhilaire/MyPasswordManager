using MyPasswordManager.Core.UseCases.GetAllSecrets;

namespace MyPasswordManager.Core.UseCases.SearchSecrets
{
    public class SearchSecretsResponse
    {
        public bool IsSuccess { get; set; }
        public List<SecretResponse> SecretResponses { get; set; } = new List<SecretResponse>();
    }
}
