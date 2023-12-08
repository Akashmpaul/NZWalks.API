using Microsoft.AspNetCore.Identity;

namespace NZWalks.API.Repositories
{
    public interface ITokenRepository
    {
        string CreateJwtToken(IdentityUser identityUser, List<string> roles);

    }
}
