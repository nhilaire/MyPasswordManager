using System.Collections.Concurrent;

namespace MyPasswordManager.Authentication
{
    public class SessionContext
    {
        private ConcurrentDictionary<string, LoginViewModel> _sessions = new();

        public void Add(string sessionId, LoginViewModel loginViewModel)
        {
            _sessions.TryAdd(sessionId, loginViewModel);
        }

        public LoginViewModel? Get(string sessionId)
        {
            if (_sessions.TryGetValue(sessionId, out var loginViewModel))
                return loginViewModel;
            return null;
        }
    }
}
