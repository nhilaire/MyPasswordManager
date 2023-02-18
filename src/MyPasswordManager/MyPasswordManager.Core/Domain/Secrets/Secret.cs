namespace MyPasswordManager.Core.Domain.Secrets
{
    public record Secret(string Id, string Category, string Title, string Notes, string Login, string Password, string Url);
}
