namespace MusicDistributor.Core.Entities
{
    public class Venta : EntityBase
    {
        public DateTime FechaVenta { get; set; } = DateTime.Now;
        public string ClienteEmail { get; set; } = string.Empty;
        public decimal TotalVenta { get; set; }
        public string Estatus { get; set; } = "Pendiente";
    }
}