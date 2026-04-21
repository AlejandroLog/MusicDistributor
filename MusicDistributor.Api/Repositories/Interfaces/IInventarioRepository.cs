using MusicDistributor.Core.Entities;

namespace MusicDistributor.Api.Repositories.Interfaces
{
    public interface IInventarioRepository
    {
        Task<IEnumerable<Inventario>> GetAllAsync();
        Task<Inventario?> GetByIdAsync(int id);
        Task<IEnumerable<Inventario>> GetByLanzamientoIdAsync(int lanzamientoId);
        Task<IEnumerable<Inventario>> GetByFormatoIdAsync(int formatoFisicoId);
        Task<int> AddAsync(Inventario inventario);
        Task<bool> UpdateAsync(Inventario inventario);
        Task<bool> DeleteAsync(int id, string deletedBy);
    }
}