namespace MyPasswordManager.Core.Domain.Login
{
    public interface IAuthenticate
    {
        bool Authenticate(LoginInfos loginInfos);
    }
}
