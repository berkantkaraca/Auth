using Microsoft.AspNetCore.Authorization;

namespace PolicyBasedAuthorization.Requirements;

public class TimeHandler : AuthorizationHandler<TimeRequirement>
{
    //IAuthorizationHandler: Belirlenen politika şartlarının karşılanıp karşılanmadığını kontrol eden arayüzdür.

    //Bulunulan saatin dakikası 40 ile 50 arasında ise ilgili sayfaya erişim engellensin.
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, TimeRequirement requirement)
    {
        if (DateTime.Now.Minute >= 40 && DateTime.Now.Minute < 50)
            context.Succeed(requirement);
        else
            context.Fail();

        return Task.CompletedTask;
    }
}
