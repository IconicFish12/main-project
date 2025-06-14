using main_project.Model;

namespace WebFrontend.Repository.Model
{
    public class User
    {
        public long Id { get; set; }
        public Name? Name { get; set; }
        public string? Email { get; set; }
        public string? LoginToken { get; set; } = null;
        public string? Password { get; set; }
        public bool EmailVerif { get; set; }
    }
}
