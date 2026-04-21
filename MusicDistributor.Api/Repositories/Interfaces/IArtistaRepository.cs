using MusicDistributor.Core.Entities;

namespace MusicDistributor.Api.Repositories.Interfaces
{
    public interface IArtistaRepository
    {
        Task<IEnumerable<Artista>> GetAllAsync();
        Task<Artista?> GetByIdAsync(int id);
        Task<int> AddAsync(Artista artista);
        Task<bool> UpdateAsync(Artista artista);
        Task<bool> DeleteAsync(int id, string deletedBy);
    }
}