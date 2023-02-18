namespace MyPasswordManager.Core.UseCases.UpdateSecret
{
    public class UpdateSecretRequest
    {
        public required string Id { get; init; }
        public required string Salt { get; init; }
        public required string EncryptionKey { get; init; }
        public required string Category { get; init; }
        public required string Title { get; init; }
        public required string Notes { get; init; }
        public required string Url { get; init; }
        public required string Login { get; init; }
        public required string Password { get; init; }

    }
}
