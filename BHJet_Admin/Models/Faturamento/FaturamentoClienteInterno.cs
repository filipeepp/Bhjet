using System;

namespace BHJet_Admin.Models.Faturamento
{
    public class FaturamentoClienteInterno
    {
        public string DataRelatorio { get => DateTime.Now.ToLongDateString(); }
        public string TotalCorrida { get; set; }
        public string TotalDiaria { get; set; }
        public FaturamentoClienteInternoCorrida[] Corridas { get; set; }
        public FaturamentoClienteInternoDiaria[] Diarias { get; set; }
    }

    public class FaturamentoClienteInternoCorrida
    {
        public DateTime? DataCorrida { get; set; }
        public string TipoContrato { get => "Corrida (OS Avulsa)"; }
        public long NumeroOS { get; set; }
        public string Profissional { get; set; }
        public decimal ValorEstimado { get; set; }
        public decimal ValorFinalizado { get; set; }
    }

    public class FaturamentoClienteInternoDiaria
    {
        public DateTime DataCorrida { get; set; }
        public string TipoContrato { get => "Diária"; }
        public long NumeroOS { get; set; }
        public string Profissional { get; set; }
        public decimal Valor { get; set; }
    }
}