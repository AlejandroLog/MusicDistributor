namespace MusicDistributor.Core.Entities
{
    public class Lanzamiento : EntityBase
    {
        public int ArtistaId { get; set; } // Obligatorio
        public string TituloObra { get; set; } = string.Empty; // Obligatorio
        public DateTime? FechaLanzamiento { get; set; } // Opcional (NULL en BD)
        public string TipoLanzamiento { get; set; } = string.Empty; // Obligatorio (EP, Single, LP, etc.)
    }
}