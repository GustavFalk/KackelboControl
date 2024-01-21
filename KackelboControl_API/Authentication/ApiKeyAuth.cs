using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using KackelboControl_API.Authentication.Helpers;

namespace KackelboControl_API.Authentication;

public class ApiKeyAuth : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue(AuthProperties.ApiKeyHeaderName, out var extractedApiKey))
        {
            context.Result = new UnauthorizedObjectResult("API Key missing");
            return;
        }

        var key = extractedApiKey.ToString();
        var keyHash = Hasher.GetHash(key);

        if (!AuthProperties.Keys.Any(k=>k == keyHash))
        {
            context.Result = new UnauthorizedObjectResult("Invalid API key");
            return;
        }
    }
}
