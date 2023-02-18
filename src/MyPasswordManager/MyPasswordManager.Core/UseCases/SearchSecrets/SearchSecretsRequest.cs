namespace MyPasswordManager.Core.UseCases.SearchSecrets
{
    public class SearchSecretsRequest
    {
        public required string Query { get; set; }
        public required string EncryptionKey { get; set; }
        public required string Salt { get; set; }
    }
}
