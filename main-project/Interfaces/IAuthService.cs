using main_project.Model;

namespace main_project.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse?> Authenticate(LoginRequest model);

    }
}
