namespace MusicDistributor.Core.Entities
{
    public class DetalleVenta : EntityBase
    {
        public int VentaId { get; set; }
        public int InventarioId { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
    }
}