using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Security.Claims;
using System.Security.Principal;
using WebAppApi.Data;
using WebAppApi.Models;

namespace WebAppApi.Authorization
{
    public class PermissionBasedAuthorizationFilter(AppDbContext dbContext) : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var attribute = (CheckPermissionAttribute)context.ActionDescriptor.EndpointMetadata.SingleOrDefault(x => x is CheckPermissionAttribute);
            if (attribute != null)
            {

                var claimIdentity = context.HttpContext.User.Identity as ClaimsIdentity;
                if (claimIdentity == null || !claimIdentity.IsAuthenticated)
                {
                    context.Result = new ForbidResult();
                }
                else
                {
                    var userId = int.Parse(claimIdentity.FindFirst(ClaimTypes.NameIdentifier).Value);
                    var hasPermission = dbContext.Set<UserPermission>().Any(x => x.UserId == userId &&
                    x.PermissionId == attribute.Permission);
                    if(!hasPermission)
                    {
                        context.Result = new ForbidResult();
                    }
                }
            }
        }

      
    }
}
