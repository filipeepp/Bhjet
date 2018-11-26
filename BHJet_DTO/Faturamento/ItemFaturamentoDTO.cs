namespace BHJet_DTO.Faturamento
{
    public class ItemFaturamentoDTO
    {
        public long ID { get; set; }
        public string NomeCliente { get; set; }
        public string Periodo { get; set; }
        public string TipoContrato { get; set; }
        public decimal Valor { get; set; }
    }
}
