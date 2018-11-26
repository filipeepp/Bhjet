namespace BHJet_Repositorio.Admin.Entidade
{
    public class ItemFaturamentoEntidade
    {
        public int? idCorrida { get; set; }
        public int? idRegistroDiaria { get; set; }
        public int? idTarifario { get; set; }
        public int idColaboradorEmpresaSistema { get; set; }
        public int idUsuarioFaturado { get; set; }
        public int? idPeriodoFaturamento { get; set; }
        public decimal decValor { get; set; }
        public decimal decValorComissao { get; set; }
        public bool? bitFaturado { get; set; }
    }

    public class ItemFaturamentoResumidoEntidade
    {
        public long ID { get; set; }
        public string NomeCliente { get; set; }
        public string Periodo { get; set; }
        public string TipoContrato { get; set; }
        public decimal Valor { get; set; }
    }
}
