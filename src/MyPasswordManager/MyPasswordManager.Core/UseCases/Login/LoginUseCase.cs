using MyPasswordManager.Core.Domain.Login;

namespace MyPasswordManager.Core.UseCases.Login
{
    public class LoginUseCase
    {
        private readonly IAuthenticate _authenticate;

        public LoginUseCase(IAuthenticate authenticate)
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
