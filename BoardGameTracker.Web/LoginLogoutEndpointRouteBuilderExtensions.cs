using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Threading.Tasks;

namespace BoardGameTracker.Web
{
    internal static class LoginLogoutEndpointRouteBuilderExtensions
    {
        internal static IEndpointConventionBuilder MapLoginAndLogout(
            this IEndpointRouteBuilder endpoints)
        {
            var group = endpoints.MapGroup("authentication");

            group.MapGet(pattern: "/login", OnLogin).AllowAnonymous();
            group.MapGet(pattern: "/logout", OnLogout);

            return group;
        }

        static ChallengeHttpResult OnLogin() =>
            TypedResults.Challenge(properties: new AuthenticationProperties
            {
                RedirectUri = "/"
            });

        static SignOutHttpResult OnLogout() =>
            TypedResults.SignOut(properties: new AuthenticationProperties
            {
                RedirectUri = "/"
            },
            [
                CookieAuthenticationDefaults.AuthenticationScheme,
                OpenIdConnectDefaults.AuthenticationScheme
            ]);
    }
}
