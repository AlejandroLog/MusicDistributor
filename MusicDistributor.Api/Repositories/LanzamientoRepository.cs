using Dapper;
using MusicDistributor.Api.DataAccess.Interfaces;
using MusicDistributor.Api.Repositories.Interfaces;
using MusicDistributor.Core.Entities;

namespace MusicDistributor.Api.Repositories
{
    public class LanzamientoRepository : ILanzamientoRepository
    {
        private readonly IDbContext _dbContext;

        public LanzamientoRepository(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Lanzamiento>> GetAllAsync()
        {
            using var connection = _dbContext.CreateConnection();
            var query = "SELECT * FROM Lanzamiento WHERE IsDeleted = 0";
            return await connection.QueryAsync<Lanzamiento>(query);
        }

        public async Task<Lanzamiento?> GetByIdAsync(int id)
        {
            using var connection = _dbContext.CreateConnection();
            var query = "SELECT * FROM Lanzamiento WHERE Id = @Id AND IsDeleted = 0";
            return await connection.QuerySingleOrDefaultAsync<Lanzamiento>(query, new { Id = id });
        }

        public async Task<int> AddAsync(Lanzamiento lanzamiento)
        {
            using var connection = _dbContext.CreateConnection();
            var query = @"
                INSERT INTO Lanzamiento (ArtistaId, TituloObra, FechaLanzamiento, TipoLanzamiento, CreatedBy, CreatedDate, IsDeleted) 
                VALUES (@ArtistaId, @TituloObra, @FechaLanzamiento, @TipoLanzamiento, @CreatedBy, NOW(), 0);
                SELECT LAST_INSERT_ID();";
            
            var newId = await connection.ExecuteScalarAsync<ulong>(query, lanzamiento);
            return Convert.ToInt32(newId);
        }

        public async Task<bool> UpdateAsync(Lanzamiento lanzamiento)
        {
            using var connection = _dbContext.CreateConnection();
            var query = @"
                UPDATE Lanzamiento 
                SET ArtistaId = @ArtistaId, 
                    TituloObra = @TituloObra, 
                    FechaLanzamiento = @FechaLanzamiento, 
                    TipoLanzamiento = @TipoLanzamiento, 
                    UpdatedBy = @UpdatedBy, 
                    UpdatedDate = NOW() 
                WHERE Id = @Id AND IsDeleted = 0";
            var result = await connection.ExecuteAsync(query, lanzamiento);
            return result > 0;
        }

        public async Task<bool> DeleteAsync(int id, string deletedBy)
        {
            using var connection = _dbContext.CreateConnection();
            var query = @"
                UPDATE Lanzamiento 
                SET IsDeleted = 1, 
                    UpdatedBy = @DeletedBy, 
                    UpdatedDate = NOW() 
                WHERE Id = @Id AND IsDeleted = 0";
            var result = await connection.ExecuteAsync(query, new { Id = id, DeletedBy = deletedBy });
            return result > 0;
        }
    }
}