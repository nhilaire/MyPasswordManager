using MyPasswordManager.Core.Domain.Login;
using MyPasswordManager.Core.Domain.Secrets;

namespace MyPasswordManager.Core.UseCases.Login
{
    public class LoginUseCase
    {
        private readonly ISecret _authenticate;

        public LoginUseCase(ISecret authenticate)
        {
            _authenticate = authenticate;
        }

        public UserLoginResponse Execute(UserRequest userRequest)
        {
            return new UserLoginResponse
            {
                IsSuccess = _authenticate.Authenticate(new LoginInfos(userRequest.Login, userRequest.Password))
            };
        }
    }
}
