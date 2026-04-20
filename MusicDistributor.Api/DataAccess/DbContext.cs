using System.Data;
using MySql.Data.MySqlClient;
using MusicDistributor.Api.DataAccess.Interfaces;

namespace MusicDistributor.Api.DataAccess
{
    public class DbContext : IDbContext
    {
        private readonly string _connectionString;

        public DbContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                                ?? throw new ArgumentNullException("La cadena de conexión no puede ser nula.");
        }

        public IDbConnection CreateConnection() => new MySqlConnection(_connectionString);
    }
}