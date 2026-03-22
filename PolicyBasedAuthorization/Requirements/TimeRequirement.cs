using Microsoft.AspNetCore.Authorization;

namespace PolicyBasedAuthorization.Requirements;

public class TimeRequirement : IAuthorizationRequirement
{
    //IAuthorizationRequirement: Politika temelli yetkilendirmenin başarılı olup olmadığını izler
}
