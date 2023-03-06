using Microsoft.AspNetCore.Authentication.Cookies;
using MudBlazor;
using MudBlazor.Services;
using MyPasswordManager;
using MyPasswordManager.Authentication;
using MyPasswordManager.Core.ApplicationServices;
using MyPasswordManager.Core.UseCases.AddNewSecret;
using MyPasswordManager.Core.UseCases.DeleteSecret;
using MyPasswordManager.Core.UseCases.GeneratePassword;
using MyPasswordManager.Core.UseCases.GetAllSecrets;
using MyPasswordManager.Core.UseCases.Login;
using MyPasswordManager.Core.UseCases.SearchSecrets;
using MyPasswordManager.Core.UseCases.UpdateSecret;
using MyPasswordManager.Infrastructure.CosmosDb;
using MyPasswordManager.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

var cosmosDbConfiguration = new CosmosDbConfiguration();
var config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
config.GetSection("CosmosDbConfiguration").Bind(cosmosDbConfiguration);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromDays(20);
});
builder.Services.AddAuthorizationCore((Action<Microsoft.AspNetCore.Authorization.AuthorizationOptions>)(options =>
{
    options.AddPolicy(Constants.Policies.HasAccessPasswordList, Constants.Policies.HasAccessPasswordListPolicy());
}));

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopRight;
    config.SnackbarConfiguration.PreventDuplicates = false;
    config.SnackbarConfiguration.NewestOnTop = false;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 10000;
    config.SnackbarConfiguration.HideTransitionDuration = 500;
    config.SnackbarConfiguration.ShowTransitionDuration = 500;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
});
builder.Services.AddSingleton(cosmosDbConfiguration);
builder.Services.RegisterCosmosDbRepository();
builder.Services.AddTransient<AllSecretsReader>();
builder.Services.AddTransient<CryptoService>();
builder.Services.AddTransient<AddNewSecretUseCase>();
builder.Services.AddTransient<DeleteSecretUseCase>();
builder.Services.AddTransient<GeneratePasswordUseCase>();
builder.Services.AddTransient<GetAllSecretsUseCase>();
builder.Services.AddTransient<LoginUseCase>();
builder.Services.AddTransient<SearchSecretsUseCase>();
builder.Services.AddTransient<UpdateSecretUseCase>();
builder.Services.AddTransient<IUserContext, UserContext>();
builder.Services.AddSingleton<SessionContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();
app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();