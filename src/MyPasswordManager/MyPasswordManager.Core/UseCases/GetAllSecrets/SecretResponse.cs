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
            return Category.Contains(query, StringComparison.OrdinalIgnoreCase)
                || Title.Contains(query, StringComparison.OrdinalIgnoreCase)
                || Notes.Contains(query, StringComparison.OrdinalIgnoreCase)
                || Login.Contains(query, StringComparison.OrdinalIgnoreCase)
                || Url.Contains(query, StringComparison.OrdinalIgnoreCase);
        }
    }
}
