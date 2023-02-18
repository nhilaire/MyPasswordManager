namespace MyPasswordManager.Core.UseCases.GetAllSecrets
{
    public class GetAllSecretsRequest
    {
        public required string Salt { get; init; }
        public required string EncryptionKey { get; init; }
    }
}
