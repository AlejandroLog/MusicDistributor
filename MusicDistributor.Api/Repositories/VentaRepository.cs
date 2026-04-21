using Dapper;
using MusicDistributor.Api.DataAccess.Interfaces;
using MusicDistributor.Api.Repositories.Interfaces;
using MusicDistributor.Core.Entities;

namespace MusicDistributor.Api.Repositories
{
    public class VentaRepository : IVentaRepository
    {
        private readonly IDbContext _dbContext;

        public VentaRepository(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Venta>> GetAllAsync()
        {
            using var connection = _dbContext.CreateConnection();
            var query = "SELECT * FROM Venta WHERE IsDeleted = 0 ORDER BY FechaVenta DESC";
            return await connection.QueryAsync<Venta>(query);
        }

        public async Task<Venta?> GetByIdAsync(int id)
        {
            using var connection = _dbContext.CreateConnection();
            var query = "SELECT * FROM Venta WHERE Id = @Id AND IsDeleted = 0";
            return await connection.QuerySingleOrDefaultAsync<Venta>(query, new { Id = id });
        }

        public async Task<IEnumerable<Venta>> GetByClienteEmailAsync(string email)
        {
            using var connection = _dbContext.CreateConnection();
            var query = "SELECT * FROM Venta WHERE ClienteEmail = @Email AND IsDeleted = 0";
            return await connection.QueryAsync<Venta>(query, new { Email = email });
        }

        public async Task<int> AddAsync(Venta venta)
        {
            using var connection = _dbContext.CreateConnection();
            var query = @"
                INSERT INTO Venta (FechaVenta, ClienteEmail, TotalVenta, Estatus, CreatedBy, CreatedDate, IsDeleted) 
                VALUES (@FechaVenta, @ClienteEmail, @TotalVenta, @Estatus, @CreatedBy, NOW(), 0);
                SELECT LAST_INSERT_ID();";
            
            var newId = await connection.ExecuteScalarAsync<ulong>(query, venta);
            return Convert.ToInt32(newId);
        }

        public async Task<bool> UpdateAsync(Venta venta)
        {
            using var connection = _dbContext.CreateConnection();
            var query = @"
                UPDATE Venta 
                SET ClienteEmail = @ClienteEmail, 
                    TotalVenta = @TotalVenta, 
                    Estatus = @Estatus,
                    UpdatedBy = @UpdatedBy, 
                    UpdatedDate = NOW() 
                WHERE Id = @Id AND IsDeleted = 0";
            
            var result = await connection.ExecuteAsync(query, venta);
            return result > 0;
        }

        public async Task<bool> DeleteAsync(int id, string deletedBy)
        {
            using var connection = _dbContext.CreateConnection();
            var query = @"
                UPDATE Venta 
                SET IsDeleted = 1, 
                    UpdatedBy = @DeletedBy, 
                    UpdatedDate = NOW() 
                WHERE Id = @Id AND IsDeleted = 0";
            
            var result = await connection.ExecuteAsync(query, new { Id = id, DeletedBy = deletedBy });
            return result > 0;
        }
    }
}