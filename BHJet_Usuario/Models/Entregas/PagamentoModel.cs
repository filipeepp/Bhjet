namespace BHJet_Usuario.Models.Entregas
{
    public class PagamentoModel
    {
        public string NomeCartaoCredito { get; set; }
        public string NumeroCartaoCredito { get; set; }
        public string Validade { get; set; }
        public int CodigoVerificacao { get; set; }
        public double? ValorTotal { get; set; }
        public long? NumeroOS { get; set; }
    }
}