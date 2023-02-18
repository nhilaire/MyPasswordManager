namespace MyPasswordManager.Core.UseCases.GeneratePassword
{
    public class GeneratePasswordResponse
    {
        public bool IsSuccess { get; set; }
        public required string Password { get; set; }
    }
}
