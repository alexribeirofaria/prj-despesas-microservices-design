using AuthService.Settings;

namespace AuthService.Services.Interfaces;

public interface IConsulRegister
{
    Task RegisterAsync(ServiceSettings settings);
}