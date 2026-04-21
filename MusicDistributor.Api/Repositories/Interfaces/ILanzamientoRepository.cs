using MusicDistributor.Core.Entities;

namespace MusicDistributor.Api.Repositories.Interfaces
{
    public interface ILanzamientoRepository
    {
        Task<IEnumerable<Lanzamiento>> GetAllAsync();
        Task<Lanzamiento?> GetByIdAsync(int id);
        Task<int> AddAsync(Lanzamiento lanzamiento);
        Task<bool> UpdateAsync(Lanzamiento lanzamiento);
        Task<bool> DeleteAsync(int id, string deletedBy);
    }
}