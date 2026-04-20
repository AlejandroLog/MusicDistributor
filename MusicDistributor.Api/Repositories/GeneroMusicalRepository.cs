using Dapper;
using MusicDistributor.Api.DataAccess.Interfaces;
using MusicDistributor.Api.Repositories.Interfaces;
using MusicDistributor.Core.Entities;

namespace MusicDistributor.Api.Repositories
{
    public class GeneroMusicalRepository : IGeneroMusicalRepository
    {
        private readonly IDbContext _dbContext;

        public GeneroMusicalRepository(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<GeneroMusical>> GetAllAsync()
        {
            using var connection = _dbContext.CreateConnection();
            var query = "SELECT * FROM GeneroMusical WHERE IsDeleted = 0";
            return await connection.QueryAsync<GeneroMusical>(query);
        }

        public async Task<GeneroMusical?> GetByIdAsync(int id)
        {
            using var connection = _dbContext.CreateConnection();
            var query = "SELECT * FROM GeneroMusical WHERE Id = @Id AND IsDeleted = 0";
            return await connection.QuerySingleOrDefaultAsync<GeneroMusical>(query, new { Id = id });
        }

        public async Task<int> AddAsync(GeneroMusical genero)
        {
            using var connection = _dbContext.CreateConnection();
            var query = @"
                INSERT INTO GeneroMusical (Nombre, Descripcion, CreatedBy, CreatedDate, IsDeleted) 
                VALUES (@Nombre, @Descripcion, @CreatedBy, NOW(), 0);
                SELECT LAST_INSERT_ID();";
            
            var newId = await connection.ExecuteScalarAsync<ulong>(query, genero);
            return Convert.ToInt32(newId);
        }

        public async Task<bool> UpdateAsync(GeneroMusical genero)
        {
            using var connection = _dbContext.CreateConnection();
            var query = @"
                UPDATE GeneroMusical 
                SET Nombre = @Nombre, 
                    Descripcion = @Descripcion, 
                    UpdatedBy = @UpdatedBy, 
                    UpdatedDate = NOW() 
                WHERE Id = @Id AND IsDeleted = 0";
            var result = await connection.ExecuteAsync(query, genero);
            return result > 0;
        }

        public async Task<bool> DeleteAsync(int id, string deletedBy)
        {
            using var connection = _dbContext.CreateConnection();
            var query = @"
                UPDATE GeneroMusical 
                SET IsDeleted = 1, 
                    UpdatedBy = @DeletedBy, 
                    UpdatedDate = NOW() 
                WHERE Id = @Id AND IsDeleted = 0";
            var result = await connection.ExecuteAsync(query, new { Id = id, DeletedBy = deletedBy });
            return result > 0;
        }
    }
}