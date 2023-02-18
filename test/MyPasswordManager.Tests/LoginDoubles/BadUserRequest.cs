using MyPasswordManager.Core.UseCases.Login;

namespace MyPasswordManager.Tests.LoginDoubles
{
    internal class BadUserRequest : UserRequest
    {
        public BadUserRequest()
        {
            Login = "BadLogin";
            Password = "BadPassword";
        }
    }
}
