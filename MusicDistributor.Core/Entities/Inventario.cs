namespace MusicDistributor.Core.Entities
{
    public class Inventario : EntityBase
    {
        public int LanzamientoId { get; set; }
        public int FormatoFisicoId { get; set; }
        public int StockDisponible { get; set; }
        public decimal PrecioVenta { get; set; }
        public string Sku { get; set; } = string.Empty;
    }
}