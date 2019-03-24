namespace BHJet_DTO.Corrida
{
    public class CalculoCorridaDTO
    {
        public long IDCliente { get; set; }
        public int TipoVeiculo { get; set; }
        public CalculoCorridaLocalidadeDTO[] Localizacao { get; set; }

    }

    public class CalculoCorridaLocalidadeDTO
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

}
