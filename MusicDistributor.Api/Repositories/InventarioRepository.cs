using Dapper;
using MusicDistributor.Api.DataAccess.Interfaces;
using MusicDistributor.Api.Repositories.Interfaces;
using MusicDistributor.Core.Entities;

namespace MusicDistributor.Api.Repositories
{
    public class InventarioRepository : IInventarioRepository
    {
        private readonly IDbContext _dbContext;

        public InventarioRepository(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Inventario>> GetAllAsync()
        {
            using var connection = _dbContext.CreateConnection();
            var query = "SELECT * FROM Inventario WHERE IsDeleted = 0";
            return await connection.QueryAsync<Inventario>(query);
        }

        public async Task<Inventario?> GetByIdAsync(int id)
        {
            using var connection = _dbContext.CreateConnection();
            var query = "SELECT * FROM Inventario WHERE Id = @Id AND IsDeleted = 0";
            return await connection.QuerySingleOrDefaultAsync<Inventario>(query, new { Id = id });
        }

        public async Task<IEnumerable<Inventario>> GetByLanzamientoIdAsync(int lanzamientoId)
        {
            using var connection = _dbContext.CreateConnection();
            var query = "SELECT * FROM Inventario WHERE LanzamientoId = @LanzamientoId AND IsDeleted = 0";
            return await connection.QueryAsync<Inventario>(query, new { LanzamientoId = lanzamientoId });
        }

        public async Task<IEnumerable<Inventario>> GetByFormatoIdAsync(int formatoFisicoId)
        {
            using var connection = _dbContext.CreateConnection();
            var query = "SELECT * FROM Inventario WHERE FormatoFisicoId = @FormatoFisicoId AND IsDeleted = 0";
            return await connection.QueryAsync<Inventario>(query, new { FormatoFisicoId = formatoFisicoId });
        }

        public async Task<int> AddAsync(Inventario inventario)
        {
            using var connection = _dbContext.CreateConnection();
            var query = @"
                INSERT INTO Inventario (LanzamientoId, FormatoFisicoId, StockDisponible, PrecioVenta, Sku, CreatedBy, CreatedDate, IsDeleted) 
                VALUES (@LanzamientoId, @FormatoFisicoId, @StockDisponible, @PrecioVenta, @Sku, @CreatedBy, NOW(), 0);
                SELECT LAST_INSERT_ID();";
            
            var newId = await connection.ExecuteScalarAsync<ulong>(query, inventario);
            return Convert.ToInt32(newId);
        }

        public async Task<bool> UpdateAsync(Inventario inventario)
        {
            using var connection = _dbContext.CreateConnection();
            var query = @"
                UPDATE Inventario 
                SET LanzamientoId = @LanzamientoId, 
                    FormatoFisicoId = @FormatoFisicoId, 
                    StockDisponible = @StockDisponible, 
                    PrecioVenta = @PrecioVenta, 
                    Sku = @Sku,
                    UpdatedBy = @UpdatedBy, 
                    UpdatedDate = NOW() 
                WHERE Id = @Id AND IsDeleted = 0";
            var result = await connection.ExecuteAsync(query, inventario);
            return result > 0;
        }

        public async Task<bool> DeleteAsync(int id, string deletedBy)
        {
            using var connection = _dbContext.CreateConnection();
            var query = @"
                UPDATE Inventario 
                SET IsDeleted = 1, 
                    UpdatedBy = @DeletedBy, 
                    UpdatedDate = NOW() 
                WHERE Id = @Id AND IsDeleted = 0";
            var result = await connection.ExecuteAsync(query, new { Id = id, DeletedBy = deletedBy });
            return result > 0;
        }
    }
}