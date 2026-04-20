namespace MusicDistributor.Core.Entities
{
    public class Usuario : EntityBase
    {
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
    }
}