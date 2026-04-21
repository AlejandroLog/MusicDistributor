using Dapper;
using MusicDistributor.Api.DataAccess.Interfaces;
using MusicDistributor.Api.Repositories.Interfaces;
using MusicDistributor.Core.Entities;

namespace MusicDistributor.Api.Repositories
{
    public class ArtistaRepository : IArtistaRepository
    {
        private readonly IDbContext _dbContext;

        public ArtistaRepository(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Artista>> GetAllAsync()
        {
            using var connection = _dbContext.CreateConnection();
            var query = "SELECT * FROM Artista WHERE IsDeleted = 0";
            return await connection.QueryAsync<Artista>(query);
        }

        public async Task<Artista?> GetByIdAsync(int id)
        {
            using var connection = _dbContext.CreateConnection();
            var query = "SELECT * FROM Artista WHERE Id = @Id AND IsDeleted = 0";
            return await connection.QuerySingleOrDefaultAsync<Artista>(query, new { Id = id });
        }

        public async Task<int> AddAsync(Artista artista)
        {
            using var connection = _dbContext.CreateConnection();
            var query = @"
                INSERT INTO Artista (UsuarioId, GeneroMusicalId, NombreBanda, ContactoEmail, CreatedBy, CreatedDate, IsDeleted) 
                VALUES (@UsuarioId, @GeneroMusicalId, @NombreBanda, @ContactoEmail, @CreatedBy, NOW(), 0);
                SELECT LAST_INSERT_ID();";
            
            var newId = await connection.ExecuteScalarAsync<ulong>(query, artista);
            return Convert.ToInt32(newId);
        }

        public async Task<bool> UpdateAsync(Artista artista)
        {
            using var connection = _dbContext.CreateConnection();
            var query = @"
                UPDATE Artista 
                SET UsuarioId = @UsuarioId, 
                    GeneroMusicalId = @GeneroMusicalId, 
                    NombreBanda = @NombreBanda, 
                    ContactoEmail = @ContactoEmail, 
                    UpdatedBy = @UpdatedBy, 
                    UpdatedDate = NOW() 
                WHERE Id = @Id AND IsDeleted = 0";
            var result = await connection.ExecuteAsync(query, artista);
            return result > 0;
        }

        public async Task<bool> DeleteAsync(int id, string deletedBy)
        {
            using var connection = _dbContext.CreateConnection();
            var query = @"
                UPDATE Artista 
                SET IsDeleted = 1, 
                    UpdatedBy = @DeletedBy, 
                    UpdatedDate = NOW() 
                WHERE Id = @Id AND IsDeleted = 0";
            var result = await connection.ExecuteAsync(query, new { Id = id, DeletedBy = deletedBy });
            return result > 0;
        }
    }
}