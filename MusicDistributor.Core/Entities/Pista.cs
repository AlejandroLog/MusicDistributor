namespace MusicDistributor.Core.Entities
{
    public class Pista : EntityBase
    {
        public int LanzamientoId { get; set; }
        public int NumeroTrack { get; set; }
        public string TituloCancion { get; set; } = string.Empty;
        public string? Duracion { get; set; }
    }
}