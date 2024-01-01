using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.CustomAttributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Identity.Client;
using System.Reflection;

namespace ECommerceAPI.API.Filters
{
    public class RolePermissionFilter : IAsyncActionFilter
    {
        readonly IUserService _userService;

        public RolePermissionFilter(IUserService userService)
        {
            _userService = userService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var name = context.HttpContext.User.Identity?.Name;
            if (!string.IsNullOrEmpty(name) && name != "hasanhelvali")//Sadece bu kullanıcı adına sahip olan kullanıcıyı admin kabul ediyorum.
                                                                      //Burad agomulu bir cozum kullandım. Baska senaryolarda kullanılabilirdi.
            {
                var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
                var attribute = descriptor.MethodInfo.GetCustomAttribute(typeof(AuthorizeDefinitionAttribute)) as AuthorizeDefinitionAttribute;

                var httpAttribute = descriptor.MethodInfo.GetCustomAttribute(typeof(HttpMethodAttribute)) as HttpMethodAttribute;

                var code = $"{(httpAttribute != null ? httpAttribute.HttpMethods.First() : HttpMethods.Get)}.{attribute.ActionType}" +
                    $".{attribute.Definition.Replace(" ", "")}";

                var hasRole = await _userService.HasRolePermissionToEndpointAsync(name, code);
                if (!hasRole)
                    context.Result = new UnauthorizedResult();
                else
                    await next();
            }
            else
                await next();
        }
    }
}
//Burada filter i pipeline a ekliyoruz.