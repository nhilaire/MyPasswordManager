namespace MyPasswordManager.Core.UseCases.GetAllSecrets
{
    public class GetAllSecretsResponse
    {
        public bool IsSuccess { get; set; }
        public List<SecretResponse> SecretResponses { get; set; } = new List<SecretResponse>();
    }
}
