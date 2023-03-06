using Microsoft.AspNetCore.Components.Authorization;
using MyPasswordManager.Authentication;

namespace MyPasswordManager
{
    public interface IUserContext
    {
        Task<bool> IsAuthenticated();

        Task<LoginViewModel?> GetCurrentSecrets();
    }

    public class UserContext : IUserContext
    {
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly SessionContext _sessionContext;

        public UserContext(AuthenticationStateProvider authenticationStateProvider, SessionContext sessionContext)
        {
            _authenticationStateProvider = authenticationStateProvider;
            _sessionContext = sessionContext;
        }

        public async Task<bool> IsAuthenticated()
        {
            var result = await _authenticationStateProvider.GetAuthenticationStateAsync();
            if (result.User == null || !result.User.Claims.Any(x =>
                x.Type.Equals(Constants.Policies.HasAccessPasswordList)
                && x.Value.Equals("true")))
            {
                return false;
            }
            var uniqueId = result.User.Claims.FirstOrDefault(x => x.Type.Equals(Constants.Session.UniqueId))?.Value;
            if (string.IsNullOrWhiteSpace(uniqueId) || _sessionContext.Get(uniqueId) == null)
            {
                return false;
            }
            return true;
        }

        public async Task<LoginViewModel?> GetCurrentSecrets()
        {
            var result = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var uniqueId = result.User.Claims.First(x => x.Type.Equals(Constants.Session.UniqueId)).Value;
            return _sessionContext.Get(uniqueId);
        }
    }
}
