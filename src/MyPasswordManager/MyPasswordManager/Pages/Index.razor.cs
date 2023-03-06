using Microsoft.AspNetCore.Components;
using MyPasswordManager.Core.UseCases.Login;

namespace MyPasswordManager.Pages
{
    public partial class Index
    {
        [Inject]
        public IUserContext? UserContext { get; set; }

        [Inject]
        public NavigationManager? NavigationManager { get; set; }

        public LoginViewModel LoginViewModel { get; set; }

        public bool IsAuthenticated { get; set; } = false;
        public string ErrorMessage { get; set; } = string.Empty;

        public Index()
        {
            LoginViewModel = new();
        }

        protected override async Task OnInitializedAsync()
        {
            if (NavigationManager.Uri.Contains("?error=true"))
            {
                ErrorMessage = "Access denied !";
            }

            IsAuthenticated = await UserContext.IsAuthenticated();
        }
    }
}
