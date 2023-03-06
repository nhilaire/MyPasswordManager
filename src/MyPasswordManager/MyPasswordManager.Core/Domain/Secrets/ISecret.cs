using MyPasswordManager.Core.Domain.Login;

namespace MyPasswordManager.Core.Domain.Secrets
{
    public interface ISecret
    {
        bool Authenticate(LoginInfos loginInfos);
        Task<bool> AddSecret(Secret secret);
        Task Delete(string secretId);
        Task<IReadOnlyCollection<Secret>> GetAllSecrets();
        Task<bool> UpdateSecret(Secret secret);
    }
}
