using Dapper;
using MusicDistributor.Api.DataAccess.Interfaces;
using MusicDistributor.Api.Repositories.Interfaces;
using MusicDistributor.Core.Entities;

namespace MusicDistributor.Api.Repositories
{
    public class FormatoFisicoRepository : IFormatoFisicoRepository
    {
        private readonly IDbContext _dbContext;

        public FormatoFisicoRepository(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<FormatoFisico>> GetAllAsync()
        {
            using var connection = _dbContext.CreateConnection();
            var query = "SELECT * FROM FormatoFisico WHERE IsDeleted = 0";
            return await connection.QueryAsync<FormatoFisico>(query);
        }

        public async Task<FormatoFisico?> GetByIdAsync(int id)
        {
            using var connection = _dbContext.CreateConnection();
            var query = "SELECT * FROM FormatoFisico WHERE Id = @Id AND IsDeleted = 0";
            return await connection.QuerySingleOrDefaultAsync<FormatoFisico>(query, new { Id = id });
        }

        public async Task<int> AddAsync(FormatoFisico formato)
        {
            using var connection = _dbContext.CreateConnection();
            var query = @"
                INSERT INTO FormatoFisico (Nombre, RequiereEnvioFisico, CreatedBy, CreatedDate, IsDeleted) 
                VALUES (@Nombre, @RequiereEnvioFisico, @CreatedBy, NOW(), 0);
                SELECT LAST_INSERT_ID();";
            
            // Casteo a ulong para el LAST_INSERT_ID() de MySQL
            var newId = await connection.ExecuteScalarAsync<ulong>(query, formato);
            return Convert.ToInt32(newId);
        }

        public async Task<bool> UpdateAsync(FormatoFisico formato)
        {
            using var connection = _dbContext.CreateConnection();
            var query = @"
                UPDATE FormatoFisico 
                SET Nombre = @Nombre, 
                    RequiereEnvioFisico = @RequiereEnvioFisico, 
                    UpdatedBy = @UpdatedBy, 
                    UpdatedDate = NOW() 
                WHERE Id = @Id AND IsDeleted = 0";
            var result = await connection.ExecuteAsync(query, formato);
            return result > 0;
        }

        public async Task<bool> DeleteAsync(int id, string deletedBy)
        {
            using var connection = _dbContext.CreateConnection();
            var query = @"
                UPDATE FormatoFisico 
                SET IsDeleted = 1, 
                    UpdatedBy = @DeletedBy, 
                    UpdatedDate = NOW() 
                WHERE Id = @Id AND IsDeleted = 0";
            var result = await connection.ExecuteAsync(query, new { Id = id, DeletedBy = deletedBy });
            return result > 0;
        }
    }
}