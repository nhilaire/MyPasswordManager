using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MyPasswordManager.Core.UseCases.Login;

namespace MyPasswordManager.Authentication
{
    public class AuthenticationController : Controller
    {
        private readonly LoginUseCase _loginUseCase;
        private readonly SessionContext _sessionContext;

        public AuthenticationController(LoginUseCase loginUseCase, SessionContext sessionContext)
        {
            _loginUseCase = loginUseCase;
            _sessionContext = sessionContext;
        }

        [HttpPost]
        [Route("auth/signin")]
        public async Task<ActionResult> Authenticate(LoginViewModel value)
        {
            var result = _loginUseCase.Execute(new UserRequest { Login = value.Login, Password = value.Password });
            if (!result.IsSuccess)
            {
                return Redirect("/?error=true");
            }

            var uniqueId = Guid.NewGuid().ToString("N");

            var claims = new List<Claim>
            {
                new Claim(Constants.Policies.HasAccessPasswordList, "true"),
                new Claim(Constants.Session.UniqueId, uniqueId),
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties { };

            _sessionContext.Add(uniqueId, value);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                          new ClaimsPrincipal(claimsIdentity),
                                          authProperties);

            return Redirect("/list");
        }
    }
}
