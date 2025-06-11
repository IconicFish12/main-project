using main_project.Model;

namespace main_project.Interfaces
{
    public interface ICrudService<T>
    {
        List<T> GetData();
        Task<T?> FindData(string id);
        Task<T?> CreateData(User user);
        Task<T?> UpdateData(string id, User updatedUser);
        Task<bool> DeleteData(string id);
    }
}
