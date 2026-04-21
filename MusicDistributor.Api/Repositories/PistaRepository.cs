using Dapper;
using MusicDistributor.Api.DataAccess.Interfaces;
using MusicDistributor.Api.Repositories.Interfaces;
using MusicDistributor.Core.Entities;

namespace MusicDistributor.Api.Repositories
{
    public class PistaRepository : IPistaRepository
    {
        private readonly IDbContext _dbContext;

        public PistaRepository(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Pista>> GetAllAsync()
        {
            using var connection = _dbContext.CreateConnection();
            var query = "SELECT * FROM Pista WHERE IsDeleted = 0";
            return await connection.QueryAsync<Pista>(query);
        }

        public async Task<Pista?> GetByIdAsync(int id)
        {
            using var connection = _dbContext.CreateConnection();
            var query = "SELECT * FROM Pista WHERE Id = @Id AND IsDeleted = 0";
            return await connection.QuerySingleOrDefaultAsync<Pista>(query, new { Id = id });
        }

        public async Task<IEnumerable<Pista>> GetByLanzamientoIdAsync(int lanzamientoId)
        {
            using var connection = _dbContext.CreateConnection();
            var query = "SELECT * FROM Pista WHERE LanzamientoId = @LanzamientoId AND IsDeleted = 0";
            return await connection.QueryAsync<Pista>(query, new { LanzamientoId = lanzamientoId });
        }

        public async Task<int> AddAsync(Pista pista)
        {
            using var connection = _dbContext.CreateConnection();
            var query = @"
                INSERT INTO Pista (LanzamientoId, NumeroTrack, TituloCancion, Duracion, CreatedBy, CreatedDate, IsDeleted) 
                VALUES (@LanzamientoId, @NumeroTrack, @TituloCancion, @Duracion, @CreatedBy, NOW(), 0);
                SELECT LAST_INSERT_ID();";
            

            var newId = await connection.ExecuteScalarAsync<ulong>(query, pista);
            return Convert.ToInt32(newId);
        }

        public async Task<bool> UpdateAsync(Pista pista)
        {
            using var connection = _dbContext.CreateConnection();
            var query = @"
                UPDATE Pista 
                SET LanzamientoId = @LanzamientoId,
                    NumeroTrack = @NumeroTrack,
                    TituloCancion = @TituloCancion,
                    Duracion = @Duracion,
                    UpdatedBy = @UpdatedBy, 
                    UpdatedDate = NOW() 
                WHERE Id = @Id AND IsDeleted = 0";
            
            var result = await connection.ExecuteAsync(query, pista);
            return result > 0;
        }

        public async Task<bool> DeleteAsync(int id, string deletedBy)
        {
            using var connection = _dbContext.CreateConnection();
            var query = @"
                UPDATE Pista 
                SET IsDeleted = 1, 
                    UpdatedBy = @DeletedBy, 
                    UpdatedDate = NOW() 
                WHERE Id = @Id AND IsDeleted = 0";
            
            var result = await connection.ExecuteAsync(query, new { Id = id, DeletedBy = deletedBy });
            return result > 0;
        }
    }
}