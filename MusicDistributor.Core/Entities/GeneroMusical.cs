namespace MusicDistributor.Core.Entities
{
    public class GeneroMusical : EntityBase
    {
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
    }
}