using DataAnalysisSystem.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace DataAnalysisSystem.Extensions
{
    public static class UrlExtensionUtility
    {
        public static string GenerateEmailConfirmationLink(this IUrlHelper urlHelper, string userIdentificator, string authorizationToken, string scheme)
        {
            return urlHelper.Action(
                action: nameof(UserController.ConfirmEmailAddress),
                controller: "User",
                values: new { userIdentificator, authorizationToken },
                protocol: scheme);
        }

        public static string GenerateResetForgottenPasswordLink(this IUrlHelper urlHelper, string userIdentificator, string authorizationToken, string scheme)
        {
            return urlHelper.Action(
                action: nameof(UserController.ChangeForgottenPassword),
                controller: "User",
                values: new { userIdentificator, authorizationToken },
                protocol: scheme);
        }
    }
}
