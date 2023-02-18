using MyPasswordManager.Core.Domain.Login;

namespace MyPasswordManager.Tests.LoginDoubles
{
    public class FakeAuthenticate : IAuthenticate
    {
        public bool Authenticate(LoginInfos loginInfos)
        {
            var badUserRequest = new BadUserRequest();
            if (loginInfos.Login == badUserRequest.Login && loginInfos.Password == badUserRequest.Password)
            {
                return false;
            }
            return true;
        }
    }
}
