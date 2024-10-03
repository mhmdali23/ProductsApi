using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace WebAppApi.Authorization
{
    public class AgeAuthorizationHanlder : AuthorizationHandler<AgeGraterThan25Requirment>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AgeGraterThan25Requirment requirement)
        {
            var dt = DateTime.Parse(context.User.FindFirstValue("DateOfBirth"));
                if (DateTime.Today.Year - dt.Year >= 25)
                context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
