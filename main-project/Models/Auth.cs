using System.ComponentModel;

namespace main_project.Model
{
    public class LoginRequest
    {
        [DefaultValue("user@gmail.com")]
        public string? Email { get; set; }

        [DefaultValue("passwords1233")]
        public string? Password { get; set; }
    }

    public class RegistrationRequest
    {
        public Name? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
    }

    public class AuthResponse
    {
        public long Id { get; set; }
        public Name? Name { get; set; }
        public string? Email { get; set; }
        public string? Token { get; set; }

        public AuthResponse(User user, string? token)
        {
            Id = user.Id;
            Name = user.Name;
            Email = user.Email;
            Token = token;
        }
    }
}
