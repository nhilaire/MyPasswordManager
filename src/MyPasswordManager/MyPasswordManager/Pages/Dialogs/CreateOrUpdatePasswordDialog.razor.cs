using Microsoft.AspNetCore.Components;
using MudBlazor;
using MyPasswordManager.Core.UseCases.AddNewSecret;
using MyPasswordManager.Core.UseCases.GeneratePassword;
using MyPasswordManager.Core.UseCases.UpdateSecret;

namespace MyPasswordManager.Pages.Dialogs
{
    public partial class CreateOrUpdatePasswordDialog
    {
        private const int GeneratePasswordRequestLenght = 20;

        [Parameter] public SecretViewModel SecretViewModel { get; set; } = new SecretViewModel();
        [Parameter] public bool? UpdateSecret { get; set; }
        [CascadingParameter] MudDialogInstance? MudDialog { get; set; }

        [Inject]
        public AddNewSecretUseCase AddNewSecretUseCase { get; set; }

        [Inject]
        public UpdateSecretUseCase UpdateSecretUseCase { get; set; }

        [Inject]
        public GeneratePasswordUseCase GeneratePasswordUseCase { get; set; }

        [Inject]
        public IUserContext UserContext { get; set; }

        [Inject]
        public ISnackbar Snackbar { get; set; }

        protected override Task OnInitializedAsync()
        {
            if (!Updating())
            {
                SecretViewModel = new SecretViewModel();
            }
            return base.OnInitializedAsync();
        }

        async Task Submit()
        {
            var secrets = await UserContext.GetCurrentSecrets();

            if (Updating())
            {
                var result = await UpdateSecretUseCase.Execute(new UpdateSecretRequest
                {
                    Id = SecretViewModel.Id,
                    Category = SecretViewModel.Category,
                    Title = SecretViewModel.Title,
                    Notes = SecretViewModel.Notes,
                    Login = SecretViewModel.Login,
                    Password = SecretViewModel.Password,
                    Url = SecretViewModel.Url,
                    EncryptionKey = secrets.Password,
                    Salt = secrets.Login
                });
                if (result.IsSuccess)
                {
                    Snackbar.Add("Secret successfully updated", Severity.Success);
                }
                else
                {
                    Snackbar.Add("Error updating secret", Severity.Error);
                }
            }
            else
            {
                var result = await AddNewSecretUseCase.Execute(new AddSecretRequest
                {
                    Category = SecretViewModel.Category,
                    Title = SecretViewModel.Title,
                    Notes = SecretViewModel.Notes,
                    Login = SecretViewModel.Login,
                    Password = SecretViewModel.Password,
                    Url = SecretViewModel.Url,
                    EncryptionKey = secrets.Password,
                    Salt = secrets.Login
                });
                if (result.IsSuccess)
                {
                    Snackbar.Add("Secret successfully added", Severity.Success);
                }
                else
                {
                    Snackbar.Add("Error adding secret", Severity.Error);
                }
            }

            MudDialog.Close(DialogResult.Ok(true));
        }

        void Cancel() => MudDialog.Cancel();

        void OnGeneratePassword()
        {
            var result = GeneratePasswordUseCase.Execute(new GeneratePasswordRequest
            {
                Length = GeneratePasswordRequestLenght
            });
            if (result.IsSuccess)
            {
                SecretViewModel.Password = result.Password;
            }
        }

        private bool Updating()
        {
            return UpdateSecret.GetValueOrDefault(false);
        }
    }
}
