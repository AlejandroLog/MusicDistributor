using MusicDistributor.Core.Entities;

namespace MusicDistributor.Api.Repositories.Interfaces
{
    public interface IGeneroMusicalRepository
    {
        Task<IEnumerable<GeneroMusical>> GetAllAsync();
        Task<GeneroMusical?> GetByIdAsync(int id);
        Task<int> AddAsync(GeneroMusical genero);
        Task<bool> UpdateAsync(GeneroMusical genero);
        Task<bool> DeleteAsync(int id, string deletedBy);
    }
}