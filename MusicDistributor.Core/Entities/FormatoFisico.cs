namespace MusicDistributor.Core.Entities
{
    public class FormatoFisico : EntityBase
    {
        public string Nombre { get; set; } = string.Empty;
        public bool RequiereEnvioFisico { get; set; }
    }
}