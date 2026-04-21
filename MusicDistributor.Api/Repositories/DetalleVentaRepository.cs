using Dapper;
using MusicDistributor.Api.DataAccess.Interfaces;
using MusicDistributor.Api.Repositories.Interfaces;
using MusicDistributor.Core.Entities;

namespace MusicDistributor.Api.Repositories
{
    public class DetalleVentaRepository : IDetalleVentaRepository
    {
        private readonly IDbContext _dbContext;

        public DetalleVentaRepository(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<DetalleVenta>> GetAllAsync()
        {
            using var connection = _dbContext.CreateConnection();
            var query = "SELECT * FROM DetalleVenta WHERE IsDeleted = 0";
            return await connection.QueryAsync<DetalleVenta>(query);
        }

        public async Task<DetalleVenta?> GetByIdAsync(int id)
        {
            using var connection = _dbContext.CreateConnection();
            var query = "SELECT * FROM DetalleVenta WHERE Id = @Id AND IsDeleted = 0";
            return await connection.QuerySingleOrDefaultAsync<DetalleVenta>(query, new { Id = id });
        }

        public async Task<IEnumerable<DetalleVenta>> GetByVentaIdAsync(int ventaId)
        {
            using var connection = _dbContext.CreateConnection();
            var query = "SELECT * FROM DetalleVenta WHERE VentaId = @VentaId AND IsDeleted = 0";
            return await connection.QueryAsync<DetalleVenta>(query, new { VentaId = ventaId });
        }

        public async Task<int> AddAsync(DetalleVenta detalle)
        {
            using var connection = _dbContext.CreateConnection();
            var query = @"
                INSERT INTO DetalleVenta (VentaId, InventarioId, Cantidad, PrecioUnitario, CreatedBy, CreatedDate, IsDeleted) 
                VALUES (@VentaId, @InventarioId, @Cantidad, @PrecioUnitario, @CreatedBy, NOW(), 0);
                SELECT LAST_INSERT_ID();";
            
            var newId = await connection.ExecuteScalarAsync<ulong>(query, detalle);
            return Convert.ToInt32(newId);
        }

        public async Task<bool> UpdateAsync(DetalleVenta detalle)
        {
            using var connection = _dbContext.CreateConnection();
            var query = @"
                UPDATE DetalleVenta 
                SET VentaId = @VentaId, 
                    InventarioId = @InventarioId, 
                    Cantidad = @Cantidad, 
                    PrecioUnitario = @PrecioUnitario,
                    UpdatedBy = @UpdatedBy, 
                    UpdatedDate = NOW() 
                WHERE Id = @Id AND IsDeleted = 0";
            var result = await connection.ExecuteAsync(query, detalle);
            return result > 0;
        }

        public async Task<bool> DeleteAsync(int id, string deletedBy)
        {
            using var connection = _dbContext.CreateConnection();
            var query = @"
                UPDATE DetalleVenta 
                SET IsDeleted = 1, 
                    UpdatedBy = @DeletedBy, 
                    UpdatedDate = NOW() 
                WHERE Id = @Id AND IsDeleted = 0";
            var result = await connection.ExecuteAsync(query, new { Id = id, DeletedBy = deletedBy });
            return result > 0;
        }
    }
}