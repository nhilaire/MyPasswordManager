using Microsoft.AspNetCore.Authorization;

namespace MyPasswordManager
{
    public static class Constants
    {
        public static class Session
        {
            public const string UniqueId = "UniqueId";
        }

        public static class Policies
        {
            public const string HasAccessPasswordList = "HasAccessPasswordList";

            public static AuthorizationPolicy HasAccessPasswordListPolicy()
            {
                return new AuthorizationPolicyBuilder().
                    RequireAuthenticatedUser()
                    .RequireAssertion(context => context.User.HasClaim(claim => claim.Type.Equals(HasAccessPasswordList) && claim.Value.Equals("true"))).Build();
            }
        }
    }
}