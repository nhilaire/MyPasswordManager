namespace MyPasswordManager.Core.UseCases.GeneratePassword
{
    public class GeneratePasswordUseCase
    {
        private static readonly Random _random = new Random();
        private const string AllowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789@#$%!";

        public GeneratePasswordResponse Execute(GeneratePasswordRequest generatePasswordRequest)
        {
            var password = new string(Enumerable.Repeat(AllowedChars, generatePasswordRequest.Length)
              .Select(s => s[_random.Next(s.Length)]).ToArray());

            return new GeneratePasswordResponse
            {
                IsSuccess = true,
                Password = password
            };
        }
    }
}
