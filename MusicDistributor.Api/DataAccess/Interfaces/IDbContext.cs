using System.Data;

namespace MusicDistributor.Api.DataAccess.Interfaces
{
    public interface IDbContext
    {
        IDbConnection CreateConnection();
    }
}