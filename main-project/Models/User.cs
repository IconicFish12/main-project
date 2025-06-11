using System.Text.Json.Serialization;

namespace main_project.Model
{
    public class User
    {
        public long Id { get; set; }
        public Name? Name { get; set; }
        public string? Email { get; set; }
        public string? LoginToken { get; set; } = null;

        [JsonIgnore]
        public string? Password { get; set; }
        public bool EmailVerif { get; set; }

    }

    public class Name
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Username { get; set; }
    }
}
