using MusicDistributor.Core.Entities;

namespace MusicDistributor.Api.Repositories.Interfaces
{
    public interface IPistaRepository
    {
        Task<IEnumerable<Pista>> GetAllAsync();
        Task<Pista?> GetByIdAsync(int id);
        Task<IEnumerable<Pista>> GetByLanzamientoIdAsync(int lanzamientoId);
        Task<int> AddAsync(Pista pista);
        Task<bool> UpdateAsync(Pista pista);
        Task<bool> DeleteAsync(int id, string deletedBy);
    }
}