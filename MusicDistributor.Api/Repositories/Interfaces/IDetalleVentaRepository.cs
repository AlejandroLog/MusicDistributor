using MusicDistributor.Core.Entities;

namespace MusicDistributor.Api.Repositories.Interfaces
{
    public interface IDetalleVentaRepository
    {
        Task<IEnumerable<DetalleVenta>> GetAllAsync();
        Task<DetalleVenta?> GetByIdAsync(int id);
        Task<IEnumerable<DetalleVenta>> GetByVentaIdAsync(int ventaId);
        Task<int> AddAsync(DetalleVenta detalle);
        Task<bool> UpdateAsync(DetalleVenta detalle);
        Task<bool> DeleteAsync(int id, string deletedBy);
    }
}