using MusicDistributor.Core.Entities;

namespace MusicDistributor.Api.Repositories.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<IEnumerable<Usuario>> GetAllAsync();
        Task<Usuario?> GetByIdAsync(int id);
        Task<int> AddAsync(Usuario usuario);
        Task<bool> UpdateAsync(Usuario usuario);
        Task<bool> DeleteAsync(int id, string deletedBy);
        Task<Usuario?> ValidateUserAsync(string username, string passwordHash); 
    }
}