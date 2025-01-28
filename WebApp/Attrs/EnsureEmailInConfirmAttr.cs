using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApp.Attrs;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class EnsureEmailIsConfirmAttr : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var emailIsConfirm = GetEmailIsConfirmClaimValue(context.HttpContext.User);
        if (!emailIsConfirm)
        {
            context.Result = new ForbidResult();
        }
    }

    private static bool GetEmailIsConfirmClaimValue(ClaimsPrincipal user)
    {
        var subClaim = user.Claims.FirstOrDefault(x => x.Type == "EmailIsConfirm") 
                       ?? throw new Exception("Not valid JWT: name identifier not provide");

        var result = bool.TryParse(subClaim.Value, out var emailIsConfirm);
        if (!result)
        {
            throw new Exception("Not valid JWT: incorrect 'EmailIsConfirm' format");
        }

        return emailIsConfirm;
    }
}
