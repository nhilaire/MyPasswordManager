using Microsoft.AspNetCore.Components;
using MudBlazor;
using MyPasswordManager.Core.UseCases.DeleteSecret;
using MyPasswordManager.Core.UseCases.GetAllSecrets;
using MyPasswordManager.Core.UseCases.SearchSecrets;
using MyPasswordManager.Pages.Dialogs;

namespace MyPasswordManager.Pages
{
    public partial class PasswordList
    {
        [Inject]
        public IDialogService Dialog { get; set; }

        [Inject]
        public GetAllSecretsUseCase GetAllSecretsUseCase { get; set; }

        [Inject]
        public DeleteSecretUseCase DeleteSecretUseCase { get; set; }

        [Inject]
        public SearchSecretsUseCase SearchSecretsUseCase { get; set; }

        [Inject]
        public IUserContext UserContext { get; set; }

        [Inject]
        public ISnackbar Snackbar { get; set; }

        public List<SecretViewModel> AllSecrets { get; set; } = new List<SecretViewModel>();

        public string SearchString { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await LoadDatas();
                StateHasChanged();
            }
        }

        private async Task LoadDatas()
        {
            var values = await UserContext.GetCurrentSecrets();
            var response = await GetAllSecretsUseCase.Execute(new GetAllSecretsRequest
            {
                EncryptionKey = values.Password,
                Salt = values.Login
            });

            if (response.IsSuccess)
            {
                AllSecrets = ToViewModel(response.SecretResponses);
            }
        }

        private async Task OnAddNewPassword()
        {
            await CreateOrEditPassword(null);
        }

        private async Task OnEditPassword(SecretViewModel secretViewModel)
        {
            await CreateOrEditPassword(secretViewModel);
        }

        private async Task CreateOrEditPassword(SecretViewModel secretViewModel)
        {
            var parameters = new DialogParameters();
            if (secretViewModel != null)
            {
                parameters.Add(nameof(CreateOrUpdatePasswordDialog.SecretViewModel), secretViewModel);
                parameters.Add(nameof(CreateOrUpdatePasswordDialog.UpdateSecret), true);
            }
            var options = new DialogOptions { CloseOnEscapeKey = true, FullWidth = true };
            var dialog = Dialog.Show<CreateOrUpdatePasswordDialog>("Update password", parameters, options);
            var result = (bool?)(await dialog.Result).Data;
            if (result.HasValue)
            {
                await LoadDatas();
            }
        }

        private async Task OnDeletePassword(string id)
        {
            var response = await DeleteSecretUseCase.Execute(new DeleteSecretRequest
            {
                Id = id
            });
            if (response.IsSuccess)
            {
                Snackbar.Add("Secret successfully deleted", Severity.Success);
                await LoadDatas();
            }
            else
            {
                Snackbar.Add("Error deleting secret", Severity.Error);
            }
        }

        private async Task OnSearch()
        {
            var values = await UserContext.GetCurrentSecrets();
            var response = await SearchSecretsUseCase.Execute(new SearchSecretsRequest
            {
                Query = SearchString,
                EncryptionKey = values.Password,
                Salt = values.Login
            });

            if (response.IsSuccess)
            {
                AllSecrets = ToViewModel(response.SecretResponses);
            }
        }

        private static List<SecretViewModel> ToViewModel(List<SecretResponse> secretResponses)
        {
            return secretResponses.Select(x => new SecretViewModel
            {
                Category = x.Category,
                Id = x.Id,
                Login = x.Login,
                Notes = x.Notes,
                Password = x.DecodedPassword,
                Title = x.Title,
                Url = x.Url
            }).ToList();
        }
    }
}
