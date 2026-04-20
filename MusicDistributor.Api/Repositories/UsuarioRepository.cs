using Dapper;
using MusicDistributor.Api.DataAccess.Interfaces;
using MusicDistributor.Api.Repositories.Interfaces;
using MusicDistributor.Core.Entities;

namespace MusicDistributor.Api.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly IDbContext _dbContext;

        public UsuarioRepository(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Usuario>> GetAllAsync()
        {
            using var connection = _dbContext.CreateConnection();
            var query = "SELECT * FROM Usuario WHERE IsDeleted = 0";
            return await connection.QueryAsync<Usuario>(query);
        }

        public async Task<Usuario?> GetByIdAsync(int id)
        {
            using var connection = _dbContext.CreateConnection();
            var query = "SELECT * FROM Usuario WHERE Id = @Id AND IsDeleted = 0";
            return await connection.QuerySingleOrDefaultAsync<Usuario>(query, new { Id = id });
        }

        public async Task<int> AddAsync(Usuario usuario)
        {
            using var connection = _dbContext.CreateConnection();
            var query = @"
                INSERT INTO Usuario (Username, PasswordHash, Rol, CreatedBy, CreatedDate, IsDeleted) 
                VALUES (@Username, @PasswordHash, @Rol, @CreatedBy, NOW(), 0);
                SELECT LAST_INSERT_ID();";
            return await connection.ExecuteScalarAsync<int>(query, usuario);
        }

        public async Task<bool> UpdateAsync(Usuario usuario)
        {
            using var connection = _dbContext.CreateConnection();
            var query = @"
                UPDATE Usuario 
                SET Username = @Username, 
                    PasswordHash = @PasswordHash, 
                    Rol = @Rol, 
                    UpdatedBy = @UpdatedBy, 
                    UpdatedDate = NOW() 
                WHERE Id = @Id AND IsDeleted = 0";
            var result = await connection.ExecuteAsync(query, usuario);
            return result > 0;
        }

        public async Task<bool> DeleteAsync(int id, string deletedBy)
        {
            using var connection = _dbContext.CreateConnection();
            var query = @"
                UPDATE Usuario 
                SET IsDeleted = 1, 
                    UpdatedBy = @DeletedBy, 
                    UpdatedDate = NOW() 
                WHERE Id = @Id AND IsDeleted = 0";
            var result = await connection.ExecuteAsync(query, new { Id = id, DeletedBy = deletedBy });
            return result > 0;
        }

        public async Task<Usuario?> ValidateUserAsync(string username, string passwordHash)
        {
            using var connection = _dbContext.CreateConnection();
            var query = "SELECT * FROM Usuario WHERE Username = @Username AND PasswordHash = @PasswordHash AND IsDeleted = 0";
            return await connection.QuerySingleOrDefaultAsync<Usuario>(query, new { Username = username, PasswordHash = passwordHash });
        }
    }
}