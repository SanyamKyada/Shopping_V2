using Shopping.Models.DTO;

namespace Shopping.Repositories.Infrastructure
{
    public interface IUserAuthenticationService
    {
        Task<Status> LoginAsync(LoginModel model);
        Task LogoutAsync();
        Task<Status> RegisterAsync(RegistrationModel model);

        // Forget Password ?

        // Change Password
    }
}
