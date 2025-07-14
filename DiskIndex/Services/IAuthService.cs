namespace DiskIndex.Services
{
    public interface IAuthService
    {
        Task<bool> ValidateCredentialsAsync(string username, string password);
        Task<string> CreateUserAsync(string username, string role);
        Task<bool> TokenVerificationAndPasswordChange(string username, string token, string newPassword);
    }
}
