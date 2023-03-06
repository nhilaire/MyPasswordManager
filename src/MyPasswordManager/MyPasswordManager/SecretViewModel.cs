namespace MyPasswordManager
{
    public class SecretViewModel
    {
        public string? Id { get; init; }
        public string? Category { get; set; }
        public string? Title { get; set; }
        public string? Notes { get; set; }
        public string? Url { get; set; }
        public string? Login { get; set; }
        public string? Password { get; set; }
    }
}
