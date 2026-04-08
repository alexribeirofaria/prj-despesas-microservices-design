namespace AuthService.Services.Interfaces;

public interface IHashService
{
    string Compute(string input);
}