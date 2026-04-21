using MusicDistributor.Core.Entities;

namespace MusicDistributor.Api.Repositories.Interfaces
{
    public interface IFormatoFisicoRepository
    {
        Task<IEnumerable<FormatoFisico>> GetAllAsync();
        Task<FormatoFisico?> GetByIdAsync(int id);
        Task<int> AddAsync(FormatoFisico formato);
        Task<bool> UpdateAsync(FormatoFisico formato);
        Task<bool> DeleteAsync(int id, string deletedBy);
    }
}