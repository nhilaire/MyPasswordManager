namespace MyPasswordManager.Core.UseCases.GetAllSecrets
{
    public class SecretResponse
    {
        public required string Id { get; init; }
        public required string Category { get; set; }
        public required string Title { get; set; }
        public required string Notes { get; set; }
        public required string Url { get; set; }
        public required string Login { get; set; }
        public required string DecodedPassword { get; set; }

        public bool MatchQuery(string query)
        {
            return (Category != null && Category.Contains(query, StringComparison.OrdinalIgnoreCase))
                || (Title != null && Title.Contains(query, StringComparison.OrdinalIgnoreCase))
                || (Notes != null && Notes.Contains(query, StringComparison.OrdinalIgnoreCase))
                || (Login != null && Login.Contains(query, StringComparison.OrdinalIgnoreCase))
                || (Url != null && Url.Contains(query, StringComparison.OrdinalIgnoreCase));
        }
    }
}
