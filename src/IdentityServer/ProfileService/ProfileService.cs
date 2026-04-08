using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using STS.ServerData.Interfaces;
using System.Security.Claims;

namespace STS.ServerProfileService;

internal class ProfileService: IProfileService
{
    private readonly IIdentityRepository repository;

    public ProfileService(IIdentityRepository repository) => this.repository = repository;

    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var id = context.Subject.GetSubjectId();
        var user = await this.repository.FindByIdAsync(new Guid(id));

        var claims = new List<Claim>()
        {
            new Claim("UserId", user.Id.ToString()),
            new Claim("email", user.Email),
            new Claim("role", user.UserType.ToString() == "Usuario" ? "User" : user.UserType.ToString() == "Administrador" ? "Admin" : user.UserType.ToString()),
            new Claim("aud", "api-gateway")
        };
        context.IssuedClaims = claims;
    }

    public Task IsActiveAsync(IsActiveContext context)
    {
        context.IsActive = true;
        return Task.CompletedTask;
    }
}
