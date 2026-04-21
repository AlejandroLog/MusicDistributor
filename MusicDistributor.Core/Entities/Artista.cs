namespace MusicDistributor.Core.Entities
{
    public class Artista : EntityBase
    {
        public int? UsuarioId { get; set; }
        public int GeneroMusicalId { get; set; }
        public string NombreBanda { get; set; } = string.Empty;
        public string? ContactoEmail { get; set; }
    }
}