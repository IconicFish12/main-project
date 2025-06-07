using main_project.Config;
using main_project.Interfaces;
using main_project.Model;
using main_project.Services.Helper;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace main_project.Services
{
    public class UserService : ICrudService<User>
    {
        private readonly string _userDataPath = "user_data.json";

        public List<User> GetData()
        {
            return FileHelper.ReadJson<User>(_userDataPath);
        }

        public async Task<User?> FindData(string id)
        {
            var users = GetData();
            if (long.TryParse(id, out var userId))
            {
                var user = users.FirstOrDefault(u => u.Id == userId);
                return await Task.FromResult(user);
            }
            return await Task.FromResult<User?>(null);
        }

        public async Task<User?> CreateData(User user)
        {
            var users = GetData();

            if (user.Id == 0)
            {
                user.Id = users.Any() ? users.Max(u => u.Id) + 1 : 1;
            }

            users.Add(user);
            FileHelper.WriteJson(users, _userDataPath);
            return await Task.FromResult(user);
        }

        public async Task<User?> UpdateData(string id, User updatedUser)
        {
            var users = GetData();

            if (!long.TryParse(id, out var userId))
                return null;

            var user = users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
                return null;

            user.Name = updatedUser.Name ?? user.Name;
            user.Email = updatedUser.Email ?? user.Email;
            user.Password = updatedUser.Password ?? user.Password;
            user.EmailVerif = updatedUser.EmailVerif;

            FileHelper.WriteJson(users, _userDataPath);
            return await Task.FromResult(user);
        }

        public async Task<bool> DeleteData(string id)
        {
            var users = GetData();

            if (!long.TryParse(id, out var userId))
                return false;

            var user = users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
                return false;

            users.Remove(user);
            FileHelper.WriteJson(users, _userDataPath);
            return await Task.FromResult(true);
        }
    }
}
